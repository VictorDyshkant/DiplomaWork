using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using VideoHosting.Abstractions.Repositories;
using VideoHosting.DataBase;
using VideoHosting.Abstractions.Services;
using VideoHosting.Abstractions.UnitOfWork;
using VideoHosting.Core.Middlewares;
using VideoHosting.DataBase.Repositories;
using VideoHosting.DataBase.UnitOfWork;
using VideoHosting.Domain.Entities;
using VideoHosting.Services.Services;

namespace VideoHosting.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                       .AddJwtBearer(options =>
                       {
                           options.RequireHttpsMetadata = false;
                           options.TokenValidationParameters = new TokenValidationParameters
                           {
                               // укзывает, будет ли валидироваться издатель при валидации токена
                               ValidateIssuer = true,
                               // строка, представляющая издателя
                               ValidIssuer = AuthOptions.ISSUER,

                               // будет ли валидироваться потребитель токена
                               ValidateAudience = true,
                               // установка потребителя токена
                               ValidAudience = AuthOptions.AUDIENCE,
                               // будет ли валидироваться время существования
                               ValidateLifetime = true,

                               // установка ключа безопасности
                               IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                               // валидация ключа безопасности
                               ValidateIssuerSigningKey = true,
                           };
                       });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvcCore().AddDataAnnotations();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = EnvironmentName.Development;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<LogMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UserContent")),
                RequestPath = new PathString("/Content")
            });

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors("CorsPolicy");
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddDbContext<DataBaseContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("VideoHostingConnection"))
                   .UseLazyLoadingProxies();
            });

            services.AddIdentity<User, UserRole>(opts =>
                    {
                        opts.Password.RequiredLength = 5;   // минимальная длина
                        opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                        opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                        opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                        opts.Password.RequireDigit = false;
                    })
                    .AddEntityFrameworkStores<DataBaseContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICommentaryRepository, CommentaryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IAppSwitchRepository, AppSwitchRepository>();

            services.AddScoped<ICommentaryService, CommentaryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<ICredentialService, CredentialService>();

            IMapper mapper = Mapping.GetMapper(Configuration);
            services.AddSingleton(mapper);
        }
    }
}

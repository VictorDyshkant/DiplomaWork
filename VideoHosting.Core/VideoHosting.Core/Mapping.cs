using AutoMapper;
using Microsoft.Extensions.Configuration;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Core.Models;
using VideoHosting.Services.Mapper;

namespace VideoHosting.Core
{
    public class Mapping : Profile
    {
        public Mapping()
        {

	        CreateMap<UserDto, LoginUserModel>().ReverseMap();
            CreateMap<LoginUserModel, UserDto>();

            CreateMap<UserRegistrationModel, UserLoginDto>()
                .ForMember(x=>x.Email, opt => opt.MapFrom(c => c.Email))
                .ForMember(x=>x.Password, opt => opt.MapFrom(c => c.Password))
                .ForMember(x=>x.PhoneNumber, opt => opt.MapFrom(c => c.PhoneNumber));

            CreateMap<UserRegistrationModel, UserDto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Surname, opt => opt.MapFrom(c => c.Surname))
                .ForMember(x => x.Sex, opt => opt.MapFrom(c => c.Sex))
                .ForMember(x => x.Faculty, opt => opt.MapFrom(c => c.Faculty))
                .ForMember(x => x.Group, opt => opt.MapFrom(c => c.Group));
        }

        public static IMapper GetMapper(IConfiguration configuration )
        {
            var conf = new MapperConfiguration(opt =>
	            {
		            opt.AddProfiles(new Profile[] {new Mapping(), new MapperService(configuration) });
	            });

            return conf.CreateMapper();
        }
    }
}

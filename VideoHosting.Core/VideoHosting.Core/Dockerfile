FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["VideoHosting.Core/VideoHosting.Core.csproj", "VideoHosting.Core/"]
RUN dotnet restore "VideoHosting.Core/VideoHosting.Core.csproj"
COPY . .
WORKDIR "/src/VideoHosting.Core"
RUN dotnet build "VideoHosting.Core.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VideoHosting.Core.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VideoHosting.Core.dll"]
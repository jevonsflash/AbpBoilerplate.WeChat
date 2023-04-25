FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/WeChatPaySample.Web.Host/WeChatPaySample.Web.Host.csproj", "src/WeChatPaySample.Web.Host/"]
COPY ["src/WeChatPaySample.Web.Core/WeChatPaySample.Web.Core.csproj", "src/WeChatPaySample.Web.Core/"]
COPY ["src/WeChatPaySample.Application/WeChatPaySample.Application.csproj", "src/WeChatPaySample.Application/"]
COPY ["src/WeChatPaySample.Core/WeChatPaySample.Core.csproj", "src/WeChatPaySample.Core/"]
COPY ["src/WeChatPaySample.EntityFrameworkCore/WeChatPaySample.EntityFrameworkCore.csproj", "src/WeChatPaySample.EntityFrameworkCore/"]
WORKDIR "/src/src/WeChatPaySample.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/WeChatPaySample.Web.Host", "src/WeChatPaySample.Web.Host"]
COPY ["src/WeChatPaySample.Web.Core", "src/WeChatPaySample.Web.Core"]
COPY ["src/WeChatPaySample.Application", "src/WeChatPaySample.Application"]
COPY ["src/WeChatPaySample.Core", "src/WeChatPaySample.Core"]
COPY ["src/WeChatPaySample.EntityFrameworkCore", "src/WeChatPaySample.EntityFrameworkCore"]
WORKDIR "/src/src/WeChatPaySample.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "WeChatPaySample.Web.Host.dll"]

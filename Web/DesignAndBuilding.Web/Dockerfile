# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/DesignAndBuilding.Web/DesignAndBuilding.Web.csproj", "Web/DesignAndBuilding.Web/"]
COPY ["DesignAndBuilding.Common/DesignAndBuilding.Common.csproj", "DesignAndBuilding.Common/"]
COPY ["Data/DesignAndBuilding.Data.Models/DesignAndBuilding.Data.Models.csproj", "Data/DesignAndBuilding.Data.Models/"]
COPY ["Data/DesignAndBuilding.Data.Common/DesignAndBuilding.Data.Common.csproj", "Data/DesignAndBuilding.Data.Common/"]
COPY ["Data/DesignAndBuilding.Data/DesignAndBuilding.Data.csproj", "Data/DesignAndBuilding.Data/"]
COPY ["Services/DesignAndBuilding.Services.Data/DesignAndBuilding.Services.Data.csproj", "Services/DesignAndBuilding.Services.Data/"]
COPY ["Services/DesignAndBuilding.Services.Mapping/DesignAndBuilding.Services.Mapping.csproj", "Services/DesignAndBuilding.Services.Mapping/"]
COPY ["Web/DesignAndBuilding.Web.ViewModels/DesignAndBuilding.Web.ViewModels.csproj", "Web/DesignAndBuilding.Web.ViewModels/"]
COPY ["Services/DesignAndBuilding.Services/DesignAndBuilding.Services.csproj", "Services/DesignAndBuilding.Services/"]
COPY ["Services/DesignAndBuilding.Services.Messaging/DesignAndBuilding.Services.Messaging.csproj", "Services/DesignAndBuilding.Services.Messaging/"]
COPY ["Web/DesignAndBuilding.Web.Infrastructure/DesignAndBuilding.Web.Infrastructure.csproj", "Web/DesignAndBuilding.Web.Infrastructure/"]
RUN dotnet restore "./Web/DesignAndBuilding.Web/DesignAndBuilding.Web.csproj"
COPY . .
WORKDIR "/src/Web/DesignAndBuilding.Web"
RUN dotnet build "./DesignAndBuilding.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DesignAndBuilding.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DesignAndBuilding.Web.dll"]
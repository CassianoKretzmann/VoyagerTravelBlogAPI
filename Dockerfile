# Base step
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 443


# Base image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


# Copy project files, maintaining structure
COPY ["VoyagerTravelBlog.Api/VoyagerTravelBlog.Api.csproj", "VoyagerTravelBlog.Api/"]
COPY ["VoyagerTravelBlog.Application/VoyagerTravelBlog.Application.csproj", "VoyagerTravelBlog.Application/"]
COPY ["VoyagerTravelBlog.Domain/VoyagerTravelBlog.Domain.csproj", "VoyagerTravelBlog.Domain/"]
COPY ["VoyagerTravelBlog.Infrastructure/VoyagerTravelBlog.Infrastructure.csproj", "VoyagerTravelBlog.Infrastructure/"]


# Restore packages
RUN dotnet restore "VoyagerTravelBlog.Api/VoyagerTravelBlog.Api.csproj"


# Copy all source code and build
COPY . .
WORKDIR "/src/VoyagerTravelBlog.Api"
RUN dotnet build "VoyagerTravelBlog.Api.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/build


# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VoyagerTravelBlog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Final step
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VoyagerTravelBlog.Api.dll"]
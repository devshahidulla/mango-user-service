# Use the official ASP.NET Core runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 5000

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY ["Mango.UserService.API/Mango.UserService.API.csproj", "Mango.UserService.API/"]
COPY ["Mango.UserService.Application/Mango.UserService.Application.csproj", "Mango.UserService.Application/"]
COPY ["Mango.UserService.Domain/Mango.UserService.Domain.csproj", "Mango.UserService.Domain/"]
COPY ["Mango.UserService.Infrastructure/Mango.UserService.Infrastructure.csproj", "Mango.UserService.Infrastructure/"]
RUN dotnet restore "Mango.UserService.API/Mango.UserService.API.csproj"

# Copy the rest of the files
COPY . .
WORKDIR "/src/Mango.UserService.API"
RUN dotnet build "Mango.UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Mango.UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage - create the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5001
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Mango.UserService.API.dll"]


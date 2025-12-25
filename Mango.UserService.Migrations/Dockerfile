# Use the official .NET 8.0 runtime as base image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Mango.UserService.Migrations/Mango.UserService.Migrations.csproj", "Mango.UserService.Migrations/"]
RUN dotnet restore "Mango.UserService.Migrations/Mango.UserService.Migrations.csproj"

# Copy the rest of the files
COPY . .
WORKDIR "/src/Mango.UserService.Migrations"
RUN dotnet build "Mango.UserService.Migrations.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Mango.UserService.Migrations.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage - create the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variable for connection string (can be overridden at runtime)
ENV ConnectionStrings__DefaultConnection=""

ENTRYPOINT ["dotnet", "Mango.UserService.Migrations.dll"]


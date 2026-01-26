# Mango User Service

A microservice for managing user operations in the Mango application, built with .NET 9.0 and following Clean Architecture principles.

## 🏗️ Architecture

This project follows Clean Architecture with clear separation of concerns:

- **Mango.UserService.API** - Web API layer with controllers and endpoints
- **Mango.UserService.Application** - Business logic and use cases
- **Mango.UserService.Domain** - Domain entities and business rules
- **Mango.UserService.Infrastructure** - Data access and external services
- **Mango.UserService.Migrations** - Database migrations
- **Mango.UserService.Tests** - Unit and integration tests

## 🐳 Docker Support

### Quick Start with Docker Compose
```bash
docker-compose up -d
```
This starts the User Service API and connects to your existing PostgreSQL database.

### Access the API
- **API:** http://localhost:5001
- **Swagger:** http://localhost:5001/swagger
- **Health:** http://localhost:5001/health

### Available Docker Images
- `mango-user-service-api:latest` (338MB) - The API service on port 5001
- `mango-user-service-migrations:latest` (287MB) - Database migrations

### Port Configuration
- **User API:** 5001 (no conflict with UI on 8080)
- **PostgreSQL:** 5432 (running on host machine)

📖 **See [QUICK_START.md](./QUICK_START.md) for quick reference**
📖 **See [DOCKER_API_GUIDE.md](./DOCKER_API_GUIDE.md) for complete Docker documentation**

## 🚀 Getting Started (Local Development)

### Prerequisites

- .NET 9.0 SDK or later
- Visual Studio 2022 / Rider or Visual Studio Code
- PostgreSQL 16+
- Docker (optional, for containerized deployment)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/devshahidulla/mango-user-service.git
cd mango-user-service
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password"
}
```

4. Run database migrations:
```bash
cd Mango.UserService.Migrations
dotnet run
```

5. Run the application:
```bash
cd ../Mango.UserService.API
dotnet run
```

## 🔧 Configuration

Configure the application settings in `appsettings.json`:

- **Logging**: Configure log levels for different components
- **AllowedHosts**: Set allowed hosts for the application
- **ConnectionStrings**: Database connection configuration

## 📝 API Documentation

Once the application is running, you can access:

- Swagger UI: `https://localhost:{port}/swagger`
- API endpoints: `https://localhost:{port}/api`

### User Registration API

The registration endpoint accepts the following fields to match the frontend UI:

**POST** `/api/v1/users/register`

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "SecurePassword123",
  "role": "Farmer"  // Optional: Defaults to "Farmer" if not provided. Valid values: Farmer, Reseller, Wholesaler
}
```

**Response:**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "John",
  "lastName": "Doe",
  "fullName": "John Doe",
  "email": "john.doe@example.com",
  "role": "Farmer",
  "createdAt": "2024-01-15T10:30:00Z"
}
```

**Validation Rules:**
- `firstName`: Required, max 50 characters
- `lastName`: Required, max 50 characters
- `email`: Required, valid email format
- `password`: Required, minimum 8 characters
- `role`: Optional (defaults to "Farmer"), must be one of: Farmer, Reseller, Wholesaler

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

## 🛠️ Built With

- **.NET** - Framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM (if applicable)
- **Clean Architecture** - Architectural pattern

## 📦 Project Structure

```
mango-user-service/
├── Mango.UserService.API/           # API controllers and configuration
│   ├── Controllers/                 # API controllers
│   ├── Program.cs                   # Application entry point
│   └── appsettings.json            # Configuration
├── Mango.UserService.Application/   # Business logic layer
├── Mango.UserService.Domain/        # Domain entities
├── Mango.UserService.Infrastructure/# Data access layer
└── Mango.UserService.Tests/         # Test projects
```

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👤 Author

**devshahidulla**

- GitHub: [@devshahidulla](https://github.com/devshahidulla)

## 📞 Support

For support, please open an issue in the GitHub repository.

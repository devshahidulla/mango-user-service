# 🐳 Mango User Service API - Docker Guide

## ✅ Docker Images Available

### 1. User Service API
- **Image:** `mango-user-service-api:latest`
- **Size:** 338MB (Compressed: 95.2MB)
- **Base:** .NET 9.0 ASP.NET Core
- **Ports:** 8080 (HTTP), 8081 (HTTPS)

### 2. Database Migrations
- **Image:** `mango-user-service-migrations:latest`
- **Size:** 287MB (Compressed: 79.9MB)
- **Base:** .NET 9.0 Runtime

---

## 🚀 Quick Start

### Option 1: Run API with Docker Run

#### Step 1: Ensure PostgreSQL is Running
```powershell
# Check if PostgreSQL is running
Get-Service postgresql*
# OR if using Docker PostgreSQL
docker ps | Select-String "postgres"
```

#### Step 2: Run the API Container
```powershell
docker run -d -p 8080:8080 -p 8081:8081 `
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=Adib@054008" `
  -e AWS__EventBusName="mango.events" `
  -e AWS__Region="us-east-1" `
  -e ASPNETCORE_ENVIRONMENT="Development" `
  --name mango-user-api `
  mango-user-service-api:latest
```

#### Step 3: Access the API
- **API Base URL:** http://localhost:8080
- **Swagger UI:** http://localhost:8080/swagger
- **Health Check:** http://localhost:8080/health

---

### Option 2: Use Docker Compose (Recommended)

This will start PostgreSQL, run migrations, and start the API automatically:

```powershell
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f user-api

# Stop all services
docker-compose down

# Stop and remove volumes (clean slate)
docker-compose down -v
```

**What Docker Compose includes:**
- ✅ PostgreSQL database
- ✅ Automatic database migrations
- ✅ User Service API
- ✅ Network configuration
- ✅ Health checks
- ✅ Volume persistence

---

## 📋 Container Management

### View Running Containers
```powershell
docker ps
```

### View API Logs
```powershell
docker logs -f mango-user-api
```

### Stop the API
```powershell
docker stop mango-user-api
```

### Remove the API Container
```powershell
docker rm mango-user-api
```

### Restart the API
```powershell
docker restart mango-user-api
```

### Execute Commands Inside Container
```powershell
docker exec -it mango-user-api /bin/bash
```

---

## 🔧 Configuration

### Environment Variables

| Variable | Description | Default | Required |
|----------|-------------|---------|----------|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | - | ✅ |
| `AWS__EventBusName` | AWS EventBridge bus name | mango.events | ✅ |
| `AWS__Region` | AWS region | us-east-1 | ❌ |
| `ASPNETCORE_ENVIRONMENT` | Environment (Development/Production) | Production | ❌ |
| `ASPNETCORE_URLS` | URL bindings | http://+:8080 | ❌ |

### Connection String Formats

**Local PostgreSQL (from Docker):**
```
Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password
```

**Docker Compose Network:**
```
Host=postgres;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password
```

**Remote PostgreSQL:**
```
Host=your-server.com;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password
```

---

## 🧪 Testing the API

### 1. Health Check
```powershell
curl http://localhost:8080/health
```

### 2. Register a User (POST)
```powershell
$body = @{
    firstName = "John"
    lastName = "Doe"
    email = "john.doe@example.com"
    password = "SecurePass123!"
    role = 0
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:8080/api/user/register" `
    -Method POST `
    -Body $body `
    -ContentType "application/json"
```

### 3. Get User by Email (GET)
```powershell
Invoke-RestMethod -Uri "http://localhost:8080/api/user/john.doe@example.com" `
    -Method GET
```

### 4. Using Swagger UI
Open browser: http://localhost:8080/swagger

---

## 🏗️ Rebuilding the Image

### Method 1: Using the Build Script
```powershell
.\build-api-image.ps1
```

### Method 2: Manual Build
```powershell
docker build -t mango-user-service-api:latest -f Mango.UserService.API/Dockerfile .
```

### Build with Version Tag
```powershell
docker build -t mango-user-service-api:v1.0.0 -f Mango.UserService.API/Dockerfile .
docker tag mango-user-service-api:v1.0.0 mango-user-service-api:latest
```

---

## 📦 Pushing to Registry

### Tag for Registry
```powershell
# For Docker Hub
docker tag mango-user-service-api:latest your-username/mango-user-service-api:latest

# For AWS ECR
docker tag mango-user-service-api:latest 123456789.dkr.ecr.us-east-1.amazonaws.com/mango-user-service-api:latest

# For Private Registry
docker tag mango-user-service-api:latest registry.example.com/mango-user-service-api:latest
```

### Push to Registry
```powershell
# Docker Hub
docker push your-username/mango-user-service-api:latest

# AWS ECR (after authentication)
docker push 123456789.dkr.ecr.us-east-1.amazonaws.com/mango-user-service-api:latest
```

---

## 🛠️ Troubleshooting

### Issue: Cannot Connect to Database

**Symptoms:** API logs show database connection errors

**Solutions:**
1. Ensure PostgreSQL is running
2. Check connection string is correct
3. Use `host.docker.internal` instead of `localhost` when connecting to host machine
4. Verify network connectivity: `docker network ls`

### Issue: Port Already in Use

**Symptoms:** Error binding to port 8080 or 8081

**Solutions:**
```powershell
# Find what's using the port
netstat -ano | findstr :8080

# Use different ports
docker run -p 9080:8080 -p 9081:8081 ...
```

### Issue: Image Not Found

**Symptoms:** Cannot find image 'mango-user-service-api:latest'

**Solutions:**
```powershell
# Verify image exists
docker images mango-user-service-api

# Rebuild if missing
.\build-api-image.ps1
```

### Issue: Container Exits Immediately

**Symptoms:** Container stops right after starting

**Solutions:**
```powershell
# Check logs
docker logs mango-user-api

# Run in foreground to see errors
docker run --rm -it -p 8080:8080 `
  -e ConnectionStrings__DefaultConnection="..." `
  mango-user-service-api:latest
```

---

## 🔍 Monitoring & Debugging

### View Real-time Logs
```powershell
docker logs -f --tail 100 mango-user-api
```

### Inspect Container
```powershell
docker inspect mango-user-api
```

### Check Resource Usage
```powershell
docker stats mango-user-api
```

### Get Container IP
```powershell
docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' mango-user-api
```

---

## 📊 Image Details

```
REPOSITORY                    TAG      SIZE        COMPRESSED
mango-user-service-api        latest   338MB       95.2MB
mango-user-service-migrations latest   287MB       79.9MB
```

**What's in the Image:**
- ✅ .NET 9.0 ASP.NET Core Runtime
- ✅ Mango.UserService.API.dll + dependencies
- ✅ All NuGet packages (Npgsql, Dapper, AWS SDK, etc.)
- ✅ Configuration files (appsettings.json)
- ✅ Optimized for production use

---

## 🎯 Production Deployment

### Environment Variables for Production
```yaml
environment:
  ASPNETCORE_ENVIRONMENT: Production
  ASPNETCORE_URLS: http://+:8080
  ConnectionStrings__DefaultConnection: "Host=prod-db.example.com;Port=5432;Database=mango_user_service;Username=app_user;Password=${DB_PASSWORD}"
  AWS__EventBusName: "mango.events.prod"
  AWS__Region: "us-east-1"
  AWS_ACCESS_KEY_ID: "${AWS_KEY}"
  AWS_SECRET_ACCESS_KEY: "${AWS_SECRET}"
```

### Health Check Endpoint
Add this to your `Program.cs` for container health checks:
```csharp
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
```

### Resource Limits (docker-compose.yml)
```yaml
user-api:
  image: mango-user-service-api:latest
  deploy:
    resources:
      limits:
        cpus: '2'
        memory: 1G
      reservations:
        cpus: '0.5'
        memory: 512M
```

---

## 📝 API Endpoints

Based on your UserController:

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/user/register` | Register a new user |
| GET | `/api/user/{email}` | Get user by email |
| GET | `/api/user/{id}` | Get user by ID |

**Swagger Documentation:** http://localhost:8080/swagger

---

## ✅ Summary

Your Docker images are ready and published to local Docker:

- ✅ **API Image:** `mango-user-service-api:latest` (338MB)
- ✅ **Migrations Image:** `mango-user-service-migrations:latest` (287MB)
- ✅ **Docker Compose:** Configured for full stack deployment
- ✅ **Build Scripts:** Automated image building
- ✅ **Documentation:** Complete usage guide

**Next Steps:**
1. Run migrations: `docker-compose up migrations`
2. Start API: `docker-compose up -d user-api`
3. Test API: http://localhost:8080/swagger
4. Deploy to production when ready!


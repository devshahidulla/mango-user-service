# 🚀 Mango User Service API - Quick Reference

## ✅ Updated Configuration (December 20, 2025)

### Port Configuration
- **User Service API:** Port **5001** (changed from 8080 to avoid UI conflict)
- **UI Service:** Port 8080 (your existing UI)
- **PostgreSQL:** Port 5432 (running on host, not in Docker)

---

## 🐳 Docker Images

| Image | Tag | Size | Status |
|-------|-----|------|--------|
| mango-user-service-api | latest | 338MB | ✅ Ready |
| mango-user-service-migrations | latest | 287MB | ✅ Ready |

---

## 🚀 Quick Start

### Start the API (connects to your existing PostgreSQL)
```powershell
docker-compose up -d
```

### Access the API
- **API Base URL:** http://localhost:5001
- **Swagger UI:** http://localhost:5001/swagger
- **Health Check:** http://localhost:5001/health

---

## 🔧 Run API Manually

```powershell
docker run -d -p 5001:5001 -p 5000:5000 `
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=Adib@054008" `
  -e AWS__EventBusName="mango.events" `
  -e AWS__Region="us-east-1" `
  -e ASPNETCORE_ENVIRONMENT="Development" `
  --name mango-user-api `
  mango-user-service-api:latest
```

---

## 🛠️ Common Commands

### Management
```powershell
# Start API
docker-compose up -d

# View logs
docker-compose logs -f user-api

# Stop API
docker-compose stop

# Restart API
docker-compose restart

# Remove container
docker-compose down
```

### Rebuild
```powershell
# Rebuild API image
.\build-api-image.ps1

# OR manually
docker build -t mango-user-service-api:latest -f Mango.UserService.API/Dockerfile .
```

---

## 🧪 Test the API

### Health Check
```powershell
Invoke-RestMethod http://localhost:5001/health
```

### Register User
```powershell
$body = @{
    firstName = "John"
    lastName = "Doe"
    email = "john.doe@example.com"
    password = "SecurePass123!"
    role = 0
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5001/api/user/register" `
    -Method POST `
    -Body $body `
    -ContentType "application/json"
```

### Get User
```powershell
Invoke-RestMethod http://localhost:5001/api/user/john.doe@example.com
```

---

## 📋 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Health check |
| POST | `/api/user/register` | Register new user |
| GET | `/api/user/{email}` | Get user by email |
| GET | `/api/user/{id}` | Get user by ID |
| GET | `/swagger` | API documentation |

---

## 🗂️ Architecture

```
┌─────────────────────────────┐
│   Your Local Machine        │
│                             │
│  ┌──────────────┐           │
│  │  PostgreSQL  │           │
│  │  :5432       │◄──────┐   │
│  └──────────────┘       │   │
│                         │   │
│  ┌──────────────┐       │   │
│  │   UI (Web)   │       │   │
│  │  :8080       │       │   │
│  └──────────────┘       │   │
│                         │   │
│  ┌──────────────────────┴─┐ │
│  │  User API (Docker)     │ │
│  │  :5001                 │ │
│  │  mango-user-service-api│ │
│  └────────────────────────┘ │
└─────────────────────────────┘
```

---

## ⚙️ Configuration Details

### Environment Variables
```yaml
ASPNETCORE_ENVIRONMENT: Development
ASPNETCORE_URLS: http://+:5001
ConnectionStrings__DefaultConnection: Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=Adib@054008
AWS__EventBusName: mango.events
AWS__Region: us-east-1
```

### Connection String
Uses `host.docker.internal` to connect from Docker container to PostgreSQL on your host machine.

---

## 🛠️ Troubleshooting

### Port Already in Use
```powershell
# Check what's using port 5001
netstat -ano | findstr :5001

# Kill process if needed
Stop-Process -Id <PID> -Force
```

### Can't Connect to Database
- Ensure PostgreSQL is running on your host
- Verify connection string in docker-compose.yml
- Check firewall allows connections

### View Container Logs
```powershell
docker logs -f mango-user-api
```

---

## 📦 Run Database Migrations

### Option 1: Using Docker
```powershell
docker run --rm `
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=Adib@054008" `
  mango-user-service-migrations:latest
```

### Option 2: Using .NET
```powershell
cd Mango.UserService.Migrations
dotnet run
```

---

## ✅ Summary

- ✅ API running on **port 5001** (no conflict with UI on 8080)
- ✅ Connects to **your existing PostgreSQL** on host
- ✅ No database in Docker (as requested)
- ✅ Health check endpoint available
- ✅ Swagger documentation enabled

**Start your API now:**
```powershell
docker-compose up -d
```

**Then access:** http://localhost:5001/swagger


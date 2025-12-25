# PowerShell script to build and publish Docker image for Mango User Service API

# Configuration
$ImageName = "mango-user-service-api"
$ImageTag = "latest"
$FullImageName = "${ImageName}:${ImageTag}"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Building Docker Image: $FullImageName" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan

# Build the Docker image
docker build -t $FullImageName -f Mango.UserService.API/Dockerfile .

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "================================================" -ForegroundColor Green
    Write-Host "✅ Docker image built successfully!" -ForegroundColor Green
    Write-Host "================================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Image: $FullImageName" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "To run the API, use:" -ForegroundColor White
    Write-Host "docker run -d -p 5001:5001 -p 5000:5000 ``" -ForegroundColor Gray
    Write-Host "  -e ConnectionStrings__DefaultConnection=`"Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password`" ``" -ForegroundColor Gray
    Write-Host "  -e AWS__EventBusName=`"mango.events`" ``" -ForegroundColor Gray
    Write-Host "  --name mango-user-api $FullImageName" -ForegroundColor Gray
    Write-Host ""
    Write-Host "To view API logs:" -ForegroundColor White
    Write-Host "docker logs -f mango-user-api" -ForegroundColor Gray
    Write-Host ""
    Write-Host "To stop the API:" -ForegroundColor White
    Write-Host "docker stop mango-user-api" -ForegroundColor Gray
    Write-Host ""
    
    # Display image details
    Write-Host "Image Details:" -ForegroundColor Yellow
    docker images $ImageName
} else {
    Write-Host ""
    Write-Host "================================================" -ForegroundColor Red
    Write-Host "❌ Docker image build failed!" -ForegroundColor Red
    Write-Host "================================================" -ForegroundColor Red
    exit 1
}


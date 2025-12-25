# PowerShell script to build and publish Docker image for Mango User Service Migrations

# Configuration
$ImageName = "mango-user-service-migrations"
$ImageTag = "latest"
$FullImageName = "${ImageName}:${ImageTag}"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Building Docker Image: $FullImageName" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan

# Build the Docker image
docker build -t $FullImageName -f Mango.UserService.Migrations/Dockerfile .

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "================================================" -ForegroundColor Green
    Write-Host "✅ Docker image built successfully!" -ForegroundColor Green
    Write-Host "================================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Image: $FullImageName" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "To run migrations, use:" -ForegroundColor White
    Write-Host "docker run --rm -e ConnectionStrings__DefaultConnection=`"Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password`" $FullImageName" -ForegroundColor Gray
    Write-Host ""
    Write-Host "To view all images:" -ForegroundColor White
    Write-Host "docker images | Select-String $ImageName" -ForegroundColor Gray
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


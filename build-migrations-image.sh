#!/bin/bash

# Build and publish the Docker image for Mango User Service Migrations

# Configuration
IMAGE_NAME="mango-user-service-migrations"
IMAGE_TAG="latest"
FULL_IMAGE_NAME="${IMAGE_NAME}:${IMAGE_TAG}"

echo "================================================"
echo "Building Docker Image: ${FULL_IMAGE_NAME}"
echo "================================================"

# Build the Docker image
docker build -t ${FULL_IMAGE_NAME} -f Mango.UserService.Migrations/Dockerfile .

if [ $? -eq 0 ]; then
    echo "================================================"
    echo "✅ Docker image built successfully!"
    echo "================================================"
    echo ""
    echo "Image: ${FULL_IMAGE_NAME}"
    echo ""
    echo "To run migrations, use:"
    echo "docker run --rm -e ConnectionStrings__DefaultConnection=\"Host=host.docker.internal;Port=5432;Database=mongo_user_service;Username=postgres;Password=your_password\" ${FULL_IMAGE_NAME}"
    echo ""
    echo "To view all images:"
    echo "docker images | grep ${IMAGE_NAME}"
else
    echo "================================================"
    echo "❌ Docker image build failed!"
    echo "================================================"
    exit 1
fi


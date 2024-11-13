#!/bin/bash

# Check for required arguments
if [ $# -ne 2 ]; then
    echo "Usage: $0 <image_name> <image_version>"
    exit 1
fi

# Variables from arguments
IMAGE_NAME="$1"
IMAGE_TAG="$2"
DOCKERFILE_PATH="Dockerfile"  # Change if Dockerfile is in a different location
BUILD_CONTEXT="."  # Change if your build context is different

# Build the Docker image
echo "Building Docker image..."
docker build -t kadirzade/mci-${IMAGE_NAME}:${IMAGE_TAG} -f ${DOCKERFILE_PATH} ${BUILD_CONTEXT}
if [ $? -ne 0 ]; then
    echo "Docker build failed."
    exit 1
fi

# Push the Docker image to Docker Hub
echo "Pushing Docker image to Docker Hub..."
docker push kadirzade/mci-${IMAGE_NAME}:${IMAGE_TAG}
if [ $? -ne 0 ]; then
    echo "Docker push failed."
    exit 1
fi

echo "Docker image ${IMAGE_NAME}:${IMAGE_TAG} pushed successfully."

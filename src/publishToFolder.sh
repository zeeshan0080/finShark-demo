#!/bin/bash

# Publish.sh - Simple publish script for Ubuntu to generate Windows-compatible deployment files

# Configuration
PROJECT_PATH="./app.csproj"
OUTPUT_PATH="../Publish/build"
RUNTIME="win-x64"
CONFIGURATION="Release"
FRAMEWORK="net9.0"

echo "Publishing App for Windows deployment..."
echo "Project: $PROJECT_PATH"
echo "Output: $OUTPUT_PATH"
echo "Runtime: $RUNTIME"
echo "Configuration: $CONFIGURATION"
echo "Framework: $FRAMEWORK"
echo ""

# Check if project file exists
if [ ! -f "$PROJECT_PATH" ]; then
    echo "Project file not found: $PROJECT_PATH"
    exit 1
fi

# Clean previous publish output
if [ -d "$OUTPUT_PATH" ]; then
    echo "🧹 Cleaning previous publish output..."
    rm -rf "$OUTPUT_PATH"
fi

# Create output directory
mkdir -p "$OUTPUT_PATH"

echo "Publishing application..."


# Publish the application for Windows
dotnet publish "$PROJECT_PATH" \
    -c "$CONFIGURATION" \
    -r "$RUNTIME" \
    -f "$FRAMEWORK" \
    --self-contained false \
    -o "$OUTPUT_PATH"

# Check if publish was successful
if [ $? -eq 0 ]; then
    echo "Publish completed successfully!"
    echo "Output directory: $OUTPUT_PATH"
    echo "Ready for Windows deployment - copy all files to your Windows VM"
else
    echo "❌ Publish failed with exit code $?"
    exit 1
fi

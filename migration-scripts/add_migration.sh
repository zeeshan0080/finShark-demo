#!/bin/bash

# Check if migration name is provided
if [ $# -eq 0 ]; then
    echo "Error: Migration name is required"
    echo "Usage: $0 <migration_name>"
    exit 1
fi

name="$1"

# Define the project path and context
project_path="../finShark-demo.Infrastructure/finShark-demo.Infrastructure.csproj"
context="ApplicationDbContext"
output_dir="Data/Migrations"

# Run the dotnet ef migrations add command
echo "Adding migration '$name'..."
dotnet ef migrations add "$name" \
    --project "$project_path" \
    --startup-project "../api/finShark-demo.csproj" \
    --context "$context" \
    --output-dir "$output_dir"

if [ $? -eq 0 ]; then
    echo "Migration '$name' added successfully."
else
    echo "Failed to add migration '$name'. Check the error message above."
    exit $?
fi

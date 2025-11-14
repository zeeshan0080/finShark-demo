#!/bin/bash

# Define the project path and context
project_path="../finShark-demo.Infrastructure/finShark-demo.Infrastructure.csproj"
context="ApplicationDbContext"

# Run the dotnet ef migrations remove command
echo "Removing last migration..."
dotnet ef migrations remove \
    --project "$project_path" \
    --startup-project "../api/finShark-demo.csproj" \
    --context "$context"

if [ $? -eq 0 ]; then
    echo "Last migration removed successfully."
else
    echo "Failed to remove last migration. Check the error message above."
    exit $?
fi




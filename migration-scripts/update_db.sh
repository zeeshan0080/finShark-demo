#!/bin/bash

# Get migration name from parameter (optional)
migration_name="$1"

# Define the project path and context
project_path="../finShark-demo.Infrastructure/finShark-demo.Infrastructure.csproj"
context="ApplicationDbContext"

# Run the dotnet ef database update command
echo "Updating database..."
if [ -n "$migration_name" ]; then
    echo "Targeting specific migration: $migration_name"
    dotnet ef database update "$migration_name" \
        --project "$project_path" \
        --startup-project "../api/finShark-demo.csproj" \
        --context "$context"
else
    dotnet ef database update \
        --project "$project_path" \
        --startup-project "../api/finShark-demo.csproj" \
        --context "$context"
fi

if [ $? -eq 0 ]; then
    echo "Database updated successfully."
else
    echo "Failed to update database. Check the error message above."
    exit $?
fi




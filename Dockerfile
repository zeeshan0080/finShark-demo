# Stage 1: Build Stage

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# restore
COPY ["src/api/finShark-demo.csproj", "src/api/"]
COPY ["src/finShark-demo.Application/finShark-demo.Application.csproj", "src/finShark-demo.Application/"]
COPY ["src/finShark-demo.Core/finShark-demo.Core.csproj", "src/finShark-demo.Core/"]
COPY ["src/finShark-demo.Infrastructure/finShark-demo.Infrastructure.csproj", "src/finShark-demo.Infrastructure/"]

COPY . .
RUN dotnet restore "src/api/finShark-demo.csproj"

#build
COPY ["src/api", "src/api/"]
RUN dotnet build "src/api/finShark-demo.csproj" -c release -o /app/build

# Sate 2: Publish Stage

FROM build AS publish
RUN dotnet publish "src/api/finShark-demo.csproj"  -c release -o /app/publish 

# Stage 3: Run Stage 

FROM mcr.microsoft.com/dotnet/aspnet:8.0
#ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "finShark-demo.dll"] 
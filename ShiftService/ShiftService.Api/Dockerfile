# Stage 1: Base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем все csproj для восстановления зависимостей
COPY ["ShiftService/ShiftService.Api/ShiftService.Api.csproj", "ShiftService/ShiftService.Api/"]
COPY ["ShiftService/ShiftService.Application/ShiftService.Application.csproj", "ShiftService/ShiftService.Application/"]
COPY ["ShiftService/ShiftService.Domain/ShiftService.Domain.csproj", "ShiftService/ShiftService.Domain/"]
COPY ["ShiftService/ShiftService.Infrastructure/ShiftService.Infrastructure.csproj", "ShiftService/ShiftService.Infrastructure/"]

# Восстанавливаем зависимости
RUN dotnet restore "./ShiftService/ShiftService.Api/ShiftService.Api.csproj"

# Копируем весь код
COPY . .

WORKDIR "/src/ShiftService/ShiftService.Api"
RUN dotnet build "./ShiftService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "./ShiftService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShiftService.Api.dll"]
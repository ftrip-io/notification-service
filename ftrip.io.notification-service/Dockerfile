FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /build

ARG CONFIGURATION=Release

# Restore dependencies
COPY ["ftrip.io.notification-service.contracts/*.csproj", "ftrip.io.notification-service.contracts/package*.json", "./ftrip.io.notification-service.contracts/"]
RUN dotnet restore /p:Configuration=$CONFIGURATION ftrip.io.notification-service.contracts/ftrip.io.notification-service.contracts.csproj

COPY ["ftrip.io.notification-service/*.csproj", "ftrip.io.notification-service/package*.json", "./ftrip.io.notification-service/"]
RUN dotnet restore /p:Configuration=$CONFIGURATION ftrip.io.notification-service/ftrip.io.notification-service.csproj

# Publish
COPY . .

RUN dotnet publish -c $CONFIGURATION -o out ftrip.io.notification-service/ftrip.io.notification-service.csproj
 
# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS final
WORKDIR /app

RUN addgroup --system ftrip-io
RUN adduser --system notification-service

COPY --from=build --chown=notification-service:ftrip-io --chmod=777 /build/out .

EXPOSE 80

USER notification-service

ENTRYPOINT ["dotnet", "ftrip.io.notification-service.dll"]
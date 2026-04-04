# Use .NET 9 instead of 8
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# FIX: Point to the folder where the .csproj actually lives
COPY ["ProjectWasel/ProjectWasel.csproj", "ProjectWasel/"]

# FIX: Restore from the correct path
RUN dotnet restore "ProjectWasel/ProjectWasel.csproj"

COPY . .
WORKDIR "/src/ProjectWasel"
RUN dotnet build "ProjectWasel.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProjectWasel.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectWasel.dll"]
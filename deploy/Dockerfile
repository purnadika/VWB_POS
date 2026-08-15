# Multi-stage build for production
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["src/POS.Domain/POS.Domain.csproj", "src/POS.Domain/"]
COPY ["src/POS.Application/POS.Application.csproj", "src/POS.Application/"]
COPY ["src/POS.Infrastructure/POS.Infrastructure.csproj", "src/POS.Infrastructure/"]
COPY ["src/POS.WebAPI/POS.WebAPI.csproj", "src/POS.WebAPI/"]
RUN dotnet restore "src/POS.WebAPI/POS.WebAPI.csproj"

# Copy the full source and build
COPY . .
WORKDIR "/src/src/POS.WebAPI"
RUN dotnet build "POS.WebAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "POS.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "POS.WebAPI.dll"]

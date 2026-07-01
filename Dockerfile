# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar arquivos de projeto e restaurar dependências
COPY src/TalentBridge.Domain/TalentBridge.Domain.csproj src/TalentBridge.Domain/
COPY src/TalentBridge.Application/TalentBridge.Application.csproj src/TalentBridge.Application/
COPY src/TalentBridge.Infrastructure/TalentBridge.Infrastructure.csproj src/TalentBridge.Infrastructure/
COPY src/TalentBridge.Api/TalentBridge.Api.csproj src/TalentBridge.Api/
COPY db/ db/

RUN dotnet restore src/TalentBridge.Api/TalentBridge.Api.csproj

# Copiar código fonte e compilar
COPY src/ src/
RUN dotnet publish src/TalentBridge.Api/TalentBridge.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar scripts de migration
COPY db/migrations/ /app/db/migrations/

# Copiar build
COPY --from=build /app/publish .

# Expor porta
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TalentBridge.Api.dll"]
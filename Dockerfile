# Imagen base de .NET Core sin Alpine
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y openssl ca-certificates
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["API.TACO.csproj", "./"]
RUN dotnet restore "./API.TACO.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "API.TACO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.TACO.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Variables de entorno
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=https://+:4131;http://+:4130
ENV Kestrel__Certificates__Default__Path=/https/aspnetcore.pfx
ENV Kestrel__Certificates__Default__Password=sC7xSn5yG3Zl0Wc

ENTRYPOINT ["dotnet", "API.TACO.dll"]
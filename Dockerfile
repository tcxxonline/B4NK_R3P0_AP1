# .NET 11 SDK ile projeyi derle
FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
WORKDIR /src
COPY ["BankServerApi.csproj", "./"]
RUN dotnet restore "BankServerApi.csproj"
COPY . .
RUN dotnet publish "BankServerApi.csproj" -c Release -o /app/publish

# Çalıştırma ortamı
FROM mcr.microsoft.com/dotnet/aspnet:11.0 AS base
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BankServerApi.dll"]

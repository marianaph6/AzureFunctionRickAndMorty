# Base image for Azure Functions
FROM mcr.microsoft.com/azure-functions/dotnet:4.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

# Build image with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RickAndMortyFunction.csproj", "src/"]
RUN dotnet restore "src/RickAndMortyFunction.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "RickAndMortyFunction.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RickAndMortyFunction.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage with Azure Functions runtime
FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

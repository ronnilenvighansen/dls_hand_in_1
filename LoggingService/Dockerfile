FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.sln ./
COPY LoggingService/LoggingService.csproj ./LoggingService/

RUN dotnet restore LoggingService/LoggingService.csproj

COPY LoggingService/. ./LoggingService/

RUN dotnet publish LoggingService/LoggingService.csproj -c Release -o /app/out/LoggingService

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/out/LoggingService ./LoggingService

EXPOSE 80

ENTRYPOINT ["dotnet", "LoggingService/LoggingService.dll"]

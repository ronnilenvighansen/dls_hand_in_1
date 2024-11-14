FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.sln ./
COPY TracingService/TracingService.csproj ./TracingService/

RUN dotnet restore TracingService/TracingService.csproj

COPY TracingService/. ./TracingService/

RUN dotnet publish TracingService/TracingService.csproj -c Release -o /app/out/TracingService

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/out/TracingService ./TracingService

EXPOSE 80

ENTRYPOINT ["dotnet", "TracingService/TracingService.dll"]

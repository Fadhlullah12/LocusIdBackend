FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY LocusIDBackend.csproj ./
RUN dotnet restore LocusIDBackend.csproj

COPY . ./
RUN dotnet publish LocusIDBackend.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish ./
USER $APP_UID

ENTRYPOINT ["dotnet", "LocusIDBackend.dll"]
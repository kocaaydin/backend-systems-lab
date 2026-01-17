FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MicroserviceLab.GatewayApi.csproj", "."]
RUN dotnet restore "./MicroserviceLab.GatewayApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MicroserviceLab.GatewayApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceLab.GatewayApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceLab.GatewayApi.dll"]

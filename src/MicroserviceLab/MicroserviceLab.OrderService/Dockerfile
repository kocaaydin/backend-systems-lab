FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MicroserviceLab.OrderService.csproj", "."]
RUN dotnet restore "./MicroserviceLab.OrderService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MicroserviceLab.OrderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceLab.OrderService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceLab.OrderService.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RequestLimitsCheck.Net8.csproj", "./"]
RUN dotnet restore "RequestLimitsCheck.Net8.csproj"
COPY . .
RUN dotnet publish "RequestLimitsCheck.Net8.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RequestLimitsCheck.Net8.dll"]

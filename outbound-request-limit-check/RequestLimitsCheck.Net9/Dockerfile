FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RequestLimitsCheck.Net9.csproj", "./"]
RUN dotnet restore "RequestLimitsCheck.Net9.csproj"
COPY . .
RUN dotnet publish "RequestLimitsCheck.Net9.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RequestLimitsCheck.Net9.dll"]

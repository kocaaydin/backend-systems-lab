FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RequestLimitsCheck.Net10.csproj", "./"]
RUN dotnet restore "RequestLimitsCheck.Net10.csproj"
COPY . .
RUN dotnet publish "RequestLimitsCheck.Net10.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RequestLimitsCheck.Net10.dll"]

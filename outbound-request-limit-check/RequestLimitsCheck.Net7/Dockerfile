FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RequestLimitsCheck.Net7.csproj", "./"]
RUN dotnet restore "RequestLimitsCheck.Net7.csproj"
COPY . .
RUN dotnet publish "RequestLimitsCheck.Net7.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RequestLimitsCheck.Net7.dll"]

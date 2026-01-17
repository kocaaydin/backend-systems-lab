FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TargetApi.csproj", "./"]
RUN dotnet restore "TargetApi.csproj"
COPY . .
RUN dotnet publish "TargetApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y curl
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TargetApi.dll"]

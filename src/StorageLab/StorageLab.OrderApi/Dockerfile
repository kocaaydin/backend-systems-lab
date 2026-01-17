FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StorageLab.OrderApi.csproj", "."]
RUN dotnet restore "./StorageLab.OrderApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "StorageLab.OrderApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StorageLab.OrderApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StorageLab.OrderApi.dll"]

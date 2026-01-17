FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ResilienceLab.Api.csproj", "."]
RUN dotnet restore "./ResilienceLab.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ResilienceLab.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ResilienceLab.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResilienceLab.Api.dll"]

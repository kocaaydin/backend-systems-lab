FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY K6TargetApi.csproj ./
RUN dotnet restore K6TargetApi.csproj
COPY . .
RUN dotnet publish K6TargetApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5080
ENTRYPOINT ["dotnet", "K6TargetApi.dll"]

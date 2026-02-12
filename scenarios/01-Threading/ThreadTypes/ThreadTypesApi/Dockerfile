FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ThreadTypesApi.csproj ./
RUN dotnet restore ThreadTypesApi.csproj
COPY . .
RUN dotnet publish ThreadTypesApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8091
ENTRYPOINT ["dotnet", "ThreadTypesApi.dll"]

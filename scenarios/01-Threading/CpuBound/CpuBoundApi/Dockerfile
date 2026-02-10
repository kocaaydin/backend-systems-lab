FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY CpuBoundApi.csproj ./
RUN dotnet restore CpuBoundApi.csproj
COPY . .
RUN dotnet publish CpuBoundApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8085
ENTRYPOINT ["dotnet", "CpuBoundApi.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MicroserviceLab.Consumer.csproj", "."]
RUN dotnet restore "./MicroserviceLab.Consumer.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MicroserviceLab.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceLab.Consumer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceLab.Consumer.dll"]

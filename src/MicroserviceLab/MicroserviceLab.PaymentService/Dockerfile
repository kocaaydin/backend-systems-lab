FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MicroserviceLab.PaymentService.csproj", "."]
RUN dotnet restore "./MicroserviceLab.PaymentService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MicroserviceLab.PaymentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceLab.PaymentService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceLab.PaymentService.dll"]

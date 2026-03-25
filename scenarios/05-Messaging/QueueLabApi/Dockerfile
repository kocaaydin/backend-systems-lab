FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Proje dosyasini kopyala ve restore et
COPY ["QueueLabApi.csproj", "./"]
RUN dotnet restore "QueueLabApi.csproj"

# Kodlari kopyala ve build al
COPY . .
RUN dotnet publish "QueueLabApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Container icinde URL bind ayari
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "QueueLabApi.dll"]
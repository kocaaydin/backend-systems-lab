FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Generate a self-signed cert for local HTTPS/h2 benchmark traffic.
RUN apt-get update \
    && apt-get install -y --no-install-recommends openssl \
    && mkdir -p /https \
    && openssl req -x509 -newkey rsa:2048 -sha256 -nodes \
       -keyout /https/tls.key \
       -out /https/tls.crt \
       -subj "/CN=localhost" \
       -days 365 \
    && openssl pkcs12 -export \
       -out /https/tls.pfx \
       -inkey /https/tls.key \
       -in /https/tls.crt \
       -passout pass:changeit \
    && rm -rf /var/lib/apt/lists/*

# Expose ports
EXPOSE 5001
EXPOSE 5002
EXPOSE 5003

ENTRYPOINT ["dotnet", "ConnectionManagement.Api.dll"]

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ThreadingPlayground.csproj ./
RUN dotnet restore ThreadingPlayground.csproj

COPY . ./
RUN dotnet publish ThreadingPlayground.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "ThreadingPlayground.dll"]

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /src
COPY ["Foosball.CSharp.API/Foosball.CSharp.API.csproj", "Foosball.CSharp.API/"]
RUN dotnet restore "Foosball.CSharp.API/Foosball.CSharp.API.csproj"

# Copy everything
COPY . .

# Build and publish a release
RUN dotnet publish "Foosball.CSharp.API/Foosball.CSharp.API.csproj" -c Release -o /app -p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app .

ENV DOTNET_EnableDiagnostics=0 

ENTRYPOINT ["dotnet", "Foosball.CSharp.API.dll"]
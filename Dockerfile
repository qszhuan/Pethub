FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_14.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app

# Copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_14.x | bash \
    && apt-get install nodejs -yq
COPY ["Pethub/Pethub.csproj", "Pethub/"] 
RUN dotnet restore "Pethub/Pethub.csproj"
COPY . .
WORKDIR "/src/Pethub"
RUN dotnet build "Pethub.csproj" -c Release -o /app/build

# Copy everything else and build
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pethub.dll"]
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
WORKDIR /src
COPY Palantir.sln ./
COPY Palantir.IO/*.csproj ./Palantir.IO/
COPY Palantir.WebApi/*.csproj ./Palantir.WebApi/
COPY Palantir.Domain/*.csproj ./Palantir.Domain/
COPY Palantir.Exceptions/*.csproj ./Palantir.Exceptions/
COPY Palantir.GitHub/*.csproj ./Palantir.GitHub/
COPY Palantir.GitLab/*.csproj ./Palantir.GitLab/
COPY Palantir.HostedRedmine/*.csproj ./Palantir.HostedRedmine/
COPY Palantir.Redmine/*.csproj ./Palantir.Redmine/
COPY Palantir.TravisCI/*.csproj ./Palantir.TravisCI/

RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Palantir.WebApi.dll"]

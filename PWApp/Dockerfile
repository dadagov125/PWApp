FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

RUN apt-get -qq update && apt-get -qqy --no-install-recommends install wget gnupg \
    git \
    unzip

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY PWApp/PWApp.csproj PWApp/
RUN dotnet restore PWApp/PWApp.csproj
COPY . .
WORKDIR /src/PWApp
RUN dotnet build PWApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PWApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PWApp.dll"]

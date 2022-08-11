#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine AS base
WORKDIR /app

# Install Timezone data
RUN apk add --update tzdata

# Workaround Unhandled exception. System.NotSupportedException: Globalization Invariant Mode is not supported.
# https://github.com/dotnet/SqlClient/issues/220
RUN apk add --update icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

# https://github.com/dotnet/dotnet-docker/blob/master/documentation/scenarios/nuget-credentials.md#using-the-azure-artifact-credential-provider
RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh  | sh

ARG FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://neolution.pkgs.visualstudio.com/_packaging/neolution/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${FEED_ACCESSTOKEN}\"}]}"

WORKDIR /src
COPY ["Neolution.Extensions.DataSeeding.Sample/Neolution.Extensions.DataSeeding.Sample.csproj", "Neolution.Extensions.DataSeeding.Sample/packages.lock.json", "Neolution.Extensions.DataSeeding.Sample/"]
COPY ./nuget.config .

# restore nuget
RUN dotnet restore --locked-mode "Neolution.Extensions.DataSeeding.Sample/Neolution.Extensions.DataSeeding.Sample.csproj"

COPY . .
WORKDIR "/src/Neolution.Extensions.DataSeeding.Sample"
RUN dotnet build "Neolution.Extensions.DataSeeding.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neolution.Extensions.DataSeeding.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neolution.Extensions.DataSeeding.Sample.dll"]
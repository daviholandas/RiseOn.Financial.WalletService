ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:7.0
ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:7.0

FROM ${DOTNET_RUNTIME} AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9580
EXPOSE 9580

FROM ${DOTNET_SDK} AS build
WORKDIR /src
RUN dotnet nuget add source https://pkgs.dev.azure.com/RiseOn/_packaging/RiseOn/nuget/v3/index.json -n riseon -u davi_holandas@hotmail.com -p dyrdquaom43lowld4ek77auobnjcsvvpy26okrsxfjmyrbj3p2aq --store-password-in-clear-text
COPY ["./src/", "."]
RUN dotnet restore "WalletService.WebApi/WalletService.WebApi.csproj"
COPY . .

RUN dotnet build "WalletService.WebApi/WalletService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WalletService.WebApi/WalletService.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WalletService.WebApi.dll"]

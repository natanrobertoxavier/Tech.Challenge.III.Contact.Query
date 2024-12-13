FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /main
COPY ["Contact.Query.Api/Contact.Query.Api.csproj", "Contact.Query.Api/"]
RUN dotnet restore "Contact.Query.Api/Contact.Query.Api.csproj"
COPY . .

RUN dotnet build "Contact.Query.Api/Contact.Query.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contact.Query.Api/Contact.Query.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contact.Query.Api.dll"]

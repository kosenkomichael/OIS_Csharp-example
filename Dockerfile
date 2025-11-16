FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8181

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ExampleWebService/ExampleWebService.csproj", "ExampleWebService/"]
RUN dotnet restore "ExampleWebService/ExampleWebService.csproj"

COPY . .
WORKDIR "/src/ExampleWebService"
RUN dotnet publish "ExampleWebService.csproj" -c Release -r linux-x64 --self-contained false -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ExampleWebService.dll", "--urls", "http://0.0.0.0:8181"]

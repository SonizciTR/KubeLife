FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY [".", "."]

RUN pwd
RUN ls -al

WORKDIR /src/KubeLife.BlazorApp
RUN dotnet restore "KubeLife.BlazorApp.csproj"
COPY . .
RUN dotnet build "KubeLife.BlazorApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KubeLife.BlazorApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KubeLife.BlazorApp.dll"]

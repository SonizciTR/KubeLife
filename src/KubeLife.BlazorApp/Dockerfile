FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN pwd
RUN ls -al

WORKDIR /src
RUN pwd
RUN ls -al
COPY . /src
#COPY ["KubeLife.BlazorApp/KubeLife.BlazorApp.csproj", "KubeLife.BlazorApp/"]
RUN dotnet restore "KubeLife.BlazorApp/KubeLife.BlazorApp.csproj"
#COPY . .
WORKDIR "/src/KubeLife.BlazorApp"
RUN dotnet build "KubeLife.BlazorApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KubeLife.BlazorApp.csproj" -c Release -o /app/publish /p:UseAppHost=false



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KubeLife.BlazorApp.dll"]
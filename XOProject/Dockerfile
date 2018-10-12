FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["XO-Project/XO-Project.csproj", "XO-Project/"]
RUN dotnet restore "XO-Project/XO-Project.csproj"
COPY . .
WORKDIR "/src/XO-Project"
RUN dotnet build "XO-Project.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "XO-Project.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "XO-Project.dll"]
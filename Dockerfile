#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LoginMicroservice.Api/LoginMicroservice.Api.csproj", "LoginMicroservice.Api/"]
RUN dotnet restore "LoginMicroservice.Api/LoginMicroservice.Api.csproj"
COPY . .
WORKDIR "/src/LoginMicroservice.Api"
RUN dotnet build "LoginMicroservice.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoginMicroservice.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoginMicroservice.Api.dll"]
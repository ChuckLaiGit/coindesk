#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8888

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CoindeskApi/CoindeskApi.csproj", "CoindeskApi/"]
COPY ["Application/CoindeskApi.Application.csproj", "Application/"]
COPY ["Contract/CoindeskApi.Contract.csproj", "Contract/"]
COPY ["Infrastructure/CoindeskApi.Infrastructure.csproj", "Infrastructure/"]
COPY ["Db/CoindeskApi.Db.csproj", "Db/"]
COPY ["Share/CoindeskApi.Share.csproj", "Share/"]
RUN dotnet restore "CoindeskApi/CoindeskApi.csproj"
COPY . .
WORKDIR "/src/CoindeskApi"
RUN dotnet build "CoindeskApi.csproj" -c Release -o /app/build


FROM build AS publish
#set to production environment
ENV ASPNETCORE_ENVIRONMENT Development
RUN dotnet publish "CoindeskApi.csproj" -c Release -o /app/publish /p:UseAppHost=false



# Build Final Image
FROM base AS final
# Copy coinDesk App Server to image
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CoindeskApi.dll"]

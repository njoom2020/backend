FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ISPN/WebApplication1.csproj ISPN/
COPY ecommmerceApi/userApi.csproj ecommmerceApi/
COPY ecommmerceDataApi/userDataApi.csproj ecommmerceDataApi/
COPY Shared/Shared.csproj Shared/
RUN dotnet restore ISPN/WebApplication1.csproj

COPY . .
RUN dotnet publish ISPN/WebApplication1.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

CMD ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080} exec dotnet WebApplication1.dll"]

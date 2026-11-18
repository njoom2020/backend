# مرحلة البناء
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# نسخ ملف الـ csproj بناءً على مسار مشروعك الظاهر في الصورة
COPY ["WebApplication1.csproj", "./"]
RUN dotnet restore "WebApplication1.csproj"

COPY . .
RUN dotnet build "WebApplication1.csproj" -c Release -o /app/build

# مرحلة النشر
FROM build AS publish
RUN dotnet publish "WebApplication1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# مرحلة التشغيل النهائية
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApplication1.dll"]

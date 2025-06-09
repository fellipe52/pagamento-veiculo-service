# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pagamento.API/Pagamento.API.csproj", "Pagamento.API/"]
COPY ["Pagamento.Application/Pagamento.Application.csproj", "Pagamento.Application/"]
COPY ["Pagamento.Domain/Pagamento.Domain.csproj", "Pagamento.Domain/"]
COPY ["Pagamento.Infrastructure/Pagamento.Infrastructure.csproj", "Pagamento.Infrastructure/"]
RUN dotnet restore "Pagamento.API/Pagamento.API.csproj"
# Restaure as dependências
RUN dotnet restore "./Pagamento.API/Pagamento.API.csproj"

# Copiar o restante dos arquivos para o contêiner
COPY . .

# Verifique os arquivos dentro do contêiner (opcional, para depuração)
RUN ls -al /src

# Ajuste o diretório de trabalho para o diretório da API e execute o build
WORKDIR "/src/Pagamento.API"
RUN dotnet build "./Pagamento.API.csproj" -c $BUILD_CONFIGURATION -o /app/build -v detailed

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pagamento.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pagamento.API.dll"]

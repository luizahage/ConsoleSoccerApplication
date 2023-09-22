# ConsoleSoccerApplication

Este é um projeto de aplicativo de console desenvolvido com o objetivo de aprendizado e consolidação de conceitos. O projeto é relacionado à coleta e exibição de dados de competições, times e jogadores de futebol de uma API, bem como ao acesso a dados de um banco de dados usando SQL Server.

## Índice
O projeto se concentra em vários tópicos essenciais, incluindo:
### 1. [Funcionalidades Principais](#funcionalidades-principais)
### 2. [Próximos Passos](#proximos-passos)
### 3. [Instruções de Uso](#instrucoes-de-uso)
### 4. [Configuração do appsettings.json](#configuração-do-appsettingsjson)
### 5. [Dependências](#dependencias)
### 6. [Aviso](#aviso)
### 7. [Contato](#contato)

## Funcionalidades Principais:
### 1. Coleta de Dados da API: 
A aplicação utiliza o HttpClient para fazer requisições a uma API que fornece dados de competições, times e jogadores de futebol. Em alguns casos as respostas da API são processadas e os dados são armazenados localmente para uso posterior, e em outros casos os dados são coletados direto da API.

### 2. Interação com o Usuário: 
A aplicação interage com o usuário por meio de perguntas exibidas no console. As respostas do usuário guiam a exibição dos dados armazenados de acordo com as escolhas feitas.

### 3. Acesso a Banco de Dados: 
O projeto também envolve a interação com um banco de dados SQL Server. É utilizado o pacote Microsoft.Data.SqlClient para acessar dados usando SqlConnection, SqlCommand e SqlDataReader. Consultas SQL são realizadas para operações de seleção, deleção, inserção, entre outras.

### 4. Utilização de API de Tradução: 
A aplicação utiliza a API de Tradução da Microsoft Azure para mostrar a capacidade de integração com serviços externos.

### 5. Conceitos Importantes: 
Durante o desenvolvimento, são abordados conceitos como DTOs (Data Transfer Objects), Enums, Exceções e organização de arquivos para melhor compreensão do código.

## Próximos Passos:
No futuro, o projeto está planejado para evoluir com os seguintes passos:

### 1. Implementação do Dapper: 
As consultas ao banco de dados serão reimplementadas usando o Dapper, um micro ORM para .NET. Isso proporcionará uma compreensão aprofundada de como os ORMs funcionam por baixo dos panos.

### 2. Implementação do Entity Framework Core: 
Posteriormente, o Entity Framework Core será adotado, subsituindo o Dapper citado anteriormente, para explorar a implementação de um ORM mais complexo.

## Instruções de Uso:
1. Clone o repositório para sua máquina local.
2. Execute os scripts SQL localizados na pasta [ConsoleSoccerApplication/ScriptsSQL](https://github.com/luizahage/ConsoleSoccerApplication/tree/main/ScriptsSQL) para criar o banco de dados e as tabelas necessárias.
3. Lembre de personalizar no projeto todas as informações necessárias para que atenda as necessidades e detalhes específicos da sua máquina local, e conforme foi orietando nesse README.
4. Compile e execute o aplicativo de console.
5. Responda às perguntas no console para visualizar os dados de acordo com suas escolhas.
6. Explore o código fonte para entender as implementações dos diferentes conceitos.

## Configuração do appsettings.json:

O arquivo `appsettings.json` contém informações sensíveis como a connection string, o token da API e a chave da acesso a API de tradução do Azure. Devido a questões de segurança, este arquivo está listado no `.gitignore`. Você deve criar esse arquivo manualmente na raiz do projeto e preenchê-lo conforme o exemplo abaixo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "sua_connection_string"
  },
  "AppSettings": {
    "AuthToken": "seu_token_da_API"
  },
  "Azure": {
    "TranslatorApiKey": "sua_chave_de_acesso_da_API_Traducao_Azure"
  }
}
```

## Dependências:
- API football-data.org (API que fornece os dados de futebol - [https://www.football-data.org/](https://www.football-data.org/))
- Pacote Microsoft.Data.SqlClient
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Json
- Pacote System.Net.Http
- Pacote NPOI
- Microsoft.Azure.CognitiveServices.Translator.Text (API de Tradução)
- Outras dependências mencionadas nos requisitos do projeto

## Aviso:
Este projeto foi desenvolvido com o propósito de aprendizado e não tem a intenção de ser uma aplicação de produção completa. Algumas práticas podem não ser as ideais para um ambiente de produção real.

## Contato:
Se tiver dúvidas ou sugestões sobre o projeto, sinta-se à vontade para entrar em contato via [linkedIn](https://www.linkedin.com/in/luiza-teixeira-hage-firme/).

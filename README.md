# 💸 FinWiseAPI

Uma API RESTful desenvolvida com ASP.NET Core para controle financeiro pessoal, utilizando boas práticas de arquitetura em camadas e autenticação JWT. O projeto tem foco em organização de finanças, controle de entradas/saídas e categorização de transações.

---

## 🧱 Stack utilizada

- **.NET 9** (Preview)
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **AutoMapper**
- **JWT (JSON Web Token)**
- **Swagger (OpenAPI)**
- **DBeaver** (como client de banco de dados)

---

## 📁 Estrutura de Pastas

FinWiseAPI/
│
├── Business/ # Regras de negócio (Services)
├── DataAccess/ # Repositórios, contexto do EF Core
├── DTOs/ # Request e Response models
├── Models/ # Models do domínio
├── Resources/
│ ├── CommonResource.cs
│ └── ErrorResource.cs
│
├── WebAPI/ # Projeto principal da API
│ ├── Controllers/
│ ├── Configurations/ # AutoMapper, Swagger, DependencyInjection
│ ├── Extensions/
│ ├── appsettings.json
│ └── Program.cs

---

## 🚀 Funcionalidades (em desenvolvimento)

- [x] Estrutura da solução baseada em boas práticas
- [x] Configuração de AutoMapper
- [x] Configuração de injeção de dependência
- [x] Autenticação com JWT
- [ ] CRUD de transações
- [ ] Filtros por categoria/data
- [ ] Relatórios financeiros simples

---

## 🛠️ Como rodar o projeto localmente

1. Clone o repositório:

git clone https://github.com/YanPedro18/FinWiseAPI.git

Acesse a pasta do projeto:
cd FinWiseAPI

Crie e configure o banco de dados PostgreSQL:

Use o DBeaver ou outro client

Altere a connection string no appsettings.Development.json

Execute as migrations (caso configurado):
dotnet ef database update

Execute a aplicação:
dotnet run --project WebAPI

Acesse o Swagger:
https://localhost:{porta}/swagger

🔐 Autenticação
A API utiliza JWT para autenticação.

Os endpoints protegidos exigem envio do token via Authorization: Bearer {token}.

Será disponibilizado um endpoint de login/register em breve.

📌 Objetivo do projeto
Este projeto é parte do meu portfólio e aprendizado contínuo com ASP.NET Core, boas práticas de arquitetura, clean code, e segurança de APIs. A ideia é crescer a aplicação aos poucos, evoluindo a stack conforme a necessidade (ex: versionamento, testes, CI/CD, cache, etc).

✍️ Autor
Yan Bandeira
🔗 LinkedIn - YanPedro18

⭐ Se curtir a ideia, não esquece de dar uma star no repositório!

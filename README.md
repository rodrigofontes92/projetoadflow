# Projeto AdFlow

AdFlow é uma aplicação web desenvolvida para gestão de campanhas de marketing digital, com foco em organização de fluxos de trabalho, definição de prioridades e acompanhamento de desempenho.

O sistema foi projetado para auxiliar equipas e profissionais a manter controlo sobre campanhas, melhorar a produtividade e apoiar decisões baseadas em dados.

---

## Objetivo

Fornecer uma plataforma simples e eficiente para gerir campanhas de marketing digital, permitindo acompanhar o ciclo completo desde a criação até à análise de resultados.

---

## Tecnologias Utilizadas

* ASP.NET Core (MVC)
* Entity Framework Core
* SQL Server
* ASP.NET Identity (autenticação e autorização)
* Bootstrap (interface)
* C#

---

## Funcionalidades

* Gestão de campanhas
* Definição de prioridades por gestor de fluxo
* Sistema de utilizadores com permissões (roles)
* Acompanhamento de desempenho
* Organização de fluxos de trabalho
* Sistema de autenticação e autorização (Identity)

---

## Arquitetura

O projeto segue uma arquitetura baseada em:

* Separação por camadas (Model, View, Controller)
* Utilização do Entity Framework para persistência de dados
* Identity para gestão de utilizadores e roles

---

## Gestão de Utilizadores

O sistema utiliza ASP.NET Identity com:

* `UserManager` para gestão de utilizadores
* `RoleManager` para gestão de permissões
* Classe `ApplicationUser` personalizada

---

## Regras de Negócio

* A prioridade das campanhas é definida exclusivamente pelo gestor de fluxo
* Campanhas criadas ficam pendentes até definição de prioridade
* O sistema garante controlo de acessos por tipo de utilizador

---

## Como Executar o Projeto

1. Clonar o repositório:

```bash
git clone https://github.com/seu-usuario/adflow.git
```

2. Configurar a base de dados no `appsettings.json`

3. Executar as migrations:

```bash
update-database
```

4. Iniciar o projeto:

```bash
dotnet run
```

## Autor

Rodrigo Fontes
Desenvolvedor de Software | Especialista em Marketing Digital

---

## Licença

Este projeto é para fins educacionais.


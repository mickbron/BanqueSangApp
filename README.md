# Système de gestion de banque de sang

Application web développée avec Angular et ASP.NET Core permettant de gérer une banque de sang.

## Technologies utilisées

- Backend : ASP.NET Core .NET
- Frontend : Angular
- Base de données : MySQL
- Accès aux données : Dapper
- Architecture : Clean Architecture

## Structure du projet

```text
BanqueSangApp
├── backend
│   ├── BanqueSang.API
│   ├── BanqueSang.Core
│   └── BanqueSang.Infrastructure
├── frontend
│   └── banque-sang-ui
├── database
└── docs
```


## Lancement du backend

```bash
cd backend/BanqueSang
dotnet restore
dotnet run --project BanqueSang.API
```

## Lancement du frontend

```bash
cd frontend/banque-sang-ui
npm install
ng serve
```
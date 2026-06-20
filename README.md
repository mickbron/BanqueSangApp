# Système de gestion de banque de sang

## Présentation

Ce projet est une application web de gestion de banque de sang.

L’application permet de suivre le cycle complet d’une poche de sang :

1. enregistrement d’un donneur ;
2. vérification de son éligibilité ;
3. enregistrement d’un don ;
4. création automatique d’une poche de sang ;
5. réalisation des tests biologiques ;
6. validation ou rejet de la poche ;
7. gestion du stock ;
8. gestion des patients ;
9. création et suivi des demandes de sang ;
10. affichage des statistiques dans un tableau de bord.

Le projet est développé avec un backend **ASP.NET Core Web API**, une base de données **MySQL**, et un frontend **Angular**.

---

## Technologies utilisées

### Backend

* C#
* ASP.NET Core Web API
* Dapper
* MySQL
* JWT Bearer Authentication
* BCrypt
* Swagger

### Frontend

* Angular
* TypeScript
* HTML
* CSS
* Guards Angular
* Interceptor HTTP JWT

### Outils

* JetBrains Rider
* Postman
* Git
* GitHub
* MySQL Workbench

---

## Structure du projet

```text
BanqueSangApp/
├── backend/
│   └── BanqueSang/
│       ├── BanqueSang.API/
│       ├── BanqueSang.Core/
│       ├── BanqueSang.Infrastructure/
│       └── BanqueSang.sln
│
├── frontend/
│   └── banque-sang-ui/
│       └── src/app/
│           ├── core/
│           │   ├── guards/
│           │   ├── interceptors/
│           │   └── services/
│           ├── features/
│           │   ├── auth/
│           │   ├── dashboard/
│           │   ├── donneurs/
│           │   ├── dons/
│           │   ├── stock/
│           │   ├── tests/
│           │   ├── patients/
│           │   └── demandes-sang/
│           └── shared/
│               ├── components/
│               └── models/
│
├── database/
│   └── BanqueSang.sql
│
└── README.md
```

---

## Fonctionnalités principales

### Authentification

* Connexion sécurisée avec identifiant et mot de passe.
* Génération d’un token JWT.
* Stockage du token, du rôle et de l’identifiant du personnel connecté dans le navigateur.
* Déconnexion.

### Gestion des donneurs

* Afficher la liste des donneurs.
* Ajouter un nouveau donneur.
* Modifier un donneur.
* Vérifier l’éligibilité d’un donneur.
* Contrôler l’âge et la date du dernier don.

### Gestion des dons

* Enregistrer un don.
* Associer un don à un donneur.
* Créer automatiquement une poche de sang après un don.
* Mettre le don en statut `EN_ATTENTE`.

### Gestion du stock

* Afficher toutes les poches de sang.
* Afficher les poches disponibles et indisponibles.
* Afficher les poches proches de la péremption.
* Afficher le stock par groupe sanguin.

### Tests biologiques

* Afficher les types de tests biologiques.
* Enregistrer un résultat de test.
* Modifier un résultat de test.
* Valider automatiquement un don si tous les tests sont négatifs.
* Rejeter automatiquement un don si un test est positif.
* Rendre une poche disponible ou indisponible selon les résultats.

### Gestion des patients

* Afficher la liste des patients.
* Ajouter un patient.
* Modifier un patient.
* Associer un patient à un service.

### Demandes de sang

* Afficher les demandes de sang.
* Créer une demande de sang.
* Associer une demande à un patient et au personnel connecté.
* Choisir le type de composant demandé.
* Indiquer l’urgence de la demande.
* Modifier le statut d’une demande.

### Tableau de bord

* Afficher les statistiques globales :

    * nombre de donneurs ;
    * nombre de dons ;
    * nombre de poches ;
    * nombre de poches disponibles ;
    * nombre de patients ;
    * nombre de demandes ;
    * nombre de demandes en attente ;
    * nombre de tests positifs ;
    * nombre de tests négatifs.

---

## Rôles utilisateurs

L’application utilise trois rôles principaux.

| Rôle           | Accès                                                   |
| -------------- | ------------------------------------------------------- |
| Administrateur | Accès complet à toutes les fonctionnalités              |
| Technicien     | Donneurs, dons, tests biologiques, stock, dashboard     |
| Médecin        | Tests biologiques, stock, patients, demandes, dashboard |

---

## Comptes de test

Les comptes suivants peuvent être utilisés pour tester l’application.

| Rôle           | Identifiant  | Mot de passe   |
| -------------- | ------------ | -------------- |
| Administrateur | `admin`      | `Password123!` |
| Technicien     | `technicien` | `Password123!` |
| Médecin        | `medecin`    | `Password123!` |

> Remarque : les mots de passe sont stockés sous forme hachée avec BCrypt dans la base de données.

---

## Installation du backend

Se placer dans le dossier backend :

```bash
cd backend/BanqueSang
```

Restaurer les dépendances :

```bash
dotnet restore
```

Lancer l’API :

```bash
dotnet run --project BanqueSang.API
```

L’API démarre généralement sur :

```text
http://localhost:5180
```

Swagger est disponible à l’adresse :

```text
http://localhost:5180/swagger
```

---

## Configuration de la base de données

Créer une base de données MySQL :

```sql
CREATE DATABASE banque_de_sang;
```

Importer le script SQL situé dans le dossier :

```text
database/
```

Vérifier ensuite la chaîne de connexion dans :

```text
backend/BanqueSang/BanqueSang.API/appsettings.json
```

Exemple :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=banque_de_sang;User=root;Password=;"
  }
}
```

---

## Installation du frontend

Se placer dans le dossier frontend :

```bash
cd frontend/banque-sang-ui
```

Installer les dépendances :

```bash
npm install
```

Lancer Angular :

```bash
ng serve
```

L’application est disponible sur :

```text
http://localhost:4200
```

---

## Routes principales de l’API

### Authentification

```http
POST /api/auth/login
```

### Donneurs

```http
GET  /api/donneurs
GET  /api/donneurs/{id}
POST /api/donneurs
PUT  /api/donneurs/{id}
GET  /api/donneurs/{id}/eligibilite
```

### Dons

```http
GET  /api/dons
GET  /api/dons/{id}
POST /api/dons
```

### Stock

```http
GET /api/stock
GET /api/stock/groupes
GET /api/stock/alertes
```

### Tests biologiques

```http
GET  /api/tests
GET  /api/tests/resultats
POST /api/tests/resultats
PUT  /api/tests/resultats/{id}
```

### Patients

```http
GET  /api/patients
GET  /api/patients/{id}
POST /api/patients
PUT  /api/patients/{id}
```

### Demandes de sang

```http
GET  /api/demandes-sang
GET  /api/demandes-sang/{id}
POST /api/demandes-sang
PUT  /api/demandes-sang/{id}/statut
```

### Dashboard

```http
GET /api/dashboard/stats
```

---

## Scénario complet de test

Ce scénario permet de tester l’application de bout en bout.

### 1. Connexion

Se connecter avec le compte administrateur :

```text
Identifiant : admin
Mot de passe : Password123!
```

Après connexion, l’utilisateur arrive sur le tableau de bord.

---

### 2. Création d’un donneur

Aller dans le menu :

```text
Donneurs
```

Créer un donneur avec les informations suivantes :

```text
Nom : Mukendi
Prénom : Paul
Adresse : Bruxelles
Téléphone : 0488001122
Date de naissance : 1995-04-12
Groupe sanguin : O+
```

Le donneur apparaît ensuite dans la liste.

---

### 3. Vérification de l’éligibilité du donneur

Cliquer sur le bouton de vérification de l’éligibilité.

Le système vérifie :

* l’âge du donneur ;
* le statut d’éligibilité ;
* la date du dernier don.

Résultat attendu :

```text
Le donneur est éligible.
```

---

### 4. Enregistrement d’un don

Aller dans le menu :

```text
Dons
```

Créer un nouveau don :

```text
Donneur : Paul Mukendi
Centre : 1
Date du don : date actuelle
Volume : 450 ml
Type de composant : SANG_TOTAL
```

Résultat attendu :

* le don est créé ;
* le statut du don est `EN_ATTENTE` ;
* une poche de sang est créée automatiquement ;
* la poche est encore indisponible.

---

### 5. Vérification du stock

Aller dans le menu :

```text
Stock
```

Vérifier que la nouvelle poche apparaît.

Résultat attendu :

```text
Disponible : Non
```

La poche est indisponible tant que les tests biologiques ne sont pas validés.

---

### 6. Enregistrement des tests biologiques

Aller dans le menu :

```text
Tests
```

Créer les résultats de tests pour la poche créée.

Exemple de tests négatifs :

```text
VIH : NEGATIF
Hépatite B : NEGATIF
Hépatite C : NEGATIF
Syphilis : NEGATIF
Groupage ABO : NEGATIF
Numération globulaire : NEGATIF
```

Résultat attendu si tous les tests sont négatifs :

```text
Le don passe au statut VALIDE.
La poche devient disponible.
```

---

### 7. Cas d’un test positif

Créer un autre don, puis enregistrer un test biologique avec le résultat :

```text
POSITIF
```

Résultat attendu :

```text
Le don passe au statut REJETE.
La poche reste indisponible.
```

---

### 8. Création d’un patient

Aller dans le menu :

```text
Patients
```

Créer un patient :

```text
Nom : Kabila
Prénom : Jean
Adresse : Bruxelles
Téléphone : 0488112233
Date de naissance : 1990-05-12
Groupe sanguin : O+
Service : 1
```

Résultat attendu :

```text
Le patient apparaît dans la liste des patients.
```

---

### 9. Création d’une demande de sang

Aller dans le menu :

```text
Demandes
```

Créer une demande :

```text
Patient : Jean Kabila
Type de composant : SANG_TOTAL
Quantité de poches : 1
Urgence : URGENT
Date : date actuelle
Commentaire : Besoin urgent pour transfusion.
```

Résultat attendu :

```text
La demande est créée avec le statut EN_ATTENTE.
```

---

### 10. Modification du statut de la demande

Dans la liste des demandes, modifier le statut :

```text
EN_ATTENTE → RESERVEE
```

Puis :

```text
RESERVEE → TRAITEE
```

Résultat attendu :

```text
Le statut de la demande est mis à jour correctement.
```

---

### 11. Vérification du tableau de bord

Retourner dans :

```text
Tableau de bord
```

Cliquer sur :

```text
Actualiser
```

Vérifier que les statistiques ont été mises à jour :

* total donneurs ;
* total dons ;
* total poches ;
* poches disponibles ;
* patients ;
* demandes ;
* demandes en attente ;
* tests positifs ;
* tests négatifs.

---

## Scénario de test par rôle

### Administrateur

Connexion :

```text
Identifiant : admin
Mot de passe : Password123!
```

L’administrateur doit voir :

* Tableau de bord ;
* Donneurs ;
* Dons ;
* Tests ;
* Stock ;
* Patients ;
* Demandes.

---

### Technicien

Connexion :

```text
Identifiant : technicien
Mot de passe : Password123!
```

Le technicien doit voir :

* Tableau de bord ;
* Donneurs ;
* Dons ;
* Tests ;
* Stock.

Il ne doit pas voir :

* Patients ;
* Demandes.

---

### Médecin

Connexion :

```text
Identifiant : medecin
Mot de passe : Password123!
```

Le médecin doit voir :

* Tableau de bord ;
* Tests ;
* Stock ;
* Patients ;
* Demandes.

Il ne doit pas voir :

* Donneurs ;
* Dons.

---

## Sécurité

Les éléments de sécurité mis en place sont :

* authentification par JWT ;
* hachage des mots de passe avec BCrypt ;
* protection des endpoints backend avec `[Authorize]` ;
* contrôle des rôles dans les controllers ;
* protection des routes Angular avec `authGuard` et `roleGuard` ;
* ajout automatique du token JWT dans les requêtes HTTP avec un interceptor Angular ;
* stockage du rôle et de l’identifiant du personnel connecté dans le `localStorage`.

---

## Améliorations possibles

Le projet peut encore être amélioré avec :

* une gestion complète des utilisateurs depuis l’interface administrateur ;
* une réservation automatique des poches compatibles ;
* un historique des modifications des résultats biologiques ;
* des notifications pour les stocks faibles ;
* des graphiques dans le tableau de bord ;
* une meilleure gestion des services et hôpitaux ;
* des tests unitaires backend ;
* des tests frontend Angular.

---

## Auteur

Projet réalisé par Mickbron Tasse koagne.

Application développée dans le cadre d’un projet de développement d’application web.

# Création d'une application de gestion scolaire

## Contexte
Face au manque de digitalisation des administrations en Afrique, j'ai souhaité créer une plateforme de gestion scolaire complète. 
Cette solution est conçue pour faciliter l'administration et la communication au sein des établissements éducatifs, de la 
maternelle à l'enseignement supérieur, tout en simplifiant les démarches des usagers.

---
## Aperçu des principales fonctionnalités de **Gandhal**
La plateforme offre une gamme complète d'outils permettant de gérer efficacement les établissements scolaires. Voici les fonctionnalités disponibles :

### 1. Authentification
- **Création de compte** : L'utilisateur peut créer un compte s'il n'en possède pas déjà un.
- **Connexion** : Une fois connecté, l'utilisateur peut administrer l'école.
- **À venir : Gestion des rôles**
  - **Directeurs** : Accès à tous les droits et fonctionnalités.
  - **Personnels administratifs** : Permissions spécifiques, comme la gestion des élèves, la consultation des paiements, etc.

---

### 2. Menu principal
- **Informations des élèves** :
  - Détails : ID, nom et prénom, date de naissance, classe, solde, statut de paiement (entièrement payé, partiellement payé, non payé).
  - Visualisation : Courbes et graphiques pour suivre les paiements mensuels ou annuels.
- **Informations des enseignants** :
  - Accès à leurs statistiques et fiches.

---

### 3. Gestion de la vie scolaire

#### **Fiches élèves**
- Informations complètes : Nom, prénom, date de naissance, date d’entrée, photo de profil, etc.
- Gestion : Ajouter, modifier ou supprimer un élève.
- Export : Générer un PDF contenant toutes les informations des élèves.

#### **Fiches enseignants**
- Informations complètes : Nom, prénom, photo de profil, matières enseignées, classes assignées, etc.
- Gestion : Ajouter, modifier ou supprimer un enseignant.
- Suivi des notes :
  - Attribution des notes par matière et élève.
  - Calculs automatiques basés sur les coefficients.

#### **Gestion des classes**
- Organisation : Affectation des enseignants, matières, options, etc., pour chaque classe (élémentaires, collèges, lycée).

#### **Certificat de scolarité** *(À venir)*
- Génération automatique des certificats de scolarité au format PDF grâce aux données enregistrées lors de l’inscription.

#### **Résultats et classement des élèves** *(À venir)*

---

### 4. Gestion de la comptabilité
- **Suivi des paiements** :
  - Affichage du statut de paiement pour chaque élève (payé, partiellement payé, en retard).
  - Historique détaillé des paiements par élève.
- **Gestion des salaires** :
  - Paiement des soldes des enseignants.
- **Exportation des données** :
  - Possibilité d'exporter les données comptables au format Excel ou PDF.

---

### 5. Gestion des emplois du temps *(À venir)*
- Création interactive des emplois du temps en fonction des classes :
  - Champs : ID, titre, date et heure de début, date et heure de fin, cours, classes, nom du professeur.
- Fonctionnalités supplémentaires prévues :
  - **Édition d'événements** : Modifier les événements existants.
  - **Suppression d'événements** : Supprimer un événement via un bouton dédié.

### 6. Frontend
Le développement du **frontend** fera l'objet d'un travail ultérieur. Le backend est actuellement en cours de développement et permettra d'assurer 
la gestion des fonctionnalités mentionnées ci-dessus. Dès que le backend sera opérationnel, le frontend sera conçu pour fournir une interface utilisateur conviviale et interactive.


---



# Outils Technique : 
Backend : ASP.NET Core 8.
Base de données : SQL Server
Le frontend
Frontend : React.js 
Visualisation des données :
Graphiques : Chart.js.
Calendrier : FullCalendar.js.


**Gandhal** vise à moderniser et centraliser la gestion scolaire, en apportant des outils performants et adaptés aux besoins des établissements éducatifs. 


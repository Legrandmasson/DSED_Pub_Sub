# Système de Gestion de Commandes - Pub/Sub avec RabbitMQ

Ce projet implémente un système de messagerie asynchrone basé sur le patron **Publish/Subscribe** en C#. Il simule la création de commandes aléatoires et leur traitement par différents services via **RabbitMQ** (v6.8.1).

## 🛠 Architecture Technique

Le système utilise le modèle d'échange `Topic` de RabbitMQ pour router les messages en fonction de la priorité du client (Premium ou Normal).

* **Producteur :** Génère des objets `Commande` avec des données aléatoires.
* **Routing Key :** * `Commande.Placee.Normal`
    * `Commande.Placee.Premium`



## 📦 Modèle de Données

### Commande
- `Guid Reference` : Identifiant unique de la commande.
- `string NomClient` : Nom du client généré aléatoirement.
- `List<Article> Articles` : Liste de produits.
- `bool EstPremium` : Indicateur du statut client.

### Article
- `Guid Reference` : Identifiant unique du produit.
- `string NomProduit` : Désignation de l'article.
- `decimal Prix` : Prix unitaire.
- `int Quantite` : Nombre d'articles.

---

## 👥 Consommateurs (Services)

Le projet comporte 4 consommateurs distincts qui réagissent aux messages circulant sur le bus :

| Service | Filtre (Binding) | Action |
| :--- | :--- | :--- |
| **Journalisation** | `#` (Tous) | Crée un dossier `Journalisation` et écrit le détail brut de chaque message dans un fichier. |
| **Facturation** | `Commande.Placee.*` | Crée un dossier `Facturation` et génère un fichier de facture pour chaque commande. |
| **Expédition** | `Commande.Placee.*` | Affiche dans la console la liste des articles à préparer. |
| **Courriel** | `Commande.Placee.*` | Affiche une notification console personnalisée selon le statut (Premium/Normal). |

---

## 🚀 Installation et Configuration

### Prérequis
* [.NET SDK](https://dotnet.microsoft.com/download) (Version 6.0 ou +)
* **RabbitMQ Server** installé et fonctionnel (Version 6.8.1).

### Dépendances NuGet
```bash
dotnet add package RabbitMQ.Client --version 6.8.1
dotnet add package Newtonsoft.Json
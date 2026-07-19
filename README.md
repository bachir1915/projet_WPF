# Backend API PHP

Ce projet contient la partie backend écrite en PHP (API REST) pour communiquer avec l'application frontend C# (.NET / Windows Forms).

## Comment ouvrir ce projet ?
Ouvrez ce dossier (`api_backend_php`) directement dans **Visual Studio Code**.

## Comment démarrer le serveur Backend ?
Pour que le frontend C# puisse communiquer avec ce backend PHP, démarrez le serveur PHP intégré localement sur le port `8000` :

1. Ouvrez le terminal intégré de VS Code (`Ctrl + ~` ou `Ctrl + Shift + \``).
2. Lancez la commande suivante :
   ```bash
   php -S localhost:8000
   ```
3. Initialisez la base de données (si ce n'est pas déjà fait) en ouvrant cette URL dans votre navigateur web :
   [http://localhost:8000/setup_db.php](http://localhost:8000/setup_db.php)

Une fois configuré et lancé, votre frontend C# lancé sous **Visual Studio** pourra appeler les endpoints de ce backend en toute transparence.

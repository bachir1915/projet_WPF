# Tutoriel WPF : CRUD + « CSS » (Styles XAML)

## ⚠️ Précision importante
WPF n'utilise pas le CSS. Le CSS est un langage pour le web (HTML). WPF a son
propre système de style, basé en XAML, avec les balises **`Style`** et
**`ResourceDictionary`**. Il joue exactement le même rôle que le CSS :

| CSS (web) | WPF (XAML) |
|---|---|
| `.classe { ... }` | `<Style x:Key="..." TargetType="...">` |
| `:hover` | `<Trigger Property="IsMouseOver" Value="True">` |
| `:focus` | `<Trigger Property="IsFocused" Value="True">` |
| variables `--couleur` | `<SolidColorBrush x:Key="...">` |
| `<link rel="stylesheet">` | `<ResourceDictionary Source="Styles.xaml">` |
| héritage de classes | `BasedOn="{StaticResource ...}"` |

C'est ce système que ce projet utilise.

## Structure du projet
```
CrudWpfDemo/
├── CrudWpfDemo.csproj      → fichier projet .NET 8 WPF
├── App.xaml / App.xaml.cs  → point d'entrée, charge Styles.xaml
├── MainWindow.xaml         → interface (formulaire + liste)
├── MainWindow.xaml.cs      → logique CRUD (Create/Read/Update/Delete)
├── Models/
│   └── Personne.cs         → modèle de données
└── Styles/
    └── Styles.xaml         → la "feuille de style" (équivalent CSS)
```

## Comment exécuter le projet
1. Installer **Visual Studio 2022** (charge de travail « Développement .NET desktop »)
   ou le SDK **.NET 8** avec VS Code.
2. Ouvrir le dossier `CrudWpfDemo` (ou double-cliquer sur `CrudWpfDemo.csproj`).
3. Compiler et lancer avec F5, ou en ligne de commande (Windows uniquement,
   WPF ne fonctionne pas sur Linux/Mac) :
   ```
   dotnet run
   ```

## Ce que fait l'application
Une fenêtre avec :
- un **formulaire** à gauche (Nom, Prénom, Email) avec 4 boutons :
  **Ajouter**, **Modifier**, **Supprimer**, **Effacer**
- une **liste (DataGrid)** à droite qui affiche toutes les personnes

### Le C (Create)
`BtnAjouter_Click` valide le formulaire puis ajoute un nouvel objet
`Personne` dans une `ObservableCollection<Personne>`. Cette collection
spéciale prévient automatiquement l'interface qu'un élément a été ajouté.

### Le R (Read)
La `DataGrid` est simplement liée (`ItemsSource`) à la collection.
Cliquer sur une ligne remplit le formulaire (`GrillePersonnes_SelectionChanged`).

### Le U (Update)
`BtnModifier_Click` modifie les propriétés de l'objet sélectionné.
Comme `Personne` implémente `INotifyPropertyChanged`, la grille se
rafraîchit toute seule, sans code supplémentaire.

### Le D (Delete)
`BtnSupprimer_Click` demande une confirmation puis retire l'objet de
la collection avec `_personnes.Remove(...)`.

## Le "CSS" en détail (Styles/Styles.xaml)
- Des **couleurs réutilisables** (`ColorPrimaire`, `ColorDanger`, ...),
  comme des variables CSS.
- Un style global sur `Window` (comme un `body { }` en CSS).
- Des styles nommés avec `x:Key` (comme des classes CSS) :
  `TitrePage`, `Libelle`, `ChampSaisie`, `BoutonPrimaire`, `BoutonDanger`.
- De l'**héritage de style** avec `BasedOn="{StaticResource BoutonBase}"`
  (comme composer plusieurs classes CSS ensemble).
- Des **effets interactifs** avec `Style.Triggers` : le bouton change de
  couleur au survol (`IsMouseOver`), le champ de texte change de bordure
  au focus (`IsFocused`) — l'équivalent de `:hover` et `:focus` en CSS.

Pour appliquer un style à un contrôle, on écrit simplement :
```xml
<Button Content="Ajouter" Style="{StaticResource BoutonPrimaire}" />
```

## Pour aller plus loin
- Remplacer la liste en mémoire par une vraie base de données (SQLite avec
  Entity Framework Core) pour une persistance réelle.
- Passer à une architecture **MVVM** (ViewModel + `ICommand` au lieu des
  gestionnaires `Click` dans le code-behind) — c'est la pratique standard
  en WPF pour des applications plus grandes.
- Ajouter des `DataTrigger` dans les styles pour changer l'apparence selon
  une propriété de la donnée (ex. ligne en rouge si un champ est vide).

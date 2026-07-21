using System.Collections.Generic;
using System.Linq;
using CrudWpfDemo.Models;

namespace CrudWpfDemo.Metier
{
    /// <summary>
    /// Couche métier : contient toute la logique de calcul (nombres, moyennes)
    /// (étudiants, inscriptions, filières, moyennes). Cette classe ne dépend
    /// d'aucun élément d'interface (XAML) : elle prend des données en entrée
    /// et renvoie des résultats calculés. Elle peut donc être testée et
    /// réutilisée indépendamment de MainWindow.xaml.cs.
    /// </summary>
    public static class CalculMetier
    {
        /// <summary>
        /// Nombre total d'étudiants enregistrés.
        /// </summary>
        public static int CalculerNombreEtudiants(IEnumerable<Etudiant> etudiants)
        {
            return etudiants?.Count() ?? 0;
        }

        /// <summary>
        /// Nombre total d'inscriptions (= nombre d'étudiants actuellement
        /// inscrits dans une filière/classe). On considère qu'un étudiant
        /// inscrit compte pour une inscription.
        /// </summary>
        public static int CalculerNombreInscriptions(IEnumerable<Etudiant> etudiants)
        {
            return etudiants?.Count(e => e.IdClasse > 0) ?? 0;
        }

        /// <summary>
        /// Nombre total de filières (classes) existantes.
        /// </summary>
        public static int CalculerNombreClasses(IEnumerable<Classe> classes)
        {
            return classes?.Count() ?? 0;
        }

        // nombre total d'enseignants
        public static int CalculerNombreEnseignants(IEnumerable<Enseignant> enseignants)
        {
            return enseignants?.Count() ?? 0;
        }

        // nombre total de matieres
        public static int CalculerNombreMatieres(IEnumerable<Matiere> matieres)
        {
            return matieres?.Count() ?? 0;
        }

        /// <summary>
        /// Moyenne générale toutes notes confondues.
        /// </summary>
        public static double CalculerMoyenneGenerale(IEnumerable<Note> notes)
        {
            var liste = notes?.ToList() ?? new List<Note>();
            if (liste.Count == 0) return 0.0;
            return liste.Average(n => n.Valeur);
        }

        /// <summary>
        /// Classe les étudiants par filière (regroupement par nom de classe).
        /// Clé = nom de la filière, Valeur = liste des étudiants de cette filière.
        /// </summary>
        public static Dictionary<string, List<Etudiant>> ClasserEtudiantsParFiliere(IEnumerable<Etudiant> etudiants)
        {
            var resultat = new Dictionary<string, List<Etudiant>>();
            if (etudiants == null) return resultat;

            foreach (var etudiant in etudiants)
            {
                string filiere = string.IsNullOrWhiteSpace(etudiant.NomClasse) ? "Non assigné" : etudiant.NomClasse;

                if (!resultat.ContainsKey(filiere))
                {
                    resultat[filiere] = new List<Etudiant>();
                }
                resultat[filiere].Add(etudiant);
            }

            return resultat;
        }

        /// <summary>
        /// Calcule, pour chaque filière (classe), le nombre d'étudiants,
        /// le nombre d'inscriptions et la moyenne des notes. C'est ce qui
        /// alimente le tableau récapitulatif du Dashboard.
        /// </summary>
        public static List<FiliereResume> CalculerResumeParFiliere(
            IEnumerable<Etudiant> etudiants,
            IEnumerable<Classe> classes,
            IEnumerable<Note> notes)
        {
            var etudiantsList = etudiants?.ToList() ?? new List<Etudiant>();
            var classesList = classes?.ToList() ?? new List<Classe>();
            var notesList = notes?.ToList() ?? new List<Note>();

            var etudiantsParFiliere = ClasserEtudiantsParFiliere(etudiantsList);
            var resultat = new List<FiliereResume>();

            // On part de la liste des classes pour garder même les filières sans étudiant
            foreach (var classe in classesList)
            {
                etudiantsParFiliere.TryGetValue(classe.Nom, out var etudiantsDeLaFiliere);
                etudiantsDeLaFiliere ??= new List<Etudiant>();

                var idsEtudiants = etudiantsDeLaFiliere.Select(e => e.Id).ToHashSet();
                var notesDeLaFiliere = notesList.Where(n => idsEtudiants.Contains(n.IdEtudiant)).ToList();

                double moyenne = notesDeLaFiliere.Count > 0
                    ? notesDeLaFiliere.Average(n => n.Valeur)
                    : 0.0;

                resultat.Add(new FiliereResume
                {
                    NomFiliere = classe.Nom,
                    NombreEtudiants = etudiantsDeLaFiliere.Count,
                    NombreInscriptions = CalculerNombreInscriptions(etudiantsDeLaFiliere),
                    Moyenne = moyenne
                });
            }

            return resultat;
        }

        /// <summary>
        /// Détermine le "major de promotion" : l'étudiant avec la meilleure
        /// moyenne, sous forme de texte prêt à afficher ("Nom (xx.xx/20)").
        /// </summary>
        public static string TrouverMajorPromotion(IEnumerable<Note> notes)
        {
            var notesList = notes?.ToList() ?? new List<Note>();
            if (notesList.Count == 0) return "Aucun";

            var moyennesParEtudiant = notesList
                .GroupBy(n => n.IdEtudiant)
                .Select(g => new
                {
                    Nom = g.First().NomCompletEtudiant,
                    Moyenne = g.Average(n => n.Valeur)
                })
                .OrderByDescending(x => x.Moyenne)
                .FirstOrDefault();

            return moyennesParEtudiant == null
                ? "Aucun"
                : $"{moyennesParEtudiant.Nom} ({moyennesParEtudiant.Moyenne:F1}/20)";
        }
    }
}

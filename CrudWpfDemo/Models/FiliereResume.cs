using System.ComponentModel;

namespace CrudWpfDemo.Models
{
    /// <summary>
    /// Représente le récapitulatif d'une filière (classe) :
    /// nombre d'étudiants, nombre d'inscriptions et moyenne des notes.
    /// Utilisé pour l'affichage dans le tableau du Dashboard.
    /// </summary>
    public class FiliereResume : INotifyPropertyChanged
    {
        public string NomFiliere { get; set; } = string.Empty;
        public int NombreEtudiants { get; set; }
        public int NombreInscriptions { get; set; }
        public double Moyenne { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

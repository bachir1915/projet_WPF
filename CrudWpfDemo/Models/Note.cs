using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CrudWpfDemo.Models
{
    public class Note : INotifyPropertyChanged
    {
        private int _idEtudiant;
        private string _nomCompletEtudiant = string.Empty;
        private string _nomClasse = string.Empty;
        private string _matiere = string.Empty;
        private float _valeur;

        [JsonPropertyName("id")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int Id { get; set; }

        [JsonPropertyName("id_etudiant")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int IdEtudiant
        {
            get => _idEtudiant;
            set { _idEtudiant = value; OnPropertyChanged(nameof(IdEtudiant)); }
        }

        [JsonPropertyName("nom_complet_etudiant")]
        public string NomCompletEtudiant
        {
            get => _nomCompletEtudiant;
            set { _nomCompletEtudiant = value; OnPropertyChanged(nameof(NomCompletEtudiant)); }
        }

        [JsonPropertyName("nom_classe")]
        public string NomClasse
        {
            get => _nomClasse;
            set { _nomClasse = value; OnPropertyChanged(nameof(NomClasse)); }
        }

        [JsonPropertyName("matiere")]
        public string Matiere
        {
            get => _matiere;
            set { _matiere = value; OnPropertyChanged(nameof(Matiere)); }
        }

        [JsonPropertyName("valeur")]
        [JsonConverter(typeof(FlexibleFloatConverter))]
        public float Valeur
        {
            get => _valeur;
            set { _valeur = value; OnPropertyChanged(nameof(Valeur)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CrudWpfDemo.Models
{
    // classe pour les enseignants
    public class Enseignant : INotifyPropertyChanged
    {
        private string _nom = string.Empty;
        private string _prenom = string.Empty;
        private string _email = string.Empty;
        private string _specialite = string.Empty;
        private int _idClasse;
        private string _nomClasse = string.Empty;

        [JsonPropertyName("id")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int Id { get; set; }

        [JsonPropertyName("nom")]
        public string Nom
        {
            get => _nom;
            set { _nom = value; OnPropertyChanged(nameof(Nom)); OnPropertyChanged(nameof(NomComplet)); }
        }

        [JsonPropertyName("prenom")]
        public string Prenom
        {
            get => _prenom;
            set { _prenom = value; OnPropertyChanged(nameof(Prenom)); OnPropertyChanged(nameof(NomComplet)); }
        }

        [JsonPropertyName("email")]
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        [JsonPropertyName("specialite")]
        public string Specialite
        {
            get => _specialite;
            set { _specialite = value; OnPropertyChanged(nameof(Specialite)); }
        }

        [JsonPropertyName("id_classe")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int IdClasse
        {
            get => _idClasse;
            set { _idClasse = value; OnPropertyChanged(nameof(IdClasse)); }
        }

        [JsonPropertyName("nom_classe")]
        public string NomClasse
        {
            get => _nomClasse;
            set { _nomClasse = value; OnPropertyChanged(nameof(NomClasse)); }
        }

        // propriété calculée pour afficher dans les combobox
        [JsonIgnore]
        public string NomComplet => $"{Prenom} {Nom}";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

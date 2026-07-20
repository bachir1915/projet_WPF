using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CrudWpfDemo.Models
{
    public class Etudiant : INotifyPropertyChanged
    {
        private string _nom = string.Empty;
        private string _prenom = string.Empty;
        private string _email = string.Empty;
        private int _idClasse;
        private string _nomClasse = string.Empty;

        [JsonPropertyName("id")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int Id { get; set; }

        [JsonPropertyName("nom")]
        public string Nom
        {
            get => _nom;
            set { _nom = value; OnPropertyChanged(nameof(Nom)); }
        }

        [JsonPropertyName("prenom")]
        public string Prenom
        {
            get => _prenom;
            set { _prenom = value; OnPropertyChanged(nameof(Prenom)); }
        }

        [JsonPropertyName("email")]
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
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

        private bool _isSelectedForEmail;

        [JsonIgnore]
        public bool IsSelectedForEmail
        {
            get => _isSelectedForEmail;
            set { _isSelectedForEmail = value; OnPropertyChanged(nameof(IsSelectedForEmail)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

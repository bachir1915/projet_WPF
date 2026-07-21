using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CrudWpfDemo.Models
{
    // classe pour les matieres
    public class Matiere : INotifyPropertyChanged
    {
        private string _nom = string.Empty;
        private int _coefficient = 1;
        private int _idEnseignant;
        private string _nomEnseignant = string.Empty;

        [JsonPropertyName("id")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int Id { get; set; }

        [JsonPropertyName("nom")]
        public string Nom
        {
            get => _nom;
            set { _nom = value; OnPropertyChanged(nameof(Nom)); }
        }

        [JsonPropertyName("coefficient")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int Coefficient
        {
            get => _coefficient;
            set { _coefficient = value; OnPropertyChanged(nameof(Coefficient)); }
        }

        [JsonPropertyName("id_enseignant")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int IdEnseignant
        {
            get => _idEnseignant;
            set { _idEnseignant = value; OnPropertyChanged(nameof(IdEnseignant)); }
        }

        [JsonPropertyName("nom_enseignant")]
        public string NomEnseignant
        {
            get => _nomEnseignant;
            set { _nomEnseignant = value; OnPropertyChanged(nameof(NomEnseignant)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

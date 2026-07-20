using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CrudWpfDemo.Models
{
    public class Classe : INotifyPropertyChanged
    {
        private string _nom = string.Empty;
        private int _nombreEtudiants;

        [JsonPropertyName("id")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int Id { get; set; }

        [JsonPropertyName("nom")]
        public string Nom
        {
            get => _nom;
            set { _nom = value; OnPropertyChanged(nameof(Nom)); }
        }

        [JsonPropertyName("nombre_etudiants")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int NombreEtudiants
        {
            get => _nombreEtudiants;
            set { _nombreEtudiants = value; OnPropertyChanged(nameof(NombreEtudiants)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

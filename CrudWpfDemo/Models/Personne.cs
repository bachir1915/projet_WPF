using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace CrudWpfDemo.Models
{
    public class FlexibleIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }
            if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (int.TryParse(value, out int result))
                {
                    return result;
                }
            }
            return 0;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class FlexibleFloatConverter : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetSingle();
            }
            if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (float.TryParse(value, out float result))
                {
                    return result;
                }
            }
            return 0.0f;
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class CreateUpdateDeleteResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("id")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int? Id { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

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

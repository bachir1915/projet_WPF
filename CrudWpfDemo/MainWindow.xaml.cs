using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CrudWpfDemo.Metier;
using CrudWpfDemo.Models;

namespace CrudWpfDemo
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseApiUrl = "http://localhost:8000/";

        private readonly ObservableCollection<Etudiant> _students = new();
        private readonly ObservableCollection<Classe> _classes = new();
        private readonly ObservableCollection<Note> _notes = new();
        private readonly ObservableCollection<FiliereResume> _filieres = new();

        private Etudiant? _selectedStudent;
        private Classe? _selectedClass;
        private Note? _selectedGrade;

        public MainWindow()
        {
            InitializeComponent();

            GrilleStudents.ItemsSource = _students;
            GrilleClasses.ItemsSource = _classes;
            GrilleNotes.ItemsSource = _notes;
            GrilleDashboardStudents.ItemsSource = _students;
            GrilleFilieres.ItemsSource = _filieres;

            // Load data on startup
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAllDataAsync();
        }

        private async Task LoadAllDataAsync()
        {
            TxtApiStatus.Text = "API: Connexion en cours...";
            TxtApiStatus.Foreground = new SolidColorBrush(Color.FromRgb(234, 179, 8)); // Yellow/Warning

            try
            {
                // Sequence of loading
                await LoadClassesAsync();
                await LoadStudentsAsync();
                await LoadNotesAsync();

                TxtApiStatus.Text = "API: Connecté (8000)";
                TxtApiStatus.Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129)); // Emerald/Success
            }
            catch (Exception ex)
            {
                TxtApiStatus.Text = "API: Erreur de connexion";
                TxtApiStatus.Foreground = new SolidColorBrush(Color.FromRgb(239, 104, 104)); // Danger

                ShowStatus("Impossible de contacter le serveur backend PHP. Vérifiez qu'il est bien démarré sur le port 8000.", true);
                MessageBox.Show($"Erreur lors de la connexion à l'API PHP :\n{ex.Message}\n\nAssurez-vous d'avoir exécuté la commande 'php -S localhost:8000' dans le dossier de l'API.", "Erreur de Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==========================================
        //  NAVIGATION MENU
        // ==========================================
        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            if (clickedButton == null) return;

            // Reset active style on all menu buttons
            BtnMenuDashboard.Style = (Style)FindResource("BoutonMenu");
            BtnMenuInscriptions.Style = (Style)FindResource("BoutonMenu");
            BtnMenuClasses.Style = (Style)FindResource("BoutonMenu");
            BtnMenuNotes.Style = (Style)FindResource("BoutonMenu");

            // Hide all views
            GridDashboard.Visibility = Visibility.Collapsed;
            GridInscriptions.Visibility = Visibility.Collapsed;
            GridClasses.Visibility = Visibility.Collapsed;
            GridNotes.Visibility = Visibility.Collapsed;

            // Activate chosen view and button style
            clickedButton.Style = (Style)FindResource("BoutonMenuActive");

            if (clickedButton == BtnMenuDashboard)
            {
                GridDashboard.Visibility = Visibility.Visible;
                UpdateDashboardStats();
            }
            else if (clickedButton == BtnMenuInscriptions)
            {
                GridInscriptions.Visibility = Visibility.Visible;
            }
            else if (clickedButton == BtnMenuClasses)
            {
                GridClasses.Visibility = Visibility.Visible;
            }
            else if (clickedButton == BtnMenuNotes)
            {
                GridNotes.Visibility = Visibility.Visible;
            }
        }

        // ==========================================
        //  API DATA FETCHING
        // ==========================================
        private async Task LoadClassesAsync()
        {
            string url = BaseApiUrl + "classe/list.php";
            var result = await _httpClient.GetFromJsonAsync<List<Classe>>(url);
            
            _classes.Clear();
            if (result != null)
            {
                foreach (var item in result)
                {
                    _classes.Add(item);
                }
            }

            ComboStudentClasse.ItemsSource = null;
            ComboStudentClasse.ItemsSource = _classes;
        }

        private async Task LoadStudentsAsync()
        {
            string url = BaseApiUrl + "etudiant/list.php";
            var result = await _httpClient.GetFromJsonAsync<List<Etudiant>>(url);

            _students.Clear();
            if (result != null)
            {
                foreach (var item in result)
                {
                    _students.Add(item);
                }
            }

            ComboGradeStudent.ItemsSource = null;
            ComboGradeStudent.ItemsSource = _students;
            
            UpdateDashboardStats();
        }

        private async Task LoadNotesAsync()
        {
            string url = BaseApiUrl + "note/list.php";
            var result = await _httpClient.GetFromJsonAsync<List<Note>>(url);

            _notes.Clear();
            if (result != null)
            {
                foreach (var item in result)
                {
                    _notes.Add(item);
                }
            }

            UpdateDashboardStats();
        }

        // ==========================================
        //  MODULE INSCRIPTIONS (STUDENTS) EVENTS
        // ==========================================
        private async void BtnStudentAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtStudentNom.Text) ||
                string.IsNullOrWhiteSpace(TxtStudentPrenom.Text) ||
                string.IsNullOrWhiteSpace(TxtStudentEmail.Text) ||
                ComboStudentClasse.SelectedValue == null)
            {
                ShowStatus("Tous les champs sont obligatoires.", true);
                return;
            }

            string email = TxtStudentEmail.Text.Trim();
            if (!IsValidEmail(email, out string errorMsg))
            {
                ShowStatus(errorMsg, true);
                return;
            }

            try
            {
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("nom", TxtStudentNom.Text.Trim()),
                    new KeyValuePair<string, string>("prenom", TxtStudentPrenom.Text.Trim()),
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("id_classe", ComboStudentClasse.SelectedValue.ToString()!)
                });

                var response = await _httpClient.PostAsync(BaseApiUrl + "etudiant/create.php", postData);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                if (result != null && result.Success)
                {
                    ShowStatus("Étudiant inscrit avec succès !");
                    ViderStudentFormulaire();
                    await LoadStudentsAsync();
                    await LoadClassesAsync(); // refresh student count on classes
                }
                else
                {
                    ShowStatus("Erreur : " + (result?.Message ?? "Échec de l'inscription"), true);
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Erreur d'appel API : " + ex.Message, true);
            }
        }

        private async void BtnStudentModifier_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStudent == null)
            {
                ShowStatus("Sélectionnez d'abord un étudiant dans la liste.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtStudentNom.Text) ||
                string.IsNullOrWhiteSpace(TxtStudentPrenom.Text) ||
                string.IsNullOrWhiteSpace(TxtStudentEmail.Text) ||
                ComboStudentClasse.SelectedValue == null)
            {
                ShowStatus("Tous les champs sont obligatoires.", true);
                return;
            }

            string email = TxtStudentEmail.Text.Trim();
            if (!IsValidEmail(email, out string errorMsg))
            {
                ShowStatus(errorMsg, true);
                return;
            }

            try
            {
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id", _selectedStudent.Id.ToString()),
                    new KeyValuePair<string, string>("nom", TxtStudentNom.Text.Trim()),
                    new KeyValuePair<string, string>("prenom", TxtStudentPrenom.Text.Trim()),
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("id_classe", ComboStudentClasse.SelectedValue.ToString()!)
                });

                var response = await _httpClient.PostAsync(BaseApiUrl + "etudiant/update.php", postData);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                if (result != null && result.Success)
                {
                    ShowStatus("Informations de l'étudiant mises à jour.");
                    ViderStudentFormulaire();
                    await LoadStudentsAsync();
                    await LoadClassesAsync(); // update counts
                    await LoadNotesAsync(); // update names in notes grid
                }
                else
                {
                    ShowStatus("Erreur de modification: " + (result?.Message ?? "Inconnue"), true);
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Erreur d'appel API : " + ex.Message, true);
            }
        }

        private async void BtnStudentSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStudent == null)
            {
                ShowStatus("Sélectionnez d'abord un étudiant à supprimer.", true);
                return;
            }

            var confirm = MessageBox.Show(
                $"Supprimer définitivement l'inscription de {_selectedStudent.Prenom} {_selectedStudent.Nom} ?\nToutes ses notes seront également supprimées.",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    var postData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("id", _selectedStudent.Id.ToString())
                    });

                    var response = await _httpClient.PostAsync(BaseApiUrl + "etudiant/delete.php", postData);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                    if (result != null && result.Success)
                    {
                        ShowStatus("Étudiant supprimé.");
                        ViderStudentFormulaire();
                        await LoadStudentsAsync();
                        await LoadClassesAsync();
                        await LoadNotesAsync();
                    }
                    else
                    {
                        ShowStatus("Erreur de suppression: " + (result?.Message ?? "Inconnue"), true);
                    }
                }
                catch (Exception ex)
                {
                    ShowStatus("Erreur d'appel API : " + ex.Message, true);
                }
            }
        }

        private void GrilleStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedStudent = GrilleStudents.SelectedItem as Etudiant;
            if (_selectedStudent == null) return;

            TxtStudentNom.Text = _selectedStudent.Nom;
            TxtStudentPrenom.Text = _selectedStudent.Prenom;
            TxtStudentEmail.Text = _selectedStudent.Email;
            ComboStudentClasse.SelectedValue = _selectedStudent.IdClasse;
        }

        private void BtnStudentEffacer_Click(object sender, RoutedEventArgs e)
        {
            ViderStudentFormulaire();
        }

        private void ViderStudentFormulaire()
        {
            TxtStudentNom.Clear();
            TxtStudentPrenom.Clear();
            TxtStudentEmail.Clear();
            ComboStudentClasse.SelectedIndex = -1;
            _selectedStudent = null;
            GrilleStudents.SelectedItem = null;
        }

        // ==========================================
        //  MODULE CLASSES EVENTS
        // ==========================================
        private async void BtnClassAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtClassNom.Text))
            {
                ShowStatus("Le nom de la classe est obligatoire.", true);
                return;
            }

            try
            {
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("nom", TxtClassNom.Text.Trim())
                });

                var response = await _httpClient.PostAsync(BaseApiUrl + "classe/create.php", postData);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                if (result != null && result.Success)
                {
                    ShowStatus("Classe créée avec succès !");
                    ViderClassFormulaire();
                    await LoadClassesAsync();
                }
                else
                {
                    ShowStatus("Erreur : " + (result?.Message ?? "Échec de création"), true);
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Erreur d'appel API : " + ex.Message, true);
            }
        }

        private async void BtnClassModifier_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClass == null)
            {
                ShowStatus("Sélectionnez d'abord une classe dans la liste.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtClassNom.Text))
            {
                ShowStatus("Le nom de la classe est obligatoire.", true);
                return;
            }

            try
            {
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id", _selectedClass.Id.ToString()),
                    new KeyValuePair<string, string>("nom", TxtClassNom.Text.Trim())
                });

                var response = await _httpClient.PostAsync(BaseApiUrl + "classe/update.php", postData);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                if (result != null && result.Success)
                {
                    ShowStatus("Nom de la classe mis à jour.");
                    ViderClassFormulaire();
                    await LoadClassesAsync();
                    await LoadStudentsAsync(); // update classes in student view
                    await LoadNotesAsync(); // update class in notes view
                }
                else
                {
                    ShowStatus("Erreur : " + (result?.Message ?? "Échec de modification"), true);
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Erreur d'appel API : " + ex.Message, true);
            }
        }

        private async void BtnClassSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClass == null)
            {
                ShowStatus("Sélectionnez d'abord une classe à supprimer.", true);
                return;
            }

            var confirm = MessageBox.Show(
                $"Supprimer la classe '{_selectedClass.Nom}' ?\nTous les étudiants et notes rattachés seront supprimés de la base de données.",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    var postData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("id", _selectedClass.Id.ToString())
                    });

                    var response = await _httpClient.PostAsync(BaseApiUrl + "classe/delete.php", postData);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                    if (result != null && result.Success)
                    {
                        ShowStatus("Classe supprimée.");
                        ViderClassFormulaire();
                        await LoadClassesAsync();
                        await LoadStudentsAsync();
                        await LoadNotesAsync();
                    }
                    else
                    {
                        ShowStatus("Erreur : " + (result?.Message ?? "Échec de suppression"), true);
                    }
                }
                catch (Exception ex)
                {
                    ShowStatus("Erreur d'appel API : " + ex.Message, true);
                }
            }
        }

        private void GrilleClasses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedClass = GrilleClasses.SelectedItem as Classe;
            if (_selectedClass == null) return;

            TxtClassNom.Text = _selectedClass.Nom;
        }

        private void BtnClassEffacer_Click(object sender, RoutedEventArgs e)
        {
            ViderClassFormulaire();
        }

        private void ViderClassFormulaire()
        {
            TxtClassNom.Clear();
            _selectedClass = null;
            GrilleClasses.SelectedItem = null;
        }

        // ==========================================
        //  MODULE NOTES (GRADES) EVENTS
        // ==========================================
        private async void BtnGradeAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (ComboGradeStudent.SelectedValue == null ||
                string.IsNullOrWhiteSpace(TxtGradeMatiere.Text) ||
                string.IsNullOrWhiteSpace(TxtGradeValeur.Text))
            {
                ShowStatus("Tous les champs sont obligatoires.", true);
                return;
            }

            if (!float.TryParse(TxtGradeValeur.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float noteVal) || noteVal < 0 || noteVal > 20)
            {
                ShowStatus("La note doit être un nombre valide entre 0 et 20.", true);
                return;
            }

            try
            {
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id_etudiant", ComboGradeStudent.SelectedValue.ToString()!),
                    new KeyValuePair<string, string>("matiere", TxtGradeMatiere.Text.Trim()),
                    new KeyValuePair<string, string>("note", noteVal.ToString(System.Globalization.CultureInfo.InvariantCulture))
                });

                var response = await _httpClient.PostAsync(BaseApiUrl + "note/create.php", postData);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                if (result != null && result.Success)
                {
                    ShowStatus("Note enregistrée avec succès !");
                    ViderGradeFormulaire();
                    await LoadNotesAsync();
                }
                else
                {
                    ShowStatus("Erreur : " + (result?.Message ?? "Échec de création"), true);
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Erreur d'appel API : " + ex.Message, true);
            }
        }

        private async void BtnGradeModifier_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGrade == null)
            {
                ShowStatus("Sélectionnez d'abord une note dans la liste.", true);
                return;
            }

            if (ComboGradeStudent.SelectedValue == null ||
                string.IsNullOrWhiteSpace(TxtGradeMatiere.Text) ||
                string.IsNullOrWhiteSpace(TxtGradeValeur.Text))
            {
                ShowStatus("Tous les champs sont obligatoires.", true);
                return;
            }

            if (!float.TryParse(TxtGradeValeur.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float noteVal) || noteVal < 0 || noteVal > 20)
            {
                ShowStatus("La note doit être un nombre valide entre 0 et 20.", true);
                return;
            }

            try
            {
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id", _selectedGrade.Id.ToString()),
                    new KeyValuePair<string, string>("id_etudiant", ComboGradeStudent.SelectedValue.ToString()!),
                    new KeyValuePair<string, string>("matiere", TxtGradeMatiere.Text.Trim()),
                    new KeyValuePair<string, string>("note", noteVal.ToString(System.Globalization.CultureInfo.InvariantCulture))
                });

                var response = await _httpClient.PostAsync(BaseApiUrl + "note/update.php", postData);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                if (result != null && result.Success)
                {
                    ShowStatus("Note modifiée avec succès.");
                    ViderGradeFormulaire();
                    await LoadNotesAsync();
                }
                else
                {
                    ShowStatus("Erreur : " + (result?.Message ?? "Échec de modification"), true);
                }
            }
            catch (Exception ex)
            {
                ShowStatus("Erreur d'appel API : " + ex.Message, true);
            }
        }

        private async void BtnGradeSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGrade == null)
            {
                ShowStatus("Sélectionnez d'abord une note à supprimer.", true);
                return;
            }

            var confirm = MessageBox.Show(
                $"Supprimer cette note de {_selectedGrade.Valeur:F2} en {_selectedGrade.Matiere} ?",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    var postData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("id", _selectedGrade.Id.ToString())
                    });

                    var response = await _httpClient.PostAsync(BaseApiUrl + "note/delete.php", postData);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                    if (result != null && result.Success)
                    {
                        ShowStatus("Note supprimée.");
                        ViderGradeFormulaire();
                        await LoadNotesAsync();
                    }
                    else
                    {
                        ShowStatus("Erreur : " + (result?.Message ?? "Échec de suppression"), true);
                    }
                }
                catch (Exception ex)
                {
                    ShowStatus("Erreur d'appel API : " + ex.Message, true);
                }
            }
        }

        private void GrilleNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedGrade = GrilleNotes.SelectedItem as Note;
            if (_selectedGrade == null) return;

            ComboGradeStudent.SelectedValue = _selectedGrade.IdEtudiant;
            TxtGradeMatiere.Text = _selectedGrade.Matiere;
            TxtGradeValeur.Text = _selectedGrade.Valeur.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        private void BtnGradeEffacer_Click(object sender, RoutedEventArgs e)
        {
            ViderGradeFormulaire();
        }

        private void ViderGradeFormulaire()
        {
            ComboGradeStudent.SelectedIndex = -1;
            TxtGradeMatiere.Clear();
            TxtGradeValeur.Clear();
            _selectedGrade = null;
            GrilleNotes.SelectedItem = null;
        }

        // ==========================================
        //  SEARCH & LIVE FILTERING EVENTS
        // ==========================================
        private void TxtSearchStudent_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = TxtSearchStudent.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filterText))
            {
                GrilleStudents.ItemsSource = _students;
            }
            else
            {
                var filtered = new ObservableCollection<Etudiant>();
                foreach (var student in _students)
                {
                    if (student.Nom.ToLower().Contains(filterText) ||
                        student.Prenom.ToLower().Contains(filterText) ||
                        student.Email.ToLower().Contains(filterText) ||
                        student.NomClasse.ToLower().Contains(filterText))
                    {
                        filtered.Add(student);
                    }
                }
                GrilleStudents.ItemsSource = filtered;
            }
        }

        private void TxtSearchClass_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = TxtSearchClass.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filterText))
            {
                GrilleClasses.ItemsSource = _classes;
            }
            else
            {
                var filtered = new ObservableCollection<Classe>();
                foreach (var cls in _classes)
                {
                    if (cls.Nom.ToLower().Contains(filterText))
                    {
                        filtered.Add(cls);
                    }
                }
                GrilleClasses.ItemsSource = filtered;
            }
        }

        private void TxtSearchNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = TxtSearchNotes.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filterText))
            {
                GrilleNotes.ItemsSource = _notes;
            }
            else
            {
                var filtered = new ObservableCollection<Note>();
                foreach (var note in _notes)
                {
                    if (note.NomCompletEtudiant.ToLower().Contains(filterText) ||
                        note.NomClasse.ToLower().Contains(filterText) ||
                        note.Matiere.ToLower().Contains(filterText))
                    {
                        filtered.Add(note);
                    }
                }
                GrilleNotes.ItemsSource = filtered;
            }
        }

        // ==========================================
        //  DASHBOARD STATISTICS & ISI EMAIL ASSIGNMENT
        // ==========================================
        private void UpdateDashboardStats()
        {
            // Tous les calculs sont désormais délégués à la couche métier
            // (Metier/CalculMetier.cs), qui ne dépend d'aucun élément
            // d'interface. MainWindow se contente d'afficher les résultats.

            int nombreEtudiants = CalculMetier.CalculerNombreEtudiants(_students);
            int nombreInscriptions = CalculMetier.CalculerNombreInscriptions(_students);
            int nombreClasses = CalculMetier.CalculerNombreClasses(_classes);
            double moyenneGenerale = CalculMetier.CalculerMoyenneGenerale(_notes);
            string majorPromotion = CalculMetier.TrouverMajorPromotion(_notes);

            TxtTotalStudents.Text = nombreEtudiants.ToString();
            TxtTotalInscriptions.Text = nombreInscriptions.ToString();
            TxtTotalClasses.Text = nombreClasses.ToString();
            TxtMoyenne.Text = $"{moyenneGenerale:F2} / 20";
            TxtBestStudent.Text = majorPromotion;

            // Tableau récapitulatif : répartition des étudiants par filière
            // avec, pour chacune, le nombre d'étudiants, d'inscriptions et la moyenne
            var resumeParFiliere = CalculMetier.CalculerResumeParFiliere(_students, _classes, _notes);

            _filieres.Clear();
            foreach (var stat in resumeParFiliere)
            {
                _filieres.Add(stat);
            }
        }

        private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var student in _students)
            {
                student.IsSelectedForEmail = true;
            }
            GrilleDashboardStudents.Items.Refresh();
        }

        private void BtnDeselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var student in _students)
            {
                student.IsSelectedForEmail = false;
            }
            GrilleDashboardStudents.Items.Refresh();
        }

        private async void BtnApplyGroupEmail_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudents = new List<Etudiant>();
            foreach (var student in _students)
            {
                if (student.IsSelectedForEmail)
                {
                    selectedStudents.Add(student);
                }
            }

            if (selectedStudents.Count == 0)
            {
                ShowStatus("Veuillez sélectionner au moins un étudiant dans la liste.", true);
                return;
            }

            var selectedFormatItem = ComboEmailFormat.SelectedItem as ComboBoxItem;
            if (selectedFormatItem == null) return;
            string formatOption = selectedFormatItem.Content.ToString()!;

            int successCount = 0;
            int failCount = 0;

            foreach (var student in selectedStudents)
            {
                string generatedEmail = "";
                
                // Clean accents and spaces
                string cleanPrenom = CleanStringForEmail(student.Prenom);
                string cleanNom = CleanStringForEmail(student.Nom);

                if (formatOption.Contains("prenom.nom"))
                {
                    generatedEmail = $"{cleanPrenom}.{cleanNom}@groupeisi.com";
                }
                else if (formatOption.Contains("initiale.nom"))
                {
                    string initial = cleanPrenom.Length > 0 ? cleanPrenom.Substring(0, 1) : "";
                    generatedEmail = $"{initial}.{cleanNom}@groupeisi.com";
                }
                else
                {
                    generatedEmail = "etudiant@groupeisi.com";
                }

                try
                {
                    var postData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("id", student.Id.ToString()),
                        new KeyValuePair<string, string>("nom", student.Nom),
                        new KeyValuePair<string, string>("prenom", student.Prenom),
                        new KeyValuePair<string, string>("email", generatedEmail),
                        new KeyValuePair<string, string>("id_classe", student.IdClasse.ToString())
                    });

                    var response = await _httpClient.PostAsync(BaseApiUrl + "etudiant/update.php", postData);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<CreateUpdateDeleteResult>();
                        if (result != null && result.Success)
                        {
                            successCount++;
                        }
                        else
                        {
                            failCount++;
                        }
                    }
                    else
                    {
                        failCount++;
                    }
                }
                catch
                {
                    failCount++;
                }
            }

            ShowStatus($"Attribution e-mails ISI : {successCount} succès, {failCount} échecs.");
            await LoadStudentsAsync();
            GrilleDashboardStudents.Items.Refresh();
        }

        private string CleanStringForEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            
            string normalized = input.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (char c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    if (char.IsLetterOrDigit(c))
                    {
                        sb.Append(char.ToLowerInvariant(c));
                    }
                }
            }

            return sb.ToString();
        }

        // ==========================================
        //  EMAIL INPUT VALIDATION (CONTROL SASIE)
        // ==========================================
        private bool IsValidEmail(string email, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                errorMessage = "L'adresse e-mail est obligatoire.";
                return false;
            }

            // General email format regex check
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            if (!emailRegex.IsMatch(email))
            {
                errorMessage = "Format de l'e-mail incorrect (ex: exemple@domaine.com).";
                return false;
            }

            // Gmail-specific controls (user requested verification)
            if (email.Contains("gmail", StringComparison.OrdinalIgnoreCase))
            {
                if (!email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = "Une adresse Gmail doit se terminer précisément par '@gmail.com'.";
                    return false;
                }

                // Check local part of Gmail: only letters, numbers, and periods are allowed
                var localPart = email.Split('@')[0];
                if (string.IsNullOrEmpty(localPart) || !Regex.IsMatch(localPart, @"^[a-z0-9\.]+$", RegexOptions.IgnoreCase))
                {
                    errorMessage = "Identifiant Gmail invalide (seuls les lettres, chiffres et points sont permis).";
                    return false;
                }
            }

            return true;
        }

        // ==========================================
        //  SYSTEM / UI ALERTS
        // ==========================================
        private async void ShowStatus(string message, bool isError = false)
        {
            TxtStatusMessage.Text = message;

            if (isError)
            {
                StatusBanner.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242)); // Red 50
                TxtStatusMessage.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38)); // Red 600
            }
            else
            {
                StatusBanner.Background = new SolidColorBrush(Color.FromRgb(238, 242, 255)); // Indigo 50
                TxtStatusMessage.Foreground = new SolidColorBrush(Color.FromRgb(79, 70, 229)); // Indigo 600
            }

            StatusBanner.Visibility = Visibility.Visible;

            // Wait 5 seconds then hide
            await Task.Delay(5000);

            // Hide only if the message hasn't changed in between
            if (TxtStatusMessage.Text == message)
            {
                StatusBanner.Visibility = Visibility.Collapsed;
            }
        }
    }
}

namespace SimpleFarkleApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
            // Przejście do strony gry (docelowo zmień na stronę gry)
            DisplayAlert("Start", "Rozpoczynamy grę!", "OK");
        }

        private void OnSettingsClicked(object sender, EventArgs e)
        {
            // Przejście do ustawień (docelowo zamień na nawigację do strony ustawień)
            DisplayAlert("Ustawienia", "Otwieram ustawienia.", "OK");
        }

        private void OnInfoClicked(object sender, EventArgs e)
        {
            // Wyświetlenie informacji o grze
            DisplayAlert("Informacje", "Farkle - gra w kości!", "OK");
        }
    }
}
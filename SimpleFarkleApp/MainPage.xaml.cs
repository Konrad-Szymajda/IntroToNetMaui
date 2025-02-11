namespace SimpleFarkleApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnStartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage());
        }

        private void OnSettingsClicked(object sender, EventArgs e)
        {
            DisplayAlert("Ustawienia", "Otwieram ustawienia.", "OK");
        }

        private void OnInfoClicked(object sender, EventArgs e)
        {
            DisplayAlert("Informacje", "Farkle - gra w kości!", "OK");
        }
    }
}

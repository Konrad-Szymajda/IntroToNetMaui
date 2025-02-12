namespace SimpleFarkleApp
{
    public partial class GamePage : ContentPage
    {
        private Random _random = new Random();
        private int _player1Score = 0;
        private int _player2Score = 0;
        private int _roundPoints = 0;
        private int _currentPlayer = 1;

        public GamePage()
        {
            InitializeComponent();
        }

        private void OnRollDiceClicked(object sender, EventArgs e)
        {
            // Losowanie kości
            var diceImages = new[] { "dice1.png", "dice2.png", "dice3.png", "dice4.png", "dice5.png", "dice6.png" };
            var diceValues = new[] { 1, 2, 3, 4, 5, 6 };

            var diceRolled = new List<int>();

            // Losujemy kości i wyświetlamy
            var dieImages = new Image[] { Die1, Die2, Die3, Die4, Die5, Die6 };

            for (int i = 0; i < 6; i++)
            {
                int roll = _random.Next(0, diceImages.Length);
                dieImages[i].Source = diceImages[roll];
                diceRolled.Add(diceValues[roll]);
            }

            // Można tu dodać logikę do obliczania wyników na podstawie wyników kości.
            _roundPoints = diceRolled.Sum(); // Na razie sumujemy wszystkie wylosowane kości
            RoundTotal.Text = _roundPoints.ToString();
        }

        private void OnScoreAndContinueClicked(object sender, EventArgs e)
        {
            // Dodaj punkty do aktualnego gracza
            if (_currentPlayer == 1)
            {
                _player1Score += _roundPoints;
                Player1Score.Text = _player1Score.ToString();
            }
            else
            {
                _player2Score += _roundPoints;
                Player2Score.Text = _player2Score.ToString();
            }

            // Resetuj punkty rundy
            _roundPoints = 0;
            RoundTotal.Text = "Selected";

            // Przejdź do kolejnej tury
            _currentPlayer = _currentPlayer == 1 ? 2 : 1;
        }

        private void OnScoreAndPassClicked(object sender, EventArgs e)
        {
            // Przechodzi do kolejnego gracza bez dodawania punktów
            _roundPoints = 0;
            RoundTotal.Text = "Selected";

            // Przejdź do kolejnej tury
            _currentPlayer = _currentPlayer == 1 ? 2 : 1;
        }
    }
}

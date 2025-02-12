namespace SimpleFarkleApp
{
    public partial class GamePage : ContentPage
    {
        private Random _random = new Random();
        private int _player1Score = 0;
        private int _player2Score = 0;
        private int _roundPoints = 0;
        private int _currentPlayer = 1;
        private Dictionary<Image, int> dieValues = new Dictionary<Image, int>();
        private HashSet<Image> selectedDice = new HashSet<Image>();

        public GamePage()
        {
            InitializeComponent();
            AttachTapHandlers();
        }

        private void AttachTapHandlers()
        {
            var dieImages = new Image[] { Die1, Die2, Die3, Die4, Die5, Die6 };
            foreach (var die in dieImages)
            {
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += OnDieTapped;
                die.GestureRecognizers.Add(tapGesture);
            }
        }

        private void OnDieTapped(object? sender, TappedEventArgs e)
        {
            if (sender is Image die)
            {
                if (selectedDice.Contains(die))
                {
                    selectedDice.Remove(die);
                    die.Scale = 1.0;
                }
                else
                {
                    selectedDice.Add(die);
                    die.Scale = 1.20;
                }
                CalculateSelectedScore();
            }
        }

        private void OnRollDiceClicked(object sender, EventArgs e)
        {
            var diceImages = new[] { "dice1.png", "dice2.png", "dice3.png", "dice4.png", "dice5.png", "dice6.png" };
            var diceValues = new[] { 1, 2, 3, 4, 5, 6 };
            var dieImages = new Image[] { Die1, Die2, Die3, Die4, Die5, Die6 };

            selectedDice.Clear();
            dieValues.Clear();

            for (int i = 0; i < 6; i++)
            {
                int roll = _random.Next(0, diceImages.Length);
                dieImages[i].Source = diceImages[roll];
                dieValues[dieImages[i]] = diceValues[roll];
                dieImages[i].Scale = 1.0;
            }

            RoundTotal.Text = "0";
        }

        private void CalculateSelectedScore()
        {
            var selectedValues = selectedDice.Select(d => dieValues[d]).ToList();
            selectedValues.Sort();
            int score = 0;
            var counts = selectedValues.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

            if (selectedValues.SequenceEqual(new List<int> { 1, 2, 3, 4, 5, 6 })) score += 1500;
            else if (selectedValues.SequenceEqual(new List<int> { 1, 2, 3, 4, 5 })) score += 500;
            else if (selectedValues.SequenceEqual(new List<int> { 2, 3, 4, 5, 6 })) score += 750;
            else
            {
                foreach (var kvp in counts)
                {
                    int die = kvp.Key;
                    int count = kvp.Value;
                    if (die == 1)
                    {
                        if (count >= 3) score += 1000 * (int)Math.Pow(2, count - 3);
                        else score += count * 100;
                    }
                    else if (die == 5)
                    {
                        if (count >= 3) score += 500 * (int)Math.Pow(2, count - 3);
                        else score += count * 50;
                    }
                    else if (count >= 3)
                    {
                        score += die * 100 * (int)Math.Pow(2, count - 3);
                    }
                }
            }
            RoundTotal.Text = score.ToString();
        }

        private void OnScoreAndContinueClicked(object sender, EventArgs e)
        {
            int scoredPoints = int.Parse(RoundTotal.Text);

            if (_currentPlayer == 1)
            {
                _player1Score += scoredPoints;
                Player1Score.Text = _player1Score.ToString();
            }
            else
            {
                _player2Score += scoredPoints;
                Player2Score.Text = _player2Score.ToString();
            }

            _roundPoints = 0;
            RoundTotal.Text = "0";
            selectedDice.Clear();

            _currentPlayer = _currentPlayer == 1 ? 2 : 1;
        }

        private void OnScoreAndPassClicked(object sender, EventArgs e)
        {
            _roundPoints = 0;
            RoundTotal.Text = "0";
            selectedDice.Clear();

            _currentPlayer = _currentPlayer == 1 ? 2 : 1;
        }
    }
}
﻿namespace SimpleFarkleApp
{
    public partial class GamePage : ContentPage
    {
        private Random _random = new Random();
        private int _player1Score = 0;
        private int _player2Score = 0;
        private int _currentPlayer = 1;
        private Dictionary<Image, int> dieValues = new Dictionary<Image, int>();
        private HashSet<Image> selectedDice = new HashSet<Image>();

        public GamePage()
        {
            InitializeComponent();
            SetDiceToSleep();
            AttachTapHandlers();
        }

        private void SetDiceToSleep()
        {
            var dieImages = new Image[] { Die1, Die2, Die3, Die4, Die5, Die6 };
            foreach (var die in dieImages)
            {
                die.Source = "sleepydice.png";
                die.IsEnabled = false;
            }
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
            if (sender is Image die && dieValues.ContainsKey(die))
            {
                int dieValue = dieValues[die];

                if (CanDieScore(dieValue))
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
        }

        private bool CanDieScore(int dieValue)
        {
            var availableValues = dieValues.Values.ToList();
            var counts = availableValues.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

            if (counts.ContainsKey(dieValue) && counts[dieValue] >= 3)
            {
                return true;
            }
            var sortedAvailableValues = availableValues.Distinct().OrderBy(x => x).ToList();
            if (sortedAvailableValues.SequenceEqual(new List<int> { 1, 2, 3, 4, 5 }) ||
                sortedAvailableValues.SequenceEqual(new List<int> { 1, 2, 3, 4, 5, 6 }) ||
                sortedAvailableValues.SequenceEqual(new List<int> { 2, 3, 4, 5, 6 }))
            {
                return true;
            }

            if (dieValue == 1 || dieValue == 5) return true;

            return false;
        }

        private void OnRollDiceClicked(object sender, EventArgs e)
        {
            var diceImages = new[] { "dice1.png", "dice2.png", "dice3.png", "dice4.png", "dice5.png", "dice6.png" };
            var dieImagesArray = new Image[] { Die1, Die2, Die3, Die4, Die5, Die6 };

            selectedDice.Clear();
            dieValues.Clear();

            for (int i = 0; i < 6; i++)
            {
                int roll = _random.Next(0, diceImages.Length);
                dieImagesArray[i].Source = diceImages[roll];
                dieValues[dieImagesArray[i]] = roll + 1;
                dieImagesArray[i].Scale = 1.0;
                dieImagesArray[i].IsEnabled = true;
            }
        }

        private void OnRollDiceSleep(object sender, EventArgs e)
        {
            foreach (var die in selectedDice)
            {
                die.Source = "sleepydice.png";
                die.IsEnabled = false;
            }
            selectedDice.Clear();
        }

        private void CalculateSelectedScore()
        {
            var selectedValues = selectedDice
                .Where(d => dieValues.ContainsKey(d))
                .Select(d => dieValues[d])
                .ToList();
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
                    if (die == 1) score += count >= 3 ? 1000 * (int)Math.Pow(2, count - 3) : count * 100;
                    else if (die == 5) score += count >= 3 ? 500 * (int)Math.Pow(2, count - 3) : count * 50;
                    else if (count >= 3) score += die * 100 * (int)Math.Pow(2, count - 3);
                }
            }
            if (_currentPlayer == 1) Player1Selected.Text = score.ToString();
            else Player2Selected.Text = score.ToString();
        }

        private void OnScoreAndContinueClicked(object sender, EventArgs e)
        {
            int scoredPoints = 0;

            if (_currentPlayer == 1)
            {
                scoredPoints = int.Parse(Player1Selected.Text);
                _player1Score += scoredPoints;
                Player1Score.Text = _player1Score.ToString();
                Player1Selected.Text = "0";
            }
            else
            {
                scoredPoints = int.Parse(Player2Selected.Text);
                _player2Score += scoredPoints;
                Player2Score.Text = _player2Score.ToString();
                Player2Selected.Text = "0";
            }

            OnRollDiceSleep(sender, e);
        }

        private void OnScoreAndPassClicked(object sender, EventArgs e)
        {
            selectedDice.Clear();
            _currentPlayer = _currentPlayer == 1 ? 2 : 1;
        }
    }
}

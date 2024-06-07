namespace DwaCzteroZeryOsiem
{
    public partial class MainPage : ContentPage
    {
        private int[,] board = new int[4, 4];
        private Random r;
        private List<(int, int)> free;
        private bool start;
        private int wynik;
        private IDispatcherTimer timer;
        private TimeSpan czas;
        private bool canMove;
        public MainPage()
        {
            InitializeComponent();

            start = false;
            board = new int[4, 4];
            r = new Random();
            free = new List<(int, int)>();

            czas = new TimeSpan(0, 0, 0);
            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                czas = czas.Add(new TimeSpan(0, 0, 1));
                scoreAndTime.Text = $"Wynik: {wynik}   Czas: {czas.Minutes.ToString("D2")}:{czas.Seconds.ToString("D2")}";
            };
        }

        /* OBSŁUGA KLAWISZY */
        private void KeyClicked(object sender, EventArgs e)
        {
            string keyClicked = ((MenuFlyoutItem)sender).Text;

            if (!canMove) return; 

            switch (keyClicked)
            {
                case "Up": GoUp(); break;
                case "Down": GoDown(); break;
                case "Left": GoLeft(); break;
                case "Right": GoRight(); break;
                default: WrongKey(sender, e); break;
            }

            NewTile();
            GenerateGame();
        }

        /* DODAWANIE NOWEJ LICZBY NA POLE GRY */
        private void NewTile()
        {
            free.Clear();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (board[i, j] == 0) free.Add((i, j));

            if (free.Count == 0)
            {
                canMove = false;
                startBtn.Text = "You lost, try again";
                startBtn.IsVisible = true;
                timer.Stop();
                return;
            }

            int newPlace = r.Next(free.Count);
            int newNumber = (r.Next(10) == 9) ? 4 : 2;
            board[free[newPlace].Item1, free[newPlace].Item2] = newNumber;
        }

        /* ROZPOCZĘCIE GRY */
        private void StartBtn(object sender, EventArgs e)
        {
            board = new int[4, 4];
            start = true;
            canMove = true;
            wynik = 0;
            r = new Random();
            czas = new TimeSpan(0, 0, 0);

            startBtn.IsVisible = false;

            var rStart = (r.Next(4), r.Next(4));
            board[rStart.Item1, rStart.Item2] = 2;

            timer.Start();
            GenerateGame();
        }

        /* GENEROWANIE PLANSZY */
        private void GenerateGame()
        {
            gameBoard.Clear();

            if (!start) return;

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    var blok = new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        HeightRequest = 100,
                        WidthRequest = 100,
                        Text = board[i, j].ToString()
                    };

                    switch (board[i, j])
                    {
                        case 0: blok.BackgroundColor = Color.FromArgb("#ffe8d2ae"); blok.Text = ""; break;
                        case 2: blok.BackgroundColor = Color.FromArgb("#ffa3c392"); break;
                        case 4: blok.BackgroundColor = Color.FromArgb("#ffd1c770"); break;
                        case 8: blok.BackgroundColor = Color.FromArgb("#ffffca4e"); break;
                        case 16: blok.BackgroundColor = Color.FromArgb("#ff99d36c"); break;
                        case 32: blok.BackgroundColor = Color.FromArgb("#ff33db8a"); break;
                        case 64: blok.BackgroundColor = Color.FromArgb("#ff839ab5"); break;
                        case 128: blok.BackgroundColor = Color.FromArgb("#ffab79cb"); break;
                        case 256: blok.BackgroundColor = Color.FromArgb("#ffd358e0"); break;
                        case 512: blok.BackgroundColor = Color.FromArgb("#ffd767e3"); break;
                        case 1024: blok.BackgroundColor = Color.FromArgb("#ffde70ab"); break;
                        case 2048: blok.BackgroundColor = Color.FromArgb("#ffe47973"); break;
                        default: blok.BackgroundColor = Color.FromArgb("#fff5aa77"); break;
                    }

                    gameBoard.Add(blok, i, j);
                }
        }

        private void WrongKey(object s, EventArgs e)
        {
            DisplayAlert("Error", $"{((MenuFlyoutItem)s).Text} isn't supported", "OK");
        }

        /* OBSŁUGA DANEGO KIERUNKU */
        private void GoLeft()
        {
            for (int col = 0; col < 4; col++)
            {
                int[] tempCol = new int[4];
                int idx = 0;
                bool zamiana = false;

                for (int row = 0; row < 4; row++)
                {
                    if (board[row, col] != 0)
                    {
                        if (idx > 0 && tempCol[idx - 1] == board[row, col] && !zamiana)
                        {
                            tempCol[idx - 1] *= 2;
                            wynik += tempCol[idx - 1];
                            zamiana = true;
                        }
                        else
                        {
                            tempCol[idx] = board[row, col];
                            idx++;
                            zamiana = false;
                        }
                    }
                }

                for (int row = 0; row < 4; row++)
                    board[row, col] = tempCol[row];
            }
        }

        private void GoRight()
        {
            for (int col = 0; col < 4; col++)
            {
                int[] tempCol = new int[4];
                int idx = 0;
                bool zamiana = false;

                for (int row = 3; row >= 0; row--)
                {
                    if (board[row, col] != 0)
                    {
                        if (idx > 0 && tempCol[idx - 1] == board[row, col] && !zamiana)
                        {
                            tempCol[idx - 1] *= 2;
                            wynik += tempCol[idx - 1];
                            zamiana = true;
                        }
                        else
                        {
                            tempCol[idx] = board[row, col];
                            idx++;
                            zamiana = false;
                        }
                    }
                }

                for (int row = 3; row >= 0; row--)
                    board[row, col] = tempCol[3 - row];
            }
        }

        private void GoUp()
        {
            for (int row = 0; row < 4; row++)
            {
                int[] tempRow = new int[4];
                int idx = 0;
                bool zamiana = false;

                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] != 0)
                    {
                        if (idx > 0 && tempRow[idx - 1] == board[row, col] && !zamiana)
                        {
                            tempRow[idx - 1] *= 2;
                            wynik += tempRow[idx - 1];
                            zamiana = true;
                        }
                        else
                        {
                            tempRow[idx] = board[row, col];
                            idx++;
                            zamiana = false;
                        }
                    }
                }

                for (int col = 0; col < 4; col++)
                    board[row, col] = tempRow[col];
            }
        }

        private void GoDown()
        {
            for (int row = 0; row < 4; row++)
            {
                int[] tempRow = new int[4];
                int idx = 0;
                bool zamiana = false;

                for (int col = 3; col >= 0; col--)
                {
                    if (board[row, col] != 0)
                    {
                        if (idx > 0 && tempRow[idx - 1] == board[row, col] && !zamiana)
                        {
                            tempRow[idx - 1] *= 2;
                            wynik += tempRow[idx - 1];
                            zamiana = true;
                        }
                        else
                        {
                            tempRow[idx] = board[row, col];
                            idx++;
                            zamiana = false;
                        }
                    }
                }

                for (int col = 3; col >= 0; col--)
                    board[row, col] = tempRow[3 - col];
            }
        }
    }
}

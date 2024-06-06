using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace dwazeroczteryosiem
{
    public partial class MainPage : ContentPage
    {
        private int[,] board = new int[4, 4];
        private Random random = new Random();

        public MainPage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            AddRandomTile();
            AddRandomTile();
            UpdateUI();
        }

        private void AddRandomTile()
        {
            var emptyCells = from r in Enumerable.Range(0, 4)
                             from c in Enumerable.Range(0, 4)
                             where board[r, c] == 0
                             select new { Row = r, Col = c };

            if (emptyCells.Any())
            {
                var cell = emptyCells.ElementAt(random.Next(emptyCells.Count()));
                board[cell.Row, cell.Col] = random.NextDouble() < 0.9 ? 2 : 4;
            }
        }

        private void UpdateUI()
        {
            Cell00.Text = board[0, 0] == 0 ? "" : board[0, 0].ToString();
            Cell01.Text = board[0, 1] == 0 ? "" : board[0, 1].ToString();
            Cell02.Text = board[0, 2] == 0 ? "" : board[0, 2].ToString();
            Cell03.Text = board[0, 3] == 0 ? "" : board[0, 3].ToString();
            Cell10.Text = board[1, 0] == 0 ? "" : board[1, 0].ToString();
            Cell11.Text = board[1, 1] == 0 ? "" : board[1, 1].ToString();
            Cell12.Text = board[1, 2] == 0 ? "" : board[1, 2].ToString();
            Cell13.Text = board[1, 3] == 0 ? "" : board[1, 3].ToString();
            Cell20.Text = board[2, 0] == 0 ? "" : board[2, 0].ToString();
            Cell21.Text = board[2, 1] == 0 ? "" : board[2, 1].ToString();
            Cell22.Text = board[2, 2] == 0 ? "" : board[2, 2].ToString();
            Cell23.Text = board[2, 3] == 0 ? "" : board[2, 3].ToString();
            Cell30.Text = board[3, 0] == 0 ? "" : board[3, 0].ToString();
            Cell31.Text = board[3, 1] == 0 ? "" : board[3, 1].ToString();
            Cell32.Text = board[3, 2] == 0 ? "" : board[3, 2].ToString();
            Cell33.Text = board[3, 3] == 0 ? "" : board[3, 3].ToString();
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            bool boardChanged = false;

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    boardChanged = SlideLeft();
                    break;
                case SwipeDirection.Right:
                    boardChanged = SlideRight();
                    break;
                case SwipeDirection.Up:
                    boardChanged = SlideUp();
                    break;
                case SwipeDirection.Down:
                    boardChanged = SlideDown();
                    break;
            }

            if (boardChanged)
            {
                AddRandomTile();
                UpdateUI();
                if (CheckGameOver())
                {
                    DisplayAlert("Game Over", "No more moves available!", "OK");
                }
            }
        }

        private bool SlideLeft()
        {
            bool boardChanged = false;

            for (int r = 0; r < 4; r++)
            {
                int[] row = new int[4];
                int idx = 0;

                for (int c = 0; c < 4; c++)
                {
                    if (board[r, c] != 0)
                    {
                        row[idx++] = board[r, c];
                    }
                }

                for (int c = 0; c < 3; c++)
                {
                    if (row[c] != 0 && row[c] == row[c + 1])
                    {
                        row[c] *= 2;
                        row[c + 1] = 0;
                    }
                }

                int[] newRow = new int[4];
                idx = 0;

                for (int c = 0; c < 4; c++)
                {
                    if (row[c] != 0)
                    {
                        newRow[idx++] = row[c];
                    }
                }

                for (int c = 0; c < 4; c++)
                {
                    if (board[r, c] != newRow[c])
                    {
                        board[r, c] = newRow[c];
                        boardChanged = true;
                    }
                }
            }

            return boardChanged;
        }

        private bool SlideRight()
        {
            RotateBoardRight();
            bool boardChanged = SlideLeft();
            RotateBoardLeft();
            return boardChanged;
        }

        private bool SlideUp()
        {
            RotateBoardLeft();
            bool boardChanged = SlideLeft();
            RotateBoardRight();
            return boardChanged;
        }

        private bool SlideDown()
        {
            RotateBoardRight();
            RotateBoardRight();
            bool boardChanged = SlideLeft();
            RotateBoardLeft();
            RotateBoardLeft();
            return boardChanged;
        }

        private void RotateBoardRight()
        {
            int[,] newBoard = new int[4, 4];

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    newBoard[c, 3 - r] = board[r, c];
                }
            }

            board = newBoard;
        }

        private void RotateBoardLeft()
        {
            int[,] newBoard = new int[4, 4];

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    newBoard[3 - c, r] = board[r, c];
                }
            }

            board = newBoard;
        }

        private bool CheckGameOver()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (board[r, c] == 0) return false;
                    if (c < 3 && board[r, c] == board[r, c + 1]) return false;
                    if (r < 3 && board[r, c] == board[r + 1, c]) return false;
                }
            }
            return true;
        }
    }
}

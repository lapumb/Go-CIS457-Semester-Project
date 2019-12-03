using System;
using Xamarin.Forms;

namespace Go.Model
{
    public class GoGame
    {
        public GoPiece[,] GameGrid { get; set; }


        // When (turn % 2) == 0 it is Black's (player 1's) turn, otherwise it is White's (player 2's)
        public int Turn { get; set; } = 0;
        public string Opponent { get; set; }
        public int blackScore;
        public int whiteScore;
        public int myColor;
        public int whatever = 0;

        public GoGame(int boardSize)
        {
            GameGrid = new GoPiece[boardSize, boardSize];
            blackScore = 0;
            whiteScore = 0;
            InitBoard(boardSize);
        }

  
        private void InitBoard(int boardSize)
        {
            for (int c = 0; c < boardSize; c++)
            {
                for (int r = 0; r < boardSize; r++)
                {
                    GameGrid[c, r] = new GoPiece(this);
                }
            }
        }

        public void PerformMove()
        {
            if ((Turn % 2) == 0)
            {
                blackScore += CheckForCaptures();
            } else
            {
                whiteScore += CheckForCaptures();
            }
        }

        public void WaitForUserMove()
        {
           // App.GameViewModel.Running = true;
            string opponentMove = Connection.Instance.Receive();
            string[] move = opponentMove.Split();
            if(move[0] == "MOVE")
            {
            System.Diagnostics.Debug.WriteLine(opponentMove);
            int row = Int32.Parse(move[2]);
            int col = Int32.Parse(move[3]);
            GoPiece oppPiece = GameGrid[row, col];
            Button oppBtn = oppPiece.GetPiece();
            oppPiece.BtnClick(this, oppBtn);
            this.Turn = Int32.Parse(move[4]);
            }
            else if(move[0] == "QUIT")
            {
                GameOver();
            }
            //App.GameViewModel.Running = false;
        }

        /**
         * Checks tokens that owned by the not active player. If they belong to a
         * group with no liberties, they are captured and added up for the active
         * player's score
         * @return Number of tokens removed by capture.
         */
        private int CheckForCaptures()
        {
            int numCaptures = 0;
            for (int c = 0; c < GameGrid.GetLength(0); c++)
            {
                for (int r = 0; r < GameGrid.GetLength(1); r++)
                {
                    if (Turn % 2 == 0)
                    {
                        if (GameGrid[c, r].GetColor() == Color.White)
                        {
                            if (!HasLiberties(c, r))
                            {
                                numCaptures += RemoveGroup(c, r);
                            }
                        }
                    } else
                    {
                        if (GameGrid[c, r].GetColor() == Color.Black)
                        {
                            if (!HasLiberties(c, r))
                            {
                                numCaptures += RemoveGroup(c, r);
                            }
                        }
                    }
                }
            }
            whatever = 0;
            return numCaptures;
        }

        /**
         * From a given starting point, determines whether a grouping of tokens are
         * surrounded, and thus captured, by enemy tokens.
         * @param  Column and row of the board to begin looking
         * @return true if the grouping of like colored tokens have an empty adjacency,
         * otherwise false.
         */
        private bool HasLiberties(int c, int r)
        {
            Color color = GameGrid[c, r].GetColor();
            whatever++;
            if (whatever < 100)
            {
                if (c > 0)
                {
                    if (GameGrid[c - 1, r].GetColor() == color)
                    {
                        if (HasLiberties(c - 1, r))
                        {
                            return true;
                        }
                    }
                    else if (GameGrid[c - 1, r].GetColor() == Color.Transparent)
                    {
                        return true;
                    }
                }
                if (c < GameGrid.Length - 1)
                {
                    if (GameGrid[c + 1, r].GetColor() == color)
                    {
                        if (HasLiberties(c + 1, r))
                        {
                            return true;
                        }
                    }
                    else if (GameGrid[c + 1, r].GetColor() == Color.Transparent)
                    {
                        return true;
                    }
                }
                if (r > 0)
                {
                    if (GameGrid[c, r - 1].GetColor() == color)
                    {
                        if (HasLiberties(c, r - 1))
                        {
                            return true;
                        }
                    }
                    else if (GameGrid[c, r - 1].GetColor() == Color.Transparent)
                    {
                        return true;
                    }
                }
                if (r < GameGrid.Length - 1)
                {
                    if (GameGrid[c, r + 1].GetColor() == color)
                    {
                        if (HasLiberties(c, r + 1))
                        {
                            return true;
                        }
                    }
                    else if (GameGrid[c, r + 1].GetColor() == Color.Transparent)
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        /**
         * Starting from a given position, it removes all neighboring pieces of the same
         * color.
         * @param Starting location of group to remove.
         * @return Number of tokens removed this way.
         */
        private int RemoveGroup(int c, int r)
        {
            Color color = GameGrid[c, r].GetColor();
            //Captured starts at one to count itself
            int captured = 1;
            GameGrid[c, r].removePiece();
            if (c > 0)
            {
                if (GameGrid[c - 1, r].GetColor() == color)
                {
                    captured += RemoveGroup(c - 1, r);
                }
            }
            if (c < GameGrid.Length - 1)
            {
                if (GameGrid[c + 1, r].GetColor() == color)
                {
                    captured += RemoveGroup(c + 1, r);
                }
            }
            if (r > 0)
            {
                if (GameGrid[c, r - 1].GetColor() == color)
                {
                    captured += RemoveGroup(c, r - 1);
                }
            }
            if (r < GameGrid.Length - 1)
            {
                if (GameGrid[c, r + 1].GetColor() == color)
                {
                    captured += RemoveGroup(c, r + 1);
                }
            }
            return captured;
        }

        public void IncrementTurn()
        {
            Turn+=1;
            if (Turn >= GameGrid.Length)
                GameOver();
        }

        public int GetBlackCount()
        {
            int count = 0; 
            for (int c = 0; c < GameGrid.GetLength(0); c++)
            {
                for (int r = 0; r < GameGrid.GetLength(1); r++)
                {
                    if (GameGrid[c, r].GetColor() == Color.Black)
                        count++;
                }
            }

            return count; 
        }

        public int GetWhiteCount()
        {
            int count = 0;
            for (int c = 0; c < GameGrid.GetLength(0); c++)
            {
                for (int r = 0; r < GameGrid.GetLength(1); r++)
                {
                    if (GameGrid[c, r].GetColor() == Color.White)
                        count++;
                }
            }

            return count;
        }

        public async void GameOver()
        {
            blackScore += GetBlackCount();
            whiteScore += GetWhiteCount();
            string winner;
            if (blackScore > whiteScore)
            {
                winner = "Black Wins!";
            } else if (whiteScore > blackScore)
            {
                winner = "White Wins!";
            } else
            {
                winner = "Scores are tied. The game is a draw.";
            }

            await App.MainPg.DisplayAlert("Game Over", "The game is over. " + winner, "Okay");
            await App.MainPg.Navigation.PopAsync();
        }

    }

}

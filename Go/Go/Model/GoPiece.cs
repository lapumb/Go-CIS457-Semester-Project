using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace Go.Model
{/*
    public class GoPiece
    {
        private Button piece = new Button();
        private bool used = false; 
        public int row { get; set; }
        public int col { get; set; }
        public GoPiece(GoGame game)
        {
            int width = 33;  
            piece.WidthRequest = width;
            piece.HeightRequest = width;
            piece.CornerRadius = (width / 2);
            piece.HorizontalOptions = LayoutOptions.Center;
            piece.BorderWidth = 1;
            piece.BackgroundColor = Color.Transparent; //initially hide the button
            piece.BorderColor = Color.Transparent;
            piece.Pressed += (sender, args) =>
            {
                if (!used && (game.Turn % 2) == game.myColor)
                {
                    BtnClick(game, sender);
                    //WaitForUserMoveEvent(row, col, game.Turn, game.Opponent);
                }
            };
            piece.Released += (sender, args) =>
            {
                if ((game.Turn % 2) == game.myColor)
                {
                    game.IncrementTurn();
                    Connection.Instance.Send("MOVE " + game.Opponent + " " + row.ToString() + " " + col.ToString() + " " + game.Turn.ToString());
                    game.WaitForUserMove();
                }
            };
        }


        public void BtnClick(GoGame game, object sender)
        {
            if (!used)
            {
                var btn = sender as Button;
                Debug.WriteLine("Button clicked.");
                btn.BorderColor = Color.Black;
                btn.BackgroundColor = game.Turn % 2 == 0 ? Color.Black : Color.White;
                used = true;
                //Seems like this is being done again in PerformMove
               // game.Turn++;
                game.PerformMove();
            }
            else
                Debug.WriteLine("piece has already been used.");
        }

        public Button GetPiece()
        {
            return piece; 
        }

        public Color GetColor()
        {
            return piece.BackgroundColor; 
        }

        public void removePiece()
        {
            piece.BackgroundColor = Color.Transparent;
            piece.BorderColor = Color.Transparent;
        }
    }

    */
    public class GoPiece
    {
        private bool used { get; set; }
        private int row { get; set; }
        private int col { get; set; }
        private Color color { get; set; }

        public bool check { get; set; }

        public delegate void ColorUpdateEventHandler(object sender, ColorUpdateEventArgs e);
        public event ColorUpdateEventHandler OnColorUpdate;
        public GoPiece(int r, int c)
        {
            used = false;
            row = r;
            col = c;
            color = Color.Transparent;
        }
        public int GetRow()
        {
            return row;
        }
        public int GetCol()
        {
            return col;
        }
        public Color GetColor()
        {
            return color;
        }

        public bool Used()
        {
            return used;
        }

        public void RemovePiece()
        {
            color = Color.Transparent;
            used = false;
            OnColorUpdate(this, new ColorUpdateEventArgs(row, col));
        }
        public void placeBlack()
        {
            color = Color.Black;
            used = true;
            OnColorUpdate(this, new ColorUpdateEventArgs(row, col));
        }
        public void placeWhite()
        {
            color = Color.White;
            used = true;
            OnColorUpdate(this, new ColorUpdateEventArgs(row, col));
        }
    }
    public class ColorUpdateEventArgs : EventArgs
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public ColorUpdateEventArgs(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}

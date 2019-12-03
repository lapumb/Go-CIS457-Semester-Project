using System.Diagnostics;
using Xamarin.Forms;

namespace Go.Model
{
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
            piece.Clicked += (sender, args) =>
            {
                if (!used && (game.Turn % 2) == game.myColor)
                {
                    BtnClick(game, sender);
                if ((game.Turn % 2) == game.myColor)
                {
                    game.IncrementTurn();
                    Connection.Instance.Send("MOVE " + game.Opponent + " " + row.ToString() + " " + col.ToString() + " " + game.Turn.ToString());
                    game.WaitForUserMove();
                }
                    
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
}

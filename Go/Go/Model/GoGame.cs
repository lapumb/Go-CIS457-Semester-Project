using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Go.Model
{
    public class GoGame
    {
        public Dictionary<string, GoPiece> GameGrid { get; set; } = new Dictionary<string, GoPiece>();
        public int Turn { get; set; } = 0;
        public void IncrementTurn()
        {
            Turn++;
            if (Turn >= GameGrid.Count)
                GameOver();
        }

        public int GetBlackCount()
        {
            int count = 0; 
            foreach (KeyValuePair<string, GoPiece> piece in GameGrid)
            {
                if (piece.Value.GetColor() == Color.Black) 
                    count++; 
            }

            return count; 
        }

        public int GetWhiteCount()
        {
            int count = 0;
            foreach (KeyValuePair<string, GoPiece> piece in GameGrid)
            {
                if (piece.Value.GetColor() == Color.White)
                    count++;
            }

            return count;
        }

        public async void GameOver()
        {
            //do some shit about the game being over
            await App.MainPg.DisplayAlert("Game Over", "The game is over.", "Okay");
            await App.MainPg.Navigation.PopAsync();
        }
    }
}

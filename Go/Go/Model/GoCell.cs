using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Go.Model
{
    class GoCell : BoxView
    {
        public event EventHandler Tapped;

        public GoCell()
        {
            BackgroundColor = Color.White;

            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (sender, args) =>
            {
                Tapped?.Invoke(this, EventArgs.Empty);
            };
            GestureRecognizers.Add(tapGesture);
        }

        public int Col { set; get; }

        public int Row { set; get; }
    }
}
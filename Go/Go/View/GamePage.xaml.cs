using Go.ViewModel;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        //https://github.com/xamarin/xamarin-forms-samples/tree/master/BoxView/GameOfLife
        readonly int size;
        public static GamePage Game { get; set; } = null;
        public GamePage(int size = 5)
        {
            this.size = size;
            Game = this;
            InitializeComponent();
            InitLayout();
            GamePageViewController.GameController.Init();
        }

        /// <summary>
        /// Set the grid MxM size via user selection
        /// </summary>
        public void InitLayout()
        {
            //grid lines
            for (int i = 0; i < size; i++)
            {
                BoxView row = new BoxView();
                row.VerticalOptions = LayoutOptions.CenterAndExpand;
                row.HeightRequest = 1;
                row.Color = Color.Black;
                rowsLayout.Children.Add(row);

                BoxView col = new BoxView();
                col.VerticalOptions = LayoutOptions.FillAndExpand;
                col.WidthRequest = 1;
                col.Color = Color.Black;
                columnsLayout.Children.Add(col);
            }

            //circular buttons
            for (int i = 0; i < size + 2; i++)
            {
                AbsoluteLayout absolute = new AbsoluteLayout();
                absolute.VerticalOptions = LayoutOptions.FillAndExpand;
                absolute.HorizontalOptions = LayoutOptions.FillAndExpand;

                FlexLayout flex = new FlexLayout();
                flex.JustifyContent = FlexJustify.SpaceEvenly;
                flex.VerticalOptions = LayoutOptions.FillAndExpand;

                for (int j = 0; j < size + 2; j++)
                {
                    int width = 33;
                    Button button = new Button();
                    button.WidthRequest = width;
                    button.HeightRequest = width;
                    button.CornerRadius = (width / 2);
                    button.HorizontalOptions = LayoutOptions.Center;
                    button.BorderWidth = 1;
                    button.BackgroundColor = Color.Transparent; //initially hide the button
                    button.BorderColor = Color.Transparent;
                    button.Clicked += async (sender, args) =>
                    {
                        var btn = sender as Button;
                        Debug.WriteLine("Button clicked.");
                        btn.BorderColor = Color.Black;
                        btn.BackgroundColor = GamePageViewController.Turn % 2 == 0 ? Color.Black : Color.White;
                        btn.Clicked += null;
                        GamePageViewController.Turn++;
                    };
                    GamePageViewController.GameGrid.Add("" + i + j, button);
                    flex.Children.Add(button);
                }
                absolute.Children.Add(flex);
                var scalar = 10 - (size % 10);
                scalar = scalar == 1 ? 2 : scalar;
                buttonsLayout.Padding = new Thickness(scalar * -8, scalar * -12, scalar * -8, scalar * -12);
                buttonsLayout.Children.Add(absolute);
            }
        }
    }
}
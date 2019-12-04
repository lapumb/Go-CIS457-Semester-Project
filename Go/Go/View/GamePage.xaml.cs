using Go.Model;
using System.Diagnostics;
using System.Net.Sockets;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        //https://github.com/xamarin/xamarin-forms-samples/tree/master/BoxView/GameOfLife
        public readonly int size;
        public GoGame Game { get; set; }
        public TcpClient Client { get; set; }

        public Button[,] buttons { get; set; }
        
        public void OpponentSet(string opponent)
        {
            Game.Opponent = opponent;
        }
        public bool MoveMade { get; set; } = false;

        public GamePage(int size = 5)
        {
            this.size = size-1;
            Game = new GoGame(size+2);
            buttons = new Button[(size + 2),(size + 2)];
            InitializeComponent();
        }

        public void WaitForUserMove()
        {
            Game.WaitForUserMove();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.GamePg = this;
            App.Master.IsGestureEnabled = false;
            InitLayout();
            Game.OnMove += (sender, args) =>
            {
                MoveMade = true;
            };
            Game.OnTurnChange += (sender, args) =>
            {
                if (Game.Turn % 2 == Game.myColor)
                {
                    TurnLabel.Text = "Your Turn";
                }
                else
                {
                    TurnLabel.Text = "Opponent Turn";
                }
            };
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Connection.Instance.ClosingSequence(Game.Opponent); 
            App.GamePg = null;
            App.Master.IsGestureEnabled = true;
        }

        /// <summary>
        /// Set the grid MxM size via user selection, populate with transparent buttons
        /// </summary> rowsLayout,columnsLayout,buttonsLayout
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
                    GoPiece piece = Game.GameGrid[i, j];
                    GoPieceButton pieceButton = new GoPieceButton(piece);
                    pieceButton.GetButton().IsEnabled = true;
                    flex.Children.Add(pieceButton.GetButton());
                    buttons[i, j] = pieceButton.GetButton(); 
                }
                absolute.Children.Add(flex);
                var scalar = 0; 
                if (Utilities.Utilities.DeviceIsAndroid() || Utilities.Utilities.DeviceIsIphone())
                {
                    Debug.WriteLine("Init for Android/iOS device.");
                    scalar = 10 - (size % 10);
                    scalar = scalar == 1 ? 2 : scalar;
                }
                else if (Utilities.Utilities.DeviceIsUWP())
                {
                    Debug.WriteLine("Init for UWP device.");
                    scalar = 12 - (size % 10);
                    scalar = scalar == 2 ? 3 : scalar;
                }
                buttonsLayout.Padding = new Thickness(scalar * -8, scalar * -12, scalar * -8, scalar * -12);
                buttonsLayout.Children.Add(absolute);
            }
        }
    }
    public class GoPieceButton
    {
        private Button pieceButton = new Button();
        private Model.GoPiece piece { get; set; }
        public GoPieceButton(Model.GoPiece p)
        {
            piece = p;
            int width = 33;
            pieceButton.WidthRequest = width;
            pieceButton.HeightRequest = width;
            pieceButton.CornerRadius = (width / 2);
            pieceButton.HorizontalOptions = LayoutOptions.Center;
            pieceButton.BorderWidth = 1;
            pieceButton.BackgroundColor = piece.GetColor(); //initially hide the button
            pieceButton.BorderColor = Color.Transparent;
            pieceButton.Pressed += (sender, args) =>
            {
                App.GamePg.Game.PlaceStone(piece.GetRow(), piece.GetCol());
            };
            pieceButton.Released += (sender, args) =>
            {
                if (App.GamePg.MoveMade)
                {
                    App.GamePg.MoveMade = false;
                    App.GamePg.Game.SendMove(piece.GetRow(), piece.GetCol());
                }
            };
            piece.OnColorUpdate += (sender, args) =>
            {
                pieceButton.BackgroundColor = piece.GetColor();
                if (piece.GetColor() == Color.Transparent)
                {
                    pieceButton.BorderColor = Color.Transparent;
                }
                else
                {
                    pieceButton.BorderColor = Color.Black;
                }
            };
        }
        public Button GetButton()
        {
            return pieceButton;
        }
    }
}
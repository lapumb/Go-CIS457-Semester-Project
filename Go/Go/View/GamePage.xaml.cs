﻿using Go.Model;
using System;
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
        readonly int size;
        public GoGame Game { get; set; } = new GoGame();
        public TcpClient Client { get; set; }
        public GamePage(int size = 5)
        {
            //App.GamePg = this;
            this.size = size-1;
            //App.Master.IsGestureEnabled = false; 
            InitializeComponent();
            //InitLayout(); 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.GamePg = this;
            App.Master.IsGestureEnabled = false;
            InitLayout();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing(); 
            if (Client.Connected)
            {
                Client.Close();
                Client.Dispose();
                return;
            }

            Client.Dispose();
            App.GamePg = null;
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
                    GoPiece piece = new GoPiece(Game); 
                    Game.GameGrid.Add("" + i + j, piece);
                    flex.Children.Add(piece.GetPiece());
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
                    Debug.WriteLine("Init for android device.");
                    scalar = 12 - (size % 10);
                    scalar = scalar == 2 ? 3 : scalar;
                }
                buttonsLayout.Padding = new Thickness(scalar * -8, scalar * -12, scalar * -8, scalar * -12);
                buttonsLayout.Children.Add(absolute);
            }
        }
    }
}
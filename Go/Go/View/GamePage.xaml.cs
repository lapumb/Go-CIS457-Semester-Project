using Go.Model;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        //https://github.com/xamarin/xamarin-forms-samples/tree/master/BoxView/GameOfLife


        const int CellSpacing = 2;

        // Generating too many BoxView elements can impact performance, 
        //      particularly on iOS devices.
        const int MaxCellCount = 361; //19x19

        // Calculated during SizeChanged event 
        int cols;
        int rows;
        int cellSize;
        int xMargin;
        int yMargin;
        readonly int size; 

        GoGrid goGrid = new GoGrid();

        public GamePage(int size)
        {
            this.size = size; 
            InitializeComponent();
        }

        void OnLayoutSizeChanged(object sender, EventArgs args)
        {
            Layout layout = sender as Layout;
            rows = size;
            cols = size;

            if (cols * rows > MaxCellCount)
            {
                cellSize = (int)Math.Sqrt((layout.Width * layout.Height) / MaxCellCount);
                cols = (int)(layout.Width / cellSize);
                rows = (int)(layout.Height / cellSize);
            }
            else
            {
                cellSize = (int)Math.Min(layout.Width / cols, layout.Height / rows);
            }

            xMargin = (int)((layout.Width - cols * cellSize) / 2);
            yMargin = (int)((layout.Height - rows * cellSize) / 2);

            if (cols > 0 && rows > 0)
            {
                goGrid.SetSize(cols, rows);
                UpdateLayout();
            }
        }

        void UpdateLayout()
        {
            int count = rows * cols;

            System.Diagnostics.Debug.WriteLine("Count = " + count);

            // Remove unneeded GoCell children
            while (absoluteLayout.Children.Count > count)
            {
                absoluteLayout.Children.RemoveAt(0);
            }

            // Possibly add more GoCell children
            while (absoluteLayout.Children.Count < count)
            {
                GoCell GoCell = new GoCell();
                GoCell.Tapped += OnTapGestureTapped;
                absoluteLayout.Children.Add(GoCell);
            }

            int index = 0;

            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                {
                    GoCell GoCell = GoCell = (GoCell)absoluteLayout.Children[index];
                    GoCell.Col = x;
                    GoCell.Row = y;

                    Rectangle rect = new Rectangle(x * cellSize + xMargin + CellSpacing / 2,
                                                   y * cellSize + yMargin + CellSpacing / 2,
                                                   cellSize - CellSpacing,
                                                   cellSize - CellSpacing);

                    AbsoluteLayout.SetLayoutBounds(GoCell, rect);
                    index++;
                }
        }

        void OnTapGestureTapped(object sender, EventArgs args)
        {
            GoCell GoCell = (GoCell)sender;
            goGrid.SetStatus(GoCell.Col, GoCell.Row);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lab2_2
{
    interface IDrawable
    {
        void Draw(IDrawer drawer, bool border);
    }
    interface IDrawer
    {
        void Draw(int?[,] items, int numRows, int numCols, bool border);
    }
    class DrawerToConsole:IDrawer
    {
        
        public void Draw(int?[,] items, int numRows, int numCols, bool border)
        {
            if (border) Console.Write(new string('-', 8 * (numCols)));
            Console.WriteLine();
            for (int row = 0; row < numRows; row++)
            {
                if (border) Console.Write("|");
                        for (int col = 0; col < numCols; col++)
                {
                    if (items[row,col] != null) Console.Write(items[row,col] + "\t");
                    else Console.Write("\t");
                }
                if (border) Console.Write("|");
                Console.WriteLine();
            }
            if (border) Console.Write(new string('-', 8 * (numCols)));
            Console.WriteLine();
        }
    }

    class DrawerToWindow:IDrawer
    {
        public void Draw(int?[,] items, int numRows, int numCols, bool border)
        {
            Window window = new Window
            {
                Owner = Application.Current.MainWindow,
                SizeToContent = SizeToContent.WidthAndHeight,
                MaxHeight = 1000,
                MaxWidth = 1000
            };

            ScrollViewer scroll = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
            };
            Grid value = new Grid();
            for (int row = 0; row < numRows; row++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = GridLength.Auto;
                value.RowDefinitions.Add(newRow);
            }

            for (int col = 0; col < numCols; col++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = GridLength.Auto;
                value.ColumnDefinitions.Add(newCol);
            }

            for (int row = 0; row < numRows; row++)
                for (int col = 0; col < numCols; col++)
                {
                    TextBlock txt = new TextBlock();
                    if (items[row, col] != null) txt.Text = items[row, col].ToString();
                    else txt.Text = string.Empty;
                    txt.Padding = new Thickness(10);
                    Grid.SetColumn(txt, col);
                    Grid.SetRow(txt, row);
                    value.Children.Add(txt);
                }

            if (border)
            {
                Grid bord = CreateBorder(Brushes.Black, 5);
                Grid.SetRow(value, 0);
                Grid.SetColumn(value, 0);
                bord.Children.Add(value);
                scroll.Content = bord;
                window.Content = scroll;
            }
            else
            {
                scroll.Content = value;
                window.Content = scroll;
            }
            
            window.Show();
        }

        static Grid CreateBorder(Brush color, Double size)
        {
            Grid bord = new Grid();
            GridSplitter splitLeft = new GridSplitter
            {
                Background = color,
                Width = size,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            GridSplitter splitRight = new GridSplitter
            {
                Background = color,
                Width = size,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            GridSplitter splitTop = new GridSplitter
            {
                Background = color,
                Height = size,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            GridSplitter splitBot = new GridSplitter
            {
                Background = color,
                Height = size,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            
            bord.Children.Add(splitLeft);
            bord.Children.Add(splitRight);
            bord.Children.Add(splitTop);
            bord.Children.Add(splitBot);
            
            return bord;
        }
    }
}
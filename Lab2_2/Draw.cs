using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            DrawMatrix window = new DrawMatrix();
            window.Owner = Application.Current.MainWindow;

            Grid value = new Grid();
            for (int row = 0; row < numRows; row++)
                value.RowDefinitions.Add(new RowDefinition());
            for (int col = 0; col < numCols; col++)
                value.ColumnDefinitions.Add(new ColumnDefinition());

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
                Grid.SetRow(value, 0);
                Grid.SetColumn(value, 0);
                window.DefaultPlace.Children.Add(value);
            }
            else window.Content = value;
            
            window.Show();
        }
    }
}
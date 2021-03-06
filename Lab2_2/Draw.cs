﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace Lab2_2
{
    public interface IDrawable
    {
        void DrawItems(IDrawer drawer);
        void DrawBorder(IDrawer drawer);
    }
    public interface IDrawer
    {
        void Clear();
        void DrawBorder(IMatrix matrix);
        void DrawCell(int value, int row, int col);
    }

    class DrawerToConsole:IDrawer
    {
        void CheckBuffer(int row, int col)
        {
            if (Console.BufferWidth < 8 * col + 4)
                Console.BufferWidth = 8 * col + 4;
            if (Console.BufferHeight < row + 4)
                Console.BufferHeight = row + 4;
        }

        public void Clear()
        {
            Console.Clear();
        }
        public void DrawBorder(IMatrix matrix)
        {
            CheckBuffer(matrix.NumRows, matrix.NumColumns);
            Console.WriteLine(new string('-', 8 * (matrix.NumColumns) + 2));
            for (int i = 0; i < matrix.NumRows; i++)
            {
                Console.WriteLine("|");
                Console.SetCursorPosition( 8 * matrix.NumColumns + 2,i+1);
                Console.WriteLine("|");
            }
            Console.WriteLine(new string ('-', 8 * (matrix.NumColumns) +2));
        }
        public void DrawCell(int value, int row, int col)
        {
            CheckBuffer(row, col);
            Console.SetCursorPosition(col*8+1, row+1);
            Console.Write(value);
        }
    }
    class DrawerToWindow : IDrawer
    {
        Window window;
        Grid items;

        void CreateWindow()
        {
            window = new Window
            {
                Owner = Application.Current.MainWindow,
                SizeToContent = SizeToContent.WidthAndHeight,
                MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight * 0.9,
                MaxWidth = System.Windows.SystemParameters.PrimaryScreenHeight * 0.9,
                Topmost = false
            };
        }
        void CheckGrid(int numRows, int numCols)
        {
            if (items==null) items = new Grid();
            for (int row = items.RowDefinitions.Count; row <= numRows; row++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = GridLength.Auto;
                items.RowDefinitions.Add(newRow);
            }
            for (int col = items.ColumnDefinitions.Count; col <= numCols; col++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = GridLength.Auto;
                items.ColumnDefinitions.Add(newCol);
            }
        }
        void CreateBorder(Brush color, Double size)
        {
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

            Grid.SetColumn(splitLeft, 0);
            Grid.SetRowSpan(splitLeft, items.RowDefinitions.Count);
            items.Children.Add(splitLeft);
            Grid.SetColumn(splitRight, items.ColumnDefinitions.Count-1);
            Grid.SetRowSpan(splitRight, items.RowDefinitions.Count);
            items.Children.Add(splitRight);
            Grid.SetRow(splitTop, 0);
            Grid.SetColumnSpan(splitTop, items.ColumnDefinitions.Count);
            items.Children.Add(splitTop);
            Grid.SetRow(splitBot, items.RowDefinitions.Count-1);
            Grid.SetColumnSpan(splitBot, items.ColumnDefinitions.Count);
            items.Children.Add(splitBot);
        }
        void Show()
        {
            if (window == null)
                CreateWindow();
            ScrollViewer scroll = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
            };

            scroll.Content = items;
            window.Content = scroll;
            window.Show();
        }

        public void Clear()
        {
            if (window!=null)
                window.Close();
            
            window = null;
            items = null;
        }
        public void DrawBorder(IMatrix matrix)
        {
            CheckGrid(matrix.NumRows, matrix.NumColumns);
            CreateBorder(Brushes.Black, 5);
            Show();
        }
        public void DrawCell(int value, int row, int col)
        {
            CheckGrid(row, col);

            TextBlock txt = new TextBlock
            {
                Padding = new Thickness(5),
                Text = value.ToString(),
                ToolTip = new StringBuilder("Row: "+row+" Col: "+col)
            };
            Grid.SetColumn(txt, col);
            Grid.SetRow(txt, row);
            items.Children.Add(txt);
            Show();
        }
    }        
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static IMatrix matrix;
        static IDrawer[] drawers =
        {
            new DrawerToConsole(),
            new DrawerToWindow()
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        void DrawMatrix()
        {
            foreach (IDrawer dr in drawers)
            {
                dr.Clear();
            }
            if ((bool)border.IsChecked)
                foreach (IDrawer dr in drawers)
                {
                    matrix.DrawBorder(dr);
                }
            foreach (IDrawer dr in drawers)
            {
                matrix.DrawItems(dr);
            }
        }

        private void ButtonAddDefaultMatrix_Click(object sender, RoutedEventArgs e)
        {
            foreach (IDrawer dr in drawers)
            {
                dr.Clear();
            }
            matrix = new DefaultMatrix(20, 20);

            MatrixInitiator.FillMatrix(matrix, 200, 100);
            ThreadStart thStatr = new ThreadStart(DrawMatrix);
            this.DrawMatrix();
        }

        private void ButtonAddSparseMatrix_Click(object sender, RoutedEventArgs e)
        {
            foreach (IDrawer dr in drawers)
            {
                dr.Clear();
            }
            matrix = new SparseMatrix(20, 20);
            MatrixInitiator.FillMatrix(matrix, 200, 100);
            this.DrawMatrix();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (IDrawer dr in drawers)
            {
                dr.Clear();
            }
            if (matrix != null)
            {
                foreach (IDrawer dr in drawers)
                {
                    matrix.DrawBorder(dr);
                    matrix.DrawItems(dr);
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (IDrawer dr in drawers)
            {
                dr.Clear();
            }
            if (matrix != null)
            {
                foreach (IDrawer dr in drawers)
                {
                    matrix.DrawItems(dr);
                }
            }
        }
    }
}

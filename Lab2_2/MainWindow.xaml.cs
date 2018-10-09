using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading;

namespace Lab2_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static IMatrix matrix;
        static IDrawer[] drawers= {
            new DrawerToConsole(),
            new DrawerToWindow()
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        void DrawMatrix()
        {
            bool bord;
            if (border.IsChecked != null)
                bord = (bool)border.IsChecked;
            else bord = false;
            foreach (IDrawer dr in drawers)
            {
                matrix.Draw(dr, bord);
            }
        }

        private void ButtonAddDefaultMatrix_Click(object sender, RoutedEventArgs e)
        {
            matrix = new DefaultMatrix(10, 10);
            MatrixInitiator.FillMatrix(matrix, 25, 100);
            ThreadStart thStatr = new ThreadStart(DrawMatrix);
            this.DrawMatrix();
        }

        private void ButtonAddSparseMatrix_Click(object sender, RoutedEventArgs e)
        {
            matrix = new SparseMatrix(10, 10);
            MatrixInitiator.FillMatrix(matrix, 25, 100);
            this.DrawMatrix();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (matrix != null)
            {
                foreach (IDrawer dr in drawers)
                {
                    matrix.Draw(dr, true);
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (matrix != null)
            {
                foreach (IDrawer dr in drawers)
                {
                    matrix.Draw(dr, false);
                }
            }
        }
    }
}

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
            new DrawerToWindow(),
            new DrawerToConsole()
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

            Random r = new Random();
            IMatrix matrix1 = new DefaultMatrix(r.Next(1, 10), r.Next(1, 10));

            MatrixInitiator.FillMatrix(matrix1, matrix1.NumColumns * matrix1.NumRows, 100);

            if (matrix != null)
            {
                if ((bool)Horizontal.IsChecked)
                    matrix = new HorizontalCompositeMatrix(new List<IMatrix>() { matrix1, matrix });
                else if ((bool)Vertical.IsChecked)
                    matrix = new VerticalCompositeMatrix(new List<IMatrix>() { matrix1, matrix });
            }
            else matrix = matrix1;

            ThreadStart thStatr = new ThreadStart(DrawMatrix);
            this.DrawMatrix();
        }

        private void ButtonAddSparseMatrix_Click(object sender, RoutedEventArgs e)
        {
            foreach (IDrawer dr in drawers)
            {
                dr.Clear();
            }

            Random r = new Random();
            IMatrix matrix1 = new SparseMatrix(r.Next(1, 10), r.Next(1, 10));
            MatrixInitiator.FillMatrix(matrix1, matrix1.NumColumns * matrix1.NumRows, 100);

            if (matrix != null)
            {
                if ((bool)Horizontal.IsChecked)
                    matrix = new HorizontalCompositeMatrix(new List<IMatrix>() { matrix1, matrix });
                else if ((bool)Vertical.IsChecked)
                    matrix = new VerticalCompositeMatrix(new List<IMatrix>() { matrix1, matrix });
            }
            else matrix = matrix1;
            ThreadStart thStatr = new ThreadStart(DrawMatrix);
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

        private void RenumeratingMatrix(object sender, RoutedEventArgs e)
        {
            if (matrix == null) return;
            Random random = new Random();
            ChangeNumeratorMatrix decorMatix = new ChangeNumeratorMatrix(matrix);
            decorMatix.ChangeRow(random.Next(matrix.NumRows), random.Next(matrix.NumRows));
            matrix = decorMatix;
            DrawMatrix();
        }

        private void DenumeratingMatrix(object sender, RoutedEventArgs e)
        {
            if (matrix == null) return;
            matrix = matrix.getComponent();
            DrawMatrix();
        }

    }
}

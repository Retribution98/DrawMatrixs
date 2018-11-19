using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_2
{
    interface ICompositeMatrix: IMatrix
    {
        List<IMatrix> Elements { get; }
    }
    public class HorizontalCompositeMatrix : ICompositeMatrix
    {
        public List<IMatrix> Elements { get; private set; }
        public int NumColumns
        {
            get
            {
                int sum = 0;
                foreach (IMatrix matrix in Elements)
                {
                    sum += matrix.NumColumns;
                }
                return sum;
            }
        }
        public int NumRows
        {
            get
            {
                int max = 0;
                foreach (IMatrix matrix in Elements)
                {
                    max = (matrix.NumRows > max) ? matrix.NumRows : max;
                }
                return max;
            }
        }

        public HorizontalCompositeMatrix(List<IMatrix> matrices)
        {
            Elements = matrices;
        }
       
        public void Add(IMatrix newElement)
        {
            if (!Elements.Contains(newElement))
                Elements.Add(newElement);
        }
        public void DrawBorder(IDrawer drawer)
        {
            drawer.DrawBorder(this);  
        }
        public void DrawItems(IDrawer drawer)
        {
            Iterate((value, row, col) =>
            {
                drawer.DrawCell(value, row, col);
            });
        }
        public IMatrix getComponent()
        {
            return this;
        }
        public int GetValue(int row, int col)
        {
            foreach (IMatrix matrix in Elements)
            {
                if (matrix.NumColumns <= col)
                {
                    col -= matrix.NumColumns;
                }
                else
                {
                    if (matrix.NumRows > row)
                        return matrix.GetValue(row, col);
                    else return 0;
                }
            }
            return 0;
        }
        public void SetValue(int value, int row, int col)
        {
            foreach (IMatrix matrix in Elements)
            {
                if (matrix.NumColumns <= col)
                {
                    col -= matrix.NumColumns;
                }
                else
                {
                    if (matrix.NumRows > row)
                        matrix.SetValue(value, row, col);
                    throw new ArgumentOutOfRangeException();
                }
            }
            throw new ArgumentOutOfRangeException();
        }
        public void Iterate(DrawItemsDelegate drawItems)
        {
            int delta = 0;
            foreach (IMatrix matrix in Elements)
            {
                matrix.Iterate((value, row, col) => drawItems.Invoke(value, row, col + delta));
                delta += matrix.NumColumns;
            }
        }
    }
    public class VerticalCompositeMatrix : ICompositeMatrix
    {
        public List<IMatrix> Elements { get; private set; }
        public int NumColumns
        {
            get
            {
                int max = 0;
                foreach (IMatrix matrix in Elements)
                {
                    max = (matrix.NumColumns > max) ? matrix.NumColumns : max;
                }
                return max;
            }
        }
        public int NumRows
        {
            get
            {
                int sum = 0;
                foreach (IMatrix matrix in Elements)
                {
                    sum += matrix.NumRows;
                }
                return sum;
            }
        }

        public VerticalCompositeMatrix(List<IMatrix> matrices)
        {
            Elements = matrices;
        }

        public void Add(IMatrix newElement)
        {
            if (!Elements.Contains(newElement))
                Elements.Add(newElement);
        }
        public void DrawBorder(IDrawer drawer)
        {
            drawer.DrawBorder(this);
        }
        public void DrawItems(IDrawer drawer)
        {
            Iterate((value, row, col) =>
            {
                drawer.DrawCell(value, row, col);
            });
        }
        public IMatrix getComponent()
        {
            return this;
        }
        public int GetValue(int row, int col)
        {
            foreach (IMatrix matrix in Elements)
            {
                if (matrix.NumRows <= row)
                {
                    row -= matrix.NumRows;
                }
                else
                {
                    if (matrix.NumColumns > col)
                        return matrix.GetValue(row, col);
                    else return 0;
                }
            }
            return 0;
        }
        public void SetValue(int value, int row, int col)
        {
            foreach (IMatrix matrix in Elements)
            {
                if (matrix.NumRows <= row)
                {
                    row -= matrix.NumRows;
                }
                else
                {
                    if (matrix.NumColumns > col)
                        matrix.SetValue(value, row, col);
                    throw new ArgumentOutOfRangeException();
                }
            }
            throw new ArgumentOutOfRangeException();
        }
        public void Iterate(DrawItemsDelegate drawItems)
        {
            int delta = 0;
            foreach (IMatrix matrix in Elements)
            {
                matrix.Iterate((value, row, col) => drawItems.Invoke(value, row + delta, col));
                delta += matrix.NumRows;
            }
        }
    }
}

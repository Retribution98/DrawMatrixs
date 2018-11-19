using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_2
{
    class ChangeNumeratorMatrix : IMatrix
    {
        IMatrix _matrix;
        Dictionary<int,int> NewNumRow = new Dictionary<int, int>();
        Dictionary<int, int> NewNumCol = new Dictionary<int, int>();

        public ChangeNumeratorMatrix(IMatrix matrix)
        {
            _matrix = matrix;
        }

        public int NumColumns => _matrix.NumColumns;
        public int NumRows => _matrix.NumRows;

        public void DrawBorder(IDrawer drawer)
        {
            _matrix.DrawBorder(drawer);
        }
        public void DrawItems(IDrawer drawer)
        {
            Iterate((value, row, col) =>
            {
                drawer.DrawCell(value, row, col);
            });

        }
        public int GetValue(int row, int col)
        {
            return _matrix.GetValue(NewNumRow.ContainsKey(row) ? NewNumRow[row] : row, NewNumCol.ContainsKey(col) ? NewNumRow[col] : col);
        }
        public void SetValue(int value, int row, int col)
        {
            _matrix.SetValue(value, NewNumRow.ContainsKey(row) ? NewNumRow[row] : row, NewNumCol.ContainsKey(col) ? NewNumRow[col] : col);
        }
        public void ChangeRow(int row1, int row2)
        {
            int row = NewNumRow.ContainsKey(row1) ? NewNumRow[row1] : row1;
            NewNumRow[row1] = NewNumRow.ContainsKey(row2) ? NewNumRow[row2] : row2;
            NewNumRow[row2] = row;
        }
        public void ChangeCol(int col1, int col2)
        {
            int col = NewNumCol.ContainsKey(col1) ? NewNumRow[col1] : col1;
            NewNumCol[col1] = NewNumCol.ContainsKey(col2) ? NewNumRow[col2] : col2;
            NewNumCol[col2] = col;
        }
        public IMatrix getComponent()
        {
            return _matrix.getComponent();
        }
        public void Iterate(DrawItemsDelegate drawItems)
        {
            _matrix.Iterate((value, row, col) => {
                drawItems.Invoke(value, NewNumRow.ContainsKey(row) ? NewNumRow[row] : row, NewNumCol.ContainsKey(col) ? NewNumRow[col] : col);
            });
        }
    }
}

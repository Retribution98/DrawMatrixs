using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_2
{
    public delegate void DrawItemsDelegate(int value, int rowStartPosition, int colStartPosition);

    public interface IMatrix:IDrawable
    {
        int NumColumns { get; }
        int NumRows { get; }

        int GetValue(int index1, int index2);
        void SetValue(int value, int row, int col);
        IMatrix getComponent();
        void Iterate(DrawItemsDelegate drawItems);
    }

    abstract class SomeMatrix : IMatrix
    {
        IVector[] matrix;
        abstract protected IVector CreateVector(int size);
        public int NumColumns { get; }
        public int NumRows { get; }

        public SomeMatrix(int numColumns, int numRows)
        {
            NumColumns = numColumns;
            NumRows = numRows;
            matrix = new IVector[NumRows];
            for (int row=0; row<NumRows; row++)
            {
                matrix[row] = CreateVector(NumColumns);
            }
        }
        public virtual int GetValue(int row, int col)
        {
            if ((row > NumRows) || (col > NumColumns) || (row < 0) || (col < 0))
                throw new ArgumentException("Wrong index");
            return matrix[row].GetValue(col);
        }
        public void SetValue(int value, int row, int col)
        {
            if ((row > NumRows) || (col > NumColumns) || (row < 0) || (col < 0))
                throw new ArgumentException("Wrong index");
            matrix[row].SetValue(value, col);
        }
        public virtual void DrawBorder(IDrawer drawer)
        {
            drawer.DrawBorder(this);
        }
        public virtual void DrawItems(IDrawer drawer)
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
        public virtual void Iterate(DrawItemsDelegate drawItems)
        {
            for (int row = 0; row < NumRows; row++)
                for (int col = 0; col < NumColumns; col++)
                    drawItems.Invoke(GetValue(row, col), row, col);
        }
    }

    class DefaultMatrix: SomeMatrix,IMatrix 
    {
        public DefaultMatrix(int numColumns, int numRows) : base(numColumns, numRows)
        {
        }
        protected override IVector CreateVector(int size)
        {
            return new SimpleVector(size);
        }
    }

    class SparseMatrix : SomeMatrix, IMatrix
    {
        public SparseMatrix(int numColumns, int numRows) : base(numColumns, numRows)
        {
        }
        protected override IVector CreateVector(int size)
        {
            return new SparseVector(size);
        }
        public override void Iterate(DrawItemsDelegate drawItems)
        {
            for (int row = 0; row < NumRows; row++)
                for (int col = 0; col < NumColumns; col++)
                    if (GetValue(row, col) != 0)
                        drawItems.Invoke(GetValue(row, col), row, col);
        }
    }

    static class MatrixInitiator
    {
        public static void FillMatrix(IMatrix matrix, int numValue, int maxValue)
        {
            Random random = new Random();
            for (int num = 0; num < numValue; num++)
            {
                matrix.SetValue(random.Next(maxValue), random.Next(matrix.NumRows), random.Next(matrix.NumColumns));
            }
        }
    }

    class MatrixStatistic
    {
        IMatrix useMatrix;
        public int Sum
        {
            get
            {
                int sum = 0;
                for (int row = 0; row < useMatrix.NumRows; row++)
                    for (int col = 0; col < useMatrix.NumColumns; col++)
                        sum += useMatrix.GetValue(row, col);
                return sum;
            }
        }
        public double Average
        {
            get
            {
                return this.Sum/(useMatrix.NumColumns*useMatrix.NumRows);
            }
        }
        public int Max
        {
            get
            {
                int max = 0;
                for (int row = 0; row < useMatrix.NumRows; row++)
                {
                    for (int col = 0; col < useMatrix.NumColumns; col++)
                    {
                        if (useMatrix.GetValue(row, col) > max)
                        {
                            max = useMatrix.GetValue(row,col);
                        }
                    }
                }
                return max;
            }
        }
        public int NumNotEmpty
        {
            get
            {
                int num = 0;
                for (int row = 0; row < useMatrix.NumRows; row++)
                    for (int col = 0; col < useMatrix.NumColumns; col++)
                        if (useMatrix.GetValue(row, col) != 0) num++;
                return num;
            }
        }

        public MatrixStatistic(IMatrix matrix)
        {
            useMatrix = matrix;
        }
    }
}

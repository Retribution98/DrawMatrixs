using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_2
{
    interface IMatrix:IDrawable
    {
        int NumColumns { get; }
        int NumRows { get; }

        int GetValue(int index1, int index2);
        void SetValue(int value, int index1, int index2);
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
        public int GetValue(int index1, int index2)
        {
            if ((index1 > NumRows) || (index2 > NumColumns) || (index1 < 0) || (index2 < 0))
                throw new ArgumentException("Wrong index");
            return matrix[index1].GetValue(index2);
        }
        public void SetValue(int value, int index1, int index2)
        {
            if ((index1 > NumRows) || (index2 > NumColumns) || (index1 < 0) || (index2 < 0))
                throw new ArgumentException("Wrong index");
            matrix[index1].SetValue(value, index2);
        }
        public virtual void Draw(IDrawer drawer, bool border)
        {
            int?[,] items = new int?[NumRows,NumColumns];
            for (int row = 0; row < NumRows; row++)
                for (int col = 0; col < NumColumns; col++)
                    items[row, col] = this.GetValue(row, col);
            drawer.Draw(items, NumRows, NumColumns, border);
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
        public override void Draw(IDrawer drawer, bool border)
        {
            int?[,] items = new int?[NumRows, NumColumns];
            for (int row = 0; row < NumRows; row++)
                for (int col = 0; col < NumColumns; col++)
                    if (this.GetValue(row, col) != 0) items[row, col] = this.GetValue(row, col);
                    else items[row, col] = null;
            drawer.Draw(items, NumRows, NumColumns, border);
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

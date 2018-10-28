﻿using System;
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
        void SetValue(int value, int row, int col);
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
            for (int row = 0; row < NumRows; row++)
                for (int col = 0; col < NumColumns; col++)
                    drawer.DrawCell(this, row, col);
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
        public override void DrawItems(IDrawer drawer)
        {
            for (int row = 0; row < NumRows; row++)
                for (int col = 0; col < NumColumns; col++)
                    if (this.GetValue(row, col) != 0)
                        drawer.DrawCell(this, row, col);
        }
    }

    class TransMatrix: DefaultMatrix
    {
        public TransMatrix(int numCol, int numRows):base(numCol,numRows)
        {

        }
        public override int GetValue(int row, int col)
        {
            if ((col > NumRows) || (row > NumColumns) || (col < 0) || (row < 0))
                throw new ArgumentException("Wrong index");
            return base.GetValue(col, row);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_2
{
    interface IVector
    {
        int Count { get; }

        int GetValue(int index);
        void SetValue(int value, int index);
    }

    class SimpleVector : IVector
    {
        int[] vector;
        public int Count { get; }

        public SimpleVector(int count)
        {
            Count = count;
            vector = new int[count];
        }
        public int GetValue(int index)
        {
            if ((index > Count)||(index<0))
                throw new ArgumentException("Wrong index");
            return vector[index];
        }
        public void SetValue(int value, int index)
        {
            if ((index > Count) || (index < 0))
                throw new ArgumentException("Wrong index"); 
            vector[index] = value;
        }
    }

    class SparseVector : IVector
    {
        Dictionary<int,int> vector = new Dictionary<int,int>();
        public int Count { get; }
        
        public SparseVector(int count)
        {
            Count = count;
        }
        public int GetValue(int index)
        {
            if ((index > Count) || (index < 0)) throw new ArgumentException("Wrong index");
            if (vector.ContainsKey(index))
                return vector[index];
            else return 0;
        }
        public void SetValue(int value, int index)
        {
            if ((index > Count) || (index < 0)) throw new ArgumentException("Wrong index");
            vector[index] = value;
        }
    }

}

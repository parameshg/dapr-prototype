using System;
using System.Collections.Generic;

namespace Backend.Models
{
    [Serializable]
    public class Dataset
    {
        private static object guard = new object();

        public List<int> Items { get; set; } = new List<int>();

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public int Sum
        {
            get
            {
                var result = 0;

                foreach (var item in Items)
                {
                    result += item;
                }

                return result;
            }
        }

        public double Mean
        {
            get
            {
                double result = 0;

                if (!Items.Count.Equals(0))
                {
                    result = (double)Sum / Items.Count;
                }

                return Math.Round(result, 2);
            }
        }

        public double Variance
        {
            get
            {
                double result = 0;

                double sum = 0;

                if (!Items.Count.Equals(0))
                {
                    foreach (var item in Items)
                    {
                        sum += Math.Pow(Mean - item, 2);
                    }

                    result = sum / Items.Count - 1;
                }

                return Math.Round(result, 2);
            }
        }

        public double StandardDeviation
        {
            get
            {
                double result = 0;

                if (!Items.Count.Equals(0))
                {
                    result = Math.Sqrt(Variance);
                }

                return Math.Round(result, 2);
            }
        }

        public void Add(int value)
        {
            lock (guard)
            {
                Items.Add(value);
            }
        }

        public void Clear()
        {
            lock (guard)
            {
                Items.Clear();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Area
    {
        
        //private int length;
        //private int width=10;
        //public Area(int l,int b)
        //{
        //    length = l;
        //    width = b;
        //}
        //public Area(int l)
        //{
        //    length = l;
        //}

        public Area(int length,int width = 0)
        {
            if(width == 0)
            {
                Console.WriteLine("Area of Square:" + length * length);
            }
            else
            {
                Console.WriteLine("Area of rectangle:" + length * width);
            }
        }
        //public int CalculateArea()
        //{
        //    if (width == 0)
        //    {
        //        return length * length;
        //    }
        //    else
        //    {
        //        return length * width;
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Car
    {
        public int ModelNo { get; set; }
        public int Color { get; set; }
        public string Steering { get; set; }
        public float FuelCapacity { get; set; }
        public virtual void Discount()
        {
            Console.WriteLine("Sorry no discount");
        }
        public class Maruti:Car
        {
          
            public override void Discount()
            {
                Console.WriteLine("There is a 10% discount");
            }
            public void M1()
            {

            }

        }
        public class BMW : Car
        {

            public override void Discount()
            {
                Console.WriteLine("Sorry no discount");
            }
            

        }
    }
}

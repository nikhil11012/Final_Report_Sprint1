using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Encapsule
    {
        private string Message;
        private int age;

        public Encapsule(string s,int a)
        {
            Message = s;
            age = a;
        }

        public string greeting()
        {
            if(Message == "male" && age>10)
            {
                return "Hello! Sir";
            }
            else
            {
                return "Hello! Mam";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsOOPGruppProjekt
{
    internal class Retailer
    {
        //name = id

        string name;
        public Retailer(string name)
        {
            this.name = name;
        }   
        public string Name { get { return name; } }
    }
}

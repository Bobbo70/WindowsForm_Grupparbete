using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsOOPGruppProjekt
{
    internal class Manufacturer
    {
        //name = id

        string name;

        public Manufacturer(string name)
        {
            this.name = name;
        }   

        public string Name { get { return name; } }
    }
}

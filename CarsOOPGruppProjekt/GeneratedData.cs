using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsOOPGruppProjekt
{
    internal class GeneratedData
    {

        //Generic use of "Model Name"
        public static string[] modelName = {" Series", " Generation", " x", " Model", " boomer", " Gand", " super", " hybrid", " Cheesy", " Mos", " Kudde", " Glass" };
        
        public GeneratedData() { }

        public static List<Cars> generatedCars(int howMany)
        {
            // List that will return
            List<Cars> cars = new List<Cars>();

            // SQL get current Manufacturers and retailers
            sqlConnection sqlconn = new sqlConnection();
            List<Manufacturer> manf = sqlconn.GetManufacturers();
            List<Retailer> retailers= sqlconn.GetRetailers();

            // Random Class to get random value
            Random rand = new Random();

            //Runs as many times the user inputs
            for(int count = 0; count < howMany; count++)
            {
                //Get a manufacutrer between 0 and manf.count-1.... -1 is only because count doesn't start at 0              
            string manfuString = manf[(rand.Next(1, 1000) % (manf.Count))].Name;
                //Get a retailer between 0 and retailers.count-1 
            string retailersString = retailers[(rand.Next(1, 1000) % (retailers.Count))].Name;
                // Gets a random year between 1960 and 2022
            int year = rand.Next(1960, 2022);
                // Gets a random price between 50000 and 500000 and makes it looks more clean like 250000 instead of 254564
            int price = rand.Next(50000, 500000);
            price -= (price % 10000);

                // Gets a number + modelname from Array
            string model = string.Format("{0} {1}",rand.Next(1,22),modelName[rand.Next(1, modelName.Length)-1]);

                // Adds car to cars list
             cars.Add(new Cars(0,model,manfuString,year.ToString(),retailersString,price));
            }
            return cars;
        }
    }
}

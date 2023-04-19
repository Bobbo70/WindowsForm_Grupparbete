using System;


namespace CarsOOPGruppProjekt
{
    public class Cars
    {
        public int id;
        public string model;
        public string manufacturer;
        public string year;
        public string retailer;
        public int price;
        //Constructors
        public Cars(int id, string model, string manufacturer, string year
            , string retailer, int price)
        {
            this.id = id;
            this.model = model;
            this.manufacturer = manufacturer;
            this.year = year;
            this.retailer = retailer;
            this.price = price;
        }
        //Properties
        public int Id { get { return id; } }
        public string Model { get { return model; } }
        public string Manufacturer { get { return manufacturer; } }
        public string Retailer { get { return retailer; } }
        public int Price { get { return price; } }
        public string Year { get { return year; } }
    }
}

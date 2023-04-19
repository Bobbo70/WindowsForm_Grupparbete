using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarsOOPGruppProjekt
{
    public partial class RandomDataForm : Form
    {
        public RandomDataForm()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // Kollar om txtboxCount inte är tom
            if(txtboxCount.TextLength > 0)
            {

                // Försöker Parse, annars händer inget. De får skylla sig själv om de inte skriver in en siffra
                int result = 0;
                Int32.TryParse(txtboxCount.Text, out result);
                //Hämtar en lista med bilar ifrån GeneratedData Class
                List<Cars> cars = GeneratedData.generatedCars(result);
                // Ny Connection till mysql
                sqlConnection sqlconn = new sqlConnection();
                // För varje bil som genererades så lägg till det i databasen
                foreach(Cars car in cars)
                {
                    string strSql = $"CALL addCar('{car.model}','{car.manufacturer}','{car.year}','{car.retailer}','{car.price}')";
                    sqlconn.AddCar(strSql);
                }
            }
            //Göm Formen igen
            this.Hide();
        }

        // Gömmer RandomGenerater Form
        private void carsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}

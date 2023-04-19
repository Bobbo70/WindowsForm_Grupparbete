using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.ServiceProcess;
using MySqlX.XDevAPI;
using System.Drawing;

namespace CarsOOPGruppProjekt
{
    internal class sqlConnection
    {

        public MySql.Data.MySqlClient.MySqlConnection _mysqlCon;

        public sqlConnection()
        {
            //Kör functionen mysqlCon som sedan deklareras _mysqlCon
            _mysqlCon = mysqlCon();
        }


        //Måste köra VisualStudio som Admin för att det ska fungera.
        public void testMysql()
        {

            try
            {
                sqlOpen();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
                if(serviceExists("MySQL80"))
                {
                    startService("MySQL80");
                }
            }
            sqlClose();
        }

        // Bool för att se om Servicen MySQL80 finns
        public bool serviceExists(string ServiceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(ServiceName));
        }

        public void sqlOpen()
        {
            if(_mysqlCon.State == System.Data.ConnectionState.Open) { return; }
            else { _mysqlCon.Open(); }
        }
        public void sqlClose()
        {
            if (_mysqlCon.State == System.Data.ConnectionState.Closed) { return; }
            else { _mysqlCon.Close(); }
        }


        //Function som startar MySQL80 om den är nedstängd
        public void startService(string ServiceName)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName= ServiceName;
            if(_mysqlCon.State == System.Data.ConnectionState.Open)
            {

            }

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                }
                catch(InvalidOperationException e)
                {

                }
            }
            else
            {
                // Service is Already Running
            }
        }


        private MySql.Data.MySqlClient.MySqlConnection mysqlCon()
        {

            //Declarerar connectionString till null
            string connetionString = null;
            //Delcarerar en ny MySqlConnection som används för att ansluta till Mysql
            MySqlConnection cnn;
            //Själva connectionString Vilken server, Localhost = din egna dator. Database vad cars heter. uid = User_id. pwn = lösenord
            connetionString = "server=localhost;database=cars;uid=oopgrund;pwd=programmering;";
            //Försöker att connecta misslyckas det få ett felmeddelande
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
                cnn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Can not open connection ! ");
            }
            //Retunerar denna MySqlConnection så det finns i en local variable.
            return cnn;
        }

        public List<Cars> getData()
        {
            // Använder den lokala MysqlConnection för att öppna en connection
            _mysqlCon.Open();
            //Skapar en MysqlDataReader  genom att skapa en ny mysqlcommand med query + connection
            MySqlDataReader reader = new MySqlCommand("SELECT * FROM CARS", _mysqlCon).ExecuteReader();
            //Skapar en temp List med classen Cars
            List<Cars> sqlCarList = new List<Cars>();

            // Läser varje rad tills det inte finns fler rader.
            while(reader.Read())
            {
                // Lägger till i våran Lista sqlCarList med hjälp av namnen va det heter i databasen
                sqlCarList.Add(new Cars(
                    Convert.ToInt32(reader["cars_id"].ToString()),
                    reader["cars_model"].ToString(),
                    reader["manufacturers_manufacturers_name"].ToString(),
                    reader["retailers_retailers_name"].ToString(),
                    reader["cars_year"].ToString(),
                    Convert.ToInt32(reader["cars_price"])));

            }
            //Stänger mysqlconnection
            _mysqlCon.Close();
            //Retunerar en lista med alla bilar ifrån databasen
            return sqlCarList;
        }

        // Har en input query som försöker köras där man bara får tillbaka en int rowAffected hur många rader som påverkas
        public void updateSqlData(string query)
        {
            try
            {
                _mysqlCon.Open();
                MySqlCommand cmd = new MySqlCommand(query, _mysqlCon);
                int rowAffected = cmd.ExecuteNonQuery();

                _mysqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Cars> searchUpdate(string keyWord, string comboString)
        {
            //Om man raderar allt = att den kör vanliga getData();
            if(keyWord == "")
            {
                return getData();
            }
            // Ändrar om så vi får rätt colummnamn i databasen
            string columName = columNameFixer(comboString);
            // SQL Query
            string query = String.Format("SELECT * FROM CARS WHERE {0} LIKE @colValue", columName);
            // Skapar ett sql command som sedan ändrar om value på @colValue till det som är i variable. 
            MySqlCommand cmd = new MySqlCommand(query, _mysqlCon);
            cmd.Parameters.AddWithValue("@colValue", "%"+keyWord+"%");
            //Öppnar SQL connection
            _mysqlCon.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            //Skapar en temp List med classen Cars
            List<Cars> sqlCarList = new List<Cars>();

            // Läser varje rad tills det inte finns fler rader.
            while (reader.Read())
            {
                // Lägger till i våran Lista sqlCarList med hjälp av namnen va det heter i databasen
                sqlCarList.Add(new Cars(
                    Convert.ToInt32(reader["cars_id"].ToString()),
                    reader["cars_model"].ToString(),
                    reader["manufacturers_manufacturers_name"].ToString(),
                    reader["retailers_retailers_name"].ToString(),
                    reader["cars_year"].ToString(),
                    Convert.ToInt32(reader["cars_price"])));

            }
            _mysqlCon.Close();
            return sqlCarList;
        }

        public List<Manufacturer> GetManufacturers()
        {
            //Lägger till tillverkare i lista med tillverkare och returnerar listan
            List<Manufacturer> manufacturers = new List<Manufacturer>();
            _mysqlCon.Open();
            MySqlDataReader reader = new MySqlCommand("SELECT * FROM MANUFACTURERS", _mysqlCon).ExecuteReader();
            
            while (reader.Read())
            {
                manufacturers.Add(new Manufacturer(reader["manufacturers_name"].ToString()));
            }

            _mysqlCon.Close();

            return manufacturers;
        }

        public List<Retailer> GetRetailers()
        {
            //Lägger till återförsäljare i lista med återförsäljare och returnerar listan
            List<Retailer> retailers = new List<Retailer>();
            _mysqlCon.Open();
            MySqlDataReader reader = new MySqlCommand("SELECT * FROM RETAILERS", _mysqlCon).ExecuteReader();

            while (reader.Read())
            {
                retailers.Add(new Retailer(reader["retailers_name"].ToString()));
            }

            _mysqlCon.Close();

            return retailers;
        }

        public string columNameFixer(string columName)
        {
            // Simpel Switch case. Får den in "Model" så retunerar den cars_model etc.
            switch(columName)
                {
                case "Model":
                    return "cars_model";
                case "Year":
                    return "cars_Year";
                case "Price":
                    return "cars_Price";
            }

            return null;
        }


        //Lägger till ny bil till database
        public void AddCar(string sqlQuery)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sqlQuery, _mysqlCon);
                _mysqlCon.Open();
                cmd.ExecuteReader();
                _mysqlCon.Close();
                
            }
            catch
            {
                _mysqlCon.Close();
                //Ingenting händer
            }
        }


        
        //Tar bort en tillagd bil ifrån databasen
        public void DeleteCar(string sqlQuery)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sqlQuery, _mysqlCon);
                _mysqlCon.Open();

                cmd.ExecuteReader();

                _mysqlCon.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Deleted car  failed");
            }
            sqlClose();
        }
        
    }
}


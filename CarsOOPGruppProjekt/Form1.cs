using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarsOOPGruppProjekt
{
    public partial class Form1 : Form
    {
        sqlConnection sqlConn = new sqlConnection();
        RandomDataForm randDForm = new RandomDataForm();
        public Form1()
        {
            InitializeComponent();
            isMysqlRunning();
            InitializeGUI();
        }
        private void isMysqlRunning()
        {
            sqlConn.testMysql();
        }
        private void InitializeGUI()
        {
            //Fyll dropdownlistor
            List<Manufacturer> manufacturers = sqlConn.GetManufacturers();
            foreach (var manufacturer in manufacturers)
            {
                cmbManufacurers.Items.Add(manufacturer.Name); 
            }

            List<Retailer> retailers = sqlConn.GetRetailers();
            foreach (var retailer in retailers)
            {
                cmbRetailers.Items.Add(retailer.Name);
            }
            searchcmbBox.SelectedIndex= 0;

            

            FillList();
            UpdateGUI();
        }

        private void UpdateGUI()
        {
            //Sätt default värde i dropdownlistor
            cmbManufacurers.SelectedIndex = 0;
            cmbRetailers.SelectedIndex = 0;

            //Sätt defaultvärden för text
            txtModel.Text = "model";
            txtPrice.Text = "0";
            txtYear.Text = "0000";
            txtId.Text = "";

            pictureBox1.Image = CarLogos.defaultLogo;

        }

        private void FillList() 
        {
            List<Cars> cars = sqlConn.getData();
            dataGridView1.Rows.Clear();
            foreach (var car in cars)
            {
                //DataGridViewRow = en rad i datagridview t.ex. en rad i Excel.
                DataGridViewRow dgvr = new DataGridViewRow();
                //Fungerar lite som i ett excel dokument. Cell[0] = id som är gömd men kan användas av oss.
                dgvr.CreateCells(dataGridView1);
                dgvr.Cells[0].Value = car.id;
                dgvr.Cells[1].Value = car.manufacturer;
                dgvr.Cells[2].Value = car.Model;
                dgvr.Cells[4].Value = car.Year;
                dgvr.Cells[3].Value = car.Retailer;
                dgvr.Cells[5].Value = car.Price;
                dataGridView1.Rows.Add(dgvr);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string strModel = txtModel.Text;
            string strName = cmbManufacurers.Text;
            int intYear = Convert.ToInt32(txtYear.Text);
            string strRetail = cmbRetailers.Text;
            int intPrice = Convert.ToInt32(txtPrice.Text);

            string strSql = $"CALL addCar('{strModel}','{strName}','{intYear}','{strRetail}','{intPrice}')";

            sqlConn.AddCar(strSql);
            FillList();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Cars carsedit = getSelectedDataGridViewCar();
            cmbManufacurers.SelectedItem = carsedit.Manufacturer;
            txtModel.Text = carsedit.Model;
            txtYear.Text = carsedit.Year;
            cmbRetailers.SelectedItem = carsedit.Retailer;
            txtPrice.Text = carsedit.Price.ToString();
            txtId.Text = carsedit.Id.ToString();
            btnApply.Enabled = true;
            btnCancel.Enabled = true;
        }

        // Get current selected Row Data from dataGridView1 for easier handling.
        private Cars getSelectedDataGridViewCar()
        {
            DataGridViewRow dataRow;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                dataRow = dataGridView1.SelectedCells[0].OwningRow;
                dataGridView1.SelectedCells[0].OwningRow.Selected = true;
                //int id, string model, string manufacturer, string year,string retailer, int price
                //dataGridView1.SelectedCells[0].OwningRow.Selected = true;
                Cars car = new Cars(
                Convert.ToInt32(dataRow.Cells[0].Value.ToString()),
                dataRow.Cells[2].Value.ToString(),
                dataRow.Cells[1].Value.ToString(),
                dataRow.Cells[3].Value.ToString(),
                dataRow.Cells[4].Value.ToString(),
                Convert.ToInt32(dataRow.Cells[5].Value.ToString()));
                return car;
            }
            else
            {
                Cars car = new Cars(
                    0, "Default", "Default", "Default", "Default", 0);
                return car;
            }
        }
        private void btnRemove_Click(object sender, EventArgs e) 
        {
            // Delete selected car from the database.
            Cars carsedit = getSelectedDataGridViewCar();
            cmbManufacurers.SelectedItem = carsedit.manufacturer;
            txtModel.Text = carsedit.Model;
            txtYear.Text = carsedit.Year;
            cmbRetailers.SelectedItem = carsedit.Retailer;
            txtPrice.Text = carsedit.Price.ToString();

            string strSql = $"CALL deleteCar('{carsedit.id}')";

            sqlConn.DeleteCar(strSql);
            FillList();
        }   

        private void btnView_Click(object sender, EventArgs e)
        {
            FillList();
        }

        private void searchtxtbox_TextChanged(object sender, EventArgs e)
        {
            List<Cars> cars = sqlConn.searchUpdate(searchtxtbox.Text,searchcmbBox.SelectedItem.ToString());
            dataGridView1.Rows.Clear();
            foreach (var car in cars)
            {
                //DataGridViewRow = en rad i datagridview t.ex. en rad i Excel.
                DataGridViewRow dgvr = new DataGridViewRow();
                //Fungerar lite som i ett excel dokument. Cell[0] = id som är gömd men kan användas av oss.
                dgvr.CreateCells(dataGridView1);
                dgvr.Cells[0].Value = car.Id;
                dgvr.Cells[1].Value = car.manufacturer;
                dgvr.Cells[2].Value = car.Model;
                dgvr.Cells[4].Value = car.Year;
                dgvr.Cells[3].Value = car.Retailer;
                dgvr.Cells[5].Value = car.Price;
                dataGridView1.Rows.Add(dgvr);
            }
        }

        private void searchcmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbManufacurers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Väljer hela raden istället för just cellen man klickar på
            if(dataGridView1.SelectedCells.Count > 0)dataGridView1.SelectedCells[0].OwningRow.Selected = true;


            string car = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            switch(car)
            {
                case "Audi":
                    pictureBox1.Image = CarLogos.audiLogo;break;
                case "Ford":
                    pictureBox1.Image = CarLogos.fordLogo;break;
                case "Renault":
                    pictureBox1.Image = CarLogos.renaultLogo;break;
                case "Saab":
                    pictureBox1.Image = CarLogos.saabLogo;break;
                case "Suzuki":
                    pictureBox1.Image = CarLogos.suzukiLogo;break;
                case "Volvo":
                    pictureBox1.Image = CarLogos.volvoLogo;break;
            }

            //Autofill: Hämtar data från datagrid och representerar värden till text-&comboboxes
            //try/catch hanterar situationer då index är outofbounds
            /*
            try
            {
                Cars car = getSelectedDataGridViewCar();
                txtId.Text = car.Id.ToString();
                cmbManufacurers.Text = car.Manufacturer;
                txtModel.Text = car.Model;
                txtYear.Text = car.Year;
                cmbRetailers.Text = car.Retailer;
                txtPrice.Text = car.Price.ToString();
                btnCancel.Enabled = true;
                btnAdd.Enabled = false;
            }
            catch
            {
                //Inget händer!
            }
           */
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Uppdaterar GUI till default & sätter knappar
            UpdateGUI();
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnApply.Enabled = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string queryedit = String.Format(
                "UPDATE cars SET manufacturers_manufacturers_name = '{0}'," +
                " cars_model = '{1}', cars_year = '{2}'," +
                " retailers_retailers_name = '{3}'," +
                " cars_price = '{4}' WHERE cars_id = '{5}'",
                cmbManufacurers.SelectedItem,
                txtModel.Text,
                Convert.ToInt32(txtYear.Text),
                cmbRetailers.SelectedItem,
                Convert.ToInt32(txtPrice.Text),
                Convert.ToInt32(txtId.Text));
            sqlConn.updateSqlData(queryedit);
            btnApply.Enabled = false;
            btnCancel.Enabled = false;
            FillList();
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            randDForm.Show();
            randDForm.Width = this.Width;
            randDForm.Height = this.Height;
            randDForm.Location = this.Location;
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            //Reloads the default logo when pressed.

            pictureBox1.Image = CarLogos.defaultLogo;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0) dataGridView1.SelectedCells[0].OwningRow.Selected = true;

            
            string car = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            switch (car)
            {
                case "Audi":
                    pictureBox1.Image = CarLogos.audiLogo; break;
                case "Ford":
                    pictureBox1.Image = CarLogos.fordLogo; break;
                case "Renault":
                    pictureBox1.Image = CarLogos.renaultLogo; break;
                case "Saab":
                    pictureBox1.Image = CarLogos.saabLogo; break;
                case "Suzuki":
                    pictureBox1.Image = CarLogos.suzukiLogo; break;
                case "Volvo":
                    pictureBox1.Image = CarLogos.volvoLogo; break;
            }
        }
    }
}
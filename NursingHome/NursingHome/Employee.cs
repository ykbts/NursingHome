using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NursingHome
{
    public partial class Employee : Form
    {
        string connectionString = @"Data Source=KATYA; Initial Catalog=NursingHome; Integrated Security = True;";
        int id;
        int idEmployee;
        int idPatient;
        SqlDataAdapter adapter;
        DataSet ds;
        public Employee(int id)
        {
            InitializeComponent();
            DisplayDataOnForm(id);
            string sql = $"SELECT \r\n    P.PatientId,\r\n    P.FirstName,\r\n    P.LastName,\r\n    P.Gender,\r\n    P.Birthday,\r\n    Co.Name as 'Country',\r\n    C.Name as 'City',\r\n    R.Capacity,\r\n    R.RoomNumber,\r\n    P.ContactNumber,\r\n    P.Entry_Date\r\nFROM Patients P\r\nJOIN Addresses A ON P.AddressId = A.AddressId\r\nJOIN Rooms R ON P.RoomId = R.RoomId\r\nJOIN Traits T ON P.TraitId = T.TraitId\r\nJOIN Cities C ON A.CityId = C.CityId\r\nJOIN Countries Co ON C.CountyId = Co.CountryId\r\nWHERE P.EmployeeId = {idEmployee}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
               // dataGridView2.DataSource = ds.Tables[0];
            }
           
          

        }
        private void DisplayDataOnForm(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"Select * from Employees \r\nJOIN Addresses ON Employees.AddressId = Addresses.AddressId\r\nJOIN Cities ON Addresses.CityId = Cities.CityId\r\nJOIN Countries ON Cities.CountyId = Countries.CountryId\r\nJOIN Professions ON Professions.ProfessionId = Employees.PositionsId\r\nWHERE Employees.UserId = {id}";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                            idEmployee = reader.GetInt32(0);
                            string FirstName = reader.GetString(1);
                            string LastName = reader.GetString(2);
                            DateTime birthday = reader.GetDateTime(3);
                            string birthdayString = birthday.ToString("yyyy-MM-dd"); 
                            int addressId = reader.GetInt32(4);
                            int positionId = reader.GetInt32(5);
                            int salary = (int)reader.GetSqlMoney(6);
                            string phone = reader.GetString(7);
                            int userId = reader.GetInt32(8);
                            string Address = reader.GetString(11);
                            string postalCode = reader.GetString(12);
                            string city = reader.GetString(15);
                            string country = reader.GetString(17);
                            string proffesion = reader.GetString(19);
                            string jobDesription = reader.GetString(20);


                            textBoxID.Text = idEmployee.ToString();
                            textBoxFirstName.Text = FirstName;
                            textBoxLastName.Text = LastName;
                            textBoxBirthday.Text = birthdayString;
                            textBoxAddress.Text = Address.ToString();
                            textBoxPosition.Text = proffesion.ToString();
                            textBoxSalary.Text = salary.ToString();
                            textBoxPhone.Text = phone.ToString();
                            textBoxPostalCode.Text = postalCode.ToString();
                            textBoxCity.Text = city.ToString();
                            textBoxCountry.Text = country.ToString();
                            textBoxjobDesription.Text = jobDesription.ToString();

                        break;                      
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вийшло отримати дані з БД");
                }
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            textBoxID.ReadOnly = true;

        }

        private void textBoxSalary_TextChanged(object sender, EventArgs e)
        {
            textBoxSalary.ReadOnly = true;
        }

        private void textBoxPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Employee_Load(object sender, EventArgs e)
        {
          

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0) // Make sure a valid row is clicked
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Assuming RoomId is stored in a specific cell (change index accordingly)
                 idPatient = Convert.ToInt32(selectedRow.Cells["PatientId"].Value);

                // Filter data for the second DataGridView based on roomId
                string sql2 = $"Select * from Appoinments where PatientId={idPatient}";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    adapter = new SqlDataAdapter(sql2, connection);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView2.DataSource = ds.Tables[0];
                }
                /*
                   string sql2 = $"Select * from Appoinments where PatientId={idPatient}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql2, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView2.DataSource = ds.Tables[0];
            }*/
            }
        }

        private void FilterDataGridView2ByRoomId(int roomId)
        {
            string sql2 = $"Select * from Appoinments where PatientId={roomId}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql2, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView2.DataSource = ds.Tables[0];
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

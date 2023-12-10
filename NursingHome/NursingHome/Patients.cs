using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NursingHome
{
    public partial class Patients : Form
    {
        string connectionString = @"Data Source=KATYA; Initial Catalog=NursingHome; Integrated Security = True;";
        string selectedCountryName;
        int patientId;
        int id;

        SqlDataAdapter adapter;
        DataSet ds;
        public Patients(int id)
        {
            InitializeComponent();
            DisplayDataOnForm(id);

        }
        private void DisplayDataOnForm(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"SELECT \r\n*\r\nFROM \r\n    Patients\r\nJOIN \r\n    Addresses ON Patients.AddressId = Addresses.AddressId\r\nJOIN \r\n    Cities ON Addresses.CityId = Cities.CityId\r\nJOIN \r\n    Countries ON Cities.CountyId = Countries.CountryId\r\nJOIN \r\n    Employees ON Patients.EmployeeId = Employees.EmployeeId\r\nJOIN \r\n    Rooms ON Rooms.RoomId = Patients.RoomId\r\nJOIN \r\n    Traits ON Traits.TraitId = Patients.TraitId\r\nWHERE \r\n    Patients.UserId ={id}";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                         patientId = reader.GetInt32(0);
                        string FirstName = reader.GetString(1);
                        string LastName = reader.GetString(2);
                        string gender = reader.GetString(3);
                        DateTime birthday = reader.GetDateTime(4);
                        string birthdayString = birthday.ToString("yyyy-MM-dd");
                        int addressId = reader.GetInt32(5);
                        int roomId = reader.GetInt32(6);
                        int employeeId = reader.GetInt32(7);
                        CreateEmployee(employeeId);
                        int traitId = reader.GetInt32(8);
                        string phone = reader.GetString(9);
                        DateTime entryDate = reader.GetDateTime(10);
                        string entryDateString = birthday.ToString("yyyy-MM-dd");

                        //  int userId = reader.GetInt32(11);
                        string Address = reader.GetString(14);
                        int cityId = reader.GetInt32(13);
                        string address = reader.GetString(14);
                        string postalCode = reader.GetString(15);
                        int countryId = reader.GetInt32(16);
                        string city = reader.GetString(18);
                        selectedCountryName = reader.GetString(20);
                        string employeeFirstName = reader.GetString(22);
                        string employeeLastName = reader.GetString(23);
                        int roomNumb = reader.GetInt32(31);
                        string trait = reader.GetString(34);


                        textBoxID.Text = patientId.ToString();
                        textBoxFirstName.Text = FirstName;
                        textBoxLastName.Text = LastName;
                        dateTimeBirthday.Text = birthdayString;
                        textBoxAddress.Text = Address.ToString();
                        textBoxGender.Text = gender.ToString();
                        textBoxPhone.Text = phone.ToString();
                        textBoxPostalCode.Text = postalCode.ToString();
                        textBoxRoom.Text = roomNumb.ToString(); 
                        textBoxTrait.Text = trait.ToString();
                        CreateListCountry(countryId);
                        selectedCountryName = comboBoxCountry.Text.ToString();
                        CreateListCity(cityId, selectedCountryName);
                        comboBoxCity.Text= city;


                        break;
                    }
                    string sql = $"SELECT\r\n    S.ScheduleId,\r\n    CONCAT(E.FirstName, ' ', E.LastName) AS EmployeeFullName,\r\n    S.[Time],\r\n    T.Name AS TreatmentName,\r\n    T.Duration AS TreatmentDuration,\r\n    P.Name AS PlaceName\r\nFROM\r\n    Schedules S\r\nJOIN\r\n    Employees E ON S.EmployeeId = E.EmployeeId\r\nJOIN\r\n    Treatments T ON S.TreatmentId = T.TreatmentId\r\nJOIN\r\n    Places P ON S.PlaceId = P.PlaceId\r\nWHERE\r\n    S.PatientId = {patientId}";
                    using (SqlConnection connection1 = new SqlConnection(connectionString))
                    {
                        adapter = new SqlDataAdapter(sql, connection1);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вийшло отримати дані з БД");
                }
            }
        }
        private void CreateEmployee(int EmployeeId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql =$"SELECT FirstName + ' ' + LastName as FullName FROM Employees Where EmployeeId={EmployeeId}";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string FullName = reader.GetString(0);
                        textBoxEmployee.Text = FullName;
                    }
                    }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                }
            }
        }
        private void Patients_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Schedules' table. You can move, or remove it, as needed.
            this.schedulesTableAdapter.Fill(this.nursingHomeDataSet.Schedules);
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Treatments' table. You can move, or remove it, as needed.
            this.treatmentsTableAdapter.Fill(this.nursingHomeDataSet.Treatments);
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Places' table. You can move, or remove it, as needed.
            this.placesTableAdapter.Fill(this.nursingHomeDataSet.Places);

        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            textBoxID.ReadOnly = true;
        }

        private void comboBoxCity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCountry.SelectedItem != null)
            {
                selectedCountryName = comboBoxCountry.Text.ToString();
                CreateListCity(0, selectedCountryName); 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    string firstName = textBoxFirstName.Text;
                    string lastName = textBoxLastName.Text;                 
                    string phone = textBoxPhone.Text;
                    int cityId = GetCityId(comboBoxCity.Text, comboBoxCountry.Text);
                    int addressId = GetAddressId(textBoxAddress.Text, textBoxPostalCode.Text, cityId);


                    command.CommandText = @"UPDATE Patients
                                            SET
                                                FirstName = @FirstName,
                                                LastName = @LastName,
                                                Gender = @Gender,
                                                Birthday = @Birthday,
                                                AddressId = @AddressId,
                                                ContactNumber = @ContactNumber
                                            WHERE
                                                PatientId = @PatientId;
                                            ";
                    command.Parameters.Add("@PatientId", SqlDbType.Int).Value = patientId;
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = firstName;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = lastName;
                    DateTime birthday = dateTimeBirthday.Value;
                    command.Parameters.Add("@Gender", SqlDbType.VarChar, 50).Value = textBoxGender.Text;
                    command.Parameters.Add("@Birthday", SqlDbType.Date).Value = birthday;
                    command.Parameters.Add("@AddressId", SqlDbType.Int).Value = addressId;
                    command.Parameters.Add("@ContactNumber", SqlDbType.VarChar, 100).Value = phone;

                    command.ExecuteNonQuery();

                    MessageBox.Show("Patient information updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update employee information. Error: " + ex.Message);
                }
            }
        }

        private int GetCityId(string cityName, string countryName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT Cities.CityId " +
                                   "FROM Cities " +
                                   "JOIN Countries ON Cities.CountyId = Countries.CountryId " +
                                   "WHERE Cities.Name = @CityName AND Countries.Name = @CountryName";

                    SqlCommand command = new SqlCommand(query, connection);

                    // Set parameters
                    command.Parameters.Add("@CityName", SqlDbType.VarChar, 100).Value = cityName;
                    command.Parameters.Add("@CountryName", SqlDbType.VarChar, 100).Value = countryName;

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        // Handle the case where the city was not found
                        MessageBox.Show("City not found in the Cities table.");
                        return -1;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                    return -1;
                }
            }
        }
        private int GetAddressId(string address, string postalCode, int cityId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT AddressId FROM Addresses WHERE Address = @Address AND PostalCode = @PostalCode AND CityId = @CityId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.Add("@Address", SqlDbType.VarChar, 100).Value = address;
                        command.Parameters.Add("@PostalCode", SqlDbType.VarChar, 10).Value = postalCode;
                        command.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            // Address not found, insert a new one
                            string insertQuery = "INSERT INTO Addresses (CityId, Address, PostalCode) VALUES (@CityId, @Address, @PostalCode); SELECT SCOPE_IDENTITY();";

                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                // Set parameters for the insertion
                                insertCommand.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                                insertCommand.Parameters.Add("@Address", SqlDbType.VarChar, 100).Value = address;
                                insertCommand.Parameters.Add("@PostalCode", SqlDbType.VarChar, 10).Value = postalCode;

                                // Execute the insertion and get the new AddressId
                                object newAddressId = insertCommand.ExecuteScalar();

                                if (newAddressId != null && newAddressId != DBNull.Value)
                                {
                                    return Convert.ToInt32(newAddressId);
                                }
                                else
                                {
                                    // Handle the case where the insertion did not succeed
                                    MessageBox.Show("Failed to insert a new address into the Addresses table.");
                                    return -1;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                    return -1;
                }
            }
        }

        private void dateTimeBirthday_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBoxjobDesription_TextChanged(object sender, EventArgs e)
        {
            textBoxEmployee.ReadOnly = true;
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPostalCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPhone_TextChanged(object sender, EventArgs e)
        {
            textBoxPhone.ReadOnly = true;
        }

        private void textBoxSalary_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxPosition_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxRoom_TextChanged(object sender, EventArgs e)
        {
            textBoxRoom.ReadOnly = true;
        }

        private void textBoxTrait_TextChanged(object sender, EventArgs e)
        {
            textBoxTrait.ReadOnly = true;
        }

        private void dateTimePickerEntryDate_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerEntryDate.Enabled = false;

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void CreateListCountry(int countryId)
        {
            comboBoxCountry.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT CountryId, Name FROM Countries";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int currentCountryId = reader.GetInt32(0);
                        string countryName = reader.GetString(1);

                        comboBoxCountry.Items.Add(countryName);

                        if (currentCountryId == countryId)
                        {
                            comboBoxCountry.SelectedItem = countryName;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось прочитать из БД. " +
                       exception.Message);
                }
            }
        }

        private void CreateListCity(int cityId, string countryName)
        {
            comboBoxCity.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Cities.CityId, Cities.Name " +
                  "FROM Cities " +
                  "JOIN Countries ON Cities.CountyId = Countries.CountryId " +
                  "WHERE Countries.Name = @SelectedCountryName";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@SelectedCountryName", SqlDbType.VarChar, 100).Value = countryName;

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int currentCityId = reader.GetInt32(0);
                        string cityName = reader.GetString(1);


                        comboBoxCity.Items.Add(cityName);

                        if (currentCityId == cityId)
                        {
                            comboBoxCity.SelectedItem = cityName;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось прочитать из БД. " +
                       exception.Message);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Filter_Click(object sender, EventArgs e)
        {
            string treatment = comboBoxFilter.Text;
            string sql = $"\tSELECT\r\n    S.ScheduleId,\r\n    CONCAT(E.FirstName, ' ', E.LastName) AS EmployeeFullName,\r\n    S.[Time],\r\n    T.Name AS TreatmentName,\r\n    T.Duration AS TreatmentDuration,\r\n    P.Name AS PlaceName\r\nFROM\r\n    Schedules S\r\nJOIN\r\n    Employees E ON S.EmployeeId = E.EmployeeId\r\nJOIN\r\n    Treatments T ON S.TreatmentId = T.TreatmentId\r\nJOIN\r\n    Places P ON S.PlaceId = P.PlaceId\r\nWHERE\r\n    S.PatientId = {patientId} AND T.Name = '{treatment}'";
            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql, connection1);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
           // DisplayDataOnForm(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayDataOnForm(id);
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}

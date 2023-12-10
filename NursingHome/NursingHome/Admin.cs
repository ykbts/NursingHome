using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NursingHome
{
    public partial class Admin : Form
    {
         string connectionString = @"Data Source=KATYA; Initial Catalog=NursingHome; Integrated Security = True;";
        string selectedCountryName;
        string selectedCountryNameEmployee;
        int patientId;
        int id;
        SqlDataAdapter adapter;
        DataSet ds;
        public Admin(int userId)
        {
            InitializeComponent();
            DisplayDataOnForm(userId);
           
        }
        private void DisplayDataOnForm(int id)
        {
            PatientsOnBoxForm(id);
            PatientsOnViewForm(id);
            EmployeeOnBoxForm(id);
            EmployeeOnViewForm(id);
        }
        private void PatientsOnBoxForm(int id) 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"SELECT \r\n*\r\nFROM \r\n    Patients\r\nJOIN \r\n    Addresses ON Patients.AddressId = Addresses.AddressId\r\nJOIN \r\n    Cities ON Addresses.CityId = Cities.CityId\r\nJOIN \r\n    Countries ON Cities.CountyId = Countries.CountryId\r\nJOIN \r\n    Employees ON Patients.EmployeeId = Employees.EmployeeId\r\nJOIN \r\n    Rooms ON Rooms.RoomId = Patients.RoomId\r\nJOIN \r\n    Traits ON Traits.TraitId = Patients.TraitId";

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
                        CreateTraits(traitId);
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
                        string selectedCountryName = reader.GetString(20);
                        string employeeFirstName = reader.GetString(22);
                        string employeeLastName = reader.GetString(23);
                        int roomNumb = reader.GetInt32(31);
                        string trait = reader.GetString(34);
                        CreateRooms(roomId);

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
                        comboBoxCountry.Text = selectedCountryName;
                        CreateListCity(cityId, selectedCountryName);
                        comboBoxCity.Text = city;


                        break;
                    }


                }

                catch (Exception ex)
                {
                    MessageBox.Show("Не вийшло отримати дані з БД");
                }

            }

        }
        private void PatientsOnViewForm(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                        string query = $"SELECT \r\n Patients.PatientId,Patients.FirstName, Patients.LastName, Patients.Birthday, Addresses.Address, Rooms.RoomNumber, Patients.Gender, Addresses.PostalCode, Patients.ContactNumber, Cities.Name as 'City', Countries.Name as 'Country', CONCAT(Employees.FirstName, ' ', Employees.LastName) AS Employee, Patients.Entry_Date, Traits.Name \r\nFROM \r\n    Patients\r\nJOIN \r\n    Addresses ON Patients.AddressId = Addresses.AddressId\r\nJOIN \r\n    Cities ON Addresses.CityId = Cities.CityId\r\nJOIN \r\n    Countries ON Cities.CountyId = Countries.CountryId\r\nJOIN \r\n    Employees ON Patients.EmployeeId = Employees.EmployeeId\r\nJOIN \r\n    Rooms ON Rooms.RoomId = Patients.RoomId\r\nJOIN \r\n    Traits ON Traits.TraitId = Patients.TraitId";

                        adapter = new SqlDataAdapter(query, connection);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        dataGridView1.Columns["PatientId"].ReadOnly = true;

                        textBoxID.DataBindings.Clear();
                        textBoxFirstName.DataBindings.Clear();
                        textBoxLastName.DataBindings.Clear();
                        dateTimeBirthday.DataBindings.Clear();
                        textBoxAddress.DataBindings.Clear();
                        textBoxRoom.DataBindings.Clear();
                        textBoxGender.DataBindings.Clear();
                        textBoxPostalCode.DataBindings.Clear();
                        textBoxPhone.DataBindings.Clear();
                        comboBoxCity.DataBindings.Clear();
                        comboBoxCountry.DataBindings.Clear();
                        textBoxEmployee.DataBindings.Clear();
                        dateTimePickerEntryDate.DataBindings.Clear();
                        textBoxTrait.DataBindings.Clear();

                        textBoxID.DataBindings.Add("Text", ds.Tables[0], "PatientId", true);
                        textBoxFirstName.DataBindings.Add("Text", ds.Tables[0], "FirstName", true);
                        textBoxLastName.DataBindings.Add("Text", ds.Tables[0], "LastName", true);
                        dateTimeBirthday.DataBindings.Add("Text", ds.Tables[0], "Birthday", true);
                        textBoxAddress.DataBindings.Add("Text", ds.Tables[0], "Address", true);
                        textBoxRoom.DataBindings.Add("Text", ds.Tables[0], "RoomNumber", true);
                        textBoxGender.DataBindings.Add("Text", ds.Tables[0], "Gender", true);
                        textBoxPostalCode.DataBindings.Add("Text", ds.Tables[0], "PostalCode", true);
                        textBoxPhone.DataBindings.Add("Text", ds.Tables[0], "ContactNumber", true);
                        comboBoxCity.DataBindings.Add("Text", ds.Tables[0], "City", true);
                        comboBoxCountry.DataBindings.Add("Text", ds.Tables[0], "Country", true);
                        textBoxEmployee.DataBindings.Add("Text", ds.Tables[0], "Employee", true);
                        dateTimePickerEntryDate.DataBindings.Add("Text", ds.Tables[0], "Entry_Date", true);
                        textBoxTrait.DataBindings.Add("Text", ds.Tables[0], "Name", true);



                    }

                catch (Exception ex)
                {
                    MessageBox.Show("Не вийшло отримати дані з БД");
                }

            }

        }
        private void EmployeeOnBoxForm(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"Select * from Employees \r\nJOIN Addresses ON Employees.AddressId = Addresses.AddressId\r\nJOIN Cities ON Addresses.CityId = Cities.CityId\r\nJOIN Countries ON Cities.CountyId = Countries.CountryId\r\nJOIN Professions ON Professions.ProfessionId = Employees.PositionsId";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int idEmployee = reader.GetInt32(0);
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
                        int CountryId = reader.GetInt32(14);
                        int cityId = reader.GetInt32(13);
                        int proffesionId = reader.GetInt32(18);

                        textBoxEmployeeId.Text = idEmployee.ToString();
                        textBoxEmployeeName.Text = FirstName;
                        textBoxEmployeeSurname.Text = LastName;
                        dateTimePickerEmployeeBirthday.Text = birthdayString;
                        textBoxEmployeeAddress.Text = Address.ToString();
                        textBoxEmployeeSalary.Text = salary.ToString();
                        textBoxEmployeePhone.Text = phone.ToString();
                        textBoxEmployeeCode.Text = postalCode.ToString();




                        CreateListCountryEmployee(CountryId);
                        selectedCountryName = comboBoxEmployeeCountry.Text.ToString();
                        CreateListCityEmployee(cityId, selectedCountryName);
                        textBoxEmployeejobDesription.Text = jobDesription.ToString();

                        CreateListProffesion(proffesionId);


                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вийшло отримати дані з БД");
                }
            }
         }
        private void EmployeeOnViewForm(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = $"SELECT \r\n   Employees.EmployeeId, \r\n Employees.FirstName, \r\n Employees.LastName, \r\n    Employees.Birthday, \r\n  Addresses.Address, \r\n  \t\t\t\t   Addresses.PostalCode,                   Employees.PositionsId, \r\n                    Professions.Name AS 'Position', \r\n                    Employees.Salary, \r\n                    Employees.PhoneNumber, \r\n                    Cities.Name AS 'City', \r\n                    Countries.Name AS 'Country', \r\n                    Professions.JobDescription \r\n                FROM \r\n                    Employees \r\n                    JOIN Addresses ON Employees.AddressId = Addresses.AddressId \r\n                    JOIN Cities ON Addresses.CityId = Cities.CityId \r\n                    JOIN Countries ON Cities.CountyId = Countries.CountryId \r\n                    JOIN Professions ON Professions.ProfessionId = Employees.PositionsId ";

                    adapter = new SqlDataAdapter(query, connection);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView2.DataSource = ds.Tables[0];
                    dataGridView2.Columns["EmployeeId"].ReadOnly = true;

                    textBoxEmployeeId.DataBindings.Clear();
                    textBoxEmployeeName.DataBindings.Clear();
                    textBoxEmployeeSurname.DataBindings.Clear();
                    dateTimePickerEmployeeBirthday.DataBindings.Clear();
                    textBoxEmployeeAddress.DataBindings.Clear();
                    textBoxEmployeeSalary.DataBindings.Clear();
                    textBoxEmployeePhone.DataBindings.Clear();
                    textBoxEmployeeCode.DataBindings.Clear();
                    comboBoxEmployeeCity.DataBindings.Clear();
                    comboBoxEmployeeCountry.DataBindings.Clear();
                    comboBoxEmployeePosition.DataBindings.Clear();
                    textBoxEmployeejobDesription.DataBindings.Clear();

                    textBoxEmployeeId.DataBindings.Add("Text", ds.Tables[0], "EmployeeId", true);
                    textBoxEmployeeName.DataBindings.Add("Text", ds.Tables[0], "FirstName", true);
                    textBoxEmployeeSurname.DataBindings.Add("Text", ds.Tables[0], "LastName", true);
                    dateTimePickerEmployeeBirthday.DataBindings.Add("Text", ds.Tables[0], "Birthday", true);
                    textBoxEmployeeAddress.DataBindings.Add("Text", ds.Tables[0], "Address", true);
                    textBoxEmployeeSalary.DataBindings.Add("Text", ds.Tables[0], "Salary", true);
                    textBoxEmployeePhone.DataBindings.Add("Text", ds.Tables[0], "PhoneNumber", true);
                    textBoxEmployeeCode.DataBindings.Add("Text", ds.Tables[0], "PostalCode", true);
                    comboBoxEmployeeCity.DataBindings.Add("Text", ds.Tables[0], "City", true);
                    comboBoxEmployeeCountry.DataBindings.Add("Text", ds.Tables[0], "Country", true);
                    comboBoxEmployeePosition.DataBindings.Add("Text", ds.Tables[0], "Position", true);
                    textBoxEmployeejobDesription.DataBindings.Add("Text", ds.Tables[0], "JobDescription", true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вийшло отримати дані з БД");
                }
            }
        }

        private void CreateListCountryEmployee(int countryId)
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

                        comboBoxEmployeeCountry.Items.Add(countryName);

                        if (currentCountryId == countryId)
                        {
                            comboBoxEmployeeCountry.SelectedItem = countryName;
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

        private void CreateListCityEmployee(int cityId, string countryName)
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

                        comboBoxEmployeeCity.Items.Add(cityName);

                        if (currentCityId == cityId)
                        {
                            comboBoxEmployeeCity.SelectedItem = cityName;
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
        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(row);

            dataGridView1.ClearSelection();
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
            dataGridView1.Refresh();

        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            textBoxID.ReadOnly = true;
        }

        private void dateTimeBirthday_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTrait_TextChanged(object sender, EventArgs e)
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

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCountry.SelectedItem != null)
            {
                selectedCountryName = comboBoxCountry.Text.ToString();
                CreateListCity(0, selectedCountryName);
            }
        }
        private void CreateEmployee(int EmployeeId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = $"SELECT EmployeeId, FirstName + ' ' + LastName as FullName FROM Employees";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int employeeId = reader.GetInt32(0);
                        string FullName = reader.GetString(1);


                        textBoxEmployee.Items.Add(FullName);

                        if (employeeId == EmployeeId)
                        {
                            textBoxEmployee.SelectedItem = FullName;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                }
            }
        }
        private int GetEmployeeId(string fullName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = $"SELECT EmployeeId FROM Employees WHERE FirstName + ' ' + LastName = @FullName";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@FullName", SqlDbType.VarChar, 200).Value = fullName;

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Employee not found in the Employees table.");
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

        private void CreateRooms(int RoomId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = $"SELECT RoomId, RoomNumber FROM Rooms";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int roomId = reader.GetInt32(0);
                        int RoomNumber = reader.GetInt32(1);


                        textBoxRoom.Items.Add(RoomNumber);

                        if (RoomId == roomId)
                        {
                            textBoxRoom.SelectedItem = RoomNumber;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                }
            }
        }

        private void CreateTraits(int TraitId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = $"SELECT TraitId, Name FROM Traits";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int traitId = reader.GetInt32(0);
                        string Name = reader.GetString(1);


                        textBoxTrait.Items.Add(Name);

                        if (TraitId == traitId)
                        {
                            textBoxTrait.SelectedItem = Name;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                }
            }
        }

        private void textBoxEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    int patientID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PatientId"].Value);
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"DELETE Patients WHERE PatientId = @Id";
                    command.Parameters.Add("@Id", SqlDbType.Int);

                    ImageConverter converter = new ImageConverter();


                    command.Parameters["@Id"].Value = patientID;
                    command.ExecuteNonQuery();
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {

                        dataGridView1.Rows.Remove(row);
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось удалить из БД. " + exception.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    object selectedPatientIdValue = dataGridView1.SelectedRows[0].Cells["PatientId"].Value;

                    if (selectedPatientIdValue == DBNull.Value)
                    {
                        // Insert new user
                        string username = "user_" + Guid.NewGuid().ToString();
                        string password = GenerateRandomPassword();
                        string insertUserQuery = $"INSERT INTO Users (Username, Password, RoleId) VALUES ('{username}', '{password}', 4)";
                        command.CommandText = insertUserQuery;
                        command.ExecuteNonQuery();

                        // Get the generated UserId
                        command.CommandText = "SELECT SCOPE_IDENTITY()";
                        int userId = Convert.ToInt32(command.ExecuteScalar());

                        // Insert new patient with the obtained UserId
                        string insertPatientQuery = @"INSERT INTO Patients
                    (
                        FirstName,
                        LastName,
                        Gender,
                        Birthday,
                        AddressId,
                        RoomId,
                        EmployeeId,
                        TraitId,
                        ContactNumber,
                        Entry_Date,
                        UserId
                    )
                    VALUES
                    (
                        @FirstName,
                        @LastName,
                        @Gender,
                        @Birthday,
                        @AddressId,
                        @RoomId,
                        @EmployeeId,
                        @TraitId,
                        @ContactNumber,
                        @EntryDate,
                        @UserId
                    );";

                        command.Parameters.Clear();
                        command.CommandText = insertPatientQuery;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    }
                    else
                    {
                        // Update existing patient - your existing code here
                        int selectedPatientId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PatientId"].Value);
                        command.CommandText = @"UPDATE Patients
                                        SET
                                            FirstName = @FirstName,
                                            LastName = @LastName,
                                            Gender = @Gender,
                                            Birthday = @Birthday,
                                            AddressId = @AddressId,
                                            RoomId = @RoomId,
                                            EmployeeId = @EmployeeId,
                                            TraitId = @TraitId,
                                            ContactNumber = @ContactNumber,
                                            Entry_Date = @EntryDate
                                        WHERE
                                            PatientId = @PatientId;";
                        command.Parameters.Add("@PatientId", SqlDbType.Int).Value = selectedPatientId;
                    }

                    // Set other parameters and execute the command
                    string firstName = textBoxFirstName.Text;
                    string lastName = textBoxLastName.Text;
                    string phone = textBoxPhone.Text;
                    int cityId = GetCityId(comboBoxCity.Text, comboBoxCountry.Text);
                    int addressId = GetAddressId(textBoxAddress.Text, textBoxPostalCode.Text, cityId);
                    int employeeId = GetEmployeeId(textBoxEmployee.Text);
                    int roomId = GetRoomsId(textBoxRoom.Text);
                    int traitId = GetTraitsId(textBoxTrait.Text);

                    command.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = firstName;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = lastName;
                    DateTime birthday = dateTimeBirthday.Value;
                    DateTime entryDate = dateTimePickerEntryDate.Value;
                    command.Parameters.Add("@EntryDate", SqlDbType.Date).Value = entryDate;
                    command.Parameters.Add("@Gender", SqlDbType.VarChar, 50).Value = textBoxGender.Text;
                    command.Parameters.Add("@Birthday", SqlDbType.Date).Value = birthday;
                    command.Parameters.Add("@AddressId", SqlDbType.Int).Value = addressId;
                    command.Parameters.Add("@RoomId", SqlDbType.Int).Value = roomId;
                    command.Parameters.Add("@ContactNumber", SqlDbType.VarChar, 100).Value = phone;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;
                    command.Parameters.Add("@TraitId", SqlDbType.Int).Value = traitId;

                    command.ExecuteNonQuery();
                    dataGridView1.Refresh();
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update the record in the database. Error: " + ex.Message);
                }
                dataGridView1.Refresh();
                DisplayDataOnForm(id);

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
        private int GetRoomsId(string room)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT RoomId FROM Rooms WHERE RoomNumber = @Name";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = room;

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {

                        MessageBox.Show("Record not found in the Treatments table.");
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
        private int GetTraitsId(string trait)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT TraitId FROM Traits WHERE Name = @Name";
                    SqlCommand selectCommand = new SqlCommand(selectSql, connection);
                    selectCommand.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = trait;

                    object result = selectCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        // Trait not found, insert a new one
                        string insertSql = "INSERT INTO Traits (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                        SqlCommand insertCommand = new SqlCommand(insertSql, connection);
                        insertCommand.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = trait;

                        object newTraitId = insertCommand.ExecuteScalar();

                        if (newTraitId != null && newTraitId != DBNull.Value)
                        {
                            return Convert.ToInt32(newTraitId);
                        }
                        else
                        {
                            // Handle the case where the insertion did not succeed
                            MessageBox.Show("Failed to insert a new trait into the Traits table.");
                            return -1;
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
        static string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";
            const int passwordLength = 12; // You can adjust the length of the password

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }

            return password.ToString();
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    object selectedPatientIdValue = dataGridView2.SelectedRows[0].Cells["EmployeeId"].Value;

                    if (selectedPatientIdValue == DBNull.Value)
                    {
                        // Insert new user
                        string username = "user_" + Guid.NewGuid().ToString();
                        string password = GenerateRandomPassword();
                        string insertUserQuery = $"INSERT INTO Users (Username, Password, RoleId) VALUES ('{username}', '{password}', 2)";
                        command.CommandText = insertUserQuery;
                        command.ExecuteNonQuery();

                        // Get the generated UserId
                        command.CommandText = "SELECT SCOPE_IDENTITY()";
                        int userId = Convert.ToInt32(command.ExecuteScalar());

                        // Insert new patient with the obtained UserId
                        string insertPatientQuery = @"INSERT INTO Employees 
                                   (FirstName, LastName, Birthday, AddressId, PositionsId, Salary, PhoneNumber,  UserId) 
                                   VALUES 
                                   (@FirstName, @LastName, @Birthday, @AddressId, @PositionId, @Salary, @Phone, @UserId)";


                        command.Parameters.Clear();
                        command.CommandText = insertPatientQuery;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    }
                    else
                    {
                        int employeeId = GetEmployeeId(textBoxEmployee.Text);
                        command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;

                        command.CommandText = @"UPDATE Employees 
                                   SET FirstName = @FirstName, 
                                       LastName = @LastName, 
                                       Birthday = @Birthday, 
                                       AddressId = @AddressId, 
                                       PositionsId = @PositionId, 
                                       Salary = @Salary, 
                                       PhoneNumber = @Phone
                                   WHERE EmployeeId = @EmployeeId";
                       
                    }

                    string firstName = textBoxEmployeeName.Text;
                    string lastName = textBoxEmployeeSurname.Text;
                    int positionId = GetPositionId(comboBoxEmployeePosition.Text, textBoxEmployeejobDesription.Text);
                    decimal salary = decimal.Parse(textBoxEmployeeSalary.Text, CultureInfo.InvariantCulture);
                    string phone = textBoxEmployeePhone.Text;
                    int cityId = GetCityId(comboBoxEmployeeCity.Text, comboBoxEmployeeCountry.Text);
                    int addressId = GetAddressId(textBoxEmployeeAddress.Text, textBoxEmployeeCode.Text, cityId);
                   // command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = firstName;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = lastName;
                    DateTime birthday = dateTimeBirthday.Value;
                    command.Parameters.Add("@Birthday", SqlDbType.Date).Value = birthday;
                    command.Parameters.Add("@AddressId", SqlDbType.Int).Value = addressId;
                    command.Parameters.Add("@PositionId", SqlDbType.Int).Value = positionId;
                    command.Parameters.Add("@Salary", SqlDbType.Money).Value = salary;
                    command.Parameters.Add("@Phone", SqlDbType.VarChar, 100).Value = phone;
                    command.ExecuteNonQuery();


                    MessageBox.Show("Employee information updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update employee information. Error: " + ex.Message);
                }
                dataGridView2.Refresh();
                DisplayDataOnForm(id);
            }
        }
        private int GetPositionId(string positionName, string jobDescription)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ProfessionId FROM Professions WHERE Name = @PositionName AND JobDescription = @JobDescription";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@PositionName", SqlDbType.VarChar, 100).Value = positionName;
                    command.Parameters.Add("@JobDescription", SqlDbType.VarChar, 100).Value = jobDescription;

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {

                        return UpdateJobDescription(positionName, jobDescription);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                    return -1;
                }
            }
        }
        private void CreateListProffesion(int proffesionId)
        {
            comboBoxEmployeePosition.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT ProfessionId, Name \r\nFROM Professions";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int currebtProffesionId = reader.GetInt32(0);
                        string proffesionName = reader.GetString(1);

                        comboBoxEmployeePosition.Items.Add(proffesionName);

                        if (currebtProffesionId == proffesionId)
                        {
                            comboBoxEmployeePosition.SelectedItem = proffesionName;
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

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBoxEmployeeId.ReadOnly = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView2.Refresh();
            DataRow row = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(row);

            dataGridView2.ClearSelection();
            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Selected = true;
            dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0]; 
            dataGridView2.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    int employeeId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["EmployeeId"].Value);
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"DELETE Employees WHERE EmployeeId = @Id";
                    command.Parameters.Add("@Id", SqlDbType.Int);

                    ImageConverter converter = new ImageConverter();


                    command.Parameters["@Id"].Value = employeeId;
                    command.ExecuteNonQuery();
                    foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                    {

                        dataGridView2.Rows.Remove(row);
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось удалить из БД. " + exception.Message);
                }
            }
        }
        private int UpdateJobDescription(string positionName, string jobDescription)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string updateQuery = "UPDATE Professions SET JobDescription = @JobDescription WHERE Name = @PositionName; SELECT ProfessionId FROM Professions WHERE Name = @PositionName";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.Add("@PositionName", SqlDbType.VarChar, 100).Value = positionName;
                    updateCommand.Parameters.Add("@JobDescription", SqlDbType.VarChar, 100).Value = jobDescription;

                    object result = updateCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update job description for the existing profession.");
                        return -1;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to update job description. " + exception.Message);
                    return -1;
                }
            }
        }

        private void comboBoxEmployeeCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEmployeeCountry.SelectedItem != null)
            {
                selectedCountryNameEmployee = comboBoxEmployeeCountry.Text.ToString();
                CreateListCity(0, selectedCountryNameEmployee);
            }
        }
    }
}

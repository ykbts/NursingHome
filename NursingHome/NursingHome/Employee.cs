using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        public int idEmployee;
        string firstName;
        string lastName;
        int selectedCountryId;
        int cityId;
        string selectedCountryName;

        SqlDataAdapter adapter;
        DataSet ds;
        public Employee(int id)
        {
            InitializeComponent();
            DisplayDataOnForm(id);
            selectedCountryName = comboBoxCountry.Text.ToString();

            string sql = $"SELECT \r\n    P.PatientId,\r\n    P.FirstName,\r\n    P.LastName,\r\n    P.Gender,\r\n    P.Birthday,\r\n    Co.Name as 'Country',\r\n    C.Name as 'City',\r\n    R.Capacity,\r\n    R.RoomNumber,\r\n    P.ContactNumber,\r\n    P.Entry_Date\r\nFROM Patients P\r\nJOIN Addresses A ON P.AddressId = A.AddressId\r\nJOIN Rooms R ON P.RoomId = R.RoomId\r\nJOIN Traits T ON P.TraitId = T.TraitId\r\nJOIN Cities C ON A.CityId = C.CityId\r\nJOIN Countries Co ON C.CountyId = Co.CountryId\r\nWHERE P.EmployeeId = {idEmployee}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["PatientId"].ReadOnly = true;

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
                            selectedCountryId = reader.GetInt32(14);
                            cityId = reader.GetInt32(13);
                            int proffesionId = reader.GetInt32(18);

                            textBoxID.Text = idEmployee.ToString();
                            textBoxFirstName.Text = FirstName;
                            textBoxLastName.Text = LastName;
                            dateTimeBirthday.Text = birthdayString;
                            textBoxAddress.Text = Address.ToString();
                            textBoxSalary.Text = salary.ToString();
                            textBoxPhone.Text = phone.ToString();
                            textBoxPostalCode.Text = postalCode.ToString();




                        CreateListCountry(selectedCountryId);
                        selectedCountryName = comboBoxCountry.Text.ToString();
                        CreateListCity(cityId, selectedCountryName);
                        textBoxjobDesription.Text = jobDesription.ToString();

                        CreateListProffesion(proffesionId);


                        break;                      
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вийшло отримати дані з БД");
                }
            }
            string sql3 = $"select ScheduleId,CONCAT(Patients.FirstName, ' ', Patients.LastName) AS FullName, [Time], Treatments.Name as 'Title', Treatments.Duration, Places.Name as 'Place'\r\nfrom Schedules\r\njoin Patients on Schedules.PatientId = Patients.PatientId\r\njoin Treatments on Schedules.TreatmentId = Treatments.TreatmentId\r\njoin Places on Schedules.PlaceId = Places.PlaceId\r\nWhere Schedules.EmployeeId = {idEmployee}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql3, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView3.DataSource = ds.Tables[0];
            }

            comboBoxPatient.DataBindings.Add("Text", ds.Tables[0], "FullName", true);
            CreateListPatient();
            textBoxTime.DataBindings.Add("Text", ds.Tables[0], "Time", true);
            textBoxDuration.DataBindings.Add("Text", ds.Tables[0], "Duration", true);
            comboBoxPlace.DataBindings.Add("Text", ds.Tables[0], "Place", true);
            comboBoxTitle.DataBindings.Add("Text", ds.Tables[0], "Title", true);
            
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
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Professions' table. You can move, or remove it, as needed.
            this.professionsTableAdapter.Fill(this.nursingHomeDataSet.Professions);
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Addresses' table. You can move, or remove it, as needed.
            this.addressesTableAdapter.Fill(this.nursingHomeDataSet.Addresses);
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Cities' table. You can move, or remove it, as needed.
            this.citiesTableAdapter.Fill(this.nursingHomeDataSet.Cities);
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Countries' table. You can move, or remove it, as needed.
            this.countriesTableAdapter.Fill(this.nursingHomeDataSet.Countries);
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Places' table. You can move, or remove it, as needed.
            this.placesTableAdapter.Fill(this.nursingHomeDataSet.Places);
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Treatments' table. You can move, or remove it, as needed.
            this.treatmentsTableAdapter.Fill(this.nursingHomeDataSet.Treatments);
            // TODO: This line of code loads data into the 'nursingHomeDataSet.Schedules' table. You can move, or remove it, as needed.
            this.schedulesTableAdapter.Fill(this.nursingHomeDataSet.Schedules);


        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Assuming PatientId is stored in a specific cell (change index accordingly)
                    if (selectedRow.Cells["PatientId"].Value != null &&
                        int.TryParse(selectedRow.Cells["PatientId"].Value.ToString(), out int idPatient))
                    {
                        string sql2 = $"SELECT Medicines.Name, Dose, [Time] ,Medicines.Amount\r\nFROM Appointments \r\nJoin Medicines ON Appointments.MedicineId = Medicines.MedicineId WHERE PatientId = {idPatient}";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(sql2, connection);
                            DataSet ds = new DataSet();

                            connection.Open();
                            adapter.Fill(ds);
                            dataGridView2.DataSource = ds.Tables[0];
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid PatientId or null value.");
                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
          
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void textBoxp_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBoxBirthday_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimeBirthday_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Добавляем новую пустую строку по той же схеме что и в таблице и добавляем её туда
            DataRow row = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(row);

            //Выделяем созданную строку
            dataGridView3.ClearSelection();
            dataGridView3.Rows[dataGridView3.Rows.Count - 1].Selected = true;
            dataGridView3.CurrentCell = dataGridView3.Rows[dataGridView3.Rows.Count - 1].Cells[0];
            dataGridView3.Refresh();
        }

        private void textBoxDuration_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    object selectedScheduleIdValue = dataGridView3.SelectedRows[0].Cells["ScheduleId"].Value;

                    if (selectedScheduleIdValue == DBNull.Value)
                    {
                        // Insert new record since ScheduleId is DBNull
                        command.CommandText = @"INSERT INTO Schedules (PatientId, EmployeeId, TreatmentId, PlaceId, [Time]) 
                                       VALUES (@PatientId, @EmployeeId, @TreatmentId, @PlaceId, @Time);";

                    }
                    else
                    {
                        int selectedScheduleId = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["ScheduleId"].Value); 
                        command.CommandText = @"UPDATE Schedules 
                                   SET PatientId = @PatientId, 
                                       EmployeeId = @EmployeeId, 
                                       TreatmentId = @TreatmentId, 
                                       PlaceId = @PlaceId, 
                                       [Time] = @Time  
                                   WHERE ScheduleId = @SelectedScheduleId";

                        // Set the parameter values based on your UI controls
                        command.Parameters.Add("@SelectedScheduleId", SqlDbType.Int).Value = selectedScheduleId;
                    }
                    int treatmentId = GetTreatmentId(comboBoxTitle.Text, textBoxDuration.Text);
                    int placeId = GetPlacesId(comboBoxPlace.Text);
                    command.Parameters.Add("@PatientId", SqlDbType.Int).Value = Convert.ToInt32(comboBoxPatient.SelectedValue);
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = idEmployee; 
                    command.Parameters.Add("@TreatmentId", SqlDbType.Int).Value = treatmentId;
                    command.Parameters.Add("@PlaceId", SqlDbType.Int).Value = placeId;
                    command.Parameters.Add("@Time", SqlDbType.VarChar, 20).Value = textBoxTime.Text;

                    command.ExecuteNonQuery();
                    DisplayDataOnForm(id);
                    dataGridView3.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update the record in the database. Error: " + ex.Message);
                }

            }
        }

        private int GetTreatmentId(string name, string duration)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlSelect = "SELECT TreatmentId FROM Treatments WHERE Name = @Name AND Duration = @Duration";
                    SqlCommand selectCommand = new SqlCommand(sqlSelect, connection);
                    selectCommand.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = name;
                    selectCommand.Parameters.Add("@Duration", SqlDbType.VarChar, 255).Value = duration;

                    object result = selectCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        // Record not found, insert a new treatment
                        string sqlInsert = "INSERT INTO Treatments (Name, Duration) VALUES (@Name, @Duration); SELECT SCOPE_IDENTITY();";
                        SqlCommand insertCommand = new SqlCommand(sqlInsert, connection);
                        insertCommand.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = name;
                        insertCommand.Parameters.Add("@Duration", SqlDbType.VarChar, 255).Value = duration;

                        // Execute the insert command and get the newly inserted TreatmentId
                        object newTreatmentId = insertCommand.ExecuteScalar();

                        if (newTreatmentId != null && newTreatmentId != DBNull.Value)
                        {
                            //MessageBox.Show("New treatment inserted successfully.");
                            return Convert.ToInt32(newTreatmentId);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert a new treatment.");
                            return -1;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from or write to the database. " + exception.Message);
                    return -1;
                }
            }
        }


        private int GetPlacesId(string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT PlaceId FROM Places WHERE Name = @Name";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = name;

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

        private void GetPatient(string PatientId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = $"SELECT FirstName, LastName FROM Patients WHERE PatientId = {PatientId}";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                         firstName = reader.GetString(0);
                         lastName = reader.GetString(1);

                    }
                

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                    //    Convert.ToInt32(result);
                    }
                    else
                    {

                        MessageBox.Show("Record not found in the Treatments table.");
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                }
            }
        }
        private void CreateListPatient()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT PatientId, FirstName + ' ' + LastName as FullName FROM Patients";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    comboBoxPatient.DataSource = ds.Tables[0];
                    comboBoxPatient.DisplayMember = "FullName";
                    comboBoxPatient.ValueMember = "PatientId";

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to read from the database. " + exception.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
              
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        int selectedScheduleId = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["ScheduleId"].Value);
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = @"DELETE Schedules WHERE ScheduleId = @Id";
                        command.Parameters.Add("@Id", SqlDbType.Int);

                        ImageConverter converter = new ImageConverter();


                        command.Parameters["@Id"].Value = selectedScheduleId;
                        command.ExecuteNonQuery();
                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {

                        dataGridView3.Rows.Remove(row);
                    }

                }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Не получилось удалить из БД. " + exception.Message);
                    }
                }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
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
                    int positionId = GetPositionId(comboBoxPosition.Text, textBoxjobDesription.Text);
                    int salary = Convert.ToInt32(textBoxSalary.Text);
                    string phone = textBoxPhone.Text;
                    cityId = GetCityId(comboBoxCity.Text, comboBoxCountry.Text);
                    int addressId = GetAddressId(textBoxAddress.Text, textBoxPostalCode.Text, cityId);


                    command.CommandText = @"UPDATE Employees 
                                   SET FirstName = @FirstName, 
                                       LastName = @LastName, 
                                       Birthday = @Birthday, 
                                       AddressId = @AddressId, 
                                       PositionsId = @PositionId, 
                                       Salary = @Salary, 
                                       PhoneNumber = @Phone
                                   WHERE EmployeeId = @EmployeeId";
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = idEmployee;
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = firstName;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = lastName;
                    DateTime birthday = dateTimeBirthday.Value;
                    command.Parameters.Add("@Birthday", SqlDbType.Date).Value = birthday;
                    command.Parameters.Add("@AddressId", SqlDbType.Int).Value = addressId;
                    command.Parameters.Add("@PositionId", SqlDbType.Int).Value = positionId;
                    command.Parameters.Add("@Salary", SqlDbType.Int).Value = salary;
                    command.Parameters.Add("@Phone", SqlDbType.VarChar, 100).Value = phone;

                    command.ExecuteNonQuery();

                    MessageBox.Show("Employee information updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update employee information. Error: " + ex.Message);
                }
            }
        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCountry.SelectedItem != null)
            {
                selectedCountryName = comboBoxCountry.Text.ToString();
                CreateListCity(0, selectedCountryName); // Assuming 0 as the default cityId
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




        private void comboBoxPlace_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxPosition_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void CreateListProffesion(int proffesionId)
        {
            comboBoxPosition.Items.Clear();
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

                        comboBoxPosition.Items.Add(proffesionName);

                        if (currebtProffesionId == proffesionId)
                        {
                            comboBoxPosition.SelectedItem = proffesionName;
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

    }
}

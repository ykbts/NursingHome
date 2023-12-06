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

namespace NursingHome
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=KATYA; Initial Catalog=NursingHome; Integrated Security = True;";
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Id_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'nursingHomeDataSet1.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.nursingHomeDataSet1.Users);
            // TODO: This line of code loads data into the 'nursingHomeDataSet1.Roles' table. You can move, or remove it, as needed.
            this.rolesTableAdapter.Fill(this.nursingHomeDataSet1.Roles);
            this.rolesTableAdapter.Fill(this.nursingHomeDataSet.Roles);
            this.usersTableAdapter.Fill(this.nursingHomeDataSet.Users);

        }

        private void Continue_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string selectedRole = comboBoxChange.Text.ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT UserId FROM Users " +
                      "WHERE Username = @Username AND [Password] = @Password AND RoleId IN " +
                      "(SELECT RoleId FROM Roles WHERE RoleName = @RoleName);";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@RoleName", selectedRole);

                    var result = command.ExecuteScalar();
                    int userId = Convert.ToInt32(result);

                    if (result != null)
                    {
                            switch (selectedRole)
                            {
                                case "Employee":
                                        Employee employee = new Employee(userId);
                                        employee.Show();
                                        this.Hide();
                                        break;
                                case "Patient":
                                    Employee employee1 = new Employee(userId);
                                    employee1.Show();
                                    this.Hide();
                                    break;
                                case "Visitor":
                                    Employee employee3 = new Employee(userId);
                                    employee3.Show();
                                    this.Hide();
                                    break;
                                case "Admin":
                                    Employee employee5 = new Employee(userId);
                                    employee5.Show();
                                    this.Hide();
                                    break;
                                default:
                                        MessageBox.Show("Oops, try again");
                                        break;
                            }
                       
                    }
                    else
                    {
                        MessageBox.Show("Invalid username, password or role");
                    }
                }
            }

        }
    }
}

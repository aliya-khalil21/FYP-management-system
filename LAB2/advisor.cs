using CRUD_Operations;
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

namespace LAB2
{
    public partial class advisor : UserControl
    {
        private int id;
        private int retrievedId = -1;
        public advisor()
        {
            InitializeComponent();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkValidInputs(textBox2.Text, textBox6.Text, textBox3.Text, textBox4.Text, textBox5.Text))
            {
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a gender.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select a Designation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int designationId = ReturnSelectedDesignation(comboBox2.SelectedItem.ToString());
            int gender = returnSelectedGender(comboBox1.SelectedItem.ToString());
            if (!IsSalaryValid(textBox5.Text))
            {
                // Salary input is invalid, so exit the function
                return;
            }

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("INSERT INTO Person(FirstName,LastName,Contact,Email,DateOfBirth,Gender) VALUES (@FirstName,@LastName,@Contact,@Email,@DateOfBirth,@Gender); INSERT INTO Advisor(Id,Designation,salary) VALUES ((select id from person WHERE FirstName = @FirstName AND LastName=@LastName AND Contact=@Contact AND Email=@Email AND DateOfBirth=@DateOfBirth AND Gender=@Gender),@Designation,@Salary)", con);
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox6.Text);
                cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
                cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                cmd.Parameters.AddWithValue("@Salary", textBox5.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Designation", designationId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Advisor saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }

        }
        public int ReturnSelectedDesignation(string designation)
        {
            int designationId = 0;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Lookup WHERE Category='DESIGNATION' AND Value=@designation", con);
            cmd.Parameters.AddWithValue("@designation", designation);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                designationId = int.Parse(reader["Id"].ToString());
            }

            reader.Close();

            return designationId;
        }
        public int returnSelectedGender(string gen)
        {
            int g = 0;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Lookup WHERE Category='GENDER' AND Value=@gender", con);
            cmd.Parameters.AddWithValue("@gender", gen);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                g = int.Parse(reader["Id"].ToString());
            }
            reader.Close();
            return g;
        }
        public bool IsSalaryValid(string salaryString)
        {
            // Attempt to parse the salary string to a decimal
            if (decimal.TryParse(salaryString, out decimal salary))
            {
                // The parsing was successful, so the input is a valid number
                return true;
            }
            else
            {
                // The parsing failed, indicating that the input is not a valid number
                MessageBox.Show("Invalid Salary. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool checkValidInputs(string firstName, string lastName, string contact, string email, string salary)
        {
            if (firstName == "" || lastName == "" || firstName[0] == ' ' || lastName[0] == ' ')
            {
                MessageBox.Show("Invalid Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (contact.Length != 11 || contact[0] != '0' || contact[1] != '3')
            {
                MessageBox.Show("Invalid Contact Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!email.EndsWith("@gmail.com") || email[0] == ' ' || email == "@gmail.com" || email == "")
            {
                MessageBox.Show("Invalid Email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }
       /// <summary>
       /// //////////////////////////////////////add complete
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        /// ///////
        /// </summary>
        /// <param name="DATALOAD"></param>
        /// <param name="e"></param>

       

        private void button3_Click(object sender, EventArgs e)
        {
            if (retrievedId == -1)
            {
                MessageBox.Show("No ID retrieved. Cannot update information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check for valid inputs
            if (!checkValidInputs(textBox2.Text, textBox6.Text, textBox3.Text, textBox4.Text, textBox5.Text))
            {
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a gender.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select a Designation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get designation ID and gender
            int designationId = ReturnSelectedDesignation(comboBox2.SelectedItem.ToString());
            int gender = returnSelectedGender(comboBox1.SelectedItem.ToString());

            // Check salary validity
            if (!IsSalaryValid(textBox5.Text))
            {
                // Salary input is invalid, so exit the function
                return;
            }

            try
            {
                var con = Configuration.getInstance().getConnection();

                // Construct the SQL command for UPDATE operation
                SqlCommand cmd = new SqlCommand("UPDATE Person SET FirstName = @FirstName, LastName = @LastName," +
                    " Contact = @Contact, Email = @Email, DateOfBirth = @DateOfBirth, Gender = @Gender WHERE Id = @Id;" +
                    " UPDATE Advisor SET Designation = @Designation, Salary = @Salary WHERE Id = @Id", con);

                // Add parameters
                cmd.Parameters.AddWithValue("@Id", this.retrievedId); // Use the retrieved ID
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@LastName", textBox6.Text);
                cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
                cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                cmd.Parameters.AddWithValue("@Salary", textBox5.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Designation", designationId);

                // Execute the command
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully updated Advisor information");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating Advisor information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void advisor_Load(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Person.FirstName, Person.LastName, Person.Contact, Person.Email, Person.DateOfBirth, Person.Gender, Advisor.Salary, Lookup.Value AS Designation FROM Person INNER JOIN Advisor ON Person.Id = Advisor.Id INNER JOIN Lookup ON Advisor.Designation = Lookup.Id WHERE Lookup.Category = 'DESIGNATION' AND Person.FirstName NOT LIKE '!%' ", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading advisor data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            int designationId = ReturnSelectedDesignation(comboBox2.SelectedItem.ToString());
            try
            {
                var con = Configuration.getInstance().getConnection();

                SqlCommand cmd = new SqlCommand("SELECT P.Id FROM Person P JOIN Advisor A ON A.Id=P.Id JOIN Lookup L ON L.Id=P.Gender JOIN Lookup L1 ON L1.Id=A.Designation where p.FirstName=@FirstName and p.LastName=@LastName and p.Contact= @Contact and p.Email=@Email and A.Designation=@Designation", con);

                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox6.Text);
                cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
                cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                cmd.Parameters.AddWithValue("@Designation", designationId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.retrievedId = reader.GetInt32(0); // Store the retrieved ID
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving ID from the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string email = textBox7.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter a Registration No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            UpdateFirstNameByEmail(email);
        }
        private void UpdateFirstNameByEmail(string email)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE Person SET FirstName = '! ' + FirstName WHERE Id IN (SELECT Id FROM Advisor WHERE Email = @Email)", con);
                cmd.Parameters.AddWithValue("@Email", email);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("FirstName updated successfully for advisor with Email: " + email);
                }
                else
                {
                    MessageBox.Show("No advisor found with the provided Email: " + email, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating FirstName: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SearchFunction(textBox1.Text);
        }
        private void SearchFunction(string searchAttribute)
        {
            if (string.IsNullOrEmpty(searchAttribute))
            {
                MessageBox.Show("Please enter a search attribute.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"SELECT Person.*, Advisor.Salary, Lookup.Value AS Designation 
                                           FROM Person 
                                           INNER JOIN Advisor ON Person.Id = Advisor.Id 
                                           INNER JOIN Lookup ON Advisor.Designation = Lookup.Id 
                                           WHERE Lookup.Category = 'DESIGNATION' 
                                           AND (Person.FirstName LIKE @SearchAttribute 
                                                OR Person.LastName LIKE @SearchAttribute 
                                                OR Person.Contact LIKE @SearchAttribute 
                                                OR Person.Email LIKE @SearchAttribute 
                                                OR CONVERT(NVARCHAR(10), Person.DateOfBirth, 101) LIKE @SearchAttribute 
                                                OR (Person.Gender = CASE WHEN @SearchAttribute = 'Male' THEN 1 ELSE 2 END) 
                                                OR Advisor.Salary LIKE @SearchAttribute 
                                                OR Lookup.Value LIKE @SearchAttribute)", con);

                cmd.Parameters.AddWithValue("@SearchAttribute", "%" + searchAttribute + "%");

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    StringBuilder result = new StringBuilder();
                    while (reader.Read())
                    {
                        // Process data from the result set
                        result.AppendLine("Person Table:");
                        result.AppendLine($"ID: {reader["Id"]}, FirstName: {reader["FirstName"]}, LastName: {reader["LastName"]}, Contact: {reader["Contact"]}, Email: {reader["Email"]}, DateOfBirth: {(reader["DateOfBirth"] == DBNull.Value ? "N/A" : reader["DateOfBirth"].ToString())}, Gender: {(reader["Gender"] == DBNull.Value ? "N/A" : ((int)reader["Gender"] == 1 ? "Male" : "Female"))}, Salary: {(reader["Salary"] == DBNull.Value ? "N/A" : reader["Salary"].ToString())}, Designation: {(reader["Designation"] == DBNull.Value ? "N/A" : reader["Designation"].ToString())}");

                    }
                    MessageBox.Show("Search results:\n" + result.ToString());
                }
                else
                {
                    MessageBox.Show("No results found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// ///////
        /// </summary>
        /// <param name="DATALOAD"></param>
        /// <param name="e"></param>


    }
}

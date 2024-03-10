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
using LAB2.CLASS;
using System.Text.RegularExpressions;

namespace LAB2
{
    public partial class student : UserControl
    {
        public student()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(STU.checkValidInputs(textBox1.Text, textBox6.Text, textBox3.Text, textBox4.Text, textBox2.Text)))
            {
                return;
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a gender.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int gender = STU.returnSelectedGender(comboBox1.SelectedItem.ToString());
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("INSERT INTO Person(FirstName,LastName,Contact,Email,DateOfBirth,Gender) VALUES (@FirstName,@LastName,@Contact,@Email,@DateOfBirth,@Gender); INSERT INTO Student(Id,RegistrationNo) VALUES ((select id from person WHERE FirstName = @FirstName AND LastName=@LastName AND Contact=@Contact AND Email=@Email AND DateOfBirth=@DateOfBirth AND Gender=@Gender), @RegNo)", con);
                cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox6.Text);
                cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
                cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@RegNo", textBox2.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully student saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }
            LoadActiveStudents();
        }
        private void LoadActiveStudents()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Person.FirstName, Person.LastName, Person.Contact, Person.Email, Person.DateOfBirth, Person.Gender, Student.RegistrationNo \r\nFROM Person \r\nINNER JOIN Student ON Person.Id = Student.Id \r\nWHERE Person.FirstName NOT LIKE '!%'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void student_Load(object sender, EventArgs e)
        {
            LoadActiveStudents();
        }
        private void LoadAllStudents()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Person.FirstName, Person.LastName, Person.Contact, Person.Email, Person.DateOfBirth, " +
                                                 "CASE WHEN Person.Gender = 1 THEN 'Male' ELSE 'Female' END AS Gender, " +
                                                 "Student.RegistrationNo " +
                                                 "FROM Person " +
                                                 "INNER JOIN Student ON Person.Id = Student.Id", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string regNo = textBox2.Text;

            // Check if the inputs are valid
            if (!(STU.checkValidInputs1(textBox1.Text, textBox6.Text, textBox3.Text, textBox4.Text)))
            {
                return;
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a gender.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int gender = STU.returnSelectedGender(comboBox1.SelectedItem.ToString());

            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Person SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Email = @Email, DateOfBirth = @DateOfBirth, Gender = @Gender WHERE Id IN (SELECT S.Id FROM Student S JOIN Person P ON S.Id = P.Id JOIN Lookup L ON L.Id = P.Gender WHERE S.RegistrationNo = @RegNo)", con);
                    cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                    cmd.Parameters.AddWithValue("@LastName", textBox6.Text);
                    cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@RegNo", regNo);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Student information updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No student found with the provided registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating student information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadAllStudents();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string regNo = textBox5.Text.Trim();

            if (string.IsNullOrEmpty(regNo))
            {
                MessageBox.Show("Please enter a Registration No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Call the update function
            UpdateFirstNameByRegNo(regNo);
            LoadActiveStudents();
        }
        private void UpdateFirstNameByRegNo(string regNo)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE Person SET FirstName = '! ' + FirstName WHERE Id IN (SELECT Id FROM Student WHERE RegistrationNo = @RegNo)", con);
                cmd.Parameters.AddWithValue("@RegNo", regNo);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("FirstName updated successfully for student with Registration No: " + regNo);
                }
                else
                {
                    MessageBox.Show("No student found with the provided Registration No: " + regNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating FirstName: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SearchPerson(textBox7.Text);
        }
        private void SearchPerson(string searchAttribute)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT S.RegistrationNo AS [Reg], (P.FirstName + ' ' + P.LastName) AS Name, L.Value AS Gender, CONVERT(varchar, P.DateOfBirth, 105) AS [DoB], P.Contact, P.Email FROM Person P JOIN Student S ON S.Id = P.Id JOIN Lookup L ON L.Id = P.Gender WHERE S.RegistrationNo LIKE @FirstName  and P.FirstName NOT LIKE '!%' ORDER BY CASE WHEN S.RegistrationNo = @FirstName THEN 0 ELSE 1 END", con);
                cmd.Parameters.AddWithValue("@FirstName", "%" + searchAttribute + "%");

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    StringBuilder result = new StringBuilder();
                    while (reader.Read())
                    {
                        result.AppendLine($"Registration No: {reader["Reg"]}, Name: {reader["Name"]}, Gender: {reader["Gender"]}, Date of Birth: {reader["DoB"]}, Contact: {reader["Contact"]}, Email: {reader["Email"]}");
                    }

                    MessageBox.Show("Search results:\n" + result.ToString(), "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No results found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

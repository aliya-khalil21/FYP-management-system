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
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LAB2
{
    public partial class groupadd : UserControl
    {
        public int latestGroupId = -1; // Global variable to store the latest group ID
        public int projectid = -1;
        private bool projectSelected = false;
        public groupadd()
        {
            InitializeComponent();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(userControl);
            userControl.BringToFront();

        }
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                // Get the current date and time
                DateTime currentDate = DateTime.Now;

                // Get the database connection
                var con = Configuration.getInstance().getConnection();

                // Insert a new group into the database
                SqlCommand cmdInsertGroup = new SqlCommand("INSERT INTO [dbo].[Group] (Created_On) VALUES (@date); SELECT SCOPE_IDENTITY();", con);
                cmdInsertGroup.Parameters.AddWithValue("@date", currentDate);
                latestGroupId = Convert.ToInt32(cmdInsertGroup.ExecuteScalar());

                // Clear the project selection
                //  comboBox1.SelectedIndex = -1;

                // Display the latest group ID in a message box
                MessageBox.Show("Group created successfully. Latest Group ID: " + latestGroupId, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating group: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
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

        private void button4_Click(object sender, EventArgs e)
        {

          
                // Check if any row is selected in DataGridView1
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Get the selected row
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Add columns to DataGridView2 if not already added
                    if (dataGridView2.Columns.Count == 0)
                    {
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            dataGridView2.Columns.Add(column.Clone() as DataGridViewColumn);
                        }
                    }

                    // Add the selected row to DataGridView2
                    DataGridViewRow newRow = (DataGridViewRow)selectedRow.Clone();
                    foreach (DataGridViewCell cell in selectedRow.Cells)
                    {
                        newRow.Cells[cell.ColumnIndex].Value = cell.Value;
                    }
                    dataGridView2.Rows.Add(newRow);
                }
                else
                {
                    MessageBox.Show("Please select a row in DataGridView1.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
                // Check if any row is selected in DataGridView2
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    // Remove the selected row from DataGridView2
                    dataGridView2.Rows.Remove(dataGridView2.SelectedRows[0]);
                }
                else
                {
                    MessageBox.Show("Please select a row in DataGridView2 to remove.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells["RegistrationNo"].Value != null)
                    {
                        // Get the registration number from the cell
                        string registrationNo = row.Cells["RegistrationNo"].Value.ToString();

                        // Get the student ID using the registration number
                        int studentId = GetStudentIdByRegistrationNo(registrationNo);

                        if (studentId != -1)
                        {
                            int groupId = latestGroupId; // Replace 1 with the actual group ID to which you want to add the student
                            bool status = true; // Assuming the default status is true
                            DateTime assignmentDate = DateTime.Now; // Assuming the assignment date is the current date and time

                            addStuGroup(groupId, studentId, status, assignmentDate);

                            // Move the student to the respective folder
                            // Implement your logic to move the student here

                            MessageBox.Show($"Student with registration number {registrationNo} moved to respective folder and added to group {groupId}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"No student found with registration number {registrationNo}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error moving students to folders and adding to group: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void addStuGroup(int Gid, int Sid, bool status, DateTime dtime)
        {
            var con = Configuration.getInstance().getConnection();
          
            SqlCommand cmd = new SqlCommand("Insert into GroupStudent values( @gid, @sid, @st, @dtim )", con);
            cmd.Parameters.AddWithValue("gid", Gid);
            cmd.Parameters.AddWithValue("sid", Sid);
            cmd.Parameters.AddWithValue("st", status);
            cmd.Parameters.AddWithValue("dtim", dtime);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
        }
        private int GetStudentIdByRegistrationNo(string registrationNo)
        {
          
                int studentId = -1; // Default value if no student ID is found

                try
                {
                    var con = Configuration.getInstance().getConnection();

                    // Check if connection is null
                    if (con != null)
                    {
                        string query = "SELECT Id FROM Student WHERE RegistrationNo = @RegNo";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@RegNo", registrationNo);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                studentId = Convert.ToInt32(result);
                            }
                        }
                    }
                    else
                    {
                        // Handle null connection gracefully
                        Console.WriteLine("Connection is null. Unable to retrieve student ID.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    Console.WriteLine("Error retrieving student ID: " + ex.Message);
                }

                return studentId;
            

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

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
    public partial class assignproject : UserControl
    {
        public assignproject()
        {
            InitializeComponent();
        }

        private void assignproject_Load(object sender, EventArgs e)
        {
            LoadDesignationPersons();
            PopulateGroupDetails();
            LoadProjectData();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }
        public void LoadDesignationPersons()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                string query = @"
            SELECT p.FirstName, p.LastName, p.Contact, p.Email, p.DateOfBirth, l.Value AS Designation
            FROM Person p 
            JOIN Advisor a ON p.ID = a.ID
            JOIN Lookup l ON a.Designation = l.Id
            WHERE l.Category = 'DESIGNATION' AND p.FirstName NOT LIKE '!%'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);

                    // Bind the DataTable to DataGridView1
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void PopulateGroupDetails()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                using (DataTable dt = new DataTable("Groups"))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT id as [Group No], Created_On as [Creation Date] FROM [dbo].[Group]", con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                    dataGridView3.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadProjectData()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Title, Description FROM Project", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // If there are rows in the DataTable, bind it to the DataGridView
                    dataGridView4.DataSource = dt;
                }
                else
                {
                    // If no rows are returned, show a message
                    MessageBox.Show("No projects found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private int DetermineAdvisorRole(string textBoxText)
        {
            switch (textBoxText.ToLower())
            {
                case "textbox3":
                    return 11; // Main Advisor
                case "textbox1":
                    return 12; // Co-Advisor
                case "textbox2":
                    return 14; // Industry Advisor
                default:
                    throw new ArgumentException("Invalid textbox name");
            }
        }

        private int RetrieveAdvisorIdFromTextBox(string advisorName)
        {
            var con = Configuration.getInstance().getConnection();
            int advisorId = 0;
            string query = "SELECT Id FROM Person WHERE FirstName = @FirstName";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@FirstName", advisorName);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                advisorId = reader.GetInt32(0);
            }
            reader.Close();

            return advisorId;
        }
        private int RetrieveProjectIdFromName(string projectName)
        {
            var con = Configuration.getInstance().getConnection();
            int projectId = 0;
            string query = "SELECT Id FROM Project WHERE Title = @Title";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Title", projectName);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    projectId = reader.GetInt32(0);
                }
                reader.Close();
            }

            return projectId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                var con = Configuration.getInstance().getConnection();
                int projectId = RetrieveProjectIdFromName(dataGridView4.CurrentRow.Cells["Title"].Value.ToString());
                int groupId = Convert.ToInt32(dataGridView3.CurrentRow.Cells["Group No"].Value);
                DateTime assignmentDate = dateTimePicker1.Value;

                int mainAdvisorId = RetrieveAdvisorIdFromTextBox(textBox3.Text);
                int coAdvisorId = RetrieveAdvisorIdFromTextBox(textBox1.Text);
                int industryAdvisorId = RetrieveAdvisorIdFromTextBox(textBox2.Text);
                SaveGroupProject(projectId, groupId, assignmentDate);
                SaveProjectAdvisor(projectId, mainAdvisorId, DetermineAdvisorRole(textBox3.Name), assignmentDate);
                SaveProjectAdvisor(projectId, coAdvisorId, DetermineAdvisorRole(textBox1.Name), assignmentDate);
                SaveProjectAdvisor(projectId, industryAdvisorId, DetermineAdvisorRole(textBox2.Name), assignmentDate);


                MessageBox.Show("Assignment saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            
        }
        private void SaveGroupProject(int projectId, int groupId, DateTime assignmentDate)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "INSERT INTO GroupProject (ProjectId, GroupId, AssignmentDate) VALUES (@ProjectId, @GroupId, @AssignmentDate)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ProjectId", projectId);
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@AssignmentDate", assignmentDate);

            cmd.ExecuteNonQuery();
        }
        private void SaveProjectAdvisor(int projectId, int advisorId, int role, DateTime assignmentDate)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "INSERT INTO ProjectAdvisor (ProjectId, AdvisorId, AdvisorRole, AssignmentDate) VALUES (@ProjectId, @AdvisorId, @Role, @AssignmentDate)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ProjectId", projectId);
            cmd.Parameters.AddWithValue("@AdvisorId", advisorId);
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@AssignmentDate", assignmentDate);

            cmd.ExecuteNonQuery();
        }
    }

}


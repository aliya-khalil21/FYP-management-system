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
    public partial class group_view : UserControl
    {
        public group_view()
        {
            InitializeComponent();
        }

        private void group_view_Load(object sender, EventArgs e)
        {
            PopulateGroupDetails();
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
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static DataTable GetStuFromGid(int Gid)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                using (DataTable dt = new DataTable("Student"))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT p.id, p.FirstName, p.LastName, s.RegistrationNo, p.Contact, p.Email " +
                                                           "FROM Student AS s " +
                                                           "JOIN Person AS p ON s.Id = p.Id " +
                                                           "JOIN Lookup AS l ON p.Gender = l.Id " +
                                                           "JOIN GroupStudent AS GS ON GS.StudentId = p.Id " +
                                                           "WHERE GS.GroupId = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", Gid);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("Error getting student details: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // Check if the clicked cell is in the first column
            {
                int groupId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value); // Get the group ID from the clicked row
                dataGridView2.DataSource = GetStuFromGid(groupId); // Populate the second DataGridView with student details
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

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
    public partial class evaluationupdate : UserControl
    {
        public evaluationupdate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE Evaluation SET TotalMarks = @TotalMarks, TotalWeightage = @TotalWeightage WHERE Name = @Name", con);
                cmd.Parameters.AddWithValue("@TotalMarks", textBox7);
                cmd.Parameters.AddWithValue("@TotalWeightage", textBox1);
                cmd.Parameters.AddWithValue("@Name", textBox2);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Evaluation information updated successfully.");
                }
                else
                {
                    MessageBox.Show("No evaluation found with the provided name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating evaluation information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

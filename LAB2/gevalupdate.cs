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
    public partial class gevalupdate : UserControl
    {
        public gevalupdate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateGroupEvaluation(Convert.ToInt32(textBox3.Text), textBox1.Text, Convert.ToInt32(textBox4.Text), dateTimePicker1.Value);

        }
        public void UpdateGroupEvaluation(int groupId, string evaluationName, int obtainedMarks, DateTime evaluationDate)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                con.Open();

                // Get the EvaluationId using the evaluation name
                int evaluationId = GetEvaluationIdByName(evaluationName);

                // Check if the EvaluationId is valid
                if (evaluationId != -1)
                {
                    // Update the GroupEvaluation table
                    string query = "UPDATE GroupEvaluation SET ObtainedMarks = @obtainedMarks, EvaluationDate = @evaluationDate WHERE GroupId = @groupId AND EvaluationId = @evaluationId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@groupId", groupId);
                        cmd.Parameters.AddWithValue("@evaluationId", evaluationId);
                        cmd.Parameters.AddWithValue("@obtainedMarks", obtainedMarks);
                        cmd.Parameters.AddWithValue("@evaluationDate", evaluationDate);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Group Evaluation updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Evaluation not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating Group Evaluation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int GetEvaluationIdByName(string evaluationName)
        {
            try
            {
                int evaluationId = -1; // Default value if evaluation ID is not found
                var con = Configuration.getInstance().getConnection();

                string query = "SELECT Id FROM Evaluation WHERE Name = @evaluationName";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@evaluationName", evaluationName);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value) // Check for DBNull.Value as well
                    {
                        evaluationId = Convert.ToInt32(result);
                    }
                }
                return evaluationId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting evaluation ID: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            UpdateGroupEvaluation(Convert.ToInt32(textBox3.Text), textBox1.Text, Convert.ToInt32(textBox4.Text), dateTimePicker1.Value);

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           

        }
    }
}

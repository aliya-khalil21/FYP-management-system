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
    public partial class gevaladd : UserControl
    {
        public gevaladd()
        {
            InitializeComponent();
        }
        private bool GroupExists(int groupId)
        {
            // Check if the group ID exists in the database
            // Implement your database query here
            return true; // Dummy return, replace it with actual logic
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        private void SaveGroupEvaluation(int groupId, int evaluationId, int obtainedMarks, DateTime evaluationDate)
        {
            try
            {
                // Save the data into the GroupEvaluation table in the database
                var con = Configuration.getInstance().getConnection();
                string query = "INSERT INTO GroupEvaluation (GroupId, EvaluationId, ObtainedMarks, EvaluationDate) VALUES (@groupId, @evaluationId, @obtainedMarks, @evaluationDate)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@groupId", groupId);
                    cmd.Parameters.AddWithValue("@evaluationId", evaluationId);
                    cmd.Parameters.AddWithValue("@obtainedMarks", obtainedMarks);
                    cmd.Parameters.AddWithValue("@evaluationDate", evaluationDate);

                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving Group Evaluation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            int groupId;
            if (int.TryParse(textBox3.Text, out groupId))
            {
                if (GroupExists(groupId))
                {
                    string evaluationName = textBox1.Text;
                    int obtainedMarks = Convert.ToInt32(textBox4.Text); // Assuming obtained marks are entered in textBox3
                    DateTime evaluationDate = dateTimePicker1.Value; // Assuming evaluation date is selected using dateTimePicker1

                    int evaluationId = GetEvaluationIdByName(evaluationName);
                    if (evaluationId != -1)
                    {
                        // Save the data into GroupEvaluation table
                        SaveGroupEvaluation(groupId, evaluationId, obtainedMarks, evaluationDate);
                        MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Evaluation not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Group ID does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Group ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}

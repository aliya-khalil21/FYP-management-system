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
    public partial class evalutionadd : UserControl
    {
        public evalutionadd()
        {
            InitializeComponent();
        }
        private bool IsValidInput()
        {
            return !string.IsNullOrWhiteSpace(textBox2.Text)
                && !string.IsNullOrWhiteSpace(textBox7.Text)
                && !string.IsNullOrWhiteSpace(textBox1.Text);
        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {

            if (!IsValidInput())
            {
                MessageBox.Show("Please fill in all fields with valid values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Parse input values
            string name = textBox2.Text.Trim();
            int totalMarks;
            decimal totalWeightage;

            if (!int.TryParse(textBox7.Text, out totalMarks) || totalMarks <= 0)
            {
                MessageBox.Show("Please enter a valid positive integer for Total Marks.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(textBox1.Text, out totalWeightage) || totalWeightage <= 0 || totalWeightage > 100)
            {
                MessageBox.Show("Please enter a valid positive decimal number less than or equal to 100 for Total Weightage.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Insert data into the database
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("INSERT INTO Evaluation (Name, TotalMarks, TotalWeightage) VALUES (@Name, @TotalMarks, @TotalWeightage)", con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                cmd.Parameters.AddWithValue("@TotalWeightage", totalWeightage);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Evaluation data added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear input fields after successful insertion
                    textBox2.Clear();
                    textBox7.Clear();
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show("Failed to add evaluation data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting evaluation data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

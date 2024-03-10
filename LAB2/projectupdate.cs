using CRUD_Operations;
using LAB2.CLASS;
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
    public partial class projectupdate : UserControl
    {
        public projectupdate()
        {
            InitializeComponent();
        }
        public static bool checkValidInputs(string projectTitle, string projectDescription)
        {
            if (string.IsNullOrWhiteSpace(projectTitle) || string.IsNullOrWhiteSpace(projectDescription))
            {
                MessageBox.Show("Please enter both project title and description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!(checkValidInputs(textBox7.Text, richTextBox1.Text)))
            {
                return;
            }

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE Project SET Description = @Description WHERE Title = @Title", con);
                cmd.Parameters.AddWithValue("@Title", textBox7.Text); // Extract the text value from the TextBox control
                cmd.Parameters.AddWithValue("@Description", richTextBox1.Text); // Extract the text value from the RichTextBox control

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Successfully updated project description based on the title");
                }
                else
                {
                    MessageBox.Show("No project found with the provided title", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating project description: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LAB2.CLASS;
namespace LAB2
{
    public partial class projectadd : UserControl
    {
        public projectadd()
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
                SqlCommand cmd = new SqlCommand("INSERT INTO Project(Title, Description) VALUES (@Title, @Description)", con);
                cmd.Parameters.AddWithValue("@Title", textBox7.Text);
                cmd.Parameters.AddWithValue("@Description", richTextBox1.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully added project title and description");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

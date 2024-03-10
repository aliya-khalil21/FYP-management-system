using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB2
{
    public partial class project : UserControl
    {
        public project()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            projectadd student = new projectadd();
            adduserControl(student);
        }
        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(userControl);
            userControl.BringToFront();

        }

        private void button5_Click(object sender, EventArgs e)
        {

            projectview student = new projectview();
            adduserControl(student);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            projectupdate student = new projectupdate();
            adduserControl(student);
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

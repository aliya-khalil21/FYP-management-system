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
    public partial class group : UserControl
    {
        public group()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupadd student = new groupadd();
            adduserControl(student);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            group_view student =new  group_view();
            adduserControl(student);
        }
        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(userControl);
            userControl.BringToFront();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            assignproject student = new assignproject();
            adduserControl(student);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
       
}

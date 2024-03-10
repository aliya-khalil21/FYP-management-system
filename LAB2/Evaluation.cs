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
    public partial class Evaluation : UserControl
    {
        public Evaluation()
        {
            InitializeComponent();
        }
        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(userControl);
            userControl.BringToFront();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            evalutionadd student = new evalutionadd();
            adduserControl(student);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            evaluationupdate student = new evaluationupdate();
            adduserControl(student);
        }

        private void button5_Click(object sender, EventArgs e)
        {
             evaluationview student = new evaluationview();
            adduserControl(student);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gevaladd student = new gevaladd();
            adduserControl(student);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            gevalupdate student = new gevalupdate();
            adduserControl(student);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GEview student = new GEview();
            adduserControl(student);
        }
    }
}

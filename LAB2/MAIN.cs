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
    public partial class MAIN : Form
    {
        public MAIN()
        {
            InitializeComponent();
        }
        private void adduserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel4.Controls.Clear();
            panel4.Controls.Add(userControl);
            userControl.BringToFront();

        }

       
        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MAIN_Load(object sender, EventArgs e)
        {

        }

       

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void button1_Click(object sender, EventArgs e)
        {

            student student = new student();
            adduserControl(student);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            advisor student = new advisor();
            adduserControl(student);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            project student = new project();
            adduserControl(student);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            group student = new group();
            adduserControl(student);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Evaluation student = new Evaluation();
            adduserControl(student);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            REPORT student = new REPORT();
            adduserControl(student);
        
    }
    }
}

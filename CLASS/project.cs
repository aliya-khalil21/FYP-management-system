using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB2.CLASS
{
    internal class project
    {
        public static bool checkValidInputs(string projectTitle, string projectDescription)
        {
            if (string.IsNullOrWhiteSpace(projectTitle) || string.IsNullOrWhiteSpace(projectDescription))
            {
                MessageBox.Show("Please enter both project title and description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}

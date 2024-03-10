using CRUD_Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB2.CLASS
{
    internal class STU
    {
        public static int returnSelectedGender(string gen)
        {
            int g = 0;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Lookup WHERE Category='GENDER' AND Value=@gender", con);
            cmd.Parameters.AddWithValue("@gender", gen);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                g = int.Parse(reader["Id"].ToString());
            }
            reader.Close();
            return g;
        }
        public static bool checkValidInputs(string firstName, string lastName, string contact, string email, string regNo)
        {
            if (firstName == "" || lastName == "" || firstName[0] == ' ' || lastName[0] == ' ')
            {
                MessageBox.Show("Invalid Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (contact.Length != 11 || contact[0] != '0' || contact[1] != '3')
            {
                MessageBox.Show("Invalid Contact Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!email.EndsWith("@gmail.com") || email[0] == ' ' || email == "@gmail.com" || email == "")
            {
                MessageBox.Show("Invalid Email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (regNo == "" || regNo[0] == ' ' || query.isRegNoExist(regNo.ToUpper()))
            {
                MessageBox.Show("Invalid Registration Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        public static bool checkValidInputs1(string firstName, string lastName, string contact, string email)
        {
            if (firstName == "" || lastName == "" || firstName[0] == ' ' || lastName[0] == ' ')
            {
                MessageBox.Show("Invalid Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (contact.Length != 11 || contact[0] != '0' || contact[1] != '3')
            {
                MessageBox.Show("Invalid Contact Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!email.EndsWith("@gmail.com") || email[0] == ' ' || email == "@gmail.com" || email == "")
            {
                MessageBox.Show("Invalid Email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }



            return true;
        }
    }
}

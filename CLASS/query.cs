using CRUD_Operations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2.CLASS
{
    internal class query
    {
        public static bool isRegNoExist(string regNo)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Student WHERE RegistrationNo = @regNo", con);
            cmd.Parameters.AddWithValue("@regNo", regNo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable atdDt = new DataTable();
            da.Fill(atdDt);

            if ((int)atdDt.Rows[0][0] == 0)
            {
                return false;
            }
            return true;
        }
    }
}

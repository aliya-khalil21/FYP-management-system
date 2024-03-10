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
    public partial class GEview : UserControl
    {
        public GEview()
        {
            InitializeComponent();
        }

        private void GEview_Load(object sender, EventArgs e)
        {
            LoadEvaluationData();
        }
        public void LoadEvaluationData()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                

                string query = @"SELECT E.Id, E.Name, E.TotalMarks, SUM(G.ObtainedMarks) AS TotalObtainedMarks
                        FROM Evaluation AS E
                        LEFT JOIN GroupEvaluation AS G ON E.Id = G.EvaluationId
                        GROUP BY E.Id, E.Name, E.TotalMarks";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading evaluation data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

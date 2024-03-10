using CRUD_Operations;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB2
{
    public partial class REPORT : UserControl
    {
        public REPORT()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Document dp = new Document();
                PdfWriter.GetInstance(dp, new FileStream(@"D:\final mid\LAB2\reports\ProjectDetails.pdf", FileMode.Create));
                dp.Open();
                var p = new Paragraph(" List of projects along with advisory board and list of students  \n");
                var p1 = new Paragraph("\n");

                dp.Add(p);
                dp.Add(p1);

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"SELECT 
    pr.Title AS Project_Title,
	CONCAT_WS(' ', p.FirstName, p.LastName) AS Advisor_Name,
    l.Value AS Advisor_Role_Name,
    
    
    CONCAT_WS(' ', s.FirstName, s.LastName) AS Student_Name
FROM 
    [dbo].[ProjectAdvisor] pa
INNER JOIN [dbo].[Lookup] l ON pa.AdvisorRole = l.Id
INNER JOIN [dbo].[Person] p ON pa.AdvisorId = p.Id
INNER JOIN [dbo].[Project] pr ON pa.ProjectId = pr.Id
LEFT JOIN [dbo].[GroupProject] gp ON pr.Id = gp.ProjectId
LEFT JOIN [dbo].[GroupStudent] gs ON gp.GroupId = gs.GroupId
LEFT JOIN [dbo].[Person] s ON gs.StudentId = s.Id
WHERE 
    p.FirstName NOT LIKE '!%' AND
    s.FirstName NOT LIKE '!%';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Create a PDF table
                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.Padding = 6;
                table.DefaultCell.PaddingTop = 10;
                table.WidthPercentage = 100;

                // Add column headers to the PDF table
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dt.Columns[j].ColumnName, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
                }

                // Add data rows to the PDF table
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            table.AddCell(new Phrase(dt.Rows[i][j].ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK)));
                        }
                    }
                }

                dp.Add(table);
                dp.Close();
                MessageBox.Show("PDF generated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                Document dp = new Document();
                PdfWriter.GetInstance(dp, new FileStream(@"D:\final mid\LAB2\reports\evaluation.pdf", FileMode.Create));
                dp.Open();
                var p = new Paragraph("Marks sheet of projects that shows the marks in each evaluation against each student \n");
                var p1 = new Paragraph("\n");
                dp.Add(p);
                dp.Add(p1);

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"SELECT 
    e.Name AS Evaluation_Name,
    e.TotalMarks AS Total_Marks,
    ge.GroupId,
    ge.ObtainedMarks AS Obtained_Marks,
    gs.StudentId,
    CONCAT_WS(' ', p.FirstName, p.LastName) AS Student_Name,
    s.RegistrationNo
FROM 
    [dbo].[Evaluation] e
INNER JOIN [dbo].[GroupEvaluation] ge ON e.Id = ge.EvaluationId
INNER JOIN [dbo].[GroupStudent] gs ON ge.GroupId = gs.GroupId
INNER JOIN [dbo].[Person] p ON gs.StudentId = p.Id
INNER JOIN [dbo].[Student] s ON gs.StudentId = s.Id
WHERE 
    p.FirstName NOT LIKE '!%';
", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Create a PDF table
                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.Padding = 6;
                table.DefaultCell.PaddingTop = 10;
                table.WidthPercentage = 100;

                // Add column headers to the PDF table
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dt.Columns[j].ColumnName, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
                }

                // Add data rows to the PDF table
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            table.AddCell(new Phrase(dt.Rows[i][j].ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK)));
                        }
                    }
                }

                dp.Add(table);
                dp.Close();
                MessageBox.Show("PDF generated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Document dp = new Document();
                PdfWriter.GetInstance(dp, new FileStream(@"D:\final mid\LAB2\reports\AdvisorDetails.pdf", FileMode.Create));
                dp.Open();
                var p = new Paragraph("Advisor Details \n");
                var p1 = new Paragraph("\n");

                dp.Add(p);
                dp.Add(p1);

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"SELECT 
    CONCAT_WS(' ', p.FirstName, p.LastName) AS Advisor_Name,
    l.Value AS Designation,
    a.Salary,
    p.Contact AS Contact,
	p.email AS Email,
    DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) AS Age
FROM 
    [dbo].[Advisor] a
INNER JOIN [dbo].[Person] p ON a.Id = p.Id
INNER JOIN [dbo].[Lookup] l ON a.Designation = l.Id
WHERE 
    p.FirstName NOT LIKE '!%';

", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Create a PDF table
                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.Padding = 6;
                table.DefaultCell.PaddingTop = 10;
                table.WidthPercentage = 100;

                // Add column headers to the PDF table
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dt.Columns[j].ColumnName, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
                }

                // Add data rows to the PDF table
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            table.AddCell(new Phrase(dt.Rows[i][j].ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK)));
                        }
                    }
                }

                dp.Add(table);
                dp.Close();
                MessageBox.Show("PDF generated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                Document dp = new Document();
                PdfWriter.GetInstance(dp, new FileStream(@"D:\final mid\LAB2\reports\InactivestudentDetails.pdf", FileMode.Create));
                dp.Open();
                var p = new Paragraph("Inactive students \n");
                var p1 = new Paragraph("\n");

                dp.Add(p);
                dp.Add(p1);

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"SELECT 
    p.FirstName,
    p.LastName,
    s.RegistrationNo
FROM 
    [dbo].[GroupStudent] gs
INNER JOIN 
    [dbo].[Student] s ON gs.StudentId = s.Id
INNER JOIN 
    [dbo].[Person] p ON s.Id = p.Id
WHERE 
    gs.Status = 4;

", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Create a PDF table
                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                table.DefaultCell.Padding = 6;
                table.DefaultCell.PaddingTop = 10;
                table.WidthPercentage = 100;

                // Add column headers to the PDF table
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dt.Columns[j].ColumnName, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
                }

                // Add data rows to the PDF table
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            table.AddCell(new Phrase(dt.Rows[i][j].ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK)));
                        }
                    }
                }

                dp.Add(table);
                dp.Close();
                MessageBox.Show("PDF generated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            try
            {
                Document dp = new Document();
                PdfWriter.GetInstance(dp, new FileStream(@"D:\final mid\LAB2\reports\ALLDetails.pdf", FileMode.Create));
                dp.Open();
                var p = new Paragraph("Information \n");
                var p1 = new Paragraph("\n");

                dp.Add(p);
                dp.Add(p1);

                var con = Configuration.getInstance().getConnection();

                // Query for student details
                SqlCommand studentCmd = new SqlCommand(@"
        SELECT 
            CONCAT_WS(' ', p.FirstName, p.LastName) AS Name,
            s.RegistrationNo
        FROM 
            [dbo].[Student] s
        INNER JOIN 
            [dbo].[Person] p ON s.Id = p.Id
        WHERE
            p.FirstName NOT LIKE '!%'
    ", con);
                SqlDataAdapter studentDa = new SqlDataAdapter(studentCmd);
                DataTable studentDt = new DataTable();
                studentDa.Fill(studentDt);

                // Create a PDF table for student details
                PdfPTable studentTable = new PdfPTable(2);
                studentTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                studentTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                studentTable.DefaultCell.Padding = 6;
                studentTable.DefaultCell.PaddingTop = 10;
                studentTable.WidthPercentage = 100;

                // Add column headers to the PDF table
                studentTable.AddCell(new Phrase("Name", FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
                studentTable.AddCell(new Phrase("Registration No", FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));

                // Add data rows to the PDF table
                int studentCount = 0;
                foreach (DataRow row in studentDt.Rows)
                {
                    studentTable.AddCell(new Phrase(row["Name"].ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK)));
                    studentTable.AddCell(new Phrase(row["RegistrationNo"].ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK)));
                    studentCount++;
                }

                // Add student table to the document
                dp.Add(studentTable);
                dp.Add(new Paragraph("Total Students: " + studentCount.ToString()));
                dp.Add(new Paragraph("\n"));

                // Query for advisor details
                SqlCommand advisorCmd = new SqlCommand(@"
        SELECT 
            CONCAT_WS(' ', p.FirstName, p.LastName) AS Name
        FROM 
            [dbo].[Advisor] a
        INNER JOIN 
            [dbo].[Person] p ON a.Id = p.Id
        WHERE
            p.FirstName NOT LIKE '!%'
    ", con);
                SqlDataAdapter advisorDa = new SqlDataAdapter(advisorCmd);
                DataTable advisorDt = new DataTable();
                advisorDa.Fill(advisorDt);

                // Create a PDF table for advisor details
                PdfPTable advisorTable = new PdfPTable(1);
                advisorTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                advisorTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                advisorTable.DefaultCell.Padding = 6;
                advisorTable.DefaultCell.PaddingTop = 10;
                advisorTable.WidthPercentage = 100;

                // Add column header to the PDF table
                advisorTable.AddCell(new Phrase("Advisor Name", FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));

                // Add data rows to the PDF table
                int advisorCount = 0;
                foreach (DataRow row in advisorDt.Rows)
                {
                    advisorTable.AddCell(new Phrase(row["Name"].ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK)));
                    advisorCount++;
                }

                // Add advisor table to the document
                dp.Add(advisorTable);
                dp.Add(new Paragraph("Total Advisors: " + advisorCount.ToString()));

                dp.Close();
                MessageBox.Show("PDF generated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
   }

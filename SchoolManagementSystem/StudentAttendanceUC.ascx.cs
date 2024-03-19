using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem
{
    public partial class StudentAttendanceUC : System.Web.UI.UserControl
    {
        Commonfnx fn = new Commonfnx(); // Inisialisasi objek Commonfnx untuk digunakan dalam kelas
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass();
                
            }
        }

        private void GetClass()
        {
            DataTable dt = fn.Fetch("Select * from Class");
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, "Select Class");
        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string classId = ddlClass.SelectedValue;
            DataTable dt = fn.Fetch("SELECT * FROM Subject WHERE ClassId = '" + classId + "' ");
            ddlSubject.DataSource = dt;
            ddlSubject.DataTextField = "SubjectName";
            ddlSubject.DataValueField = "SubjectId";
            ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, "Select Subject");
        }

        protected void btnCheckAttendance_Click(object sender, EventArgs e)
        {
            DataTable dt;
            DateTime date = Convert.ToDateTime(txtMonth.Text); // Mengambil tanggal dari inputan teks dan mengonversinya menjadi tipe data DateTime

            if (ddlSubject.SelectedValue == "Select Subject")
            {
                // Mengambil data kehadiran guru dari database berdasarkan bulan dan tahun tertentu, serta ID guru yang dipilih
                dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No], s.Name, sa.Status, sa.Date FROM StudentAttedance sa
                      INNER JOIN Student s ON s.RollNo = sa.RollNo  
                      WHERE sa.ClassId = '" + ddlClass.SelectedValue + "' AND sa.RollNo = '" + txtRollNo.Text.Trim() + "' AND YEAR(sa.Date) = '" + date.Year + "' AND MONTH(sa.Date) = '" + date.Month + "' AND sa.Status = 1");
            }
            else
            {
                dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No], s.Name, sa.Status, sa.Date FROM StudentAttedance sa
                      INNER JOIN Student s ON s.RollNo = sa.RollNo  
                      WHERE sa.ClassId = '" + ddlClass.SelectedValue + "' AND sa.RollNo = '" + txtRollNo.Text.Trim() + "' AND sa.SubjectId = '" + ddlSubject.SelectedValue + "' AND YEAR(sa.Date) = '" + date.Year + "' AND MONTH(sa.Date) = '" + date.Month + "' AND sa.Status = 1");
            }

            GridView1.DataSource = dt; // Mengatur sumber data GridView ke DataTable yang berisi data kehadiran guru
            GridView1.DataBind(); // Mengikat data ke GridView untuk ditampilkan ke pengguna
        }

    }
}
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
    public partial class MarksDetailUserControl : System.Web.UI.UserControl
    {
        Commonfnx fn = new Commonfnx(); // Inisialisasi objek dari kelas Commonfnx yang digunakan untuk akses ke fungsi umum.

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass(); // Memanggil fungsi untuk mengisi dropdown dengan kelas-kelas yang tersedia.
                GetMarks(); // Memanggil fungsi untuk mendapatkan dan menampilkan data nilai.
            }
        }

        private void GetMarks()
        {
            // Mengambil data nilai siswa dari database dan menampilkan dalam GridView.
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY(SELECT 1)) as [No], e.ExamId, e.ClassId, c.ClassName, e.SubjectId, s.SubjectName,
                                    e.RollNo, e.TotalMarks, e.OutOfMarks from Exam e INNER JOIN Class c ON c.ClassId = e.ClassId INNER JOIN Subject s ON
                                    s.SubjectId = e.SubjectId");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private void GetClass()
        {
            // Mengambil daftar kelas dari database dan memasukkannya ke dalam dropdown list.
            DataTable dt = fn.Fetch("Select * from Class");
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, "Select Class");
        }

        protected void bntAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string classId = ddlClass.SelectedValue; // Mengambil nilai kelas dari dropdown.
                string rollNo = txtRoll.Text.Trim(); // Mengambil nomor roll siswa dari input teks dan menghapus spasi ekstra.

                // Mengambil data nilai berdasarkan kelas dan nomor roll siswa yang diberikan.
                DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY(SELECT 1)) as [No], e.ExamId, e.ClassId, c.ClassName, e.SubjectId, s.SubjectName,
                                    e.RollNo, e.TotalMarks, e.OutOfMarks from Exam e INNER JOIN Class c ON c.ClassId = e.ClassId INNER JOIN Subject s ON
                                    s.SubjectId = e.SubjectId WHERE e.ClassId = '" + classId + "' and e.RollNo = '" + rollNo + "'");
                GridView1.DataSource = dt; // Mengatur sumber data GridView dengan data yang diperoleh.
                GridView1.DataBind(); // Mengikat data ke GridView.
            }
            catch (Exception ex)
            {
                throw; // Melemparkan kembali pengecualian yang terjadi.
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex; // Mengatur nomor halaman saat ini ketika perubahan halaman GridView terjadi.
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class EmpAttendanceDetails : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx(); // Inisialisasi objek Commonfnx untuk digunakan dalam kelas
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            if (!IsPostBack)
            {
                GetTeacher(); // Memanggil fungsi GetTeacher() saat halaman tidak di-post kembali (refresh)
            }
        }

        // Fungsi untuk mengambil daftar guru dan memasukkannya ke dalam dropdownlist
        private void GetTeacher()
        {
            DataTable dt = fn.Fetch("Select * from Users WHERE RoleId = 2"); // Mengambil data guru dari database menggunakan fungsi Fetch() dalam objek Commonfnx
            ddlTeacher.DataSource = dt; // Mengatur sumber data dropdownlist ke DataTable yang berisi daftar guru
            ddlTeacher.DataTextField = "Name"; // Menentukan kolom dalam DataTable yang akan digunakan sebagai teks untuk item dropdown
            ddlTeacher.DataValueField = "UserId"; // Menentukan kolom dalam DataTable yang akan digunakan sebagai nilai untuk item dropdown
            ddlTeacher.DataBind(); // Mengikat data ke dropdownlist
            ddlTeacher.Items.Insert(0, "Select Teacher"); // Menambahkan item "Select Teacher" di posisi pertama dropdownlist
        }

        // Event handler yang dipanggil ketika tombol btnCheckAttendance diklik
        protected void btnCheckAttendance_Click(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtMonth.Text); // Mengambil tanggal dari inputan teks dan mengonversinya menjadi tipe data DateTime

            // Mengambil data kehadiran guru dari database berdasarkan bulan dan tahun tertentu, serta ID guru yang dipilih
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No],u.Name,ta.Status,ta.Date FROM TeacherAttedance ta
                              INNER JOIN Users u ON u.UserId = ta.UserId  
                              where DATEPART(yy,Date) ='" + date.Year + "' and DATEPART(M,Date) = '" + date.Month + "' and ta.UserId = '" + ddlTeacher.SelectedValue + "'");
            GridView1.DataSource = dt; // Mengatur sumber data GridView ke DataTable yang berisi data kehadiran guru
            GridView1.DataBind(); // Mengikat data ke GridView untuk ditampilkan ke pengguna
        }

    }
}

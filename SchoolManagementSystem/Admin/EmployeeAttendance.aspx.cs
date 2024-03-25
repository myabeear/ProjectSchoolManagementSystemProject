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
    public partial class EmployeeAttendance : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx(); // Membuat instance dari kelas Commonfnx untuk digunakan di seluruh kelas.

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            if (!IsPostBack)
            {
                Attendance(); // Memanggil fungsi Attendance() saat halaman dimuat pertama kali.
            }
        }

        private void Attendance()
        {
            // Mengambil data dari database untuk tabel Teacher.
            DataTable dt = fn.Fetch("Select UserId,Name,Mobile,Email FROM Users WHERE RoleId = 2");

            // Mengatur sumber data dari GridView1 dan mengikat data.
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // Event handler ketika tombol mark attendance diklik.
        protected void btnMarkAttendance_click(object sender, EventArgs e)
        {
            // Melakukan iterasi pada setiap baris GridView1.
            foreach (GridViewRow row in GridView1.Rows)
            {
                // Mengambil ID guru dari sel pada kolom ke-1.
                int userId = Convert.ToInt32(row.Cells[1].Text);

                // Mendapatkan status kehadiran yang dipilih oleh pengguna.
                RadioButton rb1 = (row.Cells[0].FindControl("RadioButton1") as RadioButton);
                RadioButton rb2 = (row.Cells[0].FindControl("RadioButton2") as RadioButton);
                int status = 0;
                if (rb1.Checked)
                {
                    status = 1; // Jika radio button pertama terpilih, maka status dianggap hadir (1).
                }
                else if (rb2.Checked)
                {
                    status = 0; // Jika radio button kedua terpilih, maka status dianggap tidak hadir (0).
                }

                // Memasukkan data kehadiran guru ke dalam database.
                fn.Query(@"Insert into TeacherAttendance values ('" + userId + "', '" + status + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "')");

                // Menampilkan pesan bahwa data telah dimasukkan dengan sukses.
                lblMsg.Text = "Inserted Successfully";
                lblMsg.CssClass = "alert alert-success";
            }
        }

        // Event handler untuk timer, untuk menampilkan waktu saat ini pada label.
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString(); // Menampilkan waktu saat ini pada label lblTime.
        }
    }
}

// Mengimpor namespace yang diperlukan
using SchoolManagementSystem.Models;
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
    // Kelas untuk menambahkan kelas baru ke sistem
    public partial class AddClass : System.Web.UI.Page
    {
        // Objek fungsi umum untuk pengoperasian database
        Commonfnx fn = new Commonfnx();

        // Metode yang dipanggil saat halaman dimuat
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            // Memeriksa apakah halaman dimuat kembali (postback) atau tidak
            if (!IsPostBack)
            {
                // Memanggil metode untuk mendapatkan daftar kelas
                GetClass();
            }
        }

        // Metode untuk mendapatkan daftar kelas dari database
        private void GetClass()
        {
            // Mengambil data kelas dari database menggunakan objek fungsi umum
            DataTable dt = fn.Fetch("Select Row_NUMBER() over(Order by (Select 1)) as [No], ClassId, ClassName from Class");
            // Mengatur sumber data GridView dengan hasil query
            GridView1.DataSource = dt;
            // Mengikat data ke GridView
            GridView1.DataBind();
        }

        // Metode yang dipanggil saat tombol "Tambah" diklik
        protected void bntAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Mengambil data kelas dengan nama yang sama dari database
                DataTable dt = fn.Fetch("Select * from Class where ClassName = '" + txtClass.Text.Trim() + "'");
                // Memeriksa apakah kelas sudah ada dalam database
                if (dt.Rows.Count == 0)
                {
                    // Jika kelas belum ada, menambahkannya ke database
                    string query = "Insert into Class values('" + txtClass.Text.Trim() + "')";
                    fn.Query(query);
                    // Menampilkan pesan berhasil
                    lblMsg.Text = "Inserted Successfully";
                    lblMsg.CssClass = "alert alert-success";
                    // Mengosongkan input kelas
                    txtClass.Text = string.Empty;
                    // Memperbarui daftar kelas yang ditampilkan di GridView
                    GetClass();
                }
                else
                {
                    // Jika kelas sudah ada, menampilkan pesan kesalahan
                    lblMsg.Text = "Entered Class already exists!";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                // Menampilkan pesan kesalahan jika terjadi exception
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat halaman GridView berubah
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Mengatur indeks halaman GridView
            GridView1.PageIndex = e.NewPageIndex;
            // Mendapatkan daftar kelas dengan indeks halaman yang baru
            GetClass();
        }

        // Metode yang dipanggil saat pengeditan baris GridView dibatalkan
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Mengatur indeks edit GridView ke -1 untuk membatalkan pengeditan
            GridView1.EditIndex = -1;
            // Memperbarui daftar kelas yang ditampilkan di GridView
            GetClass();
        }

        // Metode yang dipanggil saat baris GridView diedit
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Mengatur indeks edit GridView ke indeks baris yang dipilih untuk memulai pengeditan
            GridView1.EditIndex = e.NewEditIndex;
        }

        // Metode yang dipanggil saat baris GridView diperbarui
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Mendapatkan baris GridView yang diperbarui
                GridViewRow row = GridView1.Rows[e.RowIndex];
                // Mendapatkan ID kelas dari data kunci GridView
                int cId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Mendapatkan nilai nama kelas yang diperbarui
                String ClassName = (row.FindControl("txtClassEdit") as TextBox).Text;
                // Memperbarui nama kelas di database
                fn.Query("Update Class set ClassName = '" + ClassName + "' where ClassId ='" + cId + "' ");
                // Menampilkan pesan berhasil
                lblMsg.Text = "Class Updated Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Mengatur indeks edit GridView ke -1 setelah pembaruan
                GridView1.EditIndex = -1;
                // Memperbarui daftar kelas yang ditampilkan di GridView
                GetClass();
            }
            catch (Exception ex)
            {
                // Menampilkan pesan kesalahan jika terjadi exception
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Mendapatkan id mata pelajaran guru yang akan dihapus
                int cId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Menghapus data dari database
                fn.Query("Delete from Class where ClassId = '" + cId + "'");
                lblMsg.Text = " Class Deleted Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Keluar dari mode edit dan memperbarui data
                GridView1.EditIndex = -1;
                GetClass();
            }
            catch (Exception ex)
            {
                // Menangani kesalahan yang mungkin terjadi dan menampilkan pesan kesalahan
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }
    }
}

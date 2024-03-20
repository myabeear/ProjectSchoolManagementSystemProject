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
    public partial class Users : System.Web.UI.Page
    {
        // Membuat instance Commonfnx untuk mengakses fungsi umum
        Commonfnx fn = new Commonfnx();

        // Fungsi yang terpanggil saat halaman dimuat
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            // Cek apakah halaman dimuat pertama kali atau tidak
            if (!IsPostBack)
            {
                // Jika pertama kali dimuat, panggil fungsi GetTeacher untuk menampilkan data guru
                GetRole();
                GetUser();
            }
        }
        private void GetRole()
        {
            DataTable dt = fn.Fetch("Select * from UserRole");
            ddlRole.DataSource = dt;
            ddlRole.DataTextField = "Role";
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, "Select Role");
        }

        // Fungsi untuk mendapatkan nama peran berdasarkan ID peran
        protected string GetRoleName(object roleId)
        {
            // Pastikan roleId bukan null dan konversi ke tipe data yang sesuai
            if (roleId != null)
            {
                int roleIdValue = Convert.ToInt32(roleId);
                // Query untuk mendapatkan nama peran berdasarkan ID peran
                string query = "SELECT Role FROM UserRole WHERE RoleId = " + roleIdValue;

                // Ambil data menggunakan fungsi Fetch dari Commonfnx
                DataTable dt = fn.Fetch(query);

                // Pastikan data ditemukan
                if (dt != null && dt.Rows.Count > 0)
                {
                    // Ambil nilai dari kolom "Role" dari baris pertama
                    return dt.Rows[0]["Role"].ToString();
                }
            }
            return string.Empty;
        }



        // Fungsi untuk mendapatkan data guru dari database
        private void GetUser()
        {
            // Ambil data User dari database menggunakan fungsi Fetch dari Commonfnx
            DataTable dt = fn.Fetch(@"Select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as [No], IdUser, [Name], Email, Mobile, DOB, Gender, [Address], RoleId,[Password] from Users");
            // Atur sumber data GridView1 menjadi DataTable dt
            GridView1.DataSource = dt;
            // Bind data ke GridView1
            GridView1.DataBind();
        }

        // Event handler untuk tombol Tambah Guru
        protected void bntAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Cek apakah jenis kelamin dipilih atau tidak
                if (ddlGender.SelectedValue != "0")
                {
                    // Ambil alamat email dari input teks
                    string email = txtEmail.Text.Trim();
                    // Cari guru dengan alamat email yang sama di database
                    DataTable dt = fn.Fetch("Select * from Users where Email = '" + email + "'");
                    // Jika tidak ada guru dengan alamat email yang sama
                    if (dt.Rows.Count == 0)
                    {
                        // Buat query untuk menambahkan guru baru ke database
                        string query = "Insert into Users ([Name], Email, Mobile, DOB, Gender, [Address], RoleId, [Password]) values " +
                            "('" + txtName.Text.Trim() + "', '" + txtEmail.Text.Trim() + "', '" + txtMobile.Text.Trim() + "', '" + txtDoB.Text.Trim() + "'," +
                            " '" + ddlGender.SelectedValue + "', '" + txtAddress.Text.Trim() + "', '" + ddlRole.SelectedValue + "','" + txtPassword.Text.Trim() + "' ) ";

                        // Eksekusi query menggunakan fungsi Query dari Commonfnx
                        fn.Query(query);
                        // Tampilkan pesan berhasil
                        lblMsg.Text = "Inserted Successfully";
                        lblMsg.CssClass = "alert alert-success";
                        // Reset nilai beberapa input teks dan dropdown
                        ddlGender.SelectedIndex = 0;
                        txtName.Text = string.Empty;
                        txtEmail.Text = string.Empty;
                        txtMobile.Text = string.Empty;
                        txtDoB.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        ddlRole.SelectedIndex = 0;
                        txtPassword.Text = string.Empty;
                        // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
                        GetUser();
                    }
                    else
                    {
                        // Jika alamat email sudah ada di database, tampilkan pesan kesalahan
                        lblMsg.Text = "Entered <b> '" + email + "'</b>already exists!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    // Jika jenis kelamin tidak dipilih, tampilkan pesan kesalahan
                    lblMsg.Text = "Gender is required";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                // Tangani kesalahan dengan menampilkan pesan kesalahan menggunakan JavaScript
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Event handler untuk perubahan halaman di GridView1
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Atur indeks halaman GridView1
            GridView1.PageIndex = e.NewPageIndex;
            // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
            GetUser();
        }

        // Event handler untuk pembatalan edit baris di GridView1
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Atur indeks edit GridView1 menjadi -1
            GridView1.EditIndex = -1;
            // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
            GetUser();
        }

        // Event handler untuk penghapusan baris di GridView1
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Ambil ID guru dari data yang akan dihapus
                int idUser = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Hapus guru dari database menggunakan ID guru
                fn.Query("Delete from Users where IdUser = '" + idUser + "'");
                // Tampilkan pesan berhasil
                lblMsg.Text = "Users Deleted Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Atur indeks edit GridView1 menjadi -1
                GridView1.EditIndex = -1;
                // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
                GetUser();
            }
            catch (Exception ex)
            {
                // Tangani kesalahan dengan menampilkan pesan kesalahan menggunakan JavaScript
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Event handler untuk memulai edit baris di GridView1
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Atur indeks edit GridView1 sesuai dengan baris yang dipilih
            GridView1.EditIndex = e.NewEditIndex;
            // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
            GetUser();
        }

        // Event handler untuk pembaruan baris di GridView1
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Ambil baris yang akan diperbarui dari GridView1
                GridViewRow row = GridView1.Rows[e.RowIndex];
                // Ambil ID guru dari data yang akan diperbarui
                int idUser = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Ambil nilai nama, nomor telepon, kata sandi, dan alamat baru dari input teks
                string name = (row.FindControl("txtName") as TextBox).Text;
                string mobile = (row.FindControl("txtMobile") as TextBox).Text;
                string password = (row.FindControl("txtPassword") as TextBox).Text;
                string address = (row.FindControl("txtAddress") as TextBox).Text;
                string roleId = (row.FindControl("txtRoleId") as TextBox).Text;
                // Perbarui data guru di database menggunakan nilai baru
                fn.Query("Update Users set Name ='" + name.Trim() + "', Mobile='" + mobile.Trim() + "', Address='" + address.Trim() + "', RoleId='" + roleId.Trim() + "', Password='" + password.Trim() + "' where IdUser = '" + idUser + "' ");
                // Tampilkan pesan berhasil
                lblMsg.Text = "User Updated Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Atur indeks edit GridView1 menjadi -1
                GridView1.EditIndex = -1;
                // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
                GetUser();
            }
            catch (Exception ex)
            {
                // Tangani kesalahan dengan menampilkan pesan kesalahan menggunakan JavaScript
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }
    }
}
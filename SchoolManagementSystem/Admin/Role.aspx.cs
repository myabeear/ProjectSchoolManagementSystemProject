using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class Role : System.Web.UI.Page
    {
        // Membuat instance Commonfnx untuk mengakses fungsi umum
        Commonfnx fn = new Commonfnx();
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
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string role = txtRole.Text.Trim();
                // Cari role dengan nama yang sama di database
                DataTable dt = fn.Fetch("Select * from UserRole where Role = '" + role + "'");

                if (dt.Rows.Count == 0)
                {
                    // Buat query untuk menambahkan role baru ke database
                    string query = "Insert into UserRole ([Role]) values ('" + role + "' ) ";
                    // Eksekusi query menggunakan fungsi Query dari Commonfnx
                    fn.Query(query);
                    // Tampilkan pesan berhasil
                    lblMsg.Text = "Inserted Successfully";
                    lblMsg.CssClass = "alert alert-success";

                    txtRole.Text = string.Empty;
                    // Panggil kembali fungsi GetRole untuk memperbarui tampilan
                    GetRole();
                }
                else
                {
                    // Jika role sudah ada di database, tampilkan pesan kesalahan
                    lblMsg.Text = "Entered <b> '" + role + "'</b> already exists!";
                    lblMsg.CssClass = "alert alert-danger";
                }

            }
            catch (Exception ex)
            {
                // Tangani kesalahan dengan menampilkan pesan kesalahan menggunakan JavaScript
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }


        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Atur indeks edit GridView1 menjadi -1
            GridView1.EditIndex = -1;
            // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
            GetRole();
        }

        private void GetRole()
        {
            // Ambil data User dari database menggunakan fungsi Fetch dari Commonfnx
            DataTable dt = fn.Fetch(@"Select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as [No], RoleId, [Role] from UserRole");
            // Atur sumber data GridView1 menjadi DataTable dt
            GridView1.DataSource = dt;
            // Bind data ke GridView1
            GridView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Atur indeks edit GridView1 sesuai dengan baris yang dipilih
            GridView1.EditIndex = e.NewEditIndex;
            // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
            GetRole();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Atur indeks halaman GridView1
            GridView1.PageIndex = e.NewPageIndex;
            // Panggil kembali fungsi GetTeacher untuk memperbarui tampilan
            GetRole();
        }
    }
}
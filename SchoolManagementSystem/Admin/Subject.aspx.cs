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
    public partial class Subject : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        // Metode yang dipanggil saat halaman dimuat
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            // Cek apakah halaman dimuat untuk pertama kali atau tidak
            if (!IsPostBack)
            {
                // Jika halaman dimuat untuk pertama kali, ambil daftar kelas dan mata pelajaran
                GetClass();
                GetSubject();
            }
        }

        // Metode untuk mendapatkan daftar kelas
        private void GetClass()
        {
            DataTable dt = fn.Fetch("Select * from Class");
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, "Select Class");
        }

        // Event handler untuk tombol "Add"
        protected void bntAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string classVal = ddlClass.SelectedItem.Text;
                DataTable dt = fn.Fetch("Select * from Subject where ClassId = '" + ddlClass.SelectedItem.Value + "' and SubjectName='" + txtSubject.Text.Trim() + "'");
                if (dt.Rows.Count == 0)
                {
                    // Jika mata pelajaran belum ada, masukkan ke database
                    string query = "Insert into Subject values('" + ddlClass.SelectedItem.Value + "','" + txtSubject.Text.Trim() + "')";
                    fn.Query(query);
                    lblMsg.Text = "Inserted Successfully";
                    lblMsg.CssClass = "alert alert-success";
                    ddlClass.SelectedIndex = 0;
                    txtSubject.Text = string.Empty;
                    GetSubject();
                }
                else
                {
                    // Jika mata pelajaran sudah ada, tampilkan pesan kesalahan
                    lblMsg.Text = "Entered Subject already exists for <b> '" + classVal + "'</b>!";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                // Tangani pengecualian jika ada
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode untuk mendapatkan daftar mata pelajaran
        private void GetSubject()
        {
            DataTable dt = fn.Fetch(@"Select Row_NUMBER() over(Order by (Select 1)) as [No], s.SubjectId, s.ClassId, c.ClassName, 
                                        s.SubjectName from Subject s inner join Class c on c.ClassId = s.ClassId");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // Event handler untuk pergantian halaman pada GridView
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetSubject();
        }

        // Event handler untuk membatalkan mode edit pada GridView
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetSubject();
        }

        // Event handler untuk memulai mode edit pada GridView
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetSubject();
        }

        // Event handler untuk memperbarui data saat editing pada GridView
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int subjectId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string classId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("DropDownList1")).SelectedValue;
                string subjectName = (row.FindControl("TextBox1") as TextBox).Text;
                fn.Query("Update Subject set ClassId ='" + classId + "', SubjectName='" + subjectName + "' where SubjectId = '" + subjectId + "' ");
                lblMsg.Text = "Subject Updated Successfully";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetSubject();
            }
            catch (Exception ex)
            {
                // Tangani pengecualian jika ada
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Mendapatkan id mata pelajaran guru yang akan dihapus
                int subjectId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Menghapus data dari database
                fn.Query("Delete from Subject where SubjectId = '" + subjectId + "'");
                lblMsg.Text = " Subject Deleted Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Keluar dari mode edit dan memperbarui data
                GridView1.EditIndex = -1;
                GetSubject();
            }
            catch (Exception ex)
            {
                // Menangani kesalahan yang mungkin terjadi dan menampilkan pesan kesalahan
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }
    }
}

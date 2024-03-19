// Namespace untuk menyimpan kelas-kelas terkait administrasi sistem manajemen sekolah
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
    // Kelas untuk halaman Expense yang merupakan bagian dari antarmuka pengguna web
    public partial class Expense : System.Web.UI.Page
    {
        // Objek untuk mengakses fungsi-fungsi umum
        Commonfnx fn = new Commonfnx();

        // Metode yang dipanggil saat halaman dimuat
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            // Cek apakah halaman dimuat untuk pertama kali
            if (!IsPostBack)
            {
                // Memuat daftar kelas dan biaya
                GetClass();
                GetExpense();
            }
        }

        // Metode untuk mendapatkan daftar kelas dari database
        private void GetClass()
        {
            // Mengambil data kelas dari database
            DataTable dt = fn.Fetch("Select * from Class");
            // Menetapkan sumber data dan nilai-nilai yang akan ditampilkan pada DropDownList untuk kelas
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            // Menambahkan item pertama sebagai pilihan default
            ddlClass.Items.Insert(0, "Select Class");
        }

        // Metode yang dipanggil saat pilihan kelas berubah pada DropDownList
        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Mendapatkan ID kelas yang dipilih
            string classId = ddlClass.SelectedValue;
            // Mengambil daftar mata pelajaran sesuai dengan kelas yang dipilih dari database
            DataTable dt = fn.Fetch("SELECT * FROM Subject WHERE ClassId = '" + classId + "' ");
            // Menetapkan sumber data dan nilai-nilai yang akan ditampilkan pada DropDownList untuk mata pelajaran
            ddlSubject.DataSource = dt;
            ddlSubject.DataTextField = "SubjectName";
            ddlSubject.DataValueField = "SubjectId";
            ddlSubject.DataBind();
            // Menambahkan item pertama sebagai pilihan default
            ddlSubject.Items.Insert(0, "Select Subject");
        }

        // Metode untuk mendapatkan daftar biaya dari database
        private void GetExpense()
        {
            // Mengambil data biaya dari database dengan penomoran baris
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No], e.ExpenseId, e.ClassId, c.ClassName, e.SubjectId, 
                                        s.SubjectName, e.ChargeAmount from Expense e 
                              INNER JOIN Class c ON e.ClassId = c.ClassId 
                              INNER JOIN Subject s ON e.SubjectId = s.SubjectId");
            // Menetapkan sumber data untuk GridView
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // Metode yang dipanggil saat tombol "Add" ditekan
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Mendapatkan data yang dimasukkan oleh pengguna
                string classId = ddlClass.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string chargeAmt = txtExpenseAmt.Text;
                // Memeriksa apakah data biaya sudah ada dalam database
                DataTable dt = fn.Fetch("Select * from Expense where ClassId = '" + classId + "'and SubjectId='" + subjectId + "' or ChargeAmount='" + chargeAmt + "' ");
                // Jika data tidak ada, data biaya akan dimasukkan ke database
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into Expense values('" + classId + "','" + subjectId + "','" + chargeAmt + "')";
                    fn.Query(query);
                    lblMsg.Text = "Inserted Successfully";
                    lblMsg.CssClass = "alert alert-success";
                    ddlClass.SelectedIndex = 0;
                    ddlSubject.SelectedIndex = 0;
                    txtExpenseAmt.Text = string.Empty;
                    GetExpense();
                }
                else
                {
                    lblMsg.Text = "Entered <b>Data</b> already exists";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                // Menampilkan pesan kesalahan jika terjadi kesalahan saat memproses permintaan
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat halaman GridView berubah
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Mengatur indeks halaman untuk GridView
            GridView1.PageIndex = e.NewPageIndex;
            // Memuat kembali daftar biaya dengan indeks halaman yang baru
            GetExpense();
        }

        // Metode yang dipanggil saat pengeditan baris di GridView dibatalkan
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Mengakhiri mode pengeditan di GridView
            GridView1.EditIndex = -1;
            // Memuat kembali daftar biaya setelah pengeditan dibatalkan
            GetExpense();
        }

        // Metode yang dipanggil saat baris GridView dihapus
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Mendapatkan ID biaya yang akan dihapus
                int expenseId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Menghapus biaya dari database
                fn.Query("Delete from Expense where ExpenseId = '" + expenseId + "'");
                lblMsg.Text = "Expense Deleted Successfully";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                // Memuat kembali daftar biaya setelah penghapusan
                GetExpense();
            }
            catch (Exception ex)
            {
                // Menampilkan pesan kesalahan jika terjadi kesalahan saat menghapus biaya
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat baris GridView sedang diedit
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Mengatur indeks baris yang akan diedit di GridView
            GridView1.EditIndex = e.NewEditIndex;
            // Memuat kembali daftar biaya setelah mode pengeditan diaktifkan
            GetExpense();
        }

        // Metode yang dipanggil saat baris GridView diperbarui
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                // Mendapatkan baris yang sedang diperbarui di GridView
                GridViewRow row = GridView1.Rows[e.RowIndex];
                // Mendapatkan ID biaya yang akan diperbarui
                int expenseId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].

Values[0]);
                // Mendapatkan data baru yang dimasukkan oleh pengguna
                string classId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlClassGv")).SelectedValue;
                string subjectId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string chargeAmt = (row.FindControl("txtExpenseAmt") as TextBox).Text.Trim();
                // Memperbarui data biaya di database
                fn.Query("Update Expense set ClassId ='" + classId + "', SubjectId='" + subjectId + "', ChargeAmount ='" + chargeAmt + "' where ExpenseId = '" + expenseId + "' ");
                lblMsg.Text = "Record Updated Successfully";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                // Memuat kembali daftar biaya setelah pembaruan
                GetExpense();
            }
            catch (Exception ex)
            {
                // Menampilkan pesan kesalahan jika terjadi kesalahan saat memperbarui data biaya
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat pilihan kelas berubah pada DropDownList di dalam GridView
        protected void ddlClassGv_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlClassSelected = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlClassSelected.NamingContainer;
            if (row != null)
            {
                if ((row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlSubjectGV = (DropDownList)row.FindControl("ddlSubjectGv");
                    DataTable dt = fn.Fetch("Select * FROM Subject where ClassId = '" + ddlClassSelected.SelectedValue + "'");
                    ddlSubjectGV.DataSource = dt;
                    ddlSubjectGV.DataTextField = "SubjectName";
                    ddlSubjectGV.DataValueField = "SubjectId";
                    ddlSubjectGV.DataBind();
                }
            }
        }

        // Metode yang dipanggil saat data diikat ke baris GridView
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    // Mendapatkan DropDownList untuk kelas dan mata pelajaran di dalam mode edit
                    DropDownList ddlClass = (DropDownList)e.Row.FindControl("ddlClassGv");
                    DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlSubjectGv");
                    // Mendapatkan daftar mata pelajaran sesuai dengan kelas yang dipilih
                    DataTable dt = fn.Fetch("Select * FROM Subject where ClassId = '" + ddlClass.SelectedValue + "'");
                    // Menetapkan sumber data dan nilai-nilai yang akan ditampilkan pada DropDownList untuk mata pelajaran
                    ddlSubject.DataSource = dt;
                    ddlSubject.DataTextField = "SubjectName";
                    ddlSubject.DataValueField = "SubjectId";
                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, "Select Subject");
                    // Mengatur mata pelajaran yang dipilih sebelumnya pada DropDownList
                    string selectedSubject = DataBinder.Eval(e.Row.DataItem, "SubjectName").ToString();
                    ddlSubject.Items.FindByText(selectedSubject).Selected = true;

                }
            }
        }
    }
}

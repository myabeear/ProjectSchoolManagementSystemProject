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
    public partial class TeacherSubject : System.Web.UI.Page
    {
        // Deklarasi objek fungsi umum
        Commonfnx fn = new Commonfnx();

        // Metode yang dipanggil saat halaman dimuat
        protected void Page_Load(object sender, EventArgs e)
        {
            // Memastikan tidak pos back
            if (!IsPostBack)
            {
                // Memanggil metode untuk mendapatkan data kelas, guru, dan mata pelajaran guru
                GetClass();
                GetTeacher();
                GetTeacherSubject();
            }
        }

        // Metode untuk mendapatkan daftar kelas
        private void GetClass()
        {
            // Mengambil data kelas dari database
            DataTable dt = fn.Fetch("Select * from Class");
            // Mengatur sumber data dan properti tampilan untuk dropdownlist kelas
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, "Select Class");
        }

        // Metode untuk mendapatkan daftar guru
        private void GetTeacher()
        {
            // Mengambil data guru dari database
            DataTable dt = fn.Fetch("Select * from Teacher");
            // Mengatur sumber data dan properti tampilan untuk dropdownlist guru
            ddlTeacher.DataSource = dt;
            ddlTeacher.DataTextField = "Name";
            ddlTeacher.DataValueField = "TeacherId";
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, "Select Teacher");
        }

        // Metode untuk mendapatkan daftar mata pelajaran guru
        private void GetTeacherSubject()
        {
            // Mengambil data mata pelajaran guru dari database
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No], ts.Id, ts.ClassId, c.ClassName, ts.SubjectId, s.SubjectName,
                                    ts.TeacherId, t.Name 
                              FROM TeacherSubject ts 
                              INNER JOIN Class c ON ts.ClassId = c.ClassId 
                              INNER JOIN Subject s ON ts.SubjectId = s.SubjectId 
                              INNER JOIN Teacher t ON ts.TeacherId = t.TeacherId");
            // Mengatur sumber data dan mengikat data ke GridView
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // Metode yang dipanggil saat pilihan kelas berubah
        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Mendapatkan id kelas yang dipilih
            string classId = ddlClass.SelectedValue;
            // Mendapatkan daftar mata pelajaran yang sesuai dengan kelas yang dipilih
            DataTable dt = fn.Fetch("SELECT * FROM Subject WHERE ClassId = '" + classId + "' ");
            // Mengatur sumber data dan properti tampilan untuk dropdownlist mata pelajaran
            ddlSubject.DataSource = dt;
            ddlSubject.DataTextField = "SubjectName";
            ddlSubject.DataValueField = "SubjectId";
            ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, "Select Subject");
        }

        // Metode yang dipanggil saat tombol tambah diklik
        protected void bntAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Mendapatkan id kelas, id mata pelajaran, dan id guru yang dipilih
                string classId = ddlClass.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string teacherId = ddlTeacher.SelectedValue;
                // Memeriksa apakah kombinasi kelas-mata pelajaran-guru sudah ada
                DataTable dt = fn.Fetch("Select * from TeacherSubject where ClassId = '" + classId + "'and SubjectId='" + subjectId + "' or TeacherId='" + teacherId + "' ");
                if (dt.Rows.Count == 0)
                {
                    // Jika tidak ada, data baru dimasukkan ke dalam database
                    string query = "Insert into TeacherSubject values('" + classId + "','" + subjectId + "','" + teacherId + "')";
                    fn.Query(query);
                    lblMsg.Text = "Inserted Successfully";
                    lblMsg.CssClass = "alert alert-success";
                    // Mengatur ulang nilai dropdownlist
                    ddlClass.SelectedIndex = 0;
                    ddlSubject.SelectedIndex = 0;
                    ddlTeacher.SelectedIndex = 0;
                    GetTeacherSubject();
                }
                else
                {
                    // Jika sudah ada, pesan kesalahan ditampilkan
                    lblMsg.Text = "Entered <b>Teacher Subject</b> already exists";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                // Menangani kesalahan yang mungkin terjadi dan menampilkan pesan kesalahan
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat indeks halaman GridView berubah
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Mengatur indeks halaman GridView dan memperbarui data
            GridView1.PageIndex = e.NewPageIndex;
            GetTeacherSubject();
        }

        // Metode yang dipanggil saat pembatalan pengeditan baris GridView
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Membatalkan mode pengeditan dan memperbarui data
            GridView1.EditIndex = -1;
            GetTeacherSubject();
        }

        // Metode yang dipanggil saat penghapusan baris GridView
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Mendapatkan id mata pelajaran guru yang akan dihapus
                int teacherSubjectId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Menghapus data dari database
                fn.Query("Delete from TeacherSubject where Id = '" + teacherSubjectId + "'");
                lblMsg.Text = "Teacher Subject Deleted Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Keluar dari mode edit dan memperbarui data
                GridView1.EditIndex = -1;
                GetTeacherSubject();
            }
            catch (Exception ex)
            {
                // Menangani kesalahan yang mungkin terjadi dan menampilkan pesan kesalahan
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat memulai pengeditan baris GridView
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Memulai mode pengeditan dan memperbarui data
            GridView1.EditIndex = e.NewEditIndex;
            GetTeacherSubject();
        }

        // Metode yang dipanggil saat baris GridView diperbarui
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Mendapatkan baris GridView yang akan diperbarui
                GridViewRow row = GridView1.Rows[e.RowIndex];
                // Mendapatkan id mata pelajaran guru yang akan diperbarui
                int teacherSubjectId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Mendapatkan id kelas, id mata pelajaran, dan id guru yang dipilih dari dropdownlist
                string classId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlClassGv")).SelectedValue;
                string subjectId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string teacherId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlTeacherGv")).SelectedValue;
                // Memperbarui data di database
                fn.Query("Update TeacherSubject set ClassId ='" + classId + "', SubjectId='" + subjectId + "', TeacherId='" + teacherId + "' where Id = '" + teacherSubjectId + "' ");
                lblMsg.Text = "Teacher Subject Updated Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Keluar dari mode edit dan memperbarui data
                GridView1.EditIndex = -1;
                GetTeacherSubject();
            }
            catch (Exception ex)
            {
                // Menangani kesalahan yang mungkin terjadi dan menampilkan pesan kesalahan
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat pilihan kelas di GridView berubah
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

        // Metode yang dipanggil saat baris GridView diikat ke data
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlClass = (DropDownList)e.Row.FindControl("ddlClassGv");
                    DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlSubjectGv");
                    DataTable dt = fn.Fetch("Select * FROM Subject where ClassId = '" + ddlClass.SelectedValue + "'");
                    ddlSubject.DataSource = dt;
                    ddlSubject.DataTextField = "SubjectName";
                    ddlSubject.DataValueField = "SubjectId";
                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, "Select Subject");
                    string selectedSubject = DataBinder.Eval(e.Row.DataItem, "SubjectName").ToString();
                    ddlSubject.Items.FindByText(selectedSubject).Selected = true;
                }
            }
        }
    }
}

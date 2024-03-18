// Menggunakan direktif untuk mengimpor namespace yang diperlukan
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
    // Kelas Marks yang merupakan halaman web untuk administrasi nilai
    public partial class Marks : System.Web.UI.Page
    {
        // Instance dari kelas Commonfnx untuk digunakan dalam halaman ini
        Commonfnx fn = new Commonfnx();

        // Metode yang dipanggil saat halaman dimuat pertama kali
        protected void Page_Load(object sender, EventArgs e)
        {
            // Memeriksa apakah halaman dimuat karena postback, jika tidak, melakukan pengambilan kelas dan nilai
            if (!IsPostBack)
            {
                GetClass();
                GetMarks();
            }
        }

        // Metode untuk mengambil semua nilai
        private void GetMarks()
        {
            // Mengambil data nilai dari database
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No], e.ExamId, e.ClassId, c.ClassName, e.SubjectId, 
                                        s.SubjectName, e.RollNo, e.TotalMarks, e.OutOfMarks from Exam e 
                              INNER JOIN Class c ON e.ClassId = c.ClassId 
                              INNER JOIN Subject s ON e.SubjectId = s.SubjectId");
            // Menetapkan sumber data dan mengikatnya ke GridView1
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // Metode untuk mengambil semua kelas
        private void GetClass()
        {
            // Mengambil data kelas dari database
            DataTable dt = fn.Fetch("Select * from Class");
            // Menetapkan sumber data untuk ddlClass dan mengikat data teks dan nilai
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            // Menambahkan item "Select Class" di indeks pertama
            ddlClass.Items.Insert(0, "Select Class");
        }

        // Metode yang dipanggil saat tombol Add diklik
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Mengambil nilai dari kontrol-kontrol input
                string classId = ddlClass.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string rollNo = txtRoll.Text;
                string studMarks = txtStudMarks.Text;
                string outOfMarks = txtOutOfMarks.Text;
                // Mengambil data siswa berdasarkan kelas dan nomor roll
                DataTable dttbl = fn.Fetch("Select StudentId from Student where ClassId = '" + classId + "'and RollNo='" + rollNo + "'");

                // Memeriksa apakah data siswa ditemukan
                if (dttbl.Rows.Count > 0)
                {
                    // Mengambil data ujian berdasarkan kelas, mata pelajaran, dan nomor roll
                    DataTable dt = fn.Fetch("Select * from Exam where ClassId = '" + classId + "'and SubjectId='" + subjectId + "' and RollNo='" + rollNo + "' ");
                    // Memeriksa apakah data ujian tidak ditemukan
                    if (dt.Rows.Count == 0)
                    {
                        // Menyusun query untuk menyisipkan data ujian ke database
                        string query = "Insert into Exam values('" + classId + "','" + subjectId + "','" + rollNo + "','" + studMarks + "','" + outOfMarks + "')";
                        fn.Query(query);
                        // Menampilkan pesan berhasil
                        lblMsg.Text = "Inserted Successfully";
                        lblMsg.CssClass = "alert alert-success";
                        // Mengatur kembali indeks pilihan ddlClass dan ddlSubject menjadi 0, dan mengosongkan kontrol input
                        ddlClass.SelectedIndex = 0;
                        ddlSubject.SelectedIndex = 0;
                        txtRoll.Text = string.Empty;
                        txtStudMarks.Text = string.Empty;
                        txtOutOfMarks.Text = string.Empty;
                        // Memperbarui tampilan nilai
                        GetMarks();
                    }
                    else
                    {
                        // Menampilkan pesan bahwa data sudah ada
                        lblMsg.Text = "Entered <b>Data</b> already exists ";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    // Menampilkan pesan bahwa nomor roll tidak ditemukan untuk kelas yang dipilih
                    lblMsg.Text = "Entered RollNo <b>" + rollNo + "</b> does not exists for selected Class! ";
                    lblMsg.CssClass = "alert alert-danger";
                }

            }
            catch (Exception ex)
            {
                // Menampilkan pesan kesalahan jika terjadi exception
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat halaman GridView1 mengubah halaman
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Mengatur indeks halaman GridView1 dan memperbarui tampilan nilai
            GridView1.PageIndex = e.NewPageIndex;
            GetMarks();
        }

        // Metode yang dipanggil saat pengeditan baris GridView1 dibatalkan
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Mengatur indeks pengeditan GridView1 menjadi -1 dan memperbarui tampilan nilai
            GridView1.EditIndex = -1;
            GetMarks();
        }

        // Metode yang dipanggil saat baris GridView1 diedit
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Mengatur indeks pengeditan GridView1 dan memperbarui tampilan nilai
            GridView1.EditIndex = e.NewEditIndex;
            GetMarks();
        }

        // Metode yang dipanggil saat baris GridView1 diperbarui
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Mengambil baris GridView yang akan diperbarui
                GridViewRow row = GridView1.Rows[e.RowIndex];
                // Mengambil nilai ExamId dari dataKeys
                int examId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Mengambil nilai yang diperbarui dari kontrol-kontrol input di GridView
                string classId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlClassGv")).SelectedValue;
                string subjectId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string rollNo = (row.FindControl("txtRollNoGv") as TextBox).Text.Trim();
                string studMarks = (row.FindControl("txtStudMarksGv") as TextBox).Text.Trim();
                string outOfMarks = (row.FindControl("txtOutOfMarksGv") as TextBox).Text.Trim();
                // Menyusun query untuk memperbarui data ujian di database
                fn.Query("Update Exam set ClassId ='" + classId + "', SubjectId='" + subjectId + "', RollNo ='" + rollNo + "', TotalMarks ='" + studMarks + "', OutOfMarks ='" + outOfMarks + "' where ExamId = '" + examId + "' ");
                // Menampilkan pesan berhasil
                lblMsg.Text = "Record Updated Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Mengatur indeks pengeditan GridView1 menjadi -1 dan memperbarui tampilan nilai
                GridView1.EditIndex = -1;
                GetMarks();
            }
            catch (Exception ex)
            {
                // Menampilkan pesan kesalahan jika terjadi exception
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Metode yang dipanggil saat nilai ddlClassGv di GridView1 berubah
        protected void ddlClassGv_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlClassSelected = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlClassSelected.NamingContainer;
            if (row != null)
            {
                if ((row.RowState & DataControlRowState.Edit) > 0)
                {
                    // Mengambil referensi ke ddlSubjectGv dan mengisi dengan mata pelajaran yang sesuai dengan kelas yang dipilih
                    DropDownList ddlSubjectGV = (DropDownList)row.FindControl("ddlSubjectGv");
                    DataTable dt = fn.Fetch("Select * FROM Subject where ClassId = '" + ddlClassSelected.SelectedValue + "'");
                    ddlSubjectGV.DataSource = dt;
                    ddlSubjectGV.DataTextField = "SubjectName";
                    ddlSubjectGV.DataValueField = "SubjectId";
                    ddlSubjectGV.DataBind();
                }
            }
        }

        // Metode yang dipanggil saat data baris baru diikat ke GridView1
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    // Mengisi ddlSubjectGv saat baris sedang dalam mode edit
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

        // Metode yang dipanggil saat nilai ddlClass berubah
        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Mengisi ddlSubject berdasarkan kelas yang dipilih
            string classId = ddlClass.SelectedValue;
            DataTable dt = fn.Fetch("SELECT * FROM Subject WHERE ClassId = '" + classId + "' ");
            ddlSubject.DataSource = dt;
            ddlSubject.DataTextField = "SubjectName";
            ddlSubject.DataValueField = "SubjectId";
            ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, "Select Subject");
        }
    }
}

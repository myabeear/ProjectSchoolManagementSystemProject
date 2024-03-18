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
    public partial class Student : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx(); // Menginisialisasi objek dari kelas Commonfnx
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Memeriksa apakah halaman dimuat untuk pertama kali atau tidak
            {
                GetClass(); // Memanggil fungsi GetClass() untuk mengisi dropdown dengan kelas
                GetStudents(); // Memanggil fungsi GetStudents() untuk mengambil dan menampilkan data siswa
            }
        }

        // Fungsi untuk mengambil daftar kelas dan mengisinya ke dropdownlist
        private void GetClass()
        {
            DataTable dt = fn.Fetch("Select * from Class"); // Mengambil data kelas dari database
            ddlClass.DataSource = dt; // Mengatur sumber data dropdownlist
            ddlClass.DataTextField = "ClassName"; // Menentukan kolom yang akan ditampilkan sebagai teks
            ddlClass.DataValueField = "ClassId"; // Menentukan nilai dari setiap opsi dropdown
            ddlClass.DataBind(); // Mengikat data ke dropdownlist
            ddlClass.Items.Insert(0, "Select Class"); // Menambahkan item default ke dropdownlist
        }

        // Event handler untuk tombol "Add"
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlGender.SelectedValue != "0") // Memeriksa apakah jenis kelamin dipilih
                {
                    string rollNo = txtRoll.Text.Trim(); // Mengambil nomor roll dari input
                    DataTable dt = fn.Fetch("Select * from Student where RollNo = '" + rollNo + "'"); // Memeriksa apakah nomor roll sudah ada di database
                    if (dt.Rows.Count == 0) // Jika nomor roll belum ada di database
                    {
                        // Membuat query untuk memasukkan data siswa baru ke dalam database
                        string query = "Insert into Student values ('" + txtName.Text.Trim() + "','" + txtDoB.Text.Trim() + "'," +
                            " '" + ddlGender.SelectedValue + "', '" + txtMobile.Text.Trim() + "', '" + txtRoll.Text.Trim() + "', " +
                            "'" + txtAddress.Text.Trim() + "','" + ddlClass.SelectedValue + "' ) ";
                        fn.Query(query); // Menjalankan query
                        lblMsg.Text = "Inserted Successfully"; // Menampilkan pesan sukses
                        lblMsg.CssClass = "alert alert-success"; // Menentukan kelas CSS untuk pesan
                        // Mengosongkan input setelah penyisipan berhasil
                        ddlGender.SelectedIndex = 0;
                        txtName.Text = string.Empty;
                        txtDoB.Text = string.Empty;
                        txtMobile.Text = string.Empty;
                        txtRoll.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        ddlClass.SelectedIndex = 0;
                        GetStudents(); // Memperbarui tampilan data siswa setelah penyisipan berhasil
                    }
                    else // Jika nomor roll sudah ada di database
                    {
                        lblMsg.Text = "Entered Roll No. <b> '" + rollNo + "'</b>already exists for selected Class!"; // Menampilkan pesan bahwa nomor roll sudah ada
                        lblMsg.CssClass = "alert alert-danger"; // Menentukan kelas CSS untuk pesan
                    }
                }
                else // Jika jenis kelamin tidak dipilih
                {
                    lblMsg.Text = "Gender is required"; // Menampilkan pesan bahwa jenis kelamin diperlukan
                    lblMsg.CssClass = "alert alert-danger"; // Menentukan kelas CSS untuk pesan
                }
            }
            catch (Exception ex) // Menangani kesalahan yang mungkin terjadi
            {
                Response.Write("<script>alert('" + ex.Message + "'); </script>"); // Menampilkan pesan kesalahan
            }
        }

        // Fungsi untuk mengambil data siswa dan menampilkannya di GridView
        private void GetStudents()
        {
            DataTable dt = fn.Fetch(@"Select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as [No], s.StudentId, s.[Name], s.DOB, s.Gender, s.Mobile, s.RollNo, s.[Address], c.ClassId, c.ClassName
                    from Student s INNER JOIN Class c ON c.ClassId=s.ClassId"); // Mengambil data siswa dari database
            GridView1.DataSource = dt; // Mengatur sumber data GridView
            GridView1.DataBind(); // Mengikat data ke GridView
        }

        // Event handler untuk perubahan halaman pada GridView
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex; // Menentukan indeks halaman baru
            GetStudents(); // Memuat ulang data siswa dengan halaman baru
        }

        // Event handler untuk pembatalan mode pengeditan pada GridView
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1; // Keluar dari mode pengeditan
            GetStudents(); // Memuat ulang data siswa setelah pembatalan
        }

        // Event handler untuk memulai mode pengeditan pada GridView
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex; // Masuk ke mode pengeditan
            GetStudents(); // Memuat ulang data siswa setelah memulai pengeditan
        }

        // Event handler untuk menyimpan perubahan setelah pengeditan pada GridView
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex]; // Mendapatkan baris yang sedang diedit
                int studentId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]); // Mendapatkan ID siswa dari datakey
                // Mendapatkan nilai dari kolom yang diedit
                string name = (row.FindControl("txtName") as TextBox).Text;
                string mobile = (row.FindControl("txtMobile") as TextBox).Text;
                string rollNo = (row.FindControl("txtRollNo") as TextBox).Text;
                string address = (row.FindControl("txtAddress") as TextBox).Text;
                string classId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[4].FindControl("ddlClass")).SelectedValue;
                // Membuat query untuk memperbarui data siswa
                fn.Query("Update Student set Name ='" + name.Trim() + "', Mobile='" + mobile.Trim() + "', Address='" + address.Trim() + "', RollNo='" + rollNo.Trim() + "', ClassId ='" + classId + "' where StudentId = '" + studentId + "' ");
                lblMsg.Text = "Student Updated Successfully"; // Menampilkan pesan bahwa data siswa berhasil diperbarui
                lblMsg.CssClass = "alert alert-success"; // Menentukan kelas CSS untuk pesan
                GridView1.EditIndex = -1; // Keluar dari mode pengeditan
                GetStudents(); // Memuat ulang data siswa setelah perubahan disimpan
            }
            catch (Exception ex) // Menangani kesalahan yang mungkin terjadi
            {
                Response.Write("<script>alert('" + ex.Message + "'); </script>"); // Menampilkan pesan kesalahan
            }
        }

        // Event handler untuk mengikat data ke dropdownlist saat GridView diikat
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlClass = (DropDownList)e.Row.FindControl("ddlClass"); // Mendapatkan dropdownlist dari baris saat ini
                DataTable dt = fn.Fetch("Select * FROM Class"); // Mendapatkan daftar kelas dari database
                ddlClass.DataSource = dt; // Mengatur sumber data dropdownlist
                ddlClass.DataTextField = "ClassName"; // Menentukan kolom yang akan ditampilkan sebagai teks
                ddlClass.DataValueField = "ClassId"; // Menentukan nilai dari setiap opsi dropdown
                ddlClass.DataBind(); // Mengikat data ke dropdownlist
                ddlClass.Items.Insert(0, "Select Class"); // Menambahkan item default ke dropdownlist
                string selectedClass = DataBinder.Eval(e.Row.DataItem, "ClassName").ToString(); // Mendapatkan nilai kelas yang sudah dipilih
                ddlClass.Items.FindByText(selectedClass).Selected = true; // Menetapkan kelas yang sudah dipilih sebagai yang terpilih
            }
        }
    }
}


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
    public partial class ClassFees : System.Web.UI.Page
    {
        // Inisialisasi objek Commonfnx untuk penggunaan fungsi-fungsi umum
        Commonfnx fn = new Commonfnx();

        // Method yang dipanggil saat halaman dimuat
        protected void Page_Load(object sender, EventArgs e)
        {
            // Jalankan jika tidak postback
            if (!IsPostBack)
            {
                // Ambil kelas dan biaya
                GetClass();
                GetFees();
            }
        }

        // Method untuk mengambil kelas
        private void GetClass()
        {
            // Ambil data kelas dari database
            DataTable dt = fn.Fetch("Select * from Class");
            // Set data source dropdownlist kelas
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            // Tambahkan item pilih kelas di indeks pertama
            ddlClass.Items.Insert(0, "Select Class");
        }

        // Event handler untuk tombol tambah biaya
        protected void bntAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Ambil nilai kelas dari dropdownlist
                string classVal = ddlClass.SelectedItem.Text;
                // Ambil data biaya sesuai dengan kelas yang dipilih
                DataTable dt = fn.Fetch("Select * from Fees where ClassId = '" + ddlClass.SelectedItem.Value + "'");
                // Jika tidak ada data biaya untuk kelas tersebut
                if (dt.Rows.Count == 0)
                {
                    // Tambahkan data biaya baru ke database
                    string query = "Insert into Fees values('" + ddlClass.SelectedItem.Value + "','" + txtFeeAmounts.Text.Trim() + "')";
                    fn.Query(query);
                    // Tampilkan pesan berhasil ditambahkan
                    lblMsg.Text = "Inserted Successfully";
                    lblMsg.CssClass = "alert alert-success";
                    // Reset dropdownlist dan textbox
                    ddlClass.SelectedIndex = 0;
                    txtFeeAmounts.Text = string.Empty;
                    // Perbarui tampilan data biaya
                    GetFees();
                }
                else
                {
                    // Tampilkan pesan biaya sudah ada
                    lblMsg.Text = "Entered Fees already exists for <b> '" + classVal + "'</b>!";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                // Tangkap dan tampilkan pesan error
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Method untuk mengambil data biaya
        private void GetFees()
        {
            // Ambil data biaya dari database dan kaitkan dengan kelas
            DataTable dt = fn.Fetch(@"Select Row_NUMBER() over(Order by (Select 1)) as [No], f.FeesId, f.ClassId, c.ClassName, 
                                        f.FeesAmount from Fees f inner join Class c on c.ClassId = f.ClassId");
            // Set data source gridview
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // Event handler untuk perubahan halaman gridview
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Atur indeks halaman gridview dan perbarui tampilan data biaya
            GridView1.PageIndex = e.NewPageIndex;
            GetFees();
        }

        // Event handler untuk pembatalan edit baris gridview
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Batalkan mode edit dan perbarui tampilan data biaya
            GridView1.EditIndex = -1;
            GetFees();
        }

        // Event handler untuk penghapusan baris gridview
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Ambil ID biaya yang akan dihapus
                int feesId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Hapus data biaya dari database
                fn.Query("Delete from Fees where FeesId = '" + feesId + "'");
                // Tampilkan pesan berhasil dihapus
                lblMsg.Text = "Fees Deleted Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Keluar dari mode edit dan perbarui tampilan data biaya
                GridView1.EditIndex = -1;
                GetFees();
            }
            catch (Exception ex)
            {
                // Tangkap dan tampilkan pesan error
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        // Event handler untuk memulai edit baris gridview
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Masuk ke mode edit dan perbarui tampilan data biaya
            GridView1.EditIndex = e.NewEditIndex;
            GetFees();
        }

        // Event handler untuk menyimpan perubahan pada baris yang diedit dalam gridview
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // Ambil baris yang diedit
                GridViewRow row = GridView1.Rows[e.RowIndex];
                // Ambil ID biaya yang akan diupdate
                int feesId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                // Ambil nilai biaya yang diubah
                string feeAmt = (row.FindControl("TextBox1") as TextBox).Text;
                // Update data biaya di database
                fn.Query("Update Fees set FeesAmount ='" + feeAmt.Trim() + "' where FeesId = '" + feesId + "' ");
                // Tampilkan pesan berhasil diupdate
                lblMsg.Text = "Fees Updated Successfully";
                lblMsg.CssClass = "alert alert-success";
                // Keluar dari mode edit dan perbarui tampilan data biaya
                GridView1.EditIndex = -1;
                GetFees();
            }
            catch (Exception ex)
            {
                // Tangkap dan tampilkan pesan error
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }
    }
}

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
    public partial class ExpenseDetails : System.Web.UI.Page
    {
        // Inisialisasi objek dari kelas Commonfnx
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }

            // Memastikan bahwa halaman tidak dimuat ulang secara tidak sengaja (Postback)
            if (!IsPostBack)
            {
                // Memanggil fungsi GetExpenseDetails() saat halaman pertama dimuat
                GetExpenseDetails();
            }
        }

        // Fungsi untuk mendapatkan detail pengeluaran dan menampilkan pada GridView
        private void GetExpenseDetails()
        {
            // Mendapatkan data pengeluaran dari database menggunakan kueri SQL
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No], e.ExpenseId, e.ClassId, c.ClassName, e.SubjectId, 
                                        s.SubjectName, e.ChargeAmount from Expense e 
                              INNER JOIN Class c ON e.ClassId = c.ClassId 
                              INNER JOIN Subject s ON e.SubjectId = s.SubjectId");

            // Mengatur sumber data GridView dengan data yang diperoleh dari database
            GridView1.DataSource = dt;

            // Mengikat data GridView
            GridView1.DataBind();
        }
    }
}

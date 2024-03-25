using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class AdminMst : System.Web.UI.MasterPage
    {

        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            GetUsername();
            
        }



        void GetUsername()
        {
            // Periksa apakah sesi "username" sudah ada
            if (Session["username"] == null)
            {
                // Ambil alamat email dari sesi "admin" karena pengguna telah login
                string email = Session["admin"].ToString();
                // Cari admin dengan alamat email yang sama di database
                DataTable dt = fn.Fetch("Select * from Users where Email = '" + email + "'");

                // Memeriksa apakah data ditemukan
                if (dt != null && dt.Rows.Count > 0)
                {
                    // Menyimpan nama ke dalam session
                    Session["username"] = dt.Rows[0]["name"].ToString();
                    Session["address"] = dt.Rows[0]["Address"].ToString();
                }
            }
        }


        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("../Login.aspx");
        }
    }
}
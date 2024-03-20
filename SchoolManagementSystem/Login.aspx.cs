using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem
{
    public partial class Login : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email=inputEmail.Value.Trim();
            string password=inputPassword.Value.Trim();
            
            // Check if user is teacher
            DataTable dt = fn.Fetch("SELECT * FROM Users WHERE Email = '" + email + "' AND password = '" + password + "'");
            if (dt.Rows.Count > 0)
            {
                int roleId = Convert.ToInt32(dt.Rows[0]["RoleId"]);
                // Check if UserRole is admin (RoleId=1) or teacher (RoleId=2)
                if (roleId == 1)
                {
                    Session["admin"] = email;
                    Response.Redirect("Admin/AdminHome.aspx");
                }
                else if (roleId == 2)
                {
                    Session["staff"] = email;
                    Response.Redirect("Teacher/TeacherHome.aspx");
                }
                return; // Redirected, so exit the method
            }

            // If neither admin nor teacher, show login failed message
            lblMsg.Text = "Login Failed!!";
            lblMsg.ForeColor = System.Drawing.Color.Red;
        }
    }
}
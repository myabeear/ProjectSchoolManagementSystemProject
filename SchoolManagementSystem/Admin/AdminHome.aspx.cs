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
    public partial class AdminHome : System.Web.UI.Page
    {
        // Objek fungsi umum untuk pengoperasian database
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            else
            {
                StudentCount();
                TeacherCount();
                ClassCount();
                SubjectCount();
            }
        }

        void StudentCount()
        {
            DataTable dt = fn.Fetch("SELECT COUNT(*) from Student ");
            Session["student"] = dt.Rows[0][0];
        }

        void TeacherCount()
        {
            DataTable dt = fn.Fetch("SELECT COUNT(*) from Users ");
            Session["users"] = dt.Rows[0][0];
        }

        void ClassCount()
        {
            DataTable dt = fn.Fetch("SELECT COUNT(*) from Class ");
            Session["class"] = dt.Rows[0][0];
        }

        void SubjectCount()
        {
            DataTable dt = fn.Fetch("SELECT COUNT(*) from Subject ");
            Session["subject"] = dt.Rows[0][0];
        }
    }
}
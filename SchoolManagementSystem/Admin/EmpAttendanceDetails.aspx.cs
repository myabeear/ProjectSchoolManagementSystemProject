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
    public partial class EmpAttendanceDetails : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTeacher();
                
            }
        }

        private void GetTeacher()
        {
            DataTable dt = fn.Fetch("Select * from Teacher");
            ddlTeacher.DataSource = dt;
            ddlTeacher.DataTextField = "Name";
            ddlTeacher.DataValueField = "TeacherId";
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, "Select Teacher");
        }

        protected void btnCheckAttendance_Click(object sender, EventArgs e)
        {
            DateTime date= Convert.ToDateTime (txtMonth.Text);

            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No],t.Name,ta.Status,ta.Date FROM TeacherAttedance ta
                              INNER JOIN Teacher t ON t.TeacherId = ta.TeacherId  
                              where DATEPART(yy,Date) ='"+date.Year+"' and DATEPART(M,Date) = '"+date.Month+"' and ta.TeacherId = '"+ddlTeacher.SelectedValue+"'");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

    }
}
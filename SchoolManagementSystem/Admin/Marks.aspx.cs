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
    public partial class Marks : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass();
                GetMarks();
            }
        }

        private void GetMarks()
        {
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [No], e.ExamId, e.ClassId, c.ClassName, e.SubjectId, 
                                        s.SubjectName, e.RollNo, e.TotalMarks, e.OutOfMarks from Exam e 
                              INNER JOIN Class c ON e.ClassId = c.ClassId 
                              INNER JOIN Subject s ON e.SubjectId = s.SubjectId");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private void GetClass()
        {
            DataTable dt = fn.Fetch("Select * from Class");
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, "Select Class");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string classId = ddlClass.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string rollNo = txtRoll.Text;
                string studMarks = txtStudMarks.Text;
                string outOfMarks = txtOutOfMarks.Text;
                DataTable dt = fn.Fetch("Select * from Exam where ClassId = '" + classId + "'and SubjectId='" + subjectId + "' and RollNo='" + rollNo + "' ");
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into Exam values('" + classId + "','" + subjectId + "','" + rollNo + "','" + studMarks + "','" + outOfMarks + "')";
                    fn.Query(query);
                    lblMsg.Text = "Inserted Successfully";
                    lblMsg.CssClass = "alert alert-success";
                    ddlClass.SelectedIndex = 0;
                    ddlSubject.SelectedIndex = 0;
                    txtRoll.Text = string.Empty;
                    txtStudMarks.Text = string.Empty;
                    txtOutOfMarks.Text = string.Empty;
                    GetMarks();
                }
                else
                {
                    lblMsg.Text = "Entered <b>Data</b> already exists";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "'); </script>");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void ddlClassGv_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
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
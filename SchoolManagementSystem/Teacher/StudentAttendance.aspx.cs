﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Teacher
{
    public partial class StudentAttendance : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["staff"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            if (!IsPostBack)
            {
                GetClass();
                btnMarkAttend.Visible = false;
            }
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


        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt = fn.Fetch(@"SELECT StudentId, RollNo, Name, Mobile from Student where ClassId='"+ddlClass.SelectedValue+"' " );
            GridView1.DataSource = dt;
            GridView1.DataBind();
            if (dt.Rows.Count >0)
            {
                btnMarkAttend.Visible = true;
            }
            else
            {
                btnMarkAttend.Visible = false;
            }
        }

        protected void btnMarkAttendance_click(object sender, EventArgs e)
        {
            bool isTrue = false; 
            foreach (GridViewRow row in GridView1.Rows)
            {
                string rollNo =row.Cells[2].Text.Trim();

                RadioButton rb1 = (row.Cells[0].FindControl("RadioButton1") as RadioButton);
                RadioButton rb2 = (row.Cells[0].FindControl("RadioButton2") as RadioButton);
                int status = 0;
                if (rb1.Checked)
                {
                    status = 1;
                }
                else if (rb2.Checked)
                {
                    status = 0;
                }

                fn.Query(@"Insert into StudentAttedance values ('" + ddlClass.SelectedValue + "', '" + ddlSubject.SelectedValue + "','" + rollNo + "', '" + status + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "')");
                isTrue= true;
                lblMsg.Text = "Inserted Successfully";
                lblMsg.CssClass = "alert alert-success";
            }
            if (isTrue)
            {
                lblMsg.Text = "Inserted Successfully";
                lblMsg.CssClass = "alert alert-success";
            }
            else
            {
                lblMsg.Text = "Something went wrong!";
                lblMsg.CssClass = "alert alert-warning";
            }
        }
    }
}
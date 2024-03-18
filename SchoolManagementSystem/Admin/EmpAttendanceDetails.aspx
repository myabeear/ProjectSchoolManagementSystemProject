<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="EmpAttendanceDetails.aspx.cs" Inherits="SchoolManagementSystem.Admin.EmpAttendanceDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image: url('../Image/bg4.jpg'); width: 100%; height: 720px; background-repeat: no-repeat; background-size: cover; background-attachment: fixed;">
        <div class="container p-md-4 p-sm-4">
            <div>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <h3 class="text-c">Teacher Attendance Details</h3>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-6">
                    <label for="ddlTeacher">Teacher</label>
                    <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                   .
                </div>

                <div class="col-md-6">
                    <label for="ddlSubject">Month</label>
                    <asp:TextBox ID="txtMonth" CssClass="form-control" runat="server" TextMode="Month" required></asp:TextBox>
                </div>

            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5">
                <div class="col-md-3 col-md-offset-2 mb-3">
                    <asp:Button ID="btnCheckAttendance" runat="server" CssClass="btn btn-primary btn-block" BackColor="#5558C9" Text="Check Attendance" OnClick="btnCheckAttendance_Click" />
                </div>
            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5">
                <div class="col-md-12">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="No Record To Display!"
                        AutoGenerateColumns="False">
                        <columns>
                            <asp:BoundField DataField="No" HeaderText="No">
                                <itemstyle horizontalalign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="Name">
                                <itemstyle horizontalalign="Center" />
                            </asp:BoundField>
                            
                            
                            <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="label1" Text='<%# Boolean.Parse( Eval("Status").ToString()) ? "Present" : "Absent" %>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd MMMM yyyy}">
                                <itemstyle horizontalalign="Center" />
                            </asp:BoundField>

                        </columns>
                        <headerstyle backcolor="#5558C9" forecolor="White" />
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>

</asp:Content>

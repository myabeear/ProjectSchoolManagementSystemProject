<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="SchoolManagementSystem.Admin.Role" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image:url('../Image/bg4.jpg'); width:100%; height:720px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <div>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <h3 class="text-c">New Role</h3> <!-- Changed from "New Fees" to "New Role" -->

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">


                <div class="col-md-6">
                    <label for="txtRole">Role Name</label>
                    <asp:TextBox ID="txtRole" runat="server" CssClass="form-control" placeholder="Enter Role Name" required></asp:TextBox> <!-- Changed ID from "txtFeeAmounts" to "txtRole" and TextMode from "text" to "Text" -->
                </div>
            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5">
                <div class="col-md-3 col-md-offset-2 mb-3">
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block" BackColor="#5558C9" Text="Add Role" OnClick="btnAdd_Click" /> <!-- Changed ID from "bntAdd" to "btnAdd" and OnClick from "bntAdd_Click" to "btnAdd_Click" -->
                </div>
            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5">
                <div class="col-md-8">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="No Record To Display!"
                        AutoGenerateColumns="False" AllowPaging="true" PageSize="4" OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="RoleId" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                        OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                        <Columns>
                            <asp:BoundField DataField="No" HeaderText="No" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="Role">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Role") %>' CssClass="form-control"></asp:TextBox> <!-- Changed Eval to Bind -->
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Role") %>'></asp:Label> <!-- Changed Eval to Bind -->
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:CommandField CausesValidation="false" HeaderText="Operation" ShowEditButton="True">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                        </Columns>
                        <HeaderStyle BackColor="#5558C9" ForeColor="White" />
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>

</asp:Content>

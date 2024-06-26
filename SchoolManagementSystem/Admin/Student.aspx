﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="Student.aspx.cs" Inherits="SchoolManagementSystem.Admin.Student" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <div style="background-image:url('../Image/bg4.jpg'); width:100%; height:720px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
    <div class="container p-md-4 p-sm-4">
        <div>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
        <h3 class="text-c">Add Student</h3>

        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-6">
                <label for="txtName">Name</label>
               <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter Name" required></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ErrorMessage="Name should be in Characters" 
                    ForeColor="Red" ValidationExpression="^[a-zA-Z\s]+$" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtName">
                    </asp:RegularExpressionValidator>
                </div>

             <div class="col-md-6">
            <label for="txtDoB">Date of Birth</label>
              <asp:TextBox ID="txtDoB" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
 </div>
        </div>

               <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
           <div class="col-md-6">
               <label for="ddlGender">Gender</label>
               <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                   <asp:ListItem Value="0">Select Gender</asp:ListItem>
                   <asp:ListItem Value="1">Laki-Laki</asp:ListItem>
                   <asp:ListItem Value="2">Perempuan</asp:ListItem>
                   <asp:ListItem>Other</asp:ListItem>
               </asp:DropDownList>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Gender is required"
                   ForeColor="Red" ControlToValidate="ddlGender" Display="Dynamic" SetFocusOnError="true" InitialValue="Select Value" ></asp:RequiredFieldValidator>
               </div>

           <div class="col-md-6">
    <label for="txtMobile">Contact Number</label>
    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" TextMode="SingleLine" placeholder="12 Digits Mobile No" required></asp:TextBox>
    <asp:RegularExpressionValidator runat="server" ErrorMessage="Invalid Mobile No." 
        ForeColor="Red" ValidationExpression="[0-9]{13}" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMobile">
    </asp:RegularExpressionValidator>
</div>

       </div>

               <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
           <div class="col-md-6">
               <label for="txtRoll">Roll Number</label>
              <asp:TextBox ID="txtRoll" runat="server" CssClass="form-control" placeholder="Enter Roll Number" required></asp:TextBox>
               
               </div>

            <div class="col-md-6">
           <label for="ddlClass">Class</label>
                <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ErrorMessage="Class is required." ControlToValidate="ddlClass" Display="Dynamic" ForeColor="Red" InitialValue="Select Class" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </div>
       </div>

           <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
           <div class="col-md-12">
           <label for="txtAddress">Address</label>
           <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter Address" TextMode="MultiLine" required></asp:TextBox>
               
           </div>

      </div>

        <div class="row mb-3 mr-lg-5 ml-lg-5">
             <div class="col-md-3 col-md-offset-2 mb-3">
                 <asp:Button ID="bntAdd" runat="server" CssClass="btn btn-primary btn-block" BackColor="#5558C9" Text="Add Student" OnClick="btnAdd_Click" />
             </div>
        </div>

        <div class="row mb-3 mr-lg-5 ml-lg-5">
          <div class="col-md-12">
              <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="No Record To Display!" 
                  AutoGenerateColumns="False" AllowPaging="True" PageSize="4" OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="StudentId" OnRowCancelingEdit="GridView1_RowCancelingEdit" 
                  OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDataBound="GridView1_RowDataBound" >
                  <Columns>
                      <asp:BoundField DataField="No" HeaderText="No" ReadOnly="true">
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:BoundField>
                      <asp:TemplateField HeaderText="Class">
                          <EditItemTemplate>
                              <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' CssClass="form-control" 
                                  Width="100px"></asp:TextBox>
                          </EditItemTemplate>
                          <ItemTemplate>
                              <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                          </ItemTemplate>
                          <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Mobile">
                          <EditItemTemplate>
                              <asp:TextBox ID="txtMobile" runat="server" Text='<%# Eval("Mobile") %>' CssClass="form-control"
                                  Width="100px" ></asp:TextBox>
                          </EditItemTemplate>
                          <ItemTemplate>
                              <asp:Label ID="lblMobile" runat="server" Text='<%# Eval("Mobile") %>'></asp:Label>
                          </ItemTemplate>
                          <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Roll Number">
                          <EditItemTemplate>
                        <asp:TextBox ID="txtRollNo" runat="server" Text='<%# Eval("RollNo") %>' CssClass="form-control"
                            Width="100px" ></asp:TextBox>
                    </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RollNo") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Class">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control" Width="120px"></asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblClass" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>


                      <asp:TemplateField HeaderText="Address">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAddress" runat="server" Text='<%# Eval("Address") %>' CssClass="form-control"
                            Width="170px" ></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                      <asp:CommandField CausesValidation="false" HeaderText="Operation" ShowEditButton="True" >
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

<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="AddClass.aspx.cs" Inherits="SchoolManagementSystem.Admin.AddClass" %>

<%-- Menggunakan MasterPageFile untuk menentukan master page yang digunakan, dan CodeBehind untuk menentukan file kode belakang yang terkait.--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div style="background-image:url('../Image/bg4.jpg'); width:100%; height:720px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
      <%-- Mendefinisikan tampilan latar belakang dengan gambar. --%>

      <div class="container p-md-4 p-sm-4">
         <%-- Membuat kontainer untuk isi halaman.--%>
          <div>
              <asp:Label ID="lblMsg" runat="server"></asp:Label>
              <%--Menampilkan pesan yang mungkin muncul.--%>
          </div>
          <h3 class="text-c">New Class</h3>
        <%--Menampilkan judul halaman.--%>

          <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
             <%-- Membuat baris untuk form tambah kelas.--%>
              <div class="col-md-6">
                  <label for="txtClass">Class Name</label>
                  <%--Menampilkan label untuk input nama kelas--%>
                  <asp:TextBox ID="txtClass" runat="server" CssClass="form-control" placeholder="Enter Class Name" required></asp:TextBox>
                  <%--Membuat input teks untuk nama kelas.--%>
              </div>
          </div>

          <div class="row mb-3 mr-lg-5 ml-lg-5">
               <%--Membuat baris untuk tombol tambah kelas.--%>
               <div class="col-md-3 col-md-offset-2 mb-3">
                   <asp:Button ID="bntAdd" runat="server" CssClass="btn btn-primary btn-block" BackColor="#5558C9" Text="Add Class" OnClick="bntAdd_Click" />
                   <%--Menambahkan tombol untuk menambahkan kelas baru.--%>
               </div>
          </div>

          <div class="row mb-3 mr-lg-5 ml-lg-5">
            <%--Membuat baris untuk menampilkan daftar kelas.--%>
            <div class="col-md-6">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" DataKeyNames="ClassId" AutoGenerateColumns="False" 
                    EmptyDataText="No Record to display!" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" 
                    OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" AllowPaging="true" PageSize="4">
                    <%--Membuat GridView untuk menampilkan daftar kelas.--%>
                    <Columns>
                        <asp:BoundField DataField="No" HeaderText="No" ReadOnly="True">
                        <%--Menampilkan nomor urut.--%> 
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Class">
                            <%--Menampilkan nama kelas. --%>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtClassEdit" runat="server" Text='<%# Eval("ClassName") %>' CssClass="form-control"></asp:TextBox>
                                 <%--Memungkinkan pengeditan nama kelas. --%>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblClassName" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                                <%--Menampilkan nama kelas dalam mode tampilan.--%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:CommandField CausesValidation="False" HeaderText="Operation" ShowEditButton="True" />
                        <%--Menampilkan tombol edit--%>
                    </Columns>
                    <HeaderStyle BackColor="#5558C9" ForeColor="White" />
                   <%-- Menyesuaikan tampilan header GridView.--%>
                </asp:GridView>
            </div>
          </div>
      </div>

  </div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="MarksDetails.aspx.cs" Inherits="SchoolManagementSystem.Admin.MarksDetails" %>
<!-- 
    Penjelasan:
    - <%@ Page %> digunakan untuk mendefinisikan properti halaman, seperti judul, bahasa, file MasterPage, dll.
    - Title: Judul halaman (kosong).
    - Language: Bahasa yang digunakan dalam kode (C#).
    - MasterPageFile: File master yang digunakan untuk halaman ini.
    - AutoEventWireup: Menentukan apakah metode kejadian halaman secara otomatis dikaitkan dengan kejadian.
    - CodeBehind: Nama file kode belakang terkait dengan halaman ini.
    - Inherits: Nama kelas yang digunakan sebagai kode belakang untuk halaman ini.
-->

<%@ Register Src="~/MarksDetailUserControl.ascx" TagPrefix="uc" TagName="MarksDetail" %>
<!--
    Penjelasan:
    - <%@ Register %> digunakan untuk mendaftarkan kontrol pengguna kustom di halaman.
    - Src: Lokasi file user control yang akan didaftarkan.
    - TagPrefix: Awalan tag yang digunakan untuk kontrol pengguna ini.
    - TagName: Nama yang akan digunakan untuk mengacu pada kontrol pengguna ini di halaman.
-->

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<!--
    Penjelasan:
    - <%@ Content %> digunakan untuk menentukan konten yang akan dimasukkan ke dalam placeholder di file master.
    - ID: ID unik untuk konten ini.
    - ContentPlaceHolderID: ID placeholder tempat konten akan dimasukkan.
-->

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!--
    Penjelasan:
    - <%@ Content %> lagi digunakan untuk menentukan konten yang akan dimasukkan ke dalam placeholder di file master.
    - ID: ID unik untuk konten ini.
    - ContentPlaceHolderID: ID placeholder tempat konten akan dimasukkan.
-->

<uc:MarksDetail runat="server" ID="MarksDetail1"/> 
<!--
    Penjelasan:
    - Memasukkan kontrol pengguna MarksDetailUserControl ke dalam halaman.
    - runat="server": Menandakan bahwa kontrol ini dikelola oleh server.
    - ID: ID unik untuk kontrol ini.
-->
</asp:Content>

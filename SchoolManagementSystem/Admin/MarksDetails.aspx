﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="MarksDetails.aspx.cs" Inherits="SchoolManagementSystem.Admin.MarksDetails" %>


<%@ Register Src="~/MarksDetailUserControl.ascx" TagPrefix="uc" TagName="MarksDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<uc:MarksDetail runat="server" ID="MarksDetail1"/> 

</asp:Content>

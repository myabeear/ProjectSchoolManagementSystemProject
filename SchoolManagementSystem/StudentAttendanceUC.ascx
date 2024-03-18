<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudentAttendanceUC.ascx.cs" Inherits="SchoolManagementSystem.StudentAttendanceUC" %>


   <div style="background-image:url('../Image/bg4.jpg'); width:100%; height:720px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
    <div class="container p-md-4 p-sm-4">
        <div>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
       

        <h3 class="text-c">Student's Attendance Details</h3>


         <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
     <div class="col-md-6">
         <label for="ddlClass">Class </label>
        <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged"></asp:DropDownList>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ErrorMessage="Class is required." ControlToValidate="ddlClass" Display="Dynamic" ForeColor="Red" InitialValue="Select Class" SetFocusOnError="True"></asp:RequiredFieldValidator>
     </div>

      <div class="col-md-6">
     <label for="ddlSubject">Subject</label>
       <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-control" ></asp:DropDownList>
        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Roll Number is required." ControlToValidate="ddlSubject" Display="Dynamic" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
        --%> </div>
    </div>


       <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
     <div class="col-md-6">
         <label for="txtRollNo">Roll Number </label>
         <asp:TextBox ID="txtRollNo" runat="server" CssClass="form-control"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Class is required." ControlToValidate="txtRollNo" Display="Dynamic" ForeColor="Red" InitialValue="Select Class" SetFocusOnError="True"></asp:RequiredFieldValidator>
     </div>

      <div class="col-md-6">
    <label for="txtMonth">Month </label>
<asp:TextBox ID="txtMonth" runat="server" CssClass="form-control"></asp:TextBox>
<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Class is required." ControlToValidate="txtMonth" Display="Dynamic" ForeColor="Red" InitialValue="Select Class" SetFocusOnError="True"></asp:RequiredFieldValidator>
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
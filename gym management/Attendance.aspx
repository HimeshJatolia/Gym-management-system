<%@ Page Title="Attendance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="gym_management.Attendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Attendance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

    <!-- Check-In Form -->
    <div class="content-card" style="margin-bottom:24px;">
        <div class="card-header">
            <h3><i class="fas fa-calendar-check" style="color:var(--accent-green);"></i> Mark Attendance</h3>
        </div>
        <div class="card-body">
            <div class="form-grid" style="align-items:flex-end;">
                <div class="form-group">
                    <label>Select Member</label>
                    <asp:DropDownList ID="ddlMember" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnCheckIn" runat="server" Text="Check In" CssClass="btn btn-success" OnClick="btnCheckIn_Click" />
                    <asp:Button ID="btnCheckOut" runat="server" Text="Check Out" CssClass="btn btn-outline" OnClick="btnCheckOut_Click" style="margin-left:8px;" />
                </div>
            </div>
        </div>
    </div>

    <!-- Today's Attendance -->
    <div class="content-card">
        <div class="card-header">
            <h3>Today's Attendance</h3>
            <span class="badge badge-active">
                <asp:Label ID="lblTodayCount" runat="server" Text="0" /> members
            </span>
        </div>
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No attendance recorded today." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="AttendanceID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Member Name" />
                    <asp:BoundField DataField="CheckInTime" HeaderText="Check In" DataFormatString="{0:hh:mm tt}" />
                    <asp:TemplateField HeaderText="Check Out">
                        <ItemTemplate>
                            <%# Eval("CheckOutTime") == DBNull.Value ? "<span class='checked-in'>Still In</span>" : Convert.ToDateTime(Eval("CheckOutTime")).ToString("hh:mm tt") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Duration">
                        <ItemTemplate>
                            <%# Eval("CheckOutTime") == DBNull.Value ? "-" : ((Convert.ToDateTime(Eval("CheckOutTime")) - Convert.ToDateTime(Eval("CheckInTime"))).TotalMinutes).ToString("F0") + " min" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>

<%@ Page Title="My Attendance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberAttendance.aspx.cs" Inherits="gym_management.MemberAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    My Attendance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Stats -->
    <div class="stats-grid" style="grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); margin-bottom:24px;">
        <div class="stat-card orange">
            <div class="stat-icon"><i class="fas fa-calendar-check"></i></div>
            <div class="stat-value"><asp:Label ID="lblThisMonth" runat="server" Text="0" /></div>
            <div class="stat-label">This Month</div>
        </div>
        <div class="stat-card green">
            <div class="stat-icon"><i class="fas fa-calendar-alt"></i></div>
            <div class="stat-value"><asp:Label ID="lblLastMonth" runat="server" Text="0" /></div>
            <div class="stat-label">Last Month</div>
        </div>
        <div class="stat-card blue">
            <div class="stat-icon"><i class="fas fa-chart-bar"></i></div>
            <div class="stat-value"><asp:Label ID="lblTotalAll" runat="server" Text="0" /></div>
            <div class="stat-label">Total All Time</div>
        </div>
    </div>

    <!-- Attendance Table -->
    <div class="content-card">
        <div class="card-header">
            <h3><i class="fas fa-clock" style="color:var(--accent-orange);"></i> Attendance History</h3>
        </div>
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No attendance records found." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="CheckInTime" HeaderText="Check In" DataFormatString="{0:dd MMM yyyy hh:mm tt}" />
                    <asp:BoundField DataField="CheckOutTime" HeaderText="Check Out" DataFormatString="{0:dd MMM yyyy hh:mm tt}" NullDisplayText="Still In" />
                    <asp:BoundField DataField="DurationMinutes" HeaderText="Duration (Min)" NullDisplayText="-" />
                </Columns>
                <EmptyDataRowStyle CssClass="empty-state" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>

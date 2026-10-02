<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="gym_management.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Stats Cards -->
    <div class="stats-grid">
        <div class="stat-card orange">
            <div class="stat-icon"><i class="fas fa-users"></i></div>
            <div class="stat-value"><asp:Label ID="lblTotalMembers" runat="server" Text="0" /></div>
            <div class="stat-label">Total Members</div>
        </div>
        <div class="stat-card green">
            <div class="stat-icon"><i class="fas fa-user-tie"></i></div>
            <div class="stat-value"><asp:Label ID="lblTotalTrainers" runat="server" Text="0" /></div>
            <div class="stat-label">Total Trainers</div>
        </div>
        <div class="stat-card blue">
            <div class="stat-icon"><i class="fas fa-clipboard-list"></i></div>
            <div class="stat-value"><asp:Label ID="lblTotalPlans" runat="server" Text="0" /></div>
            <div class="stat-label">Membership Plans</div>
        </div>
        <div class="stat-card purple">
            <div class="stat-icon"><i class="fas fa-indian-rupee-sign"></i></div>
            <div class="stat-value">&#8377;<asp:Label ID="lblMonthlyRevenue" runat="server" Text="0" /></div>
            <div class="stat-label">Monthly Revenue</div>
        </div>
        <div class="stat-card gold">
            <div class="stat-icon"><i class="fas fa-id-card"></i></div>
            <div class="stat-value"><asp:Label ID="lblActiveMemberships" runat="server" Text="0" /></div>
            <div class="stat-label">Active Memberships</div>
        </div>
        <div class="stat-card red">
            <div class="stat-icon"><i class="fas fa-calendar-check"></i></div>
            <div class="stat-value"><asp:Label ID="lblTodayAttendance" runat="server" Text="0" /></div>
            <div class="stat-label">Today's Attendance</div>
        </div>
    </div>

    <!-- Expiring Memberships -->
    <div class="content-card">
        <div class="card-header">
            <h3><i class="fas fa-exclamation-triangle" style="color:var(--accent-gold);"></i> Expiring Memberships (Next 7 Days)</h3>
        </div>
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvExpiring" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No memberships expiring in the next 7 days." 
                GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Member Name" />
                    <asp:BoundField DataField="PlanName" HeaderText="Plan" />
                    <asp:BoundField DataField="EndDate" HeaderText="Expiry Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="DaysRemaining" HeaderText="Days Left" />
                </Columns>
                <EmptyDataRowStyle CssClass="empty-state" />
            </asp:GridView>
        </div>
    </div>

    <!-- Quick Links -->
    <div class="stats-grid" style="grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));">
        <a href="Members.aspx" class="stat-card orange" style="cursor:pointer;">
            <div class="stat-icon"><i class="fas fa-user-plus"></i></div>
            <div class="stat-label" style="font-size:14px; font-weight:600; color:var(--text-primary);">Add New Member</div>
        </a>
        <a href="Attendance.aspx" class="stat-card green" style="cursor:pointer;">
            <div class="stat-icon"><i class="fas fa-check-circle"></i></div>
            <div class="stat-label" style="font-size:14px; font-weight:600; color:var(--text-primary);">Mark Attendance</div>
        </a>
        <a href="Payments.aspx" class="stat-card blue" style="cursor:pointer;">
            <div class="stat-icon"><i class="fas fa-credit-card"></i></div>
            <div class="stat-label" style="font-size:14px; font-weight:600; color:var(--text-primary);">Record Payment</div>
        </a>
        <a href="Reports.aspx" class="stat-card purple" style="cursor:pointer;">
            <div class="stat-icon"><i class="fas fa-chart-line"></i></div>
            <div class="stat-label" style="font-size:14px; font-weight:600; color:var(--text-primary);">View Reports</div>
        </a>
    </div>

</asp:Content>

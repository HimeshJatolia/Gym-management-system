<%@ Page Title="My Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberDashboard.aspx.cs" Inherits="gym_management.MemberDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    My Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Welcome Banner & Quick Actions -->
    <div class="content-card" style="margin-bottom:24px; background: linear-gradient(135deg, rgba(99,102,241,0.15) 0%, rgba(168,85,247,0.15) 100%); border: 1px solid rgba(99,102,241,0.3);">
        <div class="card-body" style="text-align:center; padding:35px 20px 25px 20px;">
            <div style="font-size:54px; margin-bottom:12px; color:var(--accent-purple);"><i class="fas fa-dumbbell"></i></div>
            <h2 style="color:var(--text-primary); margin-bottom:6px; font-size:26px;">Welcome back, <asp:Label ID="lblWelcomeName" runat="server" Text="" />!</h2>
            <p style="color:var(--text-secondary); font-size:15px; margin:0 0 20px 0;">Here's your personal fitness overview</p>
            
            <!-- Quick Actions -->
            <div style="display:flex; justify-content:center; gap:12px; flex-wrap:wrap;">
                <a href="MemberProfile.aspx" class="btn btn-secondary" style="padding:8px 18px; font-size:13px;">
                    <i class="fas fa-user" style="color:var(--accent-blue); margin-right:6px;"></i> My Profile
                </a>
                <a href="MemberAttendance.aspx" class="btn btn-secondary" style="padding:8px 18px; font-size:13px;">
                    <i class="fas fa-calendar-check" style="color:var(--accent-orange); margin-right:6px;"></i> My Attendance
                </a>
                <a href="MemberPayments.aspx" class="btn btn-secondary" style="padding:8px 18px; font-size:13px;">
                    <i class="fas fa-credit-card" style="color:var(--accent-purple); margin-right:6px;"></i> My Payments
                </a>
            </div>
        </div>
    </div>

    <!-- Member Stats Cards -->
    <div class="stats-grid" style="grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));">
        <div class="stat-card green">
            <div class="stat-icon"><i class="fas fa-id-card"></i></div>
            <div class="stat-value"><asp:Label ID="lblMemberPlan" runat="server" Text="No Plan" /></div>
            <div class="stat-label">Current Plan</div>
        </div>
        <div class="stat-card blue">
            <div class="stat-icon"><i class="fas fa-calendar"></i></div>
            <div class="stat-value"><asp:Label ID="lblMemberExpiry" runat="server" Text="N/A" /></div>
            <div class="stat-label">Plan Expiry</div>
        </div>
        <a href="MemberAttendance.aspx" class="stat-card orange" style="display:block; text-decoration:none; cursor:pointer;">
            <div class="stat-icon"><i class="fas fa-calendar-check"></i></div>
            <div class="stat-value"><asp:Label ID="lblMemberAttendance" runat="server" Text="0" /></div>
            <div class="stat-label">This Month Attendance <i class="fas fa-arrow-right" style="font-size:11px; margin-left:4px;"></i></div>
        </a>
        <a href="MemberPayments.aspx" class="stat-card purple" style="display:block; text-decoration:none; cursor:pointer;">
            <div class="stat-icon"><i class="fas fa-indian-rupee-sign"></i></div>
            <div class="stat-value">&#8377;<asp:Label ID="lblMemberTotalPaid" runat="server" Text="0" /></div>
            <div class="stat-label">Total Paid <i class="fas fa-arrow-right" style="font-size:11px; margin-left:4px;"></i></div>
        </a>
    </div>

    <!-- Membership Status & Trainer Info -->
    <div class="stats-grid" style="grid-template-columns: 1fr 1fr; margin-top:24px;">
        
        <!-- Membership Details Card -->
        <div class="content-card">
            <div class="card-header">
                <h3><i class="fas fa-id-badge" style="color:var(--accent-green);"></i> Membership Details</h3>
            </div>
            <div class="card-body">
                <div style="display:flex; flex-direction:column; gap:16px;">
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Status</span>
                        <asp:Label ID="lblMemberStatus" runat="server" Text="Inactive" 
                            style="padding:4px 12px; border-radius:20px; font-size:12px; font-weight:600;" />
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Plan Name</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblPlanName2" runat="server" Text="N/A" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Start Date</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblStartDate" runat="server" Text="N/A" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">End Date</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblEndDate" runat="server" Text="N/A" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Days Remaining</span>
                        <span style="color:var(--accent-green); font-weight:700; font-size:18px;"><asp:Label ID="lblDaysLeft" runat="server" Text="0" /></span>
                    </div>
                </div>
            </div>
        </div>

        <!-- Trainer & Profile Card -->
        <div class="content-card">
            <div class="card-header" style="display:flex; justify-content:space-between; align-items:center;">
                <h3><i class="fas fa-user" style="color:var(--accent-blue);"></i> My Profile</h3>
                <a href="MemberProfile.aspx" class="btn btn-secondary" style="padding:5px 12px; font-size:12px;">
                    <i class="fas fa-external-link-alt"></i> View Full
                </a>
            </div>
            <div class="card-body">
                <div style="display:flex; flex-direction:column; gap:16px;">
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Full Name</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblFullName" runat="server" Text="" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Email</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblEmail" runat="server" Text="" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Phone</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblPhone" runat="server" Text="" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Trainer</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblTrainer" runat="server" Text="Not Assigned" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Join Date</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblJoinDate" runat="server" Text="" /></span>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Recent Attendance -->
    <div class="content-card" style="margin-top:24px;">
        <div class="card-header" style="display:flex; justify-content:space-between; align-items:center;">
            <h3><i class="fas fa-clock" style="color:var(--accent-orange);"></i> Recent Attendance</h3>
            <a href="MemberAttendance.aspx" class="btn btn-secondary" style="padding:5px 12px; font-size:12px;">
                <i class="fas fa-arrow-right"></i> View Full History
            </a>
        </div>
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvMemberAttendance" runat="server" AutoGenerateColumns="false" CssClass="data-table"
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

    <!-- Recent Payments -->
    <div class="content-card" style="margin-top:24px;">
        <div class="card-header" style="display:flex; justify-content:space-between; align-items:center;">
            <h3><i class="fas fa-credit-card" style="color:var(--accent-purple);"></i> Recent Payments</h3>
            <a href="MemberPayments.aspx" class="btn btn-secondary" style="padding:5px 12px; font-size:12px;">
                <i class="fas fa-arrow-right"></i> View All Payments
            </a>
        </div>
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvMemberPayments" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No payment records found." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="PaymentDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount (₹)" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="PaymentMethod" HeaderText="Method" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                </Columns>
                <EmptyDataRowStyle CssClass="empty-state" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>

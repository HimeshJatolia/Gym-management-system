<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberProfile.aspx.cs" Inherits="gym_management.MemberProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    My Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Profile Header -->
    <div class="content-card" style="margin-bottom:24px;">
        <div class="card-body" style="display:flex; align-items:center; gap:24px; padding:30px;">
            <div style="width:80px; height:80px; border-radius:50%; background:linear-gradient(135deg, var(--accent-purple), var(--accent-blue)); display:flex; align-items:center; justify-content:center; font-size:32px; font-weight:700; color:#fff;">
                <asp:Label ID="lblInitial" runat="server" Text="H" />
            </div>
            <div>
                <h2 style="color:var(--text-primary); margin:0 0 4px 0;"><asp:Label ID="lblFullName" runat="server" Text="" /></h2>
                <span style="padding:4px 14px; border-radius:20px; font-size:12px; font-weight:600; background:rgba(16,185,129,0.2); color:#10b981;">Member</span>
            </div>
        </div>
    </div>

    <!-- Profile Details -->
    <div class="stats-grid" style="grid-template-columns: 1fr 1fr;">

        <!-- Personal Info -->
        <div class="content-card">
            <div class="card-header">
                <h3><i class="fas fa-user" style="color:var(--accent-blue);"></i> Personal Information</h3>
            </div>
            <div class="card-body">
                <div style="display:flex; flex-direction:column; gap:16px;">
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Full Name</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblName" runat="server" Text="" /></span>
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
                        <span style="color:var(--text-secondary);">Gender</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblGender" runat="server" Text="" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Date of Birth</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblDOB" runat="server" Text="" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Address</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblAddress" runat="server" Text="" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Emergency Contact</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblEmergency" runat="server" Text="" /></span>
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Join Date</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblJoinDate" runat="server" Text="" /></span>
                    </div>
                </div>
            </div>
        </div>

        <!-- Membership Info -->
        <div class="content-card">
            <div class="card-header">
                <h3><i class="fas fa-id-badge" style="color:var(--accent-green);"></i> Membership Details</h3>
            </div>
            <div class="card-body">
                <div style="display:flex; flex-direction:column; gap:16px;">
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Status</span>
                        <asp:Label ID="lblStatus" runat="server" Text="Inactive" 
                            style="padding:4px 12px; border-radius:20px; font-size:12px; font-weight:600;" />
                    </div>
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Plan Name</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblPlanName" runat="server" Text="N/A" /></span>
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
                    <div style="display:flex; justify-content:space-between; padding:12px; background:rgba(255,255,255,0.03); border-radius:8px;">
                        <span style="color:var(--text-secondary);">Trainer</span>
                        <span style="color:var(--text-primary); font-weight:500;"><asp:Label ID="lblTrainer" runat="server" Text="Not Assigned" /></span>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

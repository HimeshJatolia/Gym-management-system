<%@ Page Title="My Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberPayments.aspx.cs" Inherits="gym_management.MemberPayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    My Payments
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Stats -->
    <div class="stats-grid" style="grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); margin-bottom:24px;">
        <div class="stat-card purple">
            <div class="stat-icon"><i class="fas fa-indian-rupee-sign"></i></div>
            <div class="stat-value">&#8377;<asp:Label ID="lblTotalPaid" runat="server" Text="0" /></div>
            <div class="stat-label">Total Paid</div>
        </div>
        <div class="stat-card green">
            <div class="stat-icon"><i class="fas fa-check-circle"></i></div>
            <div class="stat-value"><asp:Label ID="lblCompleted" runat="server" Text="0" /></div>
            <div class="stat-label">Completed Payments</div>
        </div>
        <div class="stat-card orange">
            <div class="stat-icon"><i class="fas fa-clock"></i></div>
            <div class="stat-value"><asp:Label ID="lblPending" runat="server" Text="0" /></div>
            <div class="stat-label">Pending Payments</div>
        </div>
    </div>

    <!-- Payments Table -->
    <div class="content-card">
        <div class="card-header">
            <h3><i class="fas fa-credit-card" style="color:var(--accent-purple);"></i> Payment History</h3>
        </div>
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvPayments" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No payment records found." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="PaymentDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="Rs.{0:N0}" />
                    <asp:BoundField DataField="PaymentMethod" HeaderText="Method" />
                    <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" NullDisplayText="-" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" NullDisplayText="-" />
                </Columns>
                <EmptyDataRowStyle CssClass="empty-state" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>

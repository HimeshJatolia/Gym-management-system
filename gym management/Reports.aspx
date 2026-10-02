<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="gym_management.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Reports
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Report Type Selector -->
    <div class="report-filters">
        <div class="form-group">
            <label>Report Type</label>
            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                <asp:ListItem Text="Attendance Report" Value="Attendance" />
                <asp:ListItem Text="Payment Report" Value="Payment" />
                <asp:ListItem Text="Member Report" Value="Member" />
                <asp:ListItem Text="Revenue Summary" Value="Revenue" />
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label>From Date</label>
            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>To Date</label>
            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
        </div>
    </div>

    <!-- Report Summary -->
    <asp:Panel ID="pnlSummary" runat="server" Visible="false">
        <div class="report-summary">
            <div class="report-summary-item">
                <div class="summary-value"><asp:Label ID="lblSummary1Value" runat="server" Text="0" /></div>
                <div class="summary-label"><asp:Label ID="lblSummary1Label" runat="server" Text="Total Records" /></div>
            </div>
            <div class="report-summary-item">
                <div class="summary-value"><asp:Label ID="lblSummary2Value" runat="server" Text="0" /></div>
                <div class="summary-label"><asp:Label ID="lblSummary2Label" runat="server" Text="Total Amount" /></div>
            </div>
            <div class="report-summary-item">
                <div class="summary-value"><asp:Label ID="lblSummary3Value" runat="server" Text="0" /></div>
                <div class="summary-label"><asp:Label ID="lblSummary3Label" runat="server" Text="Average" /></div>
            </div>
        </div>
    </asp:Panel>

    <!-- Report Data -->
    <div class="content-card">
        <div class="card-header">
            <h3><asp:Label ID="lblReportTitle" runat="server" Text="Report Results" /></h3>
        </div>
        <div class="card-body" style="padding:0;">
            <!-- Attendance Report Grid -->
            <asp:GridView ID="gvAttendanceReport" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No data found for the selected period." GridLines="None" ShowHeaderWhenEmpty="true" Visible="false">
                <Columns>
                    <asp:BoundField DataField="AttendanceID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Member" />
                    <asp:BoundField DataField="CheckInTime" HeaderText="Check In" DataFormatString="{0:dd MMM yyyy hh:mm tt}" />
                    <asp:BoundField DataField="CheckOutTime" HeaderText="Check Out" DataFormatString="{0:dd MMM yyyy hh:mm tt}" />
                    <asp:BoundField DataField="DurationMinutes" HeaderText="Duration (Min)" />
                </Columns>
            </asp:GridView>

            <!-- Payment Report Grid -->
            <asp:GridView ID="gvPaymentReport" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No data found for the selected period." GridLines="None" ShowHeaderWhenEmpty="true" Visible="false">
                <Columns>
                    <asp:BoundField DataField="PaymentID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Member" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="PaymentDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="PaymentMethod" HeaderText="Method" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                </Columns>
            </asp:GridView>

            <!-- Member Report Grid -->
            <asp:GridView ID="gvMemberReport" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No data found." GridLines="None" ShowHeaderWhenEmpty="true" Visible="false">
                <Columns>
                    <asp:BoundField DataField="MemberID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                    <asp:BoundField DataField="JoinDate" HeaderText="Join Date" DataFormatString="{0:dd MMM yyyy}" />
                </Columns>
            </asp:GridView>

            <!-- Revenue Report Grid -->
            <asp:GridView ID="gvRevenueReport" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No revenue data found." GridLines="None" ShowHeaderWhenEmpty="true" Visible="false">
                <Columns>
                    <asp:BoundField DataField="MonthName" HeaderText="Month" />
                    <asp:BoundField DataField="Revenue" HeaderText="Revenue" DataFormatString="{0:N2}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>

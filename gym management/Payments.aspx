<%@ Page Title="Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="gym_management.Payments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Payment Management
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

    <!-- Add Payment Form -->
    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="form-panel">
        <h3><asp:Label ID="lblFormTitle" runat="server" Text="Record Payment" /></h3>
        <asp:HiddenField ID="hfPaymentID" runat="server" Value="0" />
        <div class="form-grid">
            <div class="form-group">
                <label>Member *</label>
                <asp:DropDownList ID="ddlMember" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Membership</label>
                <asp:DropDownList ID="ddlMembership" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Amount (&#8377;) *</label>
                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="Enter amount"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Payment Method *</label>
                <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Cash" Value="Cash" />
                    <asp:ListItem Text="Card" Value="Card" />
                    <asp:ListItem Text="UPI" Value="UPI" />
                    <asp:ListItem Text="Online" Value="Online" />
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Transaction ID</label>
                <asp:TextBox ID="txtTransactionID" runat="server" CssClass="form-control" placeholder="Transaction reference"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Status</label>
                <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Completed" Value="Completed" />
                    <asp:ListItem Text="Pending" Value="Pending" />
                    <asp:ListItem Text="Failed" Value="Failed" />
                    <asp:ListItem Text="Refunded" Value="Refunded" />
                </asp:DropDownList>
            </div>
            <div class="form-group" style="grid-column: 1 / -1;">
                <label>Notes</label>
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Payment notes"></asp:TextBox>
            </div>
        </div>
        <div class="form-actions">
            <asp:Button ID="btnSave" runat="server" Text="Save Payment" CssClass="btn btn-success" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
    </asp:Panel>

    <!-- Toolbar -->
    <div class="toolbar">
        <div class="toolbar-left">
            <asp:Button ID="btnAdd" runat="server" Text="+ Record Payment" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
        </div>
    </div>

    <!-- Payments Grid -->
    <div class="content-card">
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvPayments" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                EmptyDataText="No payments found." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="PaymentID" HeaderText="ID" />
                    <asp:BoundField DataField="MemberName" HeaderText="Member" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="PaymentDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy hh:mm tt}" />
                    <asp:BoundField DataField="PaymentMethod" HeaderText="Method" />
                    <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# Eval("Status").ToString() == "Completed" ? "badge badge-completed" : Eval("Status").ToString() == "Pending" ? "badge badge-pending" : "badge badge-failed" %>'>
                                <%# Eval("Status") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>

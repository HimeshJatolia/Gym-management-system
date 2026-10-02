<%@ Page Title="Memberships" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Memberships.aspx.cs" Inherits="gym_management.Memberships" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Member Memberships
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

    <!-- Assign Membership Form -->
    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="form-panel">
        <h3><asp:Label ID="lblFormTitle" runat="server" Text="Assign Membership" /></h3>
        <asp:HiddenField ID="hfMembershipID" runat="server" Value="0" />
        <div class="form-grid">
            <div class="form-group">
                <label>Member *</label>
                <asp:DropDownList ID="ddlMember" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Plan *</label>
                <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Trainer (Optional)</label>
                <asp:DropDownList ID="ddlTrainer" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Start Date *</label>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>End Date *</label>
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Status</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Active" Value="Active" />
                    <asp:ListItem Text="Expired" Value="Expired" />
                    <asp:ListItem Text="Cancelled" Value="Cancelled" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-actions">
            <asp:Button ID="btnSave" runat="server" Text="Save Membership" CssClass="btn btn-success" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
    </asp:Panel>

    <!-- Toolbar -->
    <div class="toolbar">
        <div class="toolbar-left">
            <asp:Button ID="btnAdd" runat="server" Text="+ Assign Membership" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
        </div>
    </div>

    <!-- Memberships Grid -->
    <div class="content-card">
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvMemberships" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                DataKeyNames="MembershipID" OnRowCommand="gvMemberships_RowCommand"
                EmptyDataText="No memberships found." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="MembershipID" HeaderText="ID" />
                    <asp:BoundField DataField="MemberName" HeaderText="Member" />
                    <asp:BoundField DataField="PlanName" HeaderText="Plan" />
                    <asp:BoundField DataField="TrainerName" HeaderText="Trainer" />
                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# Eval("Status").ToString() == "Active" ? "badge badge-active" : Eval("Status").ToString() == "Expired" ? "badge badge-expired" : "badge badge-cancelled" %>'>
                                <%# Eval("Status") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditMembership" CommandArgument='<%# Eval("MembershipID") %>'
                                CssClass="btn-action btn-edit"><i class="fas fa-edit"></i> Edit</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>

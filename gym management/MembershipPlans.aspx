<%@ Page Title="Membership Plans" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MembershipPlans.aspx.cs" Inherits="gym_management.MembershipPlans" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Membership Plans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

    <!-- Add/Edit Form -->
    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="form-panel">
        <h3><asp:Label ID="lblFormTitle" runat="server" Text="Add New Plan" /></h3>
        <asp:HiddenField ID="hfPlanID" runat="server" Value="0" />
        <div class="form-grid">
            <div class="form-group">
                <label>Plan Name *</label>
                <asp:TextBox ID="txtPlanName" runat="server" CssClass="form-control" placeholder="e.g. Basic Monthly"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Duration (Months) *</label>
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" TextMode="Number" placeholder="Number of months"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Price (&#8377;) *</label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Plan price"></asp:TextBox>
            </div>
            <div class="form-group" style="grid-column: 1 / -1;">
                <label>Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Plan description"></asp:TextBox>
            </div>
        </div>
        <div class="form-actions">
            <asp:Button ID="btnSave" runat="server" Text="Save Plan" CssClass="btn btn-success" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
    </asp:Panel>

    <!-- Toolbar -->
    <div class="toolbar">
        <div class="toolbar-left">
            <asp:Button ID="btnAdd" runat="server" Text="+ Add Plan" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
        </div>
    </div>

    <!-- Plans Grid -->
    <div class="content-card">
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvPlans" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                DataKeyNames="PlanID" OnRowCommand="gvPlans_RowCommand"
                EmptyDataText="No plans found." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="PlanID" HeaderText="ID" />
                    <asp:BoundField DataField="PlanName" HeaderText="Plan Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="DurationMonths" HeaderText="Duration (Months)" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:N2}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge badge-active" : "badge badge-expired" %>'>
                                <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditPlan" CommandArgument='<%# Eval("PlanID") %>'
                                CssClass="btn-action btn-edit"><i class="fas fa-edit"></i> Edit</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeletePlan" CommandArgument='<%# Eval("PlanID") %>'
                                CssClass="btn-action btn-delete" OnClientClick="return confirm('Are you sure?');"><i class="fas fa-trash"></i> Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>

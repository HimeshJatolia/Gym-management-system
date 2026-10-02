<%@ Page Title="Trainers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Trainers.aspx.cs" Inherits="gym_management.Trainers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Trainer Management
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

    <!-- Add/Edit Form -->
    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="form-panel">
        <h3><asp:Label ID="lblFormTitle" runat="server" Text="Add New Trainer" /></h3>
        <asp:HiddenField ID="hfTrainerID" runat="server" Value="0" />
        <div class="form-grid">
            <div class="form-group">
                <label>Full Name *</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Enter full name"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter email"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Phone</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Enter phone"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Gender</label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-- Select --" Value="" />
                    <asp:ListItem Text="Male" Value="Male" />
                    <asp:ListItem Text="Female" Value="Female" />
                    <asp:ListItem Text="Other" Value="Other" />
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Specialization</label>
                <asp:TextBox ID="txtSpecialization" runat="server" CssClass="form-control" placeholder="e.g. Weight Training, Yoga"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Experience (Years)</label>
                <asp:TextBox ID="txtExperience" runat="server" CssClass="form-control" TextMode="Number" placeholder="Years of experience"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Salary (&#8377;)</label>
                <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" placeholder="Monthly salary"></asp:TextBox>
            </div>
        </div>
        <div class="form-actions">
            <asp:Button ID="btnSave" runat="server" Text="Save Trainer" CssClass="btn btn-success" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
    </asp:Panel>

    <!-- Toolbar -->
    <div class="toolbar">
        <div class="toolbar-left">
            <asp:Button ID="btnAdd" runat="server" Text="+ Add Trainer" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
        </div>
    </div>

    <!-- Trainers Grid -->
    <div class="content-card">
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvTrainers" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                DataKeyNames="TrainerID" OnRowCommand="gvTrainers_RowCommand"
                EmptyDataText="No trainers found." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="TrainerID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                    <asp:BoundField DataField="Specialization" HeaderText="Specialization" />
                    <asp:BoundField DataField="Experience" HeaderText="Exp (Yrs)" />
                    <asp:BoundField DataField="Salary" HeaderText="Salary" DataFormatString="{0:N0}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge badge-active" : "badge badge-expired" %>'>
                                <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditTrainer" CommandArgument='<%# Eval("TrainerID") %>'
                                CssClass="btn-action btn-edit"><i class="fas fa-edit"></i> Edit</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteTrainer" CommandArgument='<%# Eval("TrainerID") %>'
                                CssClass="btn-action btn-delete" OnClientClick="return confirm('Are you sure?');"><i class="fas fa-trash"></i> Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>

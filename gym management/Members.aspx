<%@ Page Title="Members" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="gym_management.Members" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Member Management
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

    <!-- Add/Edit Form Panel -->
    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="form-panel">
        <h3><asp:Label ID="lblFormTitle" runat="server" Text="Add New Member" /></h3>
        <asp:HiddenField ID="hfMemberID" runat="server" Value="0" />
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
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Enter phone number"></asp:TextBox>
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
                <label>Date of Birth</label>
                <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Emergency Contact</label>
                <asp:TextBox ID="txtEmergencyContact" runat="server" CssClass="form-control" placeholder="Emergency contact number"></asp:TextBox>
            </div>
            <div class="form-group" style="grid-column: 1 / -1;">
                <label>Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Enter address"></asp:TextBox>
            </div>
        </div>
        <div class="form-actions">
            <asp:Button ID="btnSave" runat="server" Text="Save Member" CssClass="btn btn-success" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
    </asp:Panel>

    <!-- Toolbar -->
    <div class="toolbar">
        <div class="toolbar-left">
            <asp:Button ID="btnAdd" runat="server" Text="+ Add Member" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
        </div>
        <div class="toolbar-right">
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search members..." 
                    AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" style="padding-left:40px;"></asp:TextBox>
            </div>
        </div>
    </div>

    <!-- Members Grid -->
    <div class="content-card">
        <div class="card-body" style="padding:0;">
            <asp:GridView ID="gvMembers" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                DataKeyNames="MemberID" OnRowCommand="gvMembers_RowCommand"
                EmptyDataText="No members found." GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="MemberID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                    <asp:BoundField DataField="JoinDate" HeaderText="Join Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge badge-active" : "badge badge-expired" %>'>
                                <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditMember" CommandArgument='<%# Eval("MemberID") %>'
                                CssClass="btn-action btn-edit"><i class="fas fa-edit"></i> Edit</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteMember" CommandArgument='<%# Eval("MemberID") %>'
                                CssClass="btn-action btn-delete" OnClientClick="return confirm('Are you sure you want to delete this member?');"><i class="fas fa-trash"></i> Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>

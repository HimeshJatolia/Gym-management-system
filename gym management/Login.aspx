<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="gym_management.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login - GYM Management System</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link rel="stylesheet" href="Site.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-wrapper">
            <div class="login-box">
                <div class="login-logo">
                    <div class="logo-icon"><i class="fas fa-dumbbell"></i></div>
                    <h1>FitPro GYM</h1>
                    <p>Management System Login</p>
                </div>

                <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>

                <div class="form-group">
                    <label for="txtUsername">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password"></asp:TextBox>
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary" OnClick="btnLogin_Click" />

                <div style="text-align:center; margin-top:20px;">
                    <span style="color:#6c6c80; font-size:12px;">Default: admin / admin123</span>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

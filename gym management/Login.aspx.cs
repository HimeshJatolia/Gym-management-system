using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace gym_management
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                string role = Session["Role"] != null ? Session["Role"].ToString() : "Admin";
                string redirectUrl = role == "Member" ? "MemberDashboard.aspx" : "Dashboard.aspx";
                Response.Redirect(redirectUrl, false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter both username and password.");
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ValidateLogin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Session["UserID"] = reader["UserID"].ToString();
                            Session["Username"] = reader["Username"].ToString();
                            Session["Role"] = reader["Role"].ToString();
                            Session["FullName"] = reader["FullName"].ToString();
                            Session["MemberID"] = reader["MemberID"] != DBNull.Value ? reader["MemberID"].ToString() : "0";
                            reader.Close();

                            string role = Session["Role"].ToString();
                            string redirectUrl = role == "Member" ? "MemberDashboard.aspx" : "Dashboard.aspx";
                            Response.Redirect(redirectUrl, false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            reader.Close();
                            ShowError("Invalid username or password. Please try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError("Database error: " + ex.Message);
                    }
                }
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
            lblError.CssClass = "error-message show";
        }
    }
}

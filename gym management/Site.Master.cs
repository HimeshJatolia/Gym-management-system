using System;
using System.Web;
using System.Web.UI;

namespace gym_management
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblUserName.Text = Session["FullName"] != null ? Session["FullName"].ToString() : "Admin";
                lblUserRole.Text = Session["Role"] != null ? Session["Role"].ToString() : "Administrator";
                lblUserInitial.Text = lblUserName.Text.Substring(0, 1).ToUpper();

                // Role-based navigation
                string role = Session["Role"] != null ? Session["Role"].ToString() : "Admin";
                if (role == "Member")
                {
                    pnlAdminDashLink.Visible = false;
                    pnlMemberDashLink.Visible = true;
                    pnlAdminNav.Visible = false;
                    pnlMemberNav.Visible = true;
                }
                else
                {
                    pnlAdminDashLink.Visible = true;
                    pnlMemberDashLink.Visible = false;
                    pnlAdminNav.Visible = true;
                    pnlMemberNav.Visible = false;
                }
            }

            // Block members from accessing admin-only pages
            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();
            string role2 = Session["Role"] != null ? Session["Role"].ToString() : "";
            if (role2 == "Member")
            {
                string[] adminPages = { "dashboard.aspx", "members.aspx", "trainers.aspx", "membershipplans.aspx", 
                                         "memberships.aspx", "payments.aspx", "attendance.aspx", "reports.aspx" };
                foreach (string page in adminPages)
                {
                    if (currentPage == page)
                    {
                        Response.Redirect("MemberDashboard.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        return;
                    }
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}

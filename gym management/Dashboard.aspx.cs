using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace gym_management
{
    public partial class Dashboard : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardStats();
                LoadExpiringMemberships();
            }
        }

        private void LoadDashboardStats()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAdminDashboardStats", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblTotalMembers.Text = reader["TotalMembers"].ToString();
                            lblTotalTrainers.Text = reader["TotalTrainers"].ToString();
                            lblTotalPlans.Text = reader["TotalPlans"].ToString();
                            lblMonthlyRevenue.Text = Convert.ToDecimal(reader["MonthlyRevenue"]).ToString("N0");
                            lblActiveMemberships.Text = reader["ActiveMemberships"].ToString();
                            lblTodayAttendance.Text = reader["TodayAttendance"].ToString();
                        }
                        reader.Close();
                    }
                    catch (Exception)
                    {
                        // If stored procedure doesn't exist, show default values
                    }
                }
            }
        }

        private void LoadExpiringMemberships()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetExpiringMemberships", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvExpiring.DataSource = dt;
                        gvExpiring.DataBind();
                    }
                    catch (Exception)
                    {
                        // If stored procedure doesn't exist
                    }
                }
            }
        }
    }
}

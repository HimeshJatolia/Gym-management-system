using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace gym_management
{
    public partial class MemberDashboard : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only members can access this page
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            string role = Session["Role"] != null ? Session["Role"].ToString() : "";
            if (role != "Member")
            {
                Response.Redirect("Dashboard.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadMemberData();
            }
        }

        private void LoadMemberData()
        {
            string memberID = Session["MemberID"] != null ? Session["MemberID"].ToString() : "0";
            string fullName = Session["FullName"] != null ? Session["FullName"].ToString() : "Member";

            lblWelcomeName.Text = fullName;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Load member profile details
                try
                {
                    string profileQuery = @"SELECT FullName, Email, Phone, JoinDate 
                                            FROM Members WHERE MemberID = @MemberID";
                    using (SqlCommand cmd = new SqlCommand(profileQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblFullName.Text = reader["FullName"].ToString();
                            lblEmail.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "Not set";
                            lblPhone.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "Not set";
                            lblJoinDate.Text = reader["JoinDate"] != DBNull.Value ? Convert.ToDateTime(reader["JoinDate"]).ToString("dd MMM yyyy") : "N/A";
                        }
                        reader.Close();
                    }
                }
                catch (Exception) { }

                // Load current membership plan details
                try
                {
                    string planQuery = @"SELECT TOP 1 mp.PlanName, mm.StartDate, mm.EndDate, mm.Status,
                                         ISNULL(t.FullName, 'Not Assigned') AS TrainerName,
                                         DATEDIFF(DAY, GETDATE(), mm.EndDate) AS DaysLeft
                                         FROM MemberMemberships mm 
                                         INNER JOIN MembershipPlans mp ON mm.PlanID = mp.PlanID 
                                         LEFT JOIN Trainers t ON mm.TrainerID = t.TrainerID
                                         WHERE mm.MemberID = @MemberID AND mm.Status = 'Active' 
                                         ORDER BY mm.EndDate DESC";
                    using (SqlCommand cmd = new SqlCommand(planQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            string planName = reader["PlanName"].ToString();
                            lblMemberPlan.Text = planName;
                            lblPlanName2.Text = planName;
                            lblMemberExpiry.Text = Convert.ToDateTime(reader["EndDate"]).ToString("dd MMM");
                            lblStartDate.Text = Convert.ToDateTime(reader["StartDate"]).ToString("dd MMM yyyy");
                            lblEndDate.Text = Convert.ToDateTime(reader["EndDate"]).ToString("dd MMM yyyy");
                            lblTrainer.Text = reader["TrainerName"].ToString();

                            int daysLeft = Convert.ToInt32(reader["DaysLeft"]);
                            lblDaysLeft.Text = daysLeft > 0 ? daysLeft.ToString() : "Expired";

                            string status = reader["Status"].ToString();
                            lblMemberStatus.Text = status;
                            if (status == "Active")
                            {
                                lblMemberStatus.Style["background"] = "rgba(16,185,129,0.2)";
                                lblMemberStatus.Style["color"] = "#10b981";
                            }
                            else
                            {
                                lblMemberStatus.Style["background"] = "rgba(239,68,68,0.2)";
                                lblMemberStatus.Style["color"] = "#ef4444";
                            }
                        }
                        else
                        {
                            lblMemberStatus.Text = "No Active Plan";
                            lblMemberStatus.Style["background"] = "rgba(239,68,68,0.2)";
                            lblMemberStatus.Style["color"] = "#ef4444";
                        }
                        reader.Close();
                    }
                }
                catch (Exception) { }

                // Get this month's attendance count
                try
                {
                    string attQuery = @"SELECT COUNT(*) FROM Attendance 
                                        WHERE MemberID = @MemberID 
                                        AND MONTH(CheckInTime) = MONTH(GETDATE()) 
                                        AND YEAR(CheckInTime) = YEAR(GETDATE())";
                    using (SqlCommand cmd = new SqlCommand(attQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        lblMemberAttendance.Text = cmd.ExecuteScalar().ToString();
                    }
                }
                catch (Exception) { }

                // Get total payments
                try
                {
                    string payQuery = @"SELECT ISNULL(SUM(Amount), 0) FROM Payments 
                                        WHERE MemberID = @MemberID AND Status = 'Completed'";
                    using (SqlCommand cmd = new SqlCommand(payQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        lblMemberTotalPaid.Text = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("N0");
                    }
                }
                catch (Exception) { }

                // Get recent attendance (last 10)
                try
                {
                    string recentAttQuery = @"SELECT TOP 10 CheckInTime, CheckOutTime,
                                              CASE WHEN CheckOutTime IS NOT NULL 
                                                   THEN DATEDIFF(MINUTE, CheckInTime, CheckOutTime) 
                                                   ELSE NULL END AS DurationMinutes
                                              FROM Attendance WHERE MemberID = @MemberID 
                                              ORDER BY CheckInTime DESC";
                    using (SqlCommand cmd = new SqlCommand(recentAttQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvMemberAttendance.DataSource = dt;
                        gvMemberAttendance.DataBind();
                    }
                }
                catch (Exception) { }

                // Get recent payments (last 10)
                try
                {
                    string recentPayQuery = @"SELECT TOP 10 PaymentDate, Amount, PaymentMethod, Status
                                              FROM Payments WHERE MemberID = @MemberID 
                                              ORDER BY PaymentDate DESC";
                    using (SqlCommand cmd = new SqlCommand(recentPayQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvMemberPayments.DataSource = dt;
                        gvMemberPayments.DataBind();
                    }
                }
                catch (Exception) { }
            }
        }
    }
}

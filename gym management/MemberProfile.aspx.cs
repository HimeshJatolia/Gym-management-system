using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace gym_management
{
    public partial class MemberProfile : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            string memberID = Session["MemberID"] != null ? Session["MemberID"].ToString() : "0";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Load member details
                try
                {
                    string query = @"SELECT FullName, Email, Phone, Gender, DateOfBirth, Address, EmergencyContact, JoinDate 
                                     FROM Members WHERE MemberID = @MemberID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            string name = reader["FullName"].ToString();
                            lblFullName.Text = name;
                            lblName.Text = name;
                            lblInitial.Text = name.Substring(0, 1).ToUpper();
                            lblEmail.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "Not set";
                            lblPhone.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "Not set";
                            lblGender.Text = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : "Not set";
                            lblDOB.Text = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd MMM yyyy") : "Not set";
                            lblAddress.Text = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "Not set";
                            lblEmergency.Text = reader["EmergencyContact"] != DBNull.Value ? reader["EmergencyContact"].ToString() : "Not set";
                            lblJoinDate.Text = reader["JoinDate"] != DBNull.Value ? Convert.ToDateTime(reader["JoinDate"]).ToString("dd MMM yyyy") : "N/A";
                        }
                        reader.Close();
                    }
                }
                catch (Exception) { }

                // Load membership details
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
                            lblPlanName.Text = reader["PlanName"].ToString();
                            lblStartDate.Text = Convert.ToDateTime(reader["StartDate"]).ToString("dd MMM yyyy");
                            lblEndDate.Text = Convert.ToDateTime(reader["EndDate"]).ToString("dd MMM yyyy");
                            lblTrainer.Text = reader["TrainerName"].ToString();
                            int daysLeft = Convert.ToInt32(reader["DaysLeft"]);
                            lblDaysLeft.Text = daysLeft > 0 ? daysLeft.ToString() : "Expired";

                            string status = reader["Status"].ToString();
                            lblStatus.Text = status;
                            lblStatus.Style["background"] = "rgba(16,185,129,0.2)";
                            lblStatus.Style["color"] = "#10b981";
                        }
                        else
                        {
                            lblStatus.Text = "No Active Plan";
                            lblStatus.Style["background"] = "rgba(239,68,68,0.2)";
                            lblStatus.Style["color"] = "#ef4444";
                        }
                        reader.Close();
                    }
                }
                catch (Exception) { }
            }
        }
    }
}

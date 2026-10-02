using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gym_management
{
    public partial class Attendance : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMembers();
                LoadTodayAttendance();
            }
        }

        private void LoadMembers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT MemberID, FullName FROM Members WHERE IsActive=1 ORDER BY FullName", conn))
                {
                    conn.Open();
                    ddlMember.DataSource = cmd.ExecuteReader();
                    ddlMember.DataTextField = "FullName";
                    ddlMember.DataValueField = "MemberID";
                    ddlMember.DataBind();
                }
            }
            ddlMember.Items.Insert(0, new ListItem("-- Select Member --", "0"));
        }

        private void LoadTodayAttendance()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT a.AttendanceID, m.FullName, a.CheckInTime, a.CheckOutTime
                                 FROM Attendance a 
                                 INNER JOIN Members m ON a.MemberID = m.MemberID
                                 WHERE CAST(a.CheckInTime AS DATE) = CAST(GETDATE() AS DATE)
                                 ORDER BY a.CheckInTime DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvAttendance.DataSource = dt;
                    gvAttendance.DataBind();
                    lblTodayCount.Text = dt.Rows.Count.ToString();
                }
            }
        }

        protected void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (ddlMember.SelectedValue == "0")
            {
                ShowMessage("Please select a member.", false);
                return;
            }

            int memberID = Convert.ToInt32(ddlMember.SelectedValue);

            // Check if already checked in today without checkout
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string checkQuery = @"SELECT COUNT(*) FROM Attendance 
                                      WHERE MemberID=@MemberID AND CAST(CheckInTime AS DATE) = CAST(GETDATE() AS DATE) 
                                      AND CheckOutTime IS NULL";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@MemberID", memberID);
                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        ShowMessage("This member is already checked in. Please check out first.", false);
                        return;
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Attendance (MemberID, CheckInTime) VALUES (@MemberID, GETDATE())", conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    ShowMessage("Check-in recorded successfully!", true);
                    LoadTodayAttendance();
                }
            }
        }

        protected void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (ddlMember.SelectedValue == "0")
            {
                ShowMessage("Please select a member.", false);
                return;
            }

            int memberID = Convert.ToInt32(ddlMember.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"UPDATE Attendance SET CheckOutTime = GETDATE() 
                                 WHERE MemberID=@MemberID AND CAST(CheckInTime AS DATE) = CAST(GETDATE() AS DATE) 
                                 AND CheckOutTime IS NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberID);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        ShowMessage("Check-out recorded successfully!", true);
                    }
                    else
                    {
                        ShowMessage("No active check-in found for this member today.", false);
                    }
                    LoadTodayAttendance();
                }
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "success-message" : "error-message show";
            lblMessage.Visible = true;
        }
    }
}

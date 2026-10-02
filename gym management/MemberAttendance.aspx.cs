using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace gym_management
{
    public partial class MemberAttendance : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAttendance();
            }
        }

        private void LoadAttendance()
        {
            string memberID = Session["MemberID"] != null ? Session["MemberID"].ToString() : "0";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // This month count
                try
                {
                    using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM Attendance 
                        WHERE MemberID=@MemberID AND MONTH(CheckInTime)=MONTH(GETDATE()) AND YEAR(CheckInTime)=YEAR(GETDATE())", conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        lblThisMonth.Text = cmd.ExecuteScalar().ToString();
                    }
                }
                catch (Exception) { }

                // Last month count
                try
                {
                    using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM Attendance 
                        WHERE MemberID=@MemberID AND MONTH(CheckInTime)=MONTH(DATEADD(MONTH,-1,GETDATE())) AND YEAR(CheckInTime)=YEAR(DATEADD(MONTH,-1,GETDATE()))", conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        lblLastMonth.Text = cmd.ExecuteScalar().ToString();
                    }
                }
                catch (Exception) { }

                // Total all time
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Attendance WHERE MemberID=@MemberID", conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        lblTotalAll.Text = cmd.ExecuteScalar().ToString();
                    }
                }
                catch (Exception) { }

                // Full history
                try
                {
                    string query = @"SELECT CheckInTime, CheckOutTime,
                                     CASE WHEN CheckOutTime IS NOT NULL 
                                          THEN DATEDIFF(MINUTE, CheckInTime, CheckOutTime) 
                                          ELSE NULL END AS DurationMinutes
                                     FROM Attendance WHERE MemberID=@MemberID ORDER BY CheckInTime DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvAttendance.DataSource = dt;
                        gvAttendance.DataBind();
                    }
                }
                catch (Exception) { }
            }
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace gym_management
{
    public partial class Reports : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideAllGrids();
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFromDate.Text) || string.IsNullOrEmpty(txtToDate.Text))
            {
                return;
            }

            DateTime fromDate = DateTime.Parse(txtFromDate.Text);
            DateTime toDate = DateTime.Parse(txtToDate.Text);

            HideAllGrids();

            switch (ddlReportType.SelectedValue)
            {
                case "Attendance":
                    GenerateAttendanceReport(fromDate, toDate);
                    break;
                case "Payment":
                    GeneratePaymentReport(fromDate, toDate);
                    break;
                case "Member":
                    GenerateMemberReport(fromDate, toDate);
                    break;
                case "Revenue":
                    GenerateRevenueReport();
                    break;
            }
        }

        private void GenerateAttendanceReport(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAttendanceReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", fromDate);
                    cmd.Parameters.AddWithValue("@EndDate", toDate);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvAttendanceReport.DataSource = dt;
                    gvAttendanceReport.DataBind();
                    gvAttendanceReport.Visible = true;

                    lblReportTitle.Text = "📅 Attendance Report";
                    pnlSummary.Visible = true;
                    lblSummary1Value.Text = dt.Rows.Count.ToString();
                    lblSummary1Label.Text = "Total Check-ins";

                    // Calculate total duration
                    decimal totalMinutes = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["DurationMinutes"] != DBNull.Value)
                            totalMinutes += Convert.ToDecimal(row["DurationMinutes"]);
                    }
                    lblSummary2Value.Text = totalMinutes.ToString("N0");
                    lblSummary2Label.Text = "Total Minutes";
                    lblSummary3Value.Text = dt.Rows.Count > 0 ? (totalMinutes / dt.Rows.Count).ToString("N0") : "0";
                    lblSummary3Label.Text = "Avg Duration (Min)";
                }
            }
        }

        private void GeneratePaymentReport(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPaymentReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", fromDate);
                    cmd.Parameters.AddWithValue("@EndDate", toDate);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvPaymentReport.DataSource = dt;
                    gvPaymentReport.DataBind();
                    gvPaymentReport.Visible = true;

                    lblReportTitle.Text = "💳 Payment Report";
                    pnlSummary.Visible = true;
                    lblSummary1Value.Text = dt.Rows.Count.ToString();
                    lblSummary1Label.Text = "Total Transactions";

                    decimal totalAmount = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalAmount += Convert.ToDecimal(row["Amount"]);
                    }
                    lblSummary2Value.Text = "₹" + totalAmount.ToString("N0");
                    lblSummary2Label.Text = "Total Amount";
                    lblSummary3Value.Text = dt.Rows.Count > 0 ? "₹" + (totalAmount / dt.Rows.Count).ToString("N0") : "₹0";
                    lblSummary3Label.Text = "Avg Payment";
                }
            }
        }

        private void GenerateMemberReport(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT MemberID, FullName, Email, Phone, Gender, JoinDate 
                                 FROM Members WHERE IsActive=1 
                                 AND JoinDate BETWEEN @From AND @To ORDER BY JoinDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@From", fromDate);
                    cmd.Parameters.AddWithValue("@To", toDate);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvMemberReport.DataSource = dt;
                    gvMemberReport.DataBind();
                    gvMemberReport.Visible = true;

                    lblReportTitle.Text = "👥 Member Report";
                    pnlSummary.Visible = true;
                    lblSummary1Value.Text = dt.Rows.Count.ToString();
                    lblSummary1Label.Text = "Members Joined";

                    int maleCount = 0, femaleCount = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Gender"].ToString() == "Male") maleCount++;
                        else if (row["Gender"].ToString() == "Female") femaleCount++;
                    }
                    lblSummary2Value.Text = maleCount.ToString();
                    lblSummary2Label.Text = "Male Members";
                    lblSummary3Value.Text = femaleCount.ToString();
                    lblSummary3Label.Text = "Female Members";
                }
            }
        }

        private void GenerateRevenueReport()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetMonthlyRevenue", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvRevenueReport.DataSource = dt;
                    gvRevenueReport.DataBind();
                    gvRevenueReport.Visible = true;

                    lblReportTitle.Text = "📈 Monthly Revenue Summary (" + DateTime.Now.Year + ")";
                    pnlSummary.Visible = true;
                    lblSummary1Value.Text = dt.Rows.Count.ToString();
                    lblSummary1Label.Text = "Months with Revenue";

                    decimal total = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        total += Convert.ToDecimal(row["Revenue"]);
                    }
                    lblSummary2Value.Text = "₹" + total.ToString("N0");
                    lblSummary2Label.Text = "Total Revenue (YTD)";
                    lblSummary3Value.Text = dt.Rows.Count > 0 ? "₹" + (total / dt.Rows.Count).ToString("N0") : "₹0";
                    lblSummary3Label.Text = "Avg Monthly Revenue";
                }
            }
        }

        private void HideAllGrids()
        {
            gvAttendanceReport.Visible = false;
            gvPaymentReport.Visible = false;
            gvMemberReport.Visible = false;
            gvRevenueReport.Visible = false;
            pnlSummary.Visible = false;
        }
    }
}

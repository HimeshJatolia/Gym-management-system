using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace gym_management
{
    public partial class MemberPayments : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPayments();
            }
        }

        private void LoadPayments()
        {
            string memberID = Session["MemberID"] != null ? Session["MemberID"].ToString() : "0";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Total Paid
                try
                {
                    using (SqlCommand cmd = new SqlCommand(@"SELECT ISNULL(SUM(Amount), 0) FROM Payments 
                        WHERE MemberID=@MemberID AND Status='Completed'", conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        object result = cmd.ExecuteScalar();
                        decimal total = result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                        lblTotalPaid.Text = total.ToString("N0");
                    }
                }
                catch (Exception) { }

                // Completed Count
                try
                {
                    using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM Payments 
                        WHERE MemberID=@MemberID AND Status='Completed'", conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        lblCompleted.Text = cmd.ExecuteScalar().ToString();
                    }
                }
                catch (Exception) { }

                // Pending Count
                try
                {
                    using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM Payments 
                        WHERE MemberID=@MemberID AND Status='Pending'", conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        lblPending.Text = cmd.ExecuteScalar().ToString();
                    }
                }
                catch (Exception) { }

                // Payments History Table
                try
                {
                    string query = @"SELECT PaymentDate, Amount, PaymentMethod, TransactionID, Status, Notes
                                     FROM Payments WHERE MemberID=@MemberID ORDER BY PaymentDate DESC, PaymentID DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvPayments.DataSource = dt;
                        gvPayments.DataBind();
                    }
                }
                catch (Exception) { }
            }
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gym_management
{
    public partial class Payments : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPayments();
                LoadDropdowns();
            }
        }

        private void LoadDropdowns()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                DataTable dt;

                // Members
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT MemberID, FullName FROM Members WHERE IsActive=1 ORDER BY FullName", conn))
                {
                    dt = new DataTable();
                    da.Fill(dt);
                    ddlMember.DataSource = dt;
                    ddlMember.DataTextField = "FullName";
                    ddlMember.DataValueField = "MemberID";
                    ddlMember.DataBind();
                }
                ddlMember.Items.Insert(0, new ListItem("-- Select Member --", "0"));

                // Memberships
                using (SqlDataAdapter da = new SqlDataAdapter(@"SELECT mm.MembershipID, m.FullName + ' - ' + mp.PlanName AS Display 
                    FROM MemberMemberships mm 
                    INNER JOIN Members m ON mm.MemberID = m.MemberID 
                    INNER JOIN MembershipPlans mp ON mm.PlanID = mp.PlanID 
                    WHERE mm.Status = 'Active'", conn))
                {
                    dt = new DataTable();
                    da.Fill(dt);
                    ddlMembership.DataSource = dt;
                    ddlMembership.DataTextField = "Display";
                    ddlMembership.DataValueField = "MembershipID";
                    ddlMembership.DataBind();
                }
                ddlMembership.Items.Insert(0, new ListItem("-- Select (Optional) --", "0"));
            }
        }

        private void LoadPayments()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT p.PaymentID, m.FullName AS MemberName, p.Amount, p.PaymentDate, 
                                 p.PaymentMethod, p.TransactionID, p.Status, p.Notes
                                 FROM Payments p 
                                 INNER JOIN Members m ON p.MemberID = m.MemberID
                                 ORDER BY p.PaymentID DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvPayments.DataSource = dt;
                    gvPayments.DataBind();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblFormTitle.Text = "Record Payment";
            pnlForm.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
            ClearForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlMember.SelectedValue == "0" || string.IsNullOrEmpty(txtAmount.Text))
            {
                ShowMessage("Please select member and enter amount.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Payments (MemberID, MembershipID, Amount, PaymentMethod, TransactionID, Status, Notes) 
                                 VALUES (@MemberID, @MembershipID, @Amount, @PaymentMethod, @TransactionID, @Status, @Notes)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", Convert.ToInt32(ddlMember.SelectedValue));
                    cmd.Parameters.AddWithValue("@MembershipID", ddlMembership.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddlMembership.SelectedValue));
                    cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtAmount.Text));
                    cmd.Parameters.AddWithValue("@PaymentMethod", ddlPaymentMethod.SelectedValue);
                    cmd.Parameters.AddWithValue("@TransactionID", txtTransactionID.Text.Trim());
                    cmd.Parameters.AddWithValue("@Status", ddlPaymentStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage("Payment recorded successfully!", true);
                        pnlForm.Visible = false;
                        ClearForm();
                        LoadPayments();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, false);
                    }
                }
            }
        }

        private void ClearForm()
        {
            hfPaymentID.Value = "0";
            ddlMember.SelectedIndex = 0;
            ddlMembership.SelectedIndex = 0;
            txtAmount.Text = "";
            ddlPaymentMethod.SelectedIndex = 0;
            txtTransactionID.Text = "";
            ddlPaymentStatus.SelectedIndex = 0;
            txtNotes.Text = "";
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "success-message" : "error-message show";
            lblMessage.Visible = true;
        }
    }
}

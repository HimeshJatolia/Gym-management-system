using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gym_management
{
    public partial class Memberships : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMemberships();
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

                // Plans
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT PlanID, PlanName + ' (Rs.' + CAST(Price AS VARCHAR) + ' / ' + CAST(DurationMonths AS VARCHAR) + ' mo)' AS PlanDisplay FROM MembershipPlans WHERE IsActive=1", conn))
                {
                    dt = new DataTable();
                    da.Fill(dt);
                    ddlPlan.DataSource = dt;
                    ddlPlan.DataTextField = "PlanDisplay";
                    ddlPlan.DataValueField = "PlanID";
                    ddlPlan.DataBind();
                }
                ddlPlan.Items.Insert(0, new ListItem("-- Select Plan --", "0"));

                // Trainers
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT TrainerID, FullName + ' (' + ISNULL(Specialization,'') + ')' AS TrainerDisplay FROM Trainers WHERE IsActive=1", conn))
                {
                    dt = new DataTable();
                    da.Fill(dt);
                    ddlTrainer.DataSource = dt;
                    ddlTrainer.DataTextField = "TrainerDisplay";
                    ddlTrainer.DataValueField = "TrainerID";
                    ddlTrainer.DataBind();
                }
                ddlTrainer.Items.Insert(0, new ListItem("-- No Trainer --", "0"));
            }
        }

        private void LoadMemberships()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT mm.MembershipID, m.FullName AS MemberName, mp.PlanName, 
                                 ISNULL(t.FullName, 'N/A') AS TrainerName, mm.StartDate, mm.EndDate, mm.Status
                                 FROM MemberMemberships mm
                                 INNER JOIN Members m ON mm.MemberID = m.MemberID
                                 INNER JOIN MembershipPlans mp ON mm.PlanID = mp.PlanID
                                 LEFT JOIN Trainers t ON mm.TrainerID = t.TrainerID
                                 ORDER BY mm.MembershipID DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvMemberships.DataSource = dt;
                    gvMemberships.DataBind();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblFormTitle.Text = "Assign Membership";
            hfMembershipID.Value = "0";
            txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            pnlForm.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
            ClearForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlMember.SelectedValue == "0" || ddlPlan.SelectedValue == "0" || string.IsNullOrEmpty(txtStartDate.Text) || string.IsNullOrEmpty(txtEndDate.Text))
            {
                ShowMessage("Please fill all required fields.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                int membershipID = Convert.ToInt32(hfMembershipID.Value);
                string query;

                if (membershipID == 0)
                {
                    query = @"INSERT INTO MemberMemberships (MemberID, PlanID, TrainerID, StartDate, EndDate, Status) 
                              VALUES (@MemberID, @PlanID, @TrainerID, @StartDate, @EndDate, @Status)";
                }
                else
                {
                    query = @"UPDATE MemberMemberships SET MemberID=@MemberID, PlanID=@PlanID, TrainerID=@TrainerID, 
                              StartDate=@StartDate, EndDate=@EndDate, Status=@Status WHERE MembershipID=@MembershipID";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", Convert.ToInt32(ddlMember.SelectedValue));
                    cmd.Parameters.AddWithValue("@PlanID", Convert.ToInt32(ddlPlan.SelectedValue));
                    cmd.Parameters.AddWithValue("@TrainerID", ddlTrainer.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddlTrainer.SelectedValue));
                    cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(txtStartDate.Text));
                    cmd.Parameters.AddWithValue("@EndDate", DateTime.Parse(txtEndDate.Text));
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                    if (membershipID > 0)
                        cmd.Parameters.AddWithValue("@MembershipID", membershipID);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage(membershipID == 0 ? "Membership assigned successfully!" : "Membership updated!", true);
                        pnlForm.Visible = false;
                        ClearForm();
                        LoadMemberships();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, false);
                    }
                }
            }
        }

        protected void gvMemberships_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditMembership")
            {
                int membershipID = Convert.ToInt32(e.CommandArgument);
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM MemberMemberships WHERE MembershipID=@ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", membershipID);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            hfMembershipID.Value = membershipID.ToString();
                            ddlMember.SelectedValue = reader["MemberID"].ToString();
                            ddlPlan.SelectedValue = reader["PlanID"].ToString();
                            if (reader["TrainerID"] != DBNull.Value)
                                ddlTrainer.SelectedValue = reader["TrainerID"].ToString();
                            txtStartDate.Text = Convert.ToDateTime(reader["StartDate"]).ToString("yyyy-MM-dd");
                            txtEndDate.Text = Convert.ToDateTime(reader["EndDate"]).ToString("yyyy-MM-dd");
                            ddlStatus.SelectedValue = reader["Status"].ToString();
                            lblFormTitle.Text = "Edit Membership";
                            pnlForm.Visible = true;
                        }
                        reader.Close();
                    }
                }
            }
        }

        private void ClearForm()
        {
            hfMembershipID.Value = "0";
            ddlMember.SelectedIndex = 0;
            ddlPlan.SelectedIndex = 0;
            ddlTrainer.SelectedIndex = 0;
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "success-message" : "error-message show";
            lblMessage.Visible = true;
        }
    }
}

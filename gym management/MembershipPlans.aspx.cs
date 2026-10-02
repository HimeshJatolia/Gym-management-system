using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gym_management
{
    public partial class MembershipPlans : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadPlans();
        }

        private void LoadPlans()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM MembershipPlans ORDER BY PlanID DESC", conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvPlans.DataSource = dt;
                    gvPlans.DataBind();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblFormTitle.Text = "Add New Plan";
            hfPlanID.Value = "0";
            pnlForm.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
            ClearForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlanName.Text.Trim()) || string.IsNullOrEmpty(txtDuration.Text) || string.IsNullOrEmpty(txtPrice.Text))
            {
                ShowMessage("Please fill all required fields.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                int planID = Convert.ToInt32(hfPlanID.Value);
                string query;

                if (planID == 0)
                {
                    query = @"INSERT INTO MembershipPlans (PlanName, Description, DurationMonths, Price) 
                              VALUES (@PlanName, @Description, @Duration, @Price)";
                }
                else
                {
                    query = @"UPDATE MembershipPlans SET PlanName=@PlanName, Description=@Description, 
                              DurationMonths=@Duration, Price=@Price WHERE PlanID=@PlanID";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PlanName", txtPlanName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Duration", Convert.ToInt32(txtDuration.Text));
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                    if (planID > 0)
                        cmd.Parameters.AddWithValue("@PlanID", planID);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage(planID == 0 ? "Plan added successfully!" : "Plan updated successfully!", true);
                        pnlForm.Visible = false;
                        ClearForm();
                        LoadPlans();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, false);
                    }
                }
            }
        }

        protected void gvPlans_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int planID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditPlan")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM MembershipPlans WHERE PlanID=@ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", planID);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            hfPlanID.Value = planID.ToString();
                            txtPlanName.Text = reader["PlanName"].ToString();
                            txtDescription.Text = reader["Description"].ToString();
                            txtDuration.Text = reader["DurationMonths"].ToString();
                            txtPrice.Text = reader["Price"].ToString();
                            lblFormTitle.Text = "Edit Plan";
                            pnlForm.Visible = true;
                        }
                        reader.Close();
                    }
                }
            }
            else if (e.CommandName == "DeletePlan")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE MembershipPlans SET IsActive=0 WHERE PlanID=@ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", planID);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage("Plan deactivated!", true);
                        LoadPlans();
                    }
                }
            }
        }

        private void ClearForm()
        {
            hfPlanID.Value = "0";
            txtPlanName.Text = "";
            txtDescription.Text = "";
            txtDuration.Text = "";
            txtPrice.Text = "";
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "success-message" : "error-message show";
            lblMessage.Visible = true;
        }
    }
}

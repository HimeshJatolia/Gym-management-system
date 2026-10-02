using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gym_management
{
    public partial class Trainers : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadTrainers();
        }

        private void LoadTrainers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Trainers ORDER BY TrainerID DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvTrainers.DataSource = dt;
                    gvTrainers.DataBind();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblFormTitle.Text = "Add New Trainer";
            hfTrainerID.Value = "0";
            pnlForm.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
            ClearForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFullName.Text.Trim()))
            {
                ShowMessage("Please enter trainer name.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query;
                int trainerID = Convert.ToInt32(hfTrainerID.Value);

                if (trainerID == 0)
                {
                    query = @"INSERT INTO Trainers (FullName, Email, Phone, Gender, Specialization, Experience, Salary) 
                              VALUES (@FullName, @Email, @Phone, @Gender, @Specialization, @Experience, @Salary)";
                }
                else
                {
                    query = @"UPDATE Trainers SET FullName=@FullName, Email=@Email, Phone=@Phone, Gender=@Gender, 
                              Specialization=@Specialization, Experience=@Experience, Salary=@Salary WHERE TrainerID=@TrainerID";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@Specialization", txtSpecialization.Text.Trim());
                    cmd.Parameters.AddWithValue("@Experience", string.IsNullOrEmpty(txtExperience.Text) ? 0 : Convert.ToInt32(txtExperience.Text));
                    cmd.Parameters.AddWithValue("@Salary", string.IsNullOrEmpty(txtSalary.Text) ? 0 : Convert.ToDecimal(txtSalary.Text));
                    if (trainerID > 0)
                        cmd.Parameters.AddWithValue("@TrainerID", trainerID);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage(trainerID == 0 ? "Trainer added successfully!" : "Trainer updated successfully!", true);
                        pnlForm.Visible = false;
                        ClearForm();
                        LoadTrainers();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, false);
                    }
                }
            }
        }

        protected void gvTrainers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int trainerID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditTrainer")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Trainers WHERE TrainerID=@ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", trainerID);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            hfTrainerID.Value = trainerID.ToString();
                            txtFullName.Text = reader["FullName"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtPhone.Text = reader["Phone"].ToString();
                            ddlGender.SelectedValue = reader["Gender"].ToString();
                            txtSpecialization.Text = reader["Specialization"].ToString();
                            txtExperience.Text = reader["Experience"].ToString();
                            txtSalary.Text = reader["Salary"].ToString();
                            lblFormTitle.Text = "Edit Trainer";
                            pnlForm.Visible = true;
                        }
                        reader.Close();
                    }
                }
            }
            else if (e.CommandName == "DeleteTrainer")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Trainers SET IsActive=0 WHERE TrainerID=@ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", trainerID);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage("Trainer deactivated successfully!", true);
                        LoadTrainers();
                    }
                }
            }
        }

        private void ClearForm()
        {
            hfTrainerID.Value = "0";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            ddlGender.SelectedIndex = 0;
            txtSpecialization.Text = "";
            txtExperience.Text = "";
            txtSalary.Text = "";
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "success-message" : "error-message show";
            lblMessage.Visible = true;
        }
    }
}

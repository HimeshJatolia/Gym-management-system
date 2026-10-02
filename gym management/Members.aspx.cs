using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gym_management
{
    public partial class Members : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMembers();
            }
        }

        private void LoadMembers(string search = "")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT MemberID, FullName, Email, Phone, Gender, DateOfBirth, Address, 
                                 EmergencyContact, JoinDate, IsActive FROM Members WHERE 1=1";
                if (!string.IsNullOrEmpty(search))
                {
                    query += " AND (FullName LIKE @Search OR Email LIKE @Search OR Phone LIKE @Search)";
                }
                query += " ORDER BY MemberID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(search))
                        cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvMembers.DataSource = dt;
                    gvMembers.DataBind();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblFormTitle.Text = "Add New Member";
            hfMemberID.Value = "0";
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
                ShowMessage("Please enter full name.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query;
                int memberID = Convert.ToInt32(hfMemberID.Value);

                if (memberID == 0)
                {
                    // Insert - also create a User account
                    query = @"INSERT INTO Users (Username, Password, Role, FullName) VALUES (@Username, 'member123', 'Member', @FullName);
                              DECLARE @UID INT = SCOPE_IDENTITY();
                              INSERT INTO Members (UserID, FullName, Email, Phone, Gender, DateOfBirth, Address, EmergencyContact) 
                              VALUES (@UID, @FullName, @Email, @Phone, @Gender, @DOB, @Address, @EmergencyContact)";
                }
                else
                {
                    // Update
                    query = @"UPDATE Members SET FullName=@FullName, Email=@Email, Phone=@Phone, 
                              Gender=@Gender, DateOfBirth=@DOB, Address=@Address, EmergencyContact=@EmergencyContact 
                              WHERE MemberID=@MemberID";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@DOB", string.IsNullOrEmpty(txtDOB.Text) ? (object)DBNull.Value : DateTime.Parse(txtDOB.Text));
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@EmergencyContact", txtEmergencyContact.Text.Trim());

                    if (memberID == 0)
                    {
                        // Generate username from name
                        string username = txtFullName.Text.Trim().ToLower().Replace(" ", "") + DateTime.Now.Ticks.ToString().Substring(12);
                        cmd.Parameters.AddWithValue("@Username", username);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                    }

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage(memberID == 0 ? "Member added successfully!" : "Member updated successfully!", true);
                        pnlForm.Visible = false;
                        ClearForm();
                        LoadMembers();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, false);
                    }
                }
            }
        }

        protected void gvMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int memberID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditMember")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT * FROM Members WHERE MemberID=@MemberID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            hfMemberID.Value = memberID.ToString();
                            txtFullName.Text = reader["FullName"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtPhone.Text = reader["Phone"].ToString();
                            ddlGender.SelectedValue = reader["Gender"].ToString();
                            if (reader["DateOfBirth"] != DBNull.Value)
                                txtDOB.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                            txtAddress.Text = reader["Address"].ToString();
                            txtEmergencyContact.Text = reader["EmergencyContact"].ToString();

                            lblFormTitle.Text = "Edit Member";
                            pnlForm.Visible = true;
                        }
                        reader.Close();
                    }
                }
            }
            else if (e.CommandName == "DeleteMember")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "UPDATE Members SET IsActive=0 WHERE MemberID=@MemberID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage("Member deactivated successfully!", true);
                        LoadMembers();
                    }
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadMembers(txtSearch.Text.Trim());
        }

        private void ClearForm()
        {
            hfMemberID.Value = "0";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            ddlGender.SelectedIndex = 0;
            txtDOB.Text = "";
            txtAddress.Text = "";
            txtEmergencyContact.Text = "";
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "success-message" : "error-message show";
            lblMessage.Visible = true;
        }
    }
}

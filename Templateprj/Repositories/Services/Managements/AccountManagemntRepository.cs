using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Data;
using System.Web;
using Templateprj.Helpers;
using Templateprj.Models.Managements;
using Templateprj.Repositories.Interfaces;

namespace Templateprj.Repositories.Services
{
    public class AccountManagemntRepository : IAccountManagemntRepository
    {
        public ManagementModel GetAccount()
        {
            ManagementModel model = new ManagementModel();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Get_Account_Details"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@v_Data_Out", MySqlDbType.Text).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    string data = cmd.Parameters["@v_Data_Out"].Value.ToString();
                    return JsonConvert.DeserializeObject<ManagementModel>(data);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("Repositories.Services.GetAccount :: Exception :: " + ex.Message);
            }
            return model;
        }

        public string SaveAccount(ManagementModel model, out string response)
        {
            response = "";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Update_Account_Details"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@v_Data_In", MySqlDbType.Text).Value = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Message_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    string status = cmd.Parameters["@n_Status_Out"].Value.ToString();
                    response = cmd.Parameters["@v_Message_Out"].Value.ToString();
                    return status;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("Repositories.Services.SaveAccount :: Exception :: " + ex.Message);
                return "";
            }
        }

    }
}
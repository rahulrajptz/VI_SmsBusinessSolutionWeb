using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using Templateprj.Helpers;
using Templateprj.Models;
using Templateprj.Models.Managements;
using Templateprj.Repositories.Interfaces;

namespace Templateprj.Repositories.Services
{
    public class TemplateManagemntRepository : ITemplateManagemntRepository
    {
        public TemplateModel GetTemplateFilters()
        {
            TemplateModel model = new TemplateModel();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Template_Dropdown"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@n_Account_Id", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["AccountID"].ToString());

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataset = new DataSet();
                        dataAdapter.Fill(dataset);

                        model.TemplateTypes = GetDatatables(dataset.Tables[0]).ToSelectList();
                        model.Status = GetDatatables(dataset.Tables[1]).ToSelectList();
                        model.ContentTypes = GetDatatables(dataset.Tables[2]).ToSelectList();
                        model.ApprovalStatus = GetDatatables(dataset.Tables[3]).ToSelectList();
                        model.ConsentType = GetDatatables(dataset.Tables[4]).ToSelectList();
                    }

                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("TemplateManagemntRepository.GetTemplateFilters :: Exception :: " + ex.Message);
            }
            return model;
        }

        public TemplateAutoFilItemModel TemplateAutoFilItems()
        {
            TemplateAutoFilItemModel template = new TemplateAutoFilItemModel();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Template_Name"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@n_Account_Id", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["AccountID"].ToString());

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataset = new DataSet();
                        dataAdapter.Fill(dataset);
                        template.TemaplteNames = GetDataTableToKeyValueText(dataset.Tables[0]);
                        template.TemplateIds = GetDataTableToKeyValueText(dataset.Tables[1]);
                        template.Headers = GetDataTableToKeyValueText(dataset.Tables[2]);
                    }

                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("TemplateManagemntRepository.TemplateAutoFilItems :: Exception :: " + ex.Message);
            }
            return template;
        }

        public string GetTemplates(TemplateModel model)
        {
            try
            {
                JsonSerializerSettings jsSettings = new JsonSerializerSettings();
                jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                string filter = JsonConvert.SerializeObject(model, Formatting.None, jsSettings);
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Get_Account_Template_List"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@n_Account_Id", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["AccountID"].ToString());
                    cmd.Parameters.Add("@v_Data_Out", MySqlDbType.Text).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    return cmd.Parameters["@v_Data_Out"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
           
                LogWriter.Write("Repositories.Services.GetAccount :: Exception :: " + ex.Message);
            }

            return string.Empty;
        }

        public string SaveTemplate(RegisterTemplateCommand command, out string response)
        {
            response = "";
            return response;
            List<RegisterTemplateCommand> commands = new List<RegisterTemplateCommand>() { command };
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Add_New_Template"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@v_Data_In", MySqlDbType.Text).Value = JsonConvert.SerializeObject(commands, new JsonSerializerSettings
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
                LogWriter.Write("Repositories.Services.SaveTemplate :: Exception :: " + ex.Message);
                return "";
            }
        }

        public string DeleteTemplate(string id, out string response)
        {
            response = "";
            return response;

            //try
            //{
            //    using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Update_Account_Details"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
            //        cmd.Parameters.Add("@v_Data_In", MySqlDbType.Text).Value = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            //        {
            //            ContractResolver = new CamelCasePropertyNamesContractResolver()
            //        });
            //        cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
            //        cmd.Parameters.Add("@v_Message_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

            //        DataTable dt = new DataTable();
            //        using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
            //        {
            //            con.Open();
            //            cmd.Connection = con;
            //            cmd.ExecuteNonQuery();
            //        }
            //        string status = cmd.Parameters["@n_Status_Out"].Value.ToString();
            //        response = cmd.Parameters["@v_Message_Out"].Value.ToString();
            //        return status;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogWriter.Write("Repositories.Services.SaveAccount :: Exception :: " + ex.Message);
            //    return "";
            //}
        }

        private DataTable GetDatatables(DataTable dt)
        {
            if (dt == null)
            {
                dt = new DataTable();
            }
            DataRow newRow = dt.NewRow();
            newRow[0] = 0;
            newRow[1] = "All";
            dt.Rows.InsertAt(newRow, 0);
            return dt;
        }


        private List<KeyValueModel> GetDataTableToKeyValue(DataTable dt)
        {
            List<KeyValueModel> keyValues = new List<KeyValueModel>();
            if (dt != null)
            {
                foreach(DataRow row in dt.Rows)
                {
                    keyValues.Add(new KeyValueModel() { Id= Convert.ToString(row["id"]),Value= Convert.ToString(row["value"]) });
                }
            }
            return keyValues;
        }

        private List<KeyValueModel> GetDataTableToKeyValueText(DataTable dt)
        {
            List<KeyValueModel> keyValues = new List<KeyValueModel>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    keyValues.Add(new KeyValueModel() { Id = Convert.ToString(row["value"]), Value = Convert.ToString(row["text"]) });
                }
            }
            return keyValues;
        }

    }
}
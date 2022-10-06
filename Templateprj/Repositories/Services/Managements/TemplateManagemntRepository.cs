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
using System.Web.Mvc;
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
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = 1;//HttpContext.Current.Session["UserID"].ToString();

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
                    }

                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("Repositories.Services.TemplateManagemntRepository :: Exception :: " + ex.Message);
            }
            return model;
        }

        public List<KeyValueModel> GetTemplateNames()
        {
            List<KeyValueModel> temaplteNames = new List<KeyValueModel>();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Template_Name"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = 1;//HttpContext.Current.Session["UserID"].ToString();

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        temaplteNames = GetDataTableToKeyValue(dataTable);
                    }

                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("TemplateManagemntRepository.GetTemplateNames :: Exception :: " + ex.Message);
            }
            return temaplteNames;
        }

        public List<KeyValueModel> GetTemplateIds()
        {
            List<KeyValueModel> temaplteIds = new List<KeyValueModel>();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Template_Id"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = 1;//HttpContext.Current.Session["UserID"].ToString();

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        temaplteIds = GetDataTableToKeyValue(dataTable);
                    }

                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("TemplateManagemntRepository.GetTemplateIds :: Exception :: " + ex.Message);
            }
            return temaplteIds;
        }

        public List<KeyValueModel> GetTemplateHeaders()
        {
            List<KeyValueModel> temaplteHeaders = new List<KeyValueModel>();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Manage_Template_Header"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = 1;//HttpContext.Current.Session["UserID"].ToString();

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        temaplteHeaders = GetDataTableToKeyValue(dataTable);
                    }

                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("TemplateManagemntRepository.GetTemplateHeaders :: Exception :: " + ex.Message);
            }
            return temaplteHeaders;
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
                var s = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

                LogWriter.Write("Repositories.Services.GetAccount :: Exception :: " + ex.Message);
            }

            return string.Empty;
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

    }
}
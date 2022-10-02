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
            DataTable dt = new DataTable();
            DataRow newRow = dt.NewRow();
            dt.Columns.Add("Value");
            dt.Columns.Add("Text");
            newRow[0] = 0;
            newRow[1] = "-Select-";
            dt.Rows.InsertAt(newRow, 0);

            newRow = dt.NewRow();
            newRow[0] = 1;
            newRow[1] = "Value1";
            dt.Rows.InsertAt(newRow, 1);

            newRow = dt.NewRow();
            newRow[0] = 2;
            newRow[1] = "Value2";
            dt.Rows.InsertAt(newRow, 2);

            model.TemplateTypes = dt.ToSelectList();
            model.TemplateIds = dt.ToSelectList();
            model.HeaderSenders = dt.ToSelectList();
            model.Status = dt.ToSelectList();
            model.ContentTypes = dt.ToSelectList();

            /*try
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
            }*/
            return model;
        }

        public List<KeyValueModel> GetTemplateNames()
        {
            KeyValueModel model = new KeyValueModel();
            List<KeyValueModel> test = new List<KeyValueModel>();
            for (int i=0;i<10;i++)
            {
                test.Add(new KeyValueModel() {Id=i.ToString(),Value=$"Template {i}" });
            }

           return test;
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

    }
}
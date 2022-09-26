using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Templateprj.Helpers;
using Templateprj.Models;
using System.Linq;
using Newtonsoft.Json;
using Templateprj.Models.InstantSms;
using System.Text.RegularExpressions;

namespace Templateprj.DataAccess
{
    public class CampaignDb
    {
        ExcelExtension _xlsx = new ExcelExtension();


        private string GetJsonString(OracleDataReader dr)
        {
            int columnCount = dr.FieldCount;
            DataTable dtSchema = dr.GetSchemaTable();

            StringBuilder json = new StringBuilder();
            string[] tmpHeadRow = new string[columnCount];

            if (dtSchema != null)
            {
                //for (int i = 0; i < 1; i++)
                //{
                //    tmpHeadRow[i] = "{\"title\":\"" + dtSchema.Rows[i]["ColumnName"].ToString() + "\", \"visible\":false}";
                //}
                for (int i = 0; i < columnCount; i++)
                {
                    tmpHeadRow[i] = "{\"title\":\"" + dtSchema.Rows[i]["ColumnName"].ToString() + "\"}";
                }
            }

            json.Append("{\"thead\":[" + string.Join(",", tmpHeadRow) + "],\"tdata\":[");

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    for (int i = 0; i < columnCount; i++)
                    {
                        tmpHeadRow[i] = dr[i].ToString();
                    }
                    json.Append("[\"" + string.Join("\",\"", tmpHeadRow) + "\"],");
                }
                json.Length--;
            }

            return json.ToString() + "]}";
        }
        private string GetJsonString(MySqlDataReader dr)
        {
            int columnCount = dr.FieldCount;
            DataTable dtSchema = dr.GetSchemaTable();

            StringBuilder json = new StringBuilder();
            string[] tmpHeadRow = new string[columnCount];

            if (dtSchema != null)
            {
                //for (int i = 0; i < 1; i++)
                //{
                //    tmpHeadRow[i] = "{\"title\":\"" + dtSchema.Rows[i]["ColumnName"].ToString() + "\", \"visible\":false}";
                //}
                for (int i = 0; i < columnCount; i++)
                {
                    tmpHeadRow[i] = "{\"title\":\"" + dtSchema.Rows[i]["ColumnName"].ToString() + "\"}";
                }
            }

            json.Append("{\"thead\":[" + string.Join(",", tmpHeadRow) + "],\"tdata\":[");

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    for (int i = 0; i < columnCount; i++)
                    {
                        tmpHeadRow[i] = dr[i].ToString().Trim();
                    }
                    json.Append("[\"" + string.Join("\",\"", tmpHeadRow) + "\"],");
                }
                json.Length--;
            }

            return json.ToString() + "]}";
        }



        #region SmsPortal
        public SelectList getcalltypelist()
        {
            //`web_get_call_type_prc`(IN n_user_id_in int, OUT n_status_out int)


            try
            {

                using (MySqlCommand cmd = new MySqlCommand("web_get_call_type_prc"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_user_id_in", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@n_status_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable.ToSelectList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaign.getcalltypelist::Exception::" + ex.Message);
                return null;
            }
        }
        public DataTable getsmstypelist()
        {
            // `Web_Get_SMS_Type`()


            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Get_SMS_Type"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaign.getsmstypelist::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable getstatuslist()
        {
            // `Web_Get_SMS_Type`()


            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Campaign_status"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaign.getstatuslist::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable getprioritylist()
        {
            // `Web_Get_SMS_Type`()


            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Priority"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaign.getprioritylist::Exception::" + ex.Message);
                return null;
            }
        }
        public DataTable getCampaigntypelist()
        {
            //`Web_Get_Campaign_Type`()
            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Campaign_Type"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaign.getCampaigntypelist::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable getCampaignNameList()
        {

            // `Web_get_campaign_name`(In N_Acc_Id Int)

            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_get_campaign_name"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@N_user_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@N_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaign.getCampaignNameList::Exception::" + ex.Message);
                return null;
            }
        }


        public DataTable getCampaignStarttypelist()
        {

            // `Web_Get_Action_Type`(In N_Acc_Id Int)

            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Action_Type"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@N_user_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    //cmd.Parameters.Add("@N_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaign.getCampaignNameList::Exception::" + ex.Message);
                return null;
            }
        }


        public DataTable getsenderIdlist()
        {
            //  `Web_Get_Sender_Id`(in N_user_id int, In N_Acc_Id int)

            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Sender_Id"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@N_user_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@N_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();



                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaign.getsenderIdlist::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable getTemplateIdFromsenderId(string senderid, string smstype)
        {
            //`Web_get_template_id`(in N_Acc_id int, in N_sender_id varchar(20))
            try
            {
                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("Web_get_template_id"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@N_Acc_id", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["AccountID"].ToString());
                    cmd.Parameters.Add("@N_sender_id", MySqlDbType.VarChar, 200).Value = senderid;
                    cmd.Parameters.Add("@N_Sms_Type", MySqlDbType.VarChar, 200).Value = smstype;


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        string text = "";
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string text1 = row["value"].ToString();
                            string text2 = row["text"].ToString();
                            if (text1 == "10" || text1 == "11")
                            {

                                //RS.SOURCE_ADDR As "Msisdn",
                                //RS.DESTINATION_ADDR As "VMN",
                                //RS.SHORT_MESSAGE As "Message",
                                //RS.RECEIVED_TIME As "Receive Time"

                                string str = text2.ToString();

                                //  string correctString = "";
                                if (text2.Trim() != "")
                                {
                                    //  correctString = str.Replace("[PARAMETER]", "005B0050004100520041004D0045005400450052005D");
                                    string s = "\\u" + Regex.Replace(text2, ".{4}", "$0\\u");
                                    text = Regex.Unescape(s.Substring(0, s.Length - 2));

                                }
                            }
                            if (text1 == "10" || text1 == "11")
                            {
                                row["text"] = text.ToString();
                            }
                            else
                            {
                                string data = text2.Replace("\r\n", "");
                                row["text"] = data;
                            }
                            //  dt.ImportRow(row);
                            // dt.Rows.Add(row);
                            //  dt.AcceptChanges();

                            //dt.ImportRow(row);

                        }
                        //dataTable.Columns.Remove("unicode_flag");
                        //con.Close();
                        return dataTable;
                    }
                }

            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Capaign.getSenderIdFromsmstype::Exce ::  :: " + ex.Message);
                return null;
            }
        }

        public string getcmapigndetailsfromcampid(string id)
        {
            //  `Web_Get_Campaign_details`(In Ln_Campaign_Id int,out V_Out text)
            try
            {
                string response = "";
                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Campaign_details"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Ln_Campaign_Id", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["id"].ToString()); ;
                    cmd.Parameters.Add("@V_Out", MySqlDbType.Text).Direction = ParameterDirection.Output;

                    // DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        string text = "";
                        string subtxt = "";
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string txt = row["UnicodeStatus"].ToString();
                            if (txt == "8")
                            {
                                string text1 = row["templateId"].ToString();
                                string text2 = row["smsContent"].ToString();
                                string text3 = row["template"].ToString();
                                if (text1 == "10" || text1 == "11")
                                {
                                    string str = text2.ToString();
                                    string str1 = text3.ToString();
                                    //  string correctString = "";
                                    if (text2.Trim() != "" && text3.Trim() != "")
                                    {
                                        //  correctString = str.Replace("[PARAMETER]", "005B0050004100520041004D0045005400450052005D");
                                        string s = "\\u" + Regex.Replace(text2, ".{4}", "$0\\u");
                                        text = Regex.Unescape(s.Substring(0, s.Length - 2));
                                        string s1 = "\\u" + Regex.Replace(text3, ".{4}", "$0\\u");
                                        subtxt = Regex.Unescape(s1.Substring(0, s1.Length - 2));
                                    }
                                }
                                if (text1 == "10" || text1 == "11")
                                {
                                    row["template"] = subtxt.ToString();
                                    row["smsContent"] = text.ToString();
                                }
                                else
                                {
                                    string data1 = text3.Replace("\r\n", "");
                                    row["template"] = data1;
                                    string data = text2.Replace("\r\n", "");
                                    row["smsContent"] = data;
                                }
                                response = "[{\"campaignName\":\"" + row["campaignName"] + "\",\"campaignType\":\"" + row["campaignType"] + "\",\"fromDate\":\"" + row["fromDate"] + "\",\"toDate\":\"" + row["toDate"] + "\",\"fromTime\":\"" + row["fromTime"] + "\",\"toTime\":\"" + row["toTime"] + "\",\"senderId\":\"" + row["senderId"] + "\", \"templateId\":\"" + row["template"] + "\",\"smsContent\":\"" + row["smsContent"] + "\"}]";


                            }
                            else
                            {
                                response = cmd.Parameters["@V_Out"].Value.ToString();
                                return response;
                            }
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaigndbprc.getcmapigndetailsfromcampid :: Exception :: " + ex.Message);
                return null;

            }
        }


        public void changestatuscampign(string id, string cstatus)
        {
            // `Web_update_control_status`(In N_scm_Id int, In n_control Int)

            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_update_control_status"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@N_scm_Id", MySqlDbType.Int32).Value = id;
                    cmd.Parameters.Add("@n_control", MySqlDbType.Int32).Value = cstatus;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {

                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaigndbprc.getcmapigndetailsfromcampid :: Exception :: " + ex.Message);

            }
        }

        public DataTable getsenderidfromsmstype(string smstype)
        {
            //`Web_Get_Sender_Id`(in N_user_id Int, In N_Acc_Id Int, In N_Sms_Type Int)
            try
            {
                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Sender_Id"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@N_user_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@N_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();
                    cmd.Parameters.Add("@N_Sms_Type", MySqlDbType.Int32).Value = smstype;


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }

            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Capaign.getSenderIdFromsmstype::Exce ::  :: " + ex.Message);
                return null;
            }
        }


        public string getTemplatebytemplateId(string template)
        {
            // string response = "";

            //`Web_Get_Template`(In n_template_Id int)
            try
            {
                DataTable dt = new DataTable();
                string response = "";
                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Template"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@N_user_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    //cmd.Parameters.Add("@N_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();
                    cmd.Parameters.Add("@n_template_Id", MySqlDbType.Int32).Value = template;
                    cmd.Parameters.Add("@n_message", MySqlDbType.LongText).Direction = ParameterDirection.Output;


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        string text = "";
                        // response = cmd.Parameters["@n_message"].Value.ToString();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string txt = row["unicode_status"].ToString();
                            if (txt == "8")
                            {
                                string text1 = row["template_id"].ToString();
                                string text2 = row["template"].ToString();
                                if (text1 == "10" || text1 == "11")
                                {
                                    string str = text2.ToString();

                                    //  string correctString = "";
                                    if (text2.Trim() != "")
                                    {
                                        //  correctString = str.Replace("[PARAMETER]", "005B0050004100520041004D0045005400450052005D");
                                        string s = "\\u" + Regex.Replace(text2, ".{4}", "$0\\u");
                                        text = Regex.Unescape(s.Substring(0, s.Length - 2));

                                    }
                                }
                                if (text1 == "10" || text1 == "11")
                                {
                                    row["template"] = text.ToString();
                                }
                                else
                                {
                                    string data = text2.Replace("\r\n", "");
                                    row["template"] = data;
                                }
                                response = "[{ \"smsLength\":\"" + row["sms_length"] + "\", \"variableCount\":" + row["variable_cnt"] + ",\"templateContent\":\"" + row["template"] + "\"}]";


                            }
                            else
                            {
                                response = cmd.Parameters["@n_message"].Value.ToString();
                                return response;
                            }
                        }
                        //dataTable.Columns.Remove("unicode_flag");
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Capaign.getTemplatebytemplateId::Exce ::  :: " + ex.Message);
                return null;
            }
        }

        public string getSmscountDetails(string json)
        {
            string response = "";

            //`Web_Get_SMS_Length_Count`(In Lv_Data Text, Out Lv_Response Text)

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Get_SMS_Length_Count"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Lv_Data", MySqlDbType.Text).Value = json;
                    cmd.Parameters.Add("@Lv_Response", MySqlDbType.LongText).Direction = ParameterDirection.Output;



                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    response = cmd.Parameters["@Lv_Response"].Value.ToString();
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.CampaignDb.getTemplatebytemplateId :: Exception :: " + ex.Message);
                return "";
            }
        }


        public string testsms(string json, out string response, out string campaignId, out string smsPushedCount)
        {
            response = "";
            string status = "";
            campaignId = "";
            smsPushedCount = "";

            //`Web_Campaign_Test_SMS`(IN n_Acc_id int, 
            //                           In n_User_Id Int, 
            //                           IN Lv_Data text, 
            //                           Out N_Sms_Pushed Int, 
            //                           Out V_Campaign_Id Varchar(50),
            //                           Out Ln_Status int, 
            //                           OUT Lv_Response varchar(50))

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Campaign_Test_SMS"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Acc_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();
                    cmd.Parameters.Add("@n_User_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@Lv_Data", MySqlDbType.Text).Value = json;
                    cmd.Parameters.Add("@N_Sms_Pushed", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@V_Campaign_Id", MySqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Ln_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Lv_Response", MySqlDbType.VarChar, 200).Direction = ParameterDirection.Output;



                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    status = cmd.Parameters["@Ln_Status"].Value.ToString();
                    campaignId = cmd.Parameters["@V_Campaign_Id"].Value.ToString();
                    smsPushedCount = cmd.Parameters["@N_Sms_Pushed"].Value.ToString();
                    response = cmd.Parameters["@Lv_Response"].Value.ToString();
                    return status;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.CampaignDb.getTemplatebytemplateId :: Exception :: " + ex.Message);
                return "";
            }
        }


        public string saveCampaign(string json, out string response)
        {
            response = "";
            string status = "";

            //`Web_Campaign_Create`(IN n_Acc_id int, IN n_User_Id int, IN v_data text, OUT Ln_Status int, OUT Lv_Response varchar(50))

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Campaign_Create"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Acc_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();
                    cmd.Parameters.Add("@n_User_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@v_data", MySqlDbType.Text).Value = json;
                    cmd.Parameters.Add("@Ln_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Lv_Response", MySqlDbType.VarChar, 200).Direction = ParameterDirection.Output;



                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    status = cmd.Parameters["@Ln_Status"].Value.ToString();
                    response = cmd.Parameters["@Lv_Response"].Value.ToString();
                    return status;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.CampaignDb.saveCampaign :: Exception :: " + ex.Message);
                return "";
            }
        }


        public string getcampaigncreatedlist(SMSCampaignModel model)
        {

            try
            {
                //`Web_Rpt_Master_Prc`(In N_User_Id  int(6),
                //                  In Lv_Circle Varchar(50),
                //                  In Lv_Vendor Varchar(50),
                //                  In Ln_VMN Varchar(50))

                using (MySqlCommand cmd = new MySqlCommand("Web_Rpt_Master_Prc"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@N_User_Id", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
                    cmd.Parameters.Add("@Lv_Circle", MySqlDbType.Int32).Value = model.listcampaignName;
                    cmd.Parameters.Add("@Lv_Vendor", MySqlDbType.VarChar).Value = model.listCreatedDate;
                    cmd.Parameters.Add("@Ln_VMN", MySqlDbType.VarChar).Value = model.listCampaignStatus;
                    cmd.Parameters.Add("@Ln_VMN", MySqlDbType.VarChar).Value = model.listCampaignType;
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        var dataReader_new = cmd.ExecuteReader();
                        return GetJsonString(dataReader_new);
                    }
                }


            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.CampaignDb.getcampaigncreatedlist :: Exception :: " + ex.Message);
                return "";
            }
        }


        public string getcampaignstatusreport(SMSCampaignModel model)
        {

            try
            {
                //`Web_Get_Campaign_Status_Details`(IN n_Acc_Id int, 
                //                            IN n_user_id int, 
                //                            IN Ln_Camp_name_Id int, 
                //                            IN Lv_From_Date varchar(50), 
                //                            IN Ln_Camp_Status int, 
                //                            IN Ln_Campaign_Type int)


                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Campaign_Status_Details"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();
                    cmd.Parameters.Add("@n_user_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@Ln_Camp_name_Id", MySqlDbType.VarChar).Value = model.statuscampaignName;
                    cmd.Parameters.Add("@Lv_To_Date", MySqlDbType.VarChar).Value = model.statusCreatedDate;
                    cmd.Parameters.Add("@Ln_Camp_Status", MySqlDbType.Int32).Value = model.statusCampaignStatus;
                    cmd.Parameters.Add("@Ln_Campaign_Type", MySqlDbType.Int32).Value = model.statusCampaignType;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        var dataReader_new = cmd.ExecuteReader();
                        return GetJsonString(dataReader_new);
                    }
                }


            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.CampaignDb.getcampaignstatusreport :: Exception :: " + ex.Message);
                return "";
            }
        }

        public string getcampaigndetailreport(SMSCampaignModel model)
        {

            try
            {
                //`Web_Get_Campaign_Summary_Report`(IN n_Acc_Id int, 
                //                                 IN n_user_id int, 
                //                                 IN Ln_Camp_name_Id int, 
                //                                 IN Lv_From_Date varchar(50),
                //                                 IN Ln_Camp_Status int)


                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Campaign_Summary_Report"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();
                    cmd.Parameters.Add("@n_user_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@Ln_Camp_name_Id", MySqlDbType.Int32).Value = model.reportcampaignName;
                    cmd.Parameters.Add("@Lv_From_Date", MySqlDbType.VarChar, 200).Value = model.reportCreatedDate;
                    cmd.Parameters.Add("@Ln_Camp_Status", MySqlDbType.Int32).Value = model.reportCampaignStatus;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        var dataReader_new = cmd.ExecuteReader();
                        return GetJsonString(dataReader_new);
                    }
                }

            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.CampaignDb.getcampaigndetailreport :: Exception :: " + ex.Message);
                return "";
            }
        }
        public string getcampaignreportDownload(SMSCampaignModel model, string fileName)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr);
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "Web_Get_SMS_detail_Report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Ln_Camp_name_Id", MySqlDbType.Int32).Value = Convert.ToInt32(model.campaignId);
                // model.SelectedCampaign.Trim();
                //cmd.Parameters.Add("@FromDate", MySqlDbType.VarChar).Value = model.fromdate;
                //cmd.Parameters.Add("@ToDate", MySqlDbType.VarChar).Value = model.todate;
                //cmd.Parameters.Add("@N_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                //var dataReader = cmd.ExecuteReader();
                //_xlsx.ExportToExcel(ds, ref fileName, out contentType, sheetName);
                //status = Convert.ToInt32(cmd.Parameters["@N_Status"].Value.ToString());
                var dataReader_new = cmd.ExecuteReader();
                return GetJsonString(dataReader_new);
                //catch (OutOfMemoryException)
                //{
                //    LogWriter.Write("DataAccess.ReportsDb.ExportDetailedReport :: OutOfMemoryException");
                //    status = -2;
                //}
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.ReportsDb.ExportDetailedReport :: Exception :: " + ex.Message);
                return "";
            }
            
        }
        #endregion


        public void getInsightDetails(out string smsallow, out string smsDeliv, out string smssub, out string success
          , out string instant, out string apibased, out string campaigns, out string apiinstant)

        {

            //`Web_Prc_dashboard`(in N_user_id int,
            //out T_sms_allow bigint,
            //out T_sms_Deliv bigint,
            //out T_sms_sub bigint,
            //out Success_percent bigint,
            //out V_instant bigint,
            //out Api_based bigint,
            //out sms_campaigns bigint,
            //out Api_based_instant bigint)

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Prc_dashboard"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@N_user_id_in", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@T_sms_allow_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@T_sms_Deliv_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@T_sms_sub_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Success_percent_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@V_instant_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Api_based_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@T_sms_campaigns_out", MySqlDbType.VarChar, 4000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Api_based_instant_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    //int status = Convert.ToInt32(cmd.Parameters["@n_status_out"].Value.ToString());
                    smsallow = cmd.Parameters["@T_sms_allow_out"].Value.ToString();
                    smsDeliv = cmd.Parameters["@T_sms_Deliv_out"].Value.ToString();
                    smssub = cmd.Parameters["@T_sms_sub_out"].Value.ToString();
                    success = cmd.Parameters["@Success_percent_out"].Value.ToString();
                    instant = cmd.Parameters["@V_instant_out"].Value.ToString();
                    apibased = cmd.Parameters["@Api_based_out"].Value.ToString();
                    campaigns = cmd.Parameters["@T_sms_campaigns_out"].Value.ToString();
                    apiinstant = cmd.Parameters["@Api_based_instant_out"].Value.ToString();
                    //return dt;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.campaigndb.Insights :: Exception :: " + ex.Message);
                smsallow = "";
                smsDeliv = "";
                smssub = "";
                success = "";
                instant = "";
                apibased = "";
                campaigns = "";
                apiinstant = "";
                //return null;

            }
        }


        public DataTable getsamplefile(string id)
        {
            // web_get_file_headers_prc(IN n_user_id_in int, IN n_camp_id_in int, OUT v_data_out varchar(4000))

            try
            {

                using (MySqlCommand cmd = new MySqlCommand("web_get_file_headers_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_user_id_in", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@n_camp_id_in", MySqlDbType.Int32).Value = id;
                    cmd.Parameters.Add("@v_data_out", MySqlDbType.VarChar,4000).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        //con.Open();
                        //cmd.Connection = con;
                        //MySqlDataAdapter da = new MySqlDataAdapter("", con);
                        //da.SelectCommand = cmd;
                        //da.Fill(dt);
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    string response = cmd.Parameters["@v_data_out"].Value.ToString();
                  //  response = "[{ \"CustomerNumber\":\"\",\"CustomerName\":\"\"}]";
                    if (response != null && response != "")
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(response);
                        
                    }
                    else
                    {
                        dt = null;

                    }
                   
                    return dt;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaigndbprc.getsamplefile :: Exception :: " + ex.Message);
                return null;

            }
        }


        public DataTable getsamplefilesms(string id)
        {
            //`Web_Get_Variable_details`(IN Ln_Campaign_Id int, OUT V_Out text)
            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Get_Variable_details"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Ln_Campaign_Id", MySqlDbType.Int32).Value = id;
            
                    cmd.Parameters.Add("@V_Out", MySqlDbType.Text).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    string response = cmd.Parameters["@V_Out"].Value.ToString();

                    DataTable dt1 = new DataTable();

                  
                    //  response = "[{ \"CustomerNumber\":\"\",\"CustomerName\":\"\"}]";
                    if (response != null && response != "")
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(response);
                      
                    }
                    else
                    {
                        dt = null;

                    }

                    return dt;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaigndbprc.getsamplefilesms :: Exception :: " + ex.Message);
                return null;

            }
        }

        public string insertfilepath(string path,string campaignid,string campaignstarttype)
        {
            path = path.Replace("\\", "/");

            //`Web_Insert_campaign_file`(IN Ln_Campaign_id int,IN Lv_Path varchar(1000),IN Ln_Action_Type int,Out Ln_Status int)

            //response = "";
            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Insert_campaign_file"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Ln_Campaign_id", MySqlDbType.Int32).Value = campaignid;
                    cmd.Parameters.Add("@Lv_Path", MySqlDbType.VarChar,200).Value = path;
                    cmd.Parameters.Add("@Ln_Action_Type", MySqlDbType.Int32).Value = campaignstarttype;

                    cmd.Parameters.Add("@Ln_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {

                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    string status = cmd.Parameters["@Ln_Status"].Value.ToString();

                    return status;

                   
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Campaigndbprc.insertfilepath :: Exception :: " + ex.Message);
                return null;

            }
        }
        public string GetUnicodeStatus(int templateid)
        {
            string statusOut = "-1";
            try
            {
                /*check_unicode_sms_api_prc(IN n_template_id bigint(21), OUT n_status int(5))
*/
                using (MySqlCommand cmd = new MySqlCommand("check_unicode_sms_api_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_template_id", MySqlDbType.Int64).Value = Convert.ToInt64(templateid);
                    cmd.Parameters.Add("@n_status", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@n_status"].Value.ToString();
                    return statusOut;

                }
            }
            
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.Campaigndb.GetUnicodeStatus::Exception ::{ ex.Message}");
                return "";

            }
           
        }

        public void GetSmsCount(string smsContent, int templateId, out long smsCount, out long length)
        {
            smsCount = 0;
            length = 0;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_get_sms_length_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@str", MySqlDbType.LongText).Value = smsContent;
                    cmd.Parameters.Add("@Lv_template_Id", MySqlDbType.Int64).Value = Convert.ToInt64(templateId);
                    cmd.Parameters.Add("@n_sms_leg", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_no_of_sms", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    smsCount = Convert.ToInt32(cmd.Parameters["@n_sms_leg"].Value);
                    length = Convert.ToInt32(cmd.Parameters["@n_no_of_sms"].Value);

                }
            }

            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.Campaigndb.GetSmsCount::Exception ::{ ex.Message}");

            }

        }

        #region InstantSms

        public string getinstantsmsreport(InstantSmsModel model)
        {

            try
            {
                //`Web_Instant_Sms_Report`(
                //    in n_MSISDN_in bigint(21), in v_template_id_in varchar(200),
                //    in v_DateRange_In varchar(100),
                //    in n_status int
                //    )

                using (MySqlCommand cmd = new MySqlCommand("Web_Instant_Sms_Report"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_MSISDN_in", MySqlDbType.Int64).Value = model.MSISDN;
                    cmd.Parameters.Add("@v_template_id_in", MySqlDbType.VarChar).Value = model.template;
                    cmd.Parameters.Add("@v_DateRange_In", MySqlDbType.VarChar).Value = model.dateFrom;
                    //cmd.Parameters.Add("@v_todate", MySqlDbType.VarChar).Value = model.dateTo;
                    cmd.Parameters.Add("@n_status", MySqlDbType.Int32).Value = model.reportStatus;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        var dataReader_new = cmd.ExecuteReader();
                        return GetJsonString(dataReader_new);
                    }
                }


            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.CampaignDb.getinstantsmsreport :: Exception :: " + ex.Message);
                return "";
            }
        }

        public DataTable getTemplateId()
        {
            //`Web_get_Instant_template_id`(in N_Acc_id int)
            try
            {
                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("Web_get_Instant_template_id"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@N_Acc_id", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["AccountID"].ToString());
                    //cmd.Parameters.Add("@N_sender_id", MySqlDbType.VarChar, 200).Value = senderid;
                    //cmd.Parameters.Add("@N_Sms_Type", MySqlDbType.Int32).Value = smstype;


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        string text = "";
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string text1 = row["value"].ToString();
                            string text2 = row["text"].ToString();
                            if (text1 == "10" || text1 == "11")
                            {

                                string str = text2.ToString();

                                //  string correctString = "";
                                if (text2.Trim() != "")
                                {
                                    //  correctString = str.Replace("[PARAMETER]", "005B0050004100520041004D0045005400450052005D");
                                    string s = "\\u" + Regex.Replace(text2, ".{4}", "$0\\u");
                                    text = Regex.Unescape(s.Substring(0, s.Length - 2));

                                }
                            }
                            if (text1 == "10" || text1 == "11")
                            {
                                row["text"] = text.ToString();
                            }
                            else
                            {
                                string data = text2.Replace("\r\n", "");
                                row["text"] = data;
                            }
                            
                        }
                        return dataTable;
                    }
                }

            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.Capaign.getTemplateId::Exce ::  :: " + ex.Message);
                return null;
            }
        }
        

        public string SendInstantSms(string json, out string response)
        {
            response = "";

            //`Web_Insert_Instant_SMS_Prc`(IN n_Acc_Id int, IN n_User_Id int, IN v_Data text, OUT Ln_Status int, OUT Lv_Response varchar(50))

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Web_Insert_Instant_SMS_Prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"].ToString();
                    cmd.Parameters.Add("@n_User_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@v_Data", MySqlDbType.Text).Value = json;
                    cmd.Parameters.Add("@Ln_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Lv_Response", MySqlDbType.VarChar, 50).Direction = ParameterDirection.Output;



                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    string status = cmd.Parameters["@Ln_Status"].Value.ToString();
                    response = cmd.Parameters["@Lv_Response"].Value.ToString();
                    return status;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.CampaignDb.SendInstantSms :: Exception :: " + ex.Message);
                return "";
            }
        }


        #endregion

    }
}
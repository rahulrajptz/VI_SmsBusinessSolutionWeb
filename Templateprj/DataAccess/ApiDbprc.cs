using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Templateprj.Helpers;
using Templateprj.Models;

namespace Templateprj.DataAccess
{
    public class ApiDbprc
    {
        CryptoAlg _EncDec = new CryptoAlg();


        MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr);
        public string ApiInsertBulksms(string apikey, string json)
        {

            //`Api_Insert_Bulk_SMS`(IN in_api_key varchar(50),
            // IN v_data longtext, /* SMS script*/
            // OUT response_out longtext)
            string statusOut = "";
            //Deliverystatus = "";
            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Api_Insert_Bulk_SMS"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@in_api_key", MySqlDbType.VarChar, 50).Value = apikey;
                    cmd.Parameters.Add("@v_data", MySqlDbType.LongText).Value = json;


                    cmd.Parameters.Add("@response_out", MySqlDbType.LongText).Direction = ParameterDirection.Output;

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@response_out"].Value.ToString();

                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.SMSDelivery::MysqlException ::{ ex.Message}");
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.SMSDelivery::Exception ::{ ex.Message}");
            }

            return statusOut;
        }
        public string ApiCreateCampaign(string apikey, string json)
        {

            //`Api_Create_Campaign`(IN in_api_key varchar(50),
            //  IN v_data longtext, /* SMS script*/
            //     OUT response_out longtext)
            string statusOut = "";
            //Deliverystatus = "";
            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Api_Create_Campaign"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@in_api_key", MySqlDbType.VarChar, 50).Value = apikey;
                    cmd.Parameters.Add("@v_data", MySqlDbType.LongText).Value = json;


                    cmd.Parameters.Add("@response_out", MySqlDbType.LongText).Direction = ParameterDirection.Output;

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@response_out"].Value.ToString();

                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.SMSDelivery::MysqlException ::{ ex.Message}");
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.SMSDelivery::Exception ::{ ex.Message}");
            }

            return statusOut;
        }
        public string CreateCampign(string key, string convertedCode, string isUnicode, Campaign campaign, out string response, out string campID)
        {
            string status = APIDataInsert(key, campaign.campname, campaign.fromdt, campaign.todt, campaign.fromtime, campaign.totime, campaign.senderid, campaign.templateid, campaign.type,
               campaign.script, campaign.pingbackurl, convertedCode, isUnicode, out response, out campID);

            return status;
        }
        public string SMSdelivery(string mobile, string status, string mid, string deliv_time, string refid, out string Deliverystatus)
        {
            string statusOut = "-1";
            Deliverystatus = "";
            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Insert_delivery_response_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_msisdn", MySqlDbType.Int64).Value = Convert.ToInt64(mobile);
                    cmd.Parameters.Add("@v_status_in", MySqlDbType.VarChar).Value = status;
                    cmd.Parameters.Add("@v_message_id", MySqlDbType.VarChar).Value = mid;
                    cmd.Parameters.Add("@deliv_time", MySqlDbType.VarChar).Value = deliv_time;
                    cmd.Parameters.Add("@v_ref_id", MySqlDbType.VarChar).Value = refid;
                    cmd.Parameters.Add("@n_status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@status_msg", MySqlDbType.VarChar, 2000).Direction = ParameterDirection.Output;

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@n_status"].Value.ToString();
                    Deliverystatus = cmd.Parameters["@status_msg"].Value.ToString();
                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.SMSDelivery::MysqlException ::{ ex.Message}");
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.SMSDelivery::Exception ::{ ex.Message}");
            }

            return statusOut;
        }
        public DataSet SMSdelivery1(string apikey, DeliveryFetch deliveryfetch)
        {
            //string statusOut = "-1";
            DataSet dtsecQs = new DataSet();
            try
            {
                /*check_unicode_sms_api_prc(IN n_template_id bigint(21), OUT n_status int(5))
*/
                using (MySqlCommand cmd = new MySqlCommand("Sms_Delivery_Status_Fetch_Prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Unique_Id_In", MySqlDbType.VarChar).Value = deliveryfetch.UniqueID;
                    cmd.Parameters.Add("@v_Api_Key_In", MySqlDbType.VarChar).Value = apikey;

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                        da.SelectCommand = cmd;
                        da.Fill(dtsecQs);
                        return dtsecQs;
                    }


                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.SMSDelivery1::MysqlException ::{ ex.Message}");
                return null;
            }
            catch (Exception ex)
            {

                LogWriter.Write($"DataAccess.APIDbPrcs.SMSDelivery1::Exception ::{ ex.Message}");
                return null;

            }


        }
        public DataSet TemplateFetch(string apikey, SMSTemplateFetch templatefetch)
        {
            //string statusOut = "-1";
            DataSet dtsecQs = new DataSet();
            try
            {
                /*sms_campaign.Sms_Template_Status_Fetch_Prc(IN n_Template_Id_In varchar(100), IN v_Api_Key_In varchar(100))
*/
                using (MySqlCommand cmd = new MySqlCommand("Sms_Template_Status_Fetch_Prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Template_Id_In", MySqlDbType.VarChar).Value = templatefetch.templateid;
                    cmd.Parameters.Add("@v_Api_Key_In", MySqlDbType.VarChar).Value = apikey;

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                        da.SelectCommand = cmd;
                        da.Fill(dtsecQs);
                        return dtsecQs;
                    }


                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.TemplateFetch::MysqlException ::{ ex.Message}");
                return null;
            }
            catch (Exception ex)
            {

                LogWriter.Write($"DataAccess.APIDbPrcs.TemplateFetch::Exception ::{ ex.Message}");
                return null;

            }


        }

        public string GetCreditBalanceDetails(string apikey, out string balance)
        {
            string statusOut = "-1";
            balance = "";
            DataSet dtsecQs = new DataSet();
            try
            {
                //api_credit_balance_out_prc(IN v_key varchar(45), OUT n_balance_out bigint(21), OUT n_status_out int)
                using (MySqlCommand cmd = new MySqlCommand("api_credit_balance_out_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@v_key", MySqlDbType.VarChar).Value = apikey;

                    cmd.Parameters.Add("@n_balance_out", MySqlDbType.Int64).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_status_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@n_Status_Out"].Value.ToString();
                    balance = cmd.Parameters["@n_balance_out"].Value.ToString();
                    return statusOut;

                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.GetCreditBalanceDetails::MysqlException ::{ ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.GetCreditBalanceDetails::Exception ::{ ex.Message}");
                return null;

            }


        }

        public string CreateSenderid(string apikey, Senderid Senderid, out string mail, out string companyname, out string msg, out string refid)
        {
            string statusOut = "-1";
            mail = "";
            msg = "server error";
            companyname = "";
            refid = "";
            DataSet dtsecQs = new DataSet();

            //// `Api_senderid_Insert_prc`(IN n_Api_Key varchar(50), 
            //                                IN v_senderid_In varchar(4000), 
            //                                IN n_pid varchar(21), 
            //                                OUT v_Email_Out varchar(1000), 
            //                                OUT v_Company_Name varchar(400), 
            //                                OUT n_Status_Out int, 
            //                                OUT v_Message_Out varchar(2000), 
            //                                OUT n_ref_id bigint)

            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Api_senderid_Insert_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_Api_Key", MySqlDbType.VarChar).Value = apikey;
                    cmd.Parameters.Add("@v_senderid_In", MySqlDbType.VarChar).Value = Senderid.SenderId.Trim();
                    cmd.Parameters.Add("@n_pid", MySqlDbType.VarChar).Value = Senderid.PrincipleId.Trim();
                    cmd.Parameters.Add("@v_Email_Out", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Company_Name", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Message_Out", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_ref_id", MySqlDbType.Int64).Direction = ParameterDirection.Output;
                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@n_Status_Out"].Value.ToString();
                    mail = cmd.Parameters["@v_Email_Out"].Value.ToString();
                    companyname = cmd.Parameters["@v_Company_Name"].Value.ToString();
                    msg = cmd.Parameters["@v_Message_Out"].Value.ToString();
                    refid = cmd.Parameters["@n_ref_id"].Value.ToString();
                    return statusOut;

                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.CreateSenderid::MysqlException ::{ ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.CreateSenderid::Exception ::{ ex.Message}");
                return null;

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }


        }
        public string UploadBase(string key, MSISDNBase mSISDNBase, int unicode)
        {
            string returnValue = "";
            MySqlConnection con = null;
            try
            {

                string query = @"insert into bulk_base (MSISDN,CAMPAIGN_ID,STATUS,param1,param2,param3,param4,param5,param6,param7,param8,param9,param10)values (@msisdn,'" + mSISDNBase.campaignid + "', 0,@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10)";
                using (MySqlConnection mConnection = new MySqlConnection(GlobalValues.ConnStr))
                {
                    mConnection.Open();
                    using (MySqlTransaction trans = mConnection.BeginTransaction())
                    {
                        if (mSISDNBase.Data == null)
                        {
                            string[] arrmsisdn = mSISDNBase.MSISDN.Split(',');
                            foreach (string arrMasisdn in arrmsisdn)
                            {
                                using (MySqlCommand myCmd = new MySqlCommand(query, mConnection, trans))
                                {
                                    myCmd.CommandType = CommandType.Text;
                                    myCmd.Parameters.AddWithValue("@MSISDN", arrMasisdn);
                                    myCmd.Parameters.AddWithValue("@param1", null);
                                    myCmd.Parameters.AddWithValue("@param2", null);
                                    myCmd.Parameters.AddWithValue("@param3", null);
                                    myCmd.Parameters.AddWithValue("@param4", null);
                                    myCmd.Parameters.AddWithValue("@param5", null);
                                    myCmd.Parameters.AddWithValue("@param6", null);
                                    myCmd.Parameters.AddWithValue("@param7", null);
                                    myCmd.Parameters.AddWithValue("@param8", null);
                                    myCmd.Parameters.AddWithValue("@param9", null);
                                    myCmd.Parameters.AddWithValue("@param10", null);
                                    Int64 result = myCmd.ExecuteNonQuery();
                                    returnValue = "1";
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < mSISDNBase.Data.Count; i++)
                            {
                                using (MySqlCommand myCmd = new MySqlCommand(query, mConnection, trans))
                                {
                                    myCmd.CommandType = CommandType.Text;
                                    myCmd.Parameters.AddWithValue("@MSISDN", mSISDNBase.Data[i].msisdn);
                                    if (unicode == 0)
                                    {
                                        myCmd.Parameters.AddWithValue("@param1", mSISDNBase.Data[i].param1);
                                        myCmd.Parameters.AddWithValue("@param2", mSISDNBase.Data[i].param2);
                                        myCmd.Parameters.AddWithValue("@param3", mSISDNBase.Data[i].param3);
                                        myCmd.Parameters.AddWithValue("@param4", mSISDNBase.Data[i].param4);
                                        myCmd.Parameters.AddWithValue("@param5", mSISDNBase.Data[i].param5);
                                        myCmd.Parameters.AddWithValue("@param6", mSISDNBase.Data[i].param6);
                                        myCmd.Parameters.AddWithValue("@param7", mSISDNBase.Data[i].param7);
                                        myCmd.Parameters.AddWithValue("@param8", mSISDNBase.Data[i].param8);
                                        myCmd.Parameters.AddWithValue("@param9", mSISDNBase.Data[i].param9);
                                        myCmd.Parameters.AddWithValue("@param10", mSISDNBase.Data[i].param10);

                                    }
                                    else
                                    {
                                        myCmd.Parameters.AddWithValue("@param1", Converter(mSISDNBase.Data[i].param1));
                                        myCmd.Parameters.AddWithValue("@param2", Converter(mSISDNBase.Data[i].param2));
                                        myCmd.Parameters.AddWithValue("@param3", Converter(mSISDNBase.Data[i].param3));
                                        myCmd.Parameters.AddWithValue("@param4", Converter(mSISDNBase.Data[i].param4));
                                        myCmd.Parameters.AddWithValue("@param5", Converter(mSISDNBase.Data[i].param5));
                                        myCmd.Parameters.AddWithValue("@param6", Converter(mSISDNBase.Data[i].param6));
                                        myCmd.Parameters.AddWithValue("@param7", Converter(mSISDNBase.Data[i].param7));
                                        myCmd.Parameters.AddWithValue("@param8", Converter(mSISDNBase.Data[i].param8));
                                        myCmd.Parameters.AddWithValue("@param9", Converter(mSISDNBase.Data[i].param9));
                                        myCmd.Parameters.AddWithValue("@param10", Converter(mSISDNBase.Data[i].param10));
                                    }
                                    Int64 result = myCmd.ExecuteNonQuery();
                                    returnValue = "1";
                                }

                            }
                        }

                        trans.Commit();
                    }
                }
            }
            catch (MySqlException ex)
            {
                returnValue = "9";
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.UploadMSISDNBase::Exception ::{ ex.Message}");
            }
            catch (Exception e)
            {
                returnValue = "-1";
                con.Close();
                LogWriter.Write($"APIDbPrc.UploadMSISDNBase::Exception ::{ e.Message}");
            }
            return returnValue;
        }

        public string Converter(string codetoConvert)
        {
            try
            {
                if (codetoConvert != null)
                {
                    byte[] unibyte = Encoding.Unicode.GetBytes(codetoConvert.Trim());
                    string uniString = string.Empty;
                    string tmp = string.Empty;
                    int i = 0;
                    foreach (byte b in unibyte)
                    {
                        if (i == 0)
                        {
                            tmp = string.Format("{0}{1}", @"", b.ToString("X2"));
                            i = 1;
                        }
                        else
                        {
                            uniString += string.Format("{0}{1}", @"", b.ToString("X2")) + tmp;
                            i = 0;
                        }
                    }
                    return uniString;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogWriter.Write($"APIDbPrc.Converter::Exception ::{ e.Message}");
                return null;
            }
        }

        private List<string[]> SplitArray(String[] arrayString)
        {

            List<string[]> splitted = new List<string[]>();//This list will contain all the splitted arrays.

            int lengthToSplit = arrayString.Length > 500 ? 500 : arrayString.Length;

            int arrayLength = arrayString.Length;


            for (int i = 0; i < arrayLength; i = i + lengthToSplit)
            {

                if (arrayLength < i + lengthToSplit)
                {
                    lengthToSplit = arrayLength - i;
                }

                string[] val = new string[lengthToSplit];

                Array.Copy(arrayString, i, val, 0, lengthToSplit);
                splitted.Add(val);
            }

            return splitted;
        }
        public string GetUnicodeStatus(string templateid)
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

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@n_status"].Value.ToString();
                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.GetUnicodeStatus::MysqlException ::{ ex.Message}");
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.GetUnicodeStatus::Exception ::{ ex.Message}");


            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return statusOut;
        }

        public int UnicodeStatus(string campaignid)
        {
            try
            {
                /*bulk_campaign_unicode_check(IN camp_id bigint(21), OUT n_Status int)*/
                MySqlCommand cmd = new MySqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "bulk_campaign_unicode_check";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@camp_id", MySqlDbType.Int32).Value = Convert.ToInt32(campaignid);

                cmd.Parameters.Add("@n_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int status = Convert.ToInt32(cmd.Parameters["@n_Status"].Value.ToString());
                con.Close();
                return status;
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.UnicodeStatus::MysqlException ::{ ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.APIDbPrc.UnicodeStatus::Exception::" + ex.Message);
                return -1;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public string CheckIPValid(string key, string ip)
        {
            string statusOut = "-1";
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("authenticate_user_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@api_key", MySqlDbType.VarChar).Value = key;
                    cmd.Parameters.Add("@ip_address", MySqlDbType.VarChar).Value = ip.Trim();
                    cmd.Parameters.Add("@n_sts", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_sts_msg", MySqlDbType.VarChar).Direction = ParameterDirection.Output;


                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@n_sts"].Value.ToString();
                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.CheckIPValid::MysqlException ::{ ex.Message}");
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.CheckIPValid::Exception ::{ ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return statusOut;
        }

        public int UploadedFile(MSISDNBase mdl)
        {
            // string status = "-1";
            try
            {
                //    `Sms_Update_Upload_Status`(In c_Campaign_Id  bigint,In c_Status  int,out n_sts int,out v_sts_msg varchar(50))
                MySqlCommand cmd = new MySqlCommand();
                con.Open();
                cmd.Connection = con;

                cmd.CommandText = "Sms_Update_Upload_Status";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@c_Campaign_Id", MySqlDbType.Int32).Value = Convert.ToInt32(mdl.campaignid);
                cmd.Parameters.Add("@c_Status", MySqlDbType.Int32).Value = 1;
                cmd.Parameters.Add("@n_sts", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@v_sts_msg", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int status = Convert.ToInt32(cmd.Parameters["@n_sts"].Value.ToString());
                //message =cmd.Parameters["@v_sts_msg"].Value.ToString();
                con.Close();
                return status;
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.UploadedFile::MysqlException ::{ ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.APIDbprc.UploadedFile::Exception::" + ex.Message);
                return -1;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }

        public string Createscript(string apikey, string isunicode, string convertedcode, out string EMAIL, out string compname, out string statusmsg, out string tempid, script1 script)
        {
            compname = "";
            tempid = "";
            EMAIL = "";
            statusmsg = "";
            string stsout = "";
            try
            {
                /*Api_sms_template_Insert_prc(IN n_Api_Key Varchar(50), IN v_Script_In varchar(4000),
                 * IN n_Unicode int, IN v_Url varchar(500),IN n_tid varchar(21), OUT v_Email_Out varchar(1000),
                 * OUT v_Company_Name varchar(400),
                 * OUT v_Status_Out varchar(400),OUT v_Message_Out varchar(2000),OUT n_Template_Id bigint(21))*/

                //// `Api_sms_template_Insert_prc`(IN n_Api_Key varchar(50), 
                //                    IN v_Script_In varchar(4000), 
                //                    IN n_Unicode int, 
                //                    IN v_Url varchar(500), 
                //                    IN n_tid varchar(21), 
                //                    IN V_Template_Name varchar(1000),
                //                    IN N_variable_count int, 
                //                    IN N_senderId int, 
                //                    IN N_SMS_Type int, 
                //                    IN V_validity int, 
                //                    IN N_Delivery_Flag int, 
                //                    OUT v_Email_Out varchar(1000), 
                //                    OUT v_Company_Name varchar(400), 
                //                    OUT v_Status_Out varchar(400), 
                //                    OUT v_Message_Out varchar(2000), 
                //                    OUT n_Template_Id bigint(21))

                using (MySqlCommand cmd = new MySqlCommand("Api_sms_template_Insert_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Api_Key", MySqlDbType.VarChar).Value = apikey;
                    if (script.Unicodestatus == "0")
                    {
                        cmd.Parameters.Add("@v_Script_In", MySqlDbType.VarChar).Value = script.Script.Trim();
                    }
                    else
                    {
                        cmd.Parameters.Add("@v_Script_In", MySqlDbType.VarChar).Value = convertedcode;
                    }
                    cmd.Parameters.Add("@n_Unicode", MySqlDbType.Int32).Value = script.Unicodestatus;
                    cmd.Parameters.Add("@v_Url", MySqlDbType.VarChar).Value = script.Pingbackurl;
                    cmd.Parameters.Add("@n_tid", MySqlDbType.VarChar).Value = script.DLTtemplateid;
                    cmd.Parameters.Add("@V_Template_Name", MySqlDbType.VarChar, 1000).Value = script.TemplateName;
                    cmd.Parameters.Add("@N_variable_count", MySqlDbType.Int32).Value = script.VariableCount;
                    cmd.Parameters.Add("@N_senderId", MySqlDbType.Int32).Value = script.SenderId;
                    cmd.Parameters.Add("@N_SMS_Type", MySqlDbType.Int32).Value = script.SMSType;
                    cmd.Parameters.Add("@V_validity", MySqlDbType.Int32).Value = script.Validity;
                    cmd.Parameters.Add("@N_Delivery_Flag", MySqlDbType.Int32).Value = script.DeliveryFlag;
                    cmd.Parameters.Add("@v_Email_Out", MySqlDbType.VarChar, 2000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Company_Name", MySqlDbType.VarChar, 2000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Status_Out", MySqlDbType.VarChar, 2000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Message_Out", MySqlDbType.VarChar, 2000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Template_Id", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();


                    }

                    EMAIL = cmd.Parameters["@v_Email_Out"].Value.ToString();
                    compname = cmd.Parameters["@v_Company_Name"].Value.ToString();
                    stsout = cmd.Parameters["@v_Status_Out"].Value.ToString();
                    statusmsg = cmd.Parameters["@v_Message_Out"].Value.ToString();
                    tempid = cmd.Parameters["@n_Template_Id"].Value.ToString();


                }


            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.Createscript::MysqlException ::{ ex.Message}");
            }
            catch (Exception ex)
            {
                LogWriter.Write($"APIDbPrc.Createscript:Exception ::{ ex.Message}");
                stsout = "-1";
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return stsout;
        }

        public string CreateInstantSMS(string key, string isUnicode, string convertedCode, out string Uniqueid, out string msg, SMSCreate sms, string time, string ip)
        {

            //api_Insert_instant_sms(IN in_api_key varchar(50), 
            //                        IN msisdn varchar(4000), 
            //                        IN script_in varchar(900), 
            //                        IN senderid bigint(21), IN n_template_id int(5), 
            //                        IN v_customer_url varchar(3000), 
            //                        IN n_sms_type int, 
            //                        IN V_api_rts varchar(50), 
            //                        OUT n_status int(5), 
            //                        OUT status_msg varchar(100), 
            //                        OUT unique_id varchar(100))


            Uniqueid = "";
            string statusOut = "-1";
            msg = "Server Error";
            //errorMsisdn = "";
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("api_Insert_instant_sms"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@in_api_key", MySqlDbType.VarChar).Value = key;
                    cmd.Parameters.Add("@msisdn", MySqlDbType.VarChar, 2000).Value = sms.msisdn;
                    if (isUnicode == "0")
                    {
                        cmd.Parameters.Add("@script_in", MySqlDbType.VarChar).Value = sms.script.Trim();
                    }
                    else
                    {
                        cmd.Parameters.Add("@script_in", MySqlDbType.VarChar).Value = convertedCode.Trim();
                    }
                    cmd.Parameters.Add("@senderid", MySqlDbType.VarChar).Value = sms.senderid;
                    cmd.Parameters.Add("@n_template_id", MySqlDbType.VarChar).Value = sms.templateid;
                    cmd.Parameters.Add("@v_customer_url", MySqlDbType.VarChar).Value = sms.pingbackurl;
                    cmd.Parameters.Add("@n_sms_type", MySqlDbType.Int32).Value = sms.smstype;
                    cmd.Parameters.Add("@V_api_rts", MySqlDbType.VarChar, 100).Value = time;
                    cmd.Parameters.Add("@ip_address", MySqlDbType.VarChar).Value = ip.Trim();
                    cmd.Parameters.Add("@n_status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@status_msg", MySqlDbType.VarChar, 2000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@unique_id", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("@error_msisdn_out", MySqlDbType.VarChar,2000).Direction = ParameterDirection.Output;

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();

                    }
                    statusOut = cmd.Parameters["@n_status"].Value.ToString();
                    Uniqueid = cmd.Parameters["@unique_id"].Value.ToString();
                    msg = cmd.Parameters["@status_msg"].Value.ToString();
                    // errorMsisdn = cmd.Parameters["@error_msisdn_out"].Value.ToString();
                    return statusOut;
                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.CreateSMS::MysqlException ::{ ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.CreateSMS::Exception ::{ ex.Message}");
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }

        public string CreateSMS(string key, string isUnicode, string convertedCode, out string Uniqueid, out string msg, SMSCreate sms, string time, string ip)
        {
            Uniqueid = "";
            string statusOut = "-1";
            msg = "Server Error";
            //errorMsisdn = "";

            try
            {
                /*Insert_instant_sms_prc(IN n_user_id bigint(21), IN msisdn bigint(12), IN script_in varchar(4000),
                 * IN senderid bigint(21), 
                 * IN n_template_id int(5),IN n_sms_type int, OUT n_status int(5),
                 * OUT status_msg varchar(1*/
                using (MySqlCommand cmd = new MySqlCommand("api_Insert_instant_sms"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@in_api_key", MySqlDbType.VarChar).Value = key;
                    cmd.Parameters.Add("@msisdn", MySqlDbType.VarChar, 2000).Value = sms.msisdn;
                    if (isUnicode == "0")
                    {
                        cmd.Parameters.Add("@script_in", MySqlDbType.VarChar).Value = sms.script.Trim();
                    }
                    else
                    {
                        cmd.Parameters.Add("@script_in", MySqlDbType.VarChar).Value = convertedCode.Trim();
                    }
                    cmd.Parameters.Add("@senderid", MySqlDbType.VarChar).Value = sms.senderid;
                    cmd.Parameters.Add("@n_template_id", MySqlDbType.VarChar).Value = sms.templateid;
                    cmd.Parameters.Add("@v_customer_url", MySqlDbType.VarChar).Value = sms.pingbackurl;
                    cmd.Parameters.Add("@n_sms_type", MySqlDbType.Int32).Value = sms.smstype;
                    cmd.Parameters.Add("@V_api_rts", MySqlDbType.VarChar, 100).Value = time;
                    cmd.Parameters.Add("@ip_address", MySqlDbType.VarChar).Value = ip.Trim();
                    cmd.Parameters.Add("@n_status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@status_msg", MySqlDbType.VarChar, 2000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@unique_id", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("@error_msisdn_out", MySqlDbType.VarChar,2000).Direction = ParameterDirection.Output;

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();

                    }
                    statusOut = cmd.Parameters["@n_status"].Value.ToString();
                    Uniqueid = cmd.Parameters["@unique_id"].Value.ToString();
                    msg = cmd.Parameters["@status_msg"].Value.ToString();
                    //errorMsisdn = cmd.Parameters["@error_msisdn_out"].Value.ToString();

                    return statusOut;
                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.CreateSMS::MysqlException ::{ ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.CreateSMS::Exception ::{ ex.Message}");
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }
        public string CheckIPValidBase(string key, string campId)
        {
            string statusOut = "-1";
            try
            {
                /*`api_baseupload_verification`(in apikey varchar(10),
                                                     in n_Camp_Id bigint(21),
                                                     out n_Status  int(5))

*/
                using (MySqlCommand cmd = new MySqlCommand("api_baseupload_verification"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@apikey", MySqlDbType.VarChar).Value = key;
                    cmd.Parameters.Add("@n_Camp_Id", MySqlDbType.VarChar).Value = campId.Trim();
                    cmd.Parameters.Add("@n_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    //  cmd.Parameters.Add("@v_sts_msg", MySqlDbType.VarChar).Direction = ParameterDirection.Output;


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@n_Status"].Value.ToString();
                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.SMSDelivery::CheckIPValidBase ::{ ex.Message}");
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.CheckIPValidBase::Exception ::{ ex.Message}");
            }
            return statusOut;
        }

        private string APIDataInsert(string key, string campname, string frmdt,
            string todt, string frmtime, string totime, string senderid, string templateid,
            string camptype, string script, string pingbackurl, string convertedCode, string isUnicode, out string resp, out string campIdOut)
        {
            // `api_Sms_Insert_Campaign_Dtls`(IN apikey varchar(10), 
            //                                    IN c_Name varchar(100), 
            //                                    IN c_Smstype int, 
            //                                    IN c_sms_template_id int, 
            //                                    IN sender_id bigint(6), 
            //                                    IN c_fromdate varchar(200), 
            //                                   IN c_todate varchar(200), 
            //                                   IN c_fromtime varchar(200), 
            //                                   IN c_totime varchar(200), 
            //                                   IN c_parameters varchar(4000), 
            //                                   IN v_customer_url varchar(3000), 
            //                                   OUT n_Camp_Id varchar(100), 
            //                                   OUT n_Status int, 
            //                                    OUT v_Status varchar(1000))

            string status = "";
            resp = "Server Error";

            try
            {

                using (MySqlCommand cmd = new MySqlCommand("api_Sms_Insert_Campaign_Dtls"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@apikey", MySqlDbType.VarChar).Value = key;
                    cmd.Parameters.Add("@c_Name", MySqlDbType.VarChar).Value = campname;
                    cmd.Parameters.Add("@c_Smstype", MySqlDbType.Int32).Value = Convert.ToInt32(camptype);
                    cmd.Parameters.Add("@c_sms_template_id", MySqlDbType.Int32).Value = Convert.ToInt32(templateid);
                    cmd.Parameters.Add("@sender_id", MySqlDbType.Int32).Value = Convert.ToInt32(senderid);
                    cmd.Parameters.Add("@c_fromdate", MySqlDbType.VarChar).Value = frmdt.Trim();
                    cmd.Parameters.Add("@c_todate", MySqlDbType.VarChar).Value = todt.Trim();
                    cmd.Parameters.Add("@c_fromtime", MySqlDbType.VarChar).Value = frmtime.Trim();
                    cmd.Parameters.Add("@c_totime", MySqlDbType.VarChar).Value = totime.Trim();
                    if (isUnicode == "0")
                    {
                        cmd.Parameters.Add("@c_parameters", MySqlDbType.VarChar, 2000).Value = script.Trim();
                    }
                    else
                    {
                        cmd.Parameters.Add("@c_parameters", MySqlDbType.VarChar, 2000).Value = convertedCode.Trim();
                    }
                    cmd.Parameters.Add("@v_customer_url", MySqlDbType.VarChar).Value = pingbackurl;

                    cmd.Parameters.Add("@n_Camp_Id", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Status", MySqlDbType.VarChar, 2000).Direction = ParameterDirection.Output;


                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    int statusOut = Convert.ToInt32(cmd.Parameters["@n_Status"].Value.ToString());
                    resp = cmd.Parameters["@v_Status"].Value.ToString();
                    campIdOut = cmd.Parameters["@n_Camp_Id"].Value.ToString();
                    return statusOut.ToString();
                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.APIDataInsert::{ ex.Message}");
                campIdOut = "";
                return status = "-1";
            }
            catch (Exception ex)
            {
                campIdOut = "";
                LogWriter.Write($"DataAccess.APIDbPrcs.APIDataInsert::Exception ::{ ex.Message}");
                return status = "-1";

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }


        }
        public MailServerModel GetMailServerDetails()
        {
            //Sec_Mail_Info`(IN n_App_Id_In int,
            //OUT v_Host_Ip_Out varchar(100),
            //OUT v_Port_Out varchar(100),
            //OUT v_Uname_Out varchar(100),
            //OUT v_Password_Out varchar(100),
            //OUT v_From_Address_Out varchar(200),
            //OUT v_Display_Name_Out varchar(200),
            //OUT n_Otp_Expire_Time_Out int,
            //OUT n_Status_Out int)


            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Sec_Mail_Info"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_App_Id_In", MySqlDbType.Int32).Value = GlobalValues.AppId;

                    cmd.Parameters.Add("@v_Host_Ip_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Port_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Uname_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Password_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_From_Address_Out", MySqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Display_Name_Out", MySqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Otp_Expire_Time_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    if (cmd.Parameters["@n_Status_Out"].Value.ToString() == "1")
                    {
                        MailServerModel model = new MailServerModel();
                        model.MailServerIP = _EncDec.DecryptDes(cmd.Parameters["@v_Host_Ip_Out"].Value.ToString());// cmd.Parameters["v_Host_Ip_Out"].Value.ToString();//
                        model.Port = Convert.ToInt32(_EncDec.DecryptDes(cmd.Parameters["@v_Port_Out"].Value.ToString()));// Convert.ToInt32(cmd.Parameters["v_Port_Out"].Value.ToString());//
                        model.UserName = cmd.Parameters["@v_Uname_Out"].Value.ToString();
                        model.Password = cmd.Parameters["@v_Password_Out"].Value.ToString();
                        model.FromAddress = cmd.Parameters["@v_From_Address_Out"].Value.ToString();
                        model.DisplayName = cmd.Parameters["@v_Display_Name_Out"].Value.ToString();
                        model.OTPExpireTime = Convert.ToInt32(cmd.Parameters["@n_Otp_Expire_Time_Out"].Value.ToString());
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.getMailServerDetails :: Exception :: " + ex.Message);

            }
            return null;

        }

        public string UpdateBulkSms(string json, string campId, string apiKey, out string response)
        {
            string statusOut = "-1";
            response = "Failed";
            try
            {

                using (MySqlCommand cmd = new MySqlCommand("Web_Bulk_SMS_Api"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Lv_Key", MySqlDbType.String).Value = apiKey;
                    cmd.Parameters.Add("@v_Json", MySqlDbType.String).Value = json;
                    cmd.Parameters.Add("@V_Campaign_Id", MySqlDbType.String).Value = campId;
                    cmd.Parameters.Add("@V_response", MySqlDbType.String).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (con)
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    statusOut = cmd.Parameters["@n_Status"].Value.ToString();
                    response = cmd.Parameters["@V_response"].Value.ToString();
                }
            }
            catch (MySqlException ex)
            {
                MySqlConnection.ClearPool(con);
                con.Close();
                LogWriter.Write($"APIDbPrc.GetUnicodeStatus::MysqlException ::{ ex.Message}");
            }
            catch (Exception ex)
            {
                LogWriter.Write($"DataAccess.APIDbPrcs.GetUnicodeStatus::Exception ::{ ex.Message}");


            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return statusOut;
        }
    }
}
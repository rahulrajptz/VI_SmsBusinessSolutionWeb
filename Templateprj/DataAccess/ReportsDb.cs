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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Templateprj.DataAccess
{
    public class ReportsDb
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

        public SelectList GetSenderIdList()
        {
            string rolid = HttpContext.Current.Session["RoleID"].ToString();
            try
            {
                //  `dropdown_campaignid_Summary`(in n_userid_in bigint(21),out n_status int)
                MySqlCommand cmd = new MySqlCommand();
                MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr);
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "Web_get_campaign_name";
                cmd.CommandType = CommandType.StoredProcedure;
                if (rolid == "1")
                {
                    cmd.Parameters.Add("@N_Acc_Id", MySqlDbType.Int32).Value = null;
                }
                else
                {
                    cmd.Parameters.Add("@N_Acc_Id", MySqlDbType.Int32).Value = HttpContext.Current.Session["AccountID"];
                }
                //cmd.Parameters.Add("@n_status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                con.Close();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dtsecQs = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(dtsecQs);
                //string selectedQnId = cmd.Parameters["@n_status"].Value.ToString();
                List<SelectListItem> customList = new List<SelectListItem>();
                customList.Add(new SelectListItem { Text = "-- select --", Value = "-1" });
                return dtsecQs.ToSelectList(customList);
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.ReportsDb.GetSenderIdList :: Exception :: " + ex.Message);
                return null;
            }
        }

        public void ExportDetailedReport(DeatailedReportModel model, string name, out int status)
        {
           // `Web_get_bulksms_report`(IN N_Campaign_id bigint(20),IN FromDate varchar(50),In ToDate Varchar(50),out N_Status int)

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr);
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "Web_get_bulksms_report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@N_Campaign_id", MySqlDbType.Int32).Value = Convert.ToInt32(model.SelectedId);// model.SelectedCampaign.Trim();
                cmd.Parameters.Add("@FromDate", MySqlDbType.VarChar).Value = model.fromdate;
                cmd.Parameters.Add("@ToDate", MySqlDbType.VarChar).Value = model.todate;
                cmd.Parameters.Add("@N_Status", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                //var dataReader = cmd.ExecuteReader();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter { SelectCommand = cmd };
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                _xlsx.ExportToExcel(dataTable, name);
                status = Convert.ToInt32(cmd.Parameters["@N_Status"].Value.ToString());

            }
            catch (OutOfMemoryException)
            {
                LogWriter.Write("DataAccess.ReportsDb.ExportDetailedReport :: OutOfMemoryException");
                status = -2;
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.ReportsDb.ExportDetailedReport :: Exception :: " + ex.Message);
                status = -1;
            }
        }
        public SelectList getinterval()
        {
            //web_get_report_intervel_prc`(IN n_user_id_in int, OUT n_status_out int)



            try
            {

                using (MySqlCommand cmd = new MySqlCommand("web_get_report_intervel_prc"))
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
                LogWriter.Write("DataAccess.Reportdbprc.getinterval::Exception::" + ex.Message);
                return null;
            }
        }

        public SelectList getagent()
        {
           // web_get_agent_name_prc(IN n_user_id_in int, OUT n_status_out int)



            try
            {
                using (MySqlCommand cmd = new MySqlCommand("web_get_agent_name_prc"))
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
                LogWriter.Write("DataAccess.Reportdbprc.getinterval::Exception::" + ex.Message);
                return null;
            }
        }

        public SelectList getcampaign()
        {
           // web_get_campaign_name_prc(IN n_user_id_in int, out n_status_out int)


            try
            {

                using (MySqlCommand cmd = new MySqlCommand("web_get_campaign_name_prc"))
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
                LogWriter.Write("DataAccess.Reportdbprc.getinterval::Exception::" + ex.Message);
                return null;
            }
        }

        public SelectList getReportType()
        {


            try
            {

                using (MySqlCommand cmd = new MySqlCommand("web_get_report_types_prc"))
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
                LogWriter.Write("DataAccess.Reportdbprc.getinterval::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable getReports(feedbackreportmodel model)
        {
            //web_summary_report_prc(IN n_user_id_in int, 
            //                        IN n_campaign_id_in int, 
            //                        n_report_type_in int, 
            //                        IN n_agent_id_in int, 
            //                        IN n_date_from date, 
            //                        IN n_date_to date, 
            //                        OUT n_status_out int)



            try
            {

                using (MySqlCommand cmd = new MySqlCommand("web_summary_report_prc"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_user_id_in", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@n_campaign_id_in", MySqlDbType.Int32).Value = model.campaignName;
                    cmd.Parameters.Add("@n_report_type_in", MySqlDbType.Int32).Value = model.reportType;
                    cmd.Parameters.Add("@n_agent_id_in", MySqlDbType.Int32).Value = model.agent;
                    cmd.Parameters.Add("@n_date_from", MySqlDbType.VarChar,200).Value = model.fromdate;
                    cmd.Parameters.Add("@n_date_to", MySqlDbType.VarChar,200).Value = model.todate;
                    cmd.Parameters.Add("@n_status_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

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
                LogWriter.Write("DataAccess.reportdbprc.getReports::Exception::" + ex.Message);
                return null;
            }
        }


        public DataTable reportDownload(feedbackreportmodel model)
        {
            // web_download_summary_report_prc(IN n_user_id_in int, IN n_campaign_id_in int, n_report_type_in int, IN n_agent_id_in int, IN n_date_in int, OUT lt_data_out longtext, OUT n_status_out int)

            //web_download_summary_report_prc(IN n_user_id_in int, 
            //                                IN n_campaign_id_in int, 
            //                                n_report_type_in int, 
            //                                IN n_agent_id_in int, 
            //                                IN n_date_from date, 
            //                                IN n_date_to date, 
            //                                OUT lt_data_out longtext, 
            //                                OUT n_status_out int)



            try
            {

                using (MySqlCommand cmd = new MySqlCommand("web_download_summary_report_prc"))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_user_id_in", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@n_campaign_id_in", MySqlDbType.Int32).Value = model.campaignName;
                    cmd.Parameters.Add("@n_report_type_in", MySqlDbType.Int32).Value = model.reportType;
                    cmd.Parameters.Add("@n_agent_id_in", MySqlDbType.Int32).Value = model.agent;
                    cmd.Parameters.Add("@n_date_from", MySqlDbType.VarChar, 200).Value = model.fromdate;
                    cmd.Parameters.Add("@n_date_to", MySqlDbType.VarChar, 200).Value = model.todate;
                    cmd.Parameters.Add("@lt_data_out", MySqlDbType.LongText).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_status_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {

                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    string response = cmd.Parameters["@lt_data_out"].Value.ToString();
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
                LogWriter.Write("DataAccess.reportdbprc.getReports::Exception::" + ex.Message);
                return null;
            }
        }
        public string getfeedback(string customerId)
        {
            string response = "";

          //  web_get_customer_feedbacks_prc(IN in_user_id int, IN in_customer_dtl_id int(8), OUT v_data_out text, OUT n_status_out int, OUT v_status_out varchar(200))


            try
            {
                using (MySqlCommand cmd = new MySqlCommand("web_get_customer_feedbacks_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@in_user_id", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("@in_customer_dtl_id", MySqlDbType.Int32).Value = customerId;
                    cmd.Parameters.Add("@v_data_out", MySqlDbType.Text).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_status_out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_status_out", MySqlDbType.VarChar,200).Direction = ParameterDirection.Output;


                    DataTable dt = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    response = cmd.Parameters["@v_data_out"].Value.ToString();
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.ReportsDb.getfeedback :: Exception :: " + ex.Message);
                response = "";
                return "";
            }
        }
    }
}

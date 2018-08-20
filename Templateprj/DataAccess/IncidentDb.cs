using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Templateprj.Helpers;
using Templateprj.Models;

namespace Templateprj.DataAccess
{
    public class IncidentDb
    {

        #region Common
        public SelectList GetAllSeverity()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_severity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_severity_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllSeverity :: Exception :: " + ex.Message);
                return null;
            }
        }

        public SelectList GetAllOwner()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_owner"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_owner_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllOwner :: Exception :: " + ex.Message);
                return null;
            }
        }

        public SelectList GetAllStatus()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_inc_status"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("(all_inc_status_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllType :: Exception :: " + ex.Message);
                return null;
            }
        }

        public SelectList GetAllCustomer()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_customer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_customer_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllCustomer :: Exception :: " + ex.Message);
                return null;
            }
        }

        public SelectList GetAllLocations()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_location"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_location_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllCustomer :: Exception :: " + ex.Message);
                return null;
            }
        }



        #endregion

        #region incident

        public DataTable RegistrationIncident(IncidentModel model, HttpPostedFileBase file, out int mid, out string MailId, out string cc, out int status, out byte[] fileDatas)
        {
            fileDatas = null;
            mid = 0;
            MailId = "";
            cc = "";
            status = 0;
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_inc_reg_prc"))
                {
                    byte[] fileData = null;
                    string fileName = "", contentType = "";
                    if (file != null && file.ContentLength > 0)
                    {
                        fileData = new byte[file.ContentLength];
                        file.InputStream.Read(fileData, 0, fileData.Length);
                        file.InputStream.Close();
                        fileName = file.FileName;
                        contentType = file.ContentType;
                    }
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("id", OracleDbType.Int32).Value = model.IncidentId;
                    cmd.Parameters.Add("alert_id", OracleDbType.Int32).Value = model.AlertId;
                    cmd.Parameters.Add("system_id", OracleDbType.Int32).Value = model.SystemId;
                    cmd.Parameters.Add("severity", OracleDbType.Int32).Value = model.SelectedSeverity;
                    cmd.Parameters.Add("owner", OracleDbType.Int32).Value = model.Owner;
                    cmd.Parameters.Add("status", OracleDbType.Int32).Value = model.Staus;
                    cmd.Parameters.Add("customer_id", OracleDbType.Varchar2, 4000).Value = string.Join(",", model.CustomerImpacted);
                    cmd.Parameters.Add("IncCreatedBy", OracleDbType.Varchar2).Value = model.InsRaisedBy;
                    cmd.Parameters.Add("mailIds", OracleDbType.Varchar2).Value = model.IncMailIds;
                    cmd.Parameters.Add("problem", OracleDbType.Varchar2).Value = model.Problem;
                    cmd.Parameters.Add("impactdes", OracleDbType.Varchar2).Value = model.ImpactDes;
                    cmd.Parameters.Add("solution", OracleDbType.Varchar2).Value = model.Resolution;
                    cmd.Parameters.Add("cause", OracleDbType.Varchar2).Value = model.Cause;
                    cmd.Parameters.Add("prev_action", OracleDbType.Varchar2).Value = model.Action;
                    cmd.Parameters.Add("location", OracleDbType.Int32).Value = model.Location;
                    cmd.Parameters.Add("inc_date", OracleDbType.Varchar2).Value = model.IncidentTime;
                    //cmd.Parameters.Add("res_date", OracleDbType.Varchar2).Value = model.ResolutionTime.Trim();
                    cmd.Parameters.Add("res_date", OracleDbType.Varchar2).Value = model.ResolutionTime;
                    cmd.Parameters.Add("pending_reason", OracleDbType.Varchar2).Value = model.Pending;

                    cmd.Parameters.Add("b_Blob_In", OracleDbType.Blob, 10000).Value = fileData;

                    cmd.Parameters.Add("b_Blob_Out", OracleDbType.Blob, 10000).Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("n_Mid_Out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mailid_out", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mailid_cc", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("inc_reg_status_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mail_details_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        status = Convert.ToInt32(cmd.Parameters["inc_reg_status_out"].Value.ToString());
                        if (status == 1 || status == 2)
                        {
                            if (dataTable != null)
                            {
                                if (dataTable.Rows.Count > 0)
                                {
                                    mid = Convert.ToInt32(cmd.Parameters["n_Mid_Out"].Value.ToString());
                                    MailId = cmd.Parameters["mailid_out"].Value.ToString();
                                    cc = cmd.Parameters["mailid_cc"].Value.ToString();

                                    OracleBlob fileBlob = (OracleBlob)cmd.Parameters["b_Blob_Out"].Value;
                                    fileDatas = new byte[fileBlob.Length];
                                    fileBlob.Read(fileDatas, 0, Convert.ToInt32(fileBlob.Length));
                                }
                            }
                            return dataTable;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.Registration :: Exception :: " + ex.Message);
            }
            return null;
        }

        public DataTable GetIncidentDetails(int id)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_inc_row_details_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("in_id", OracleDbType.Int32).Value = id;

                    cmd.Parameters.Add("inc_row_details_cursor ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetIncidentDetails::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable GetSystemIncidentDetails(int id)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_alert_row_details_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("a_id", OracleDbType.Int32).Value = id;

                    cmd.Parameters.Add("alert_row_details_cursor ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetSystemIncidentDetails::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable GetAllIncidents(ViewModel model, bool download, string fileName)
        {

            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_inc_list_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dates = model.DateRange.Split(new[] { "to" }, StringSplitOptions.RemoveEmptyEntries);
                    cmd.Parameters.Add("list_from_in", OracleDbType.Varchar2).Value = dates[0].Trim();
                    cmd.Parameters.Add("list_to_in ", OracleDbType.Varchar2).Value = dates[1].Trim();
                    cmd.Parameters.Add("inc_list_cursor_out ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        var dataReader = cmd.ExecuteReader();
                        dataAdapter.Fill(dataTable);
                        if (download)
                        {
                            ExcelExtension _xlx = new ExcelExtension();
                            _xlx.ExportToCSV(dataReader, fileName);
                        }
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllIncidents::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable GetAllOpenIncidents(bool download, string fileName)
        {

            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_inc_list_open_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("inc_list_open_cursor_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        var dataReader = cmd.ExecuteReader();
                        dataAdapter.Fill(dataTable);
                        if (download)
                        {
                            ExcelExtension _xlx = new ExcelExtension();
                            _xlx.ExportToCSV(dataReader, fileName);
                        }
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllOpenIncidents::Exception::" + ex.Message);
                return null;
            }
        }

        public DataSet MonthlyRep(ViewModel model)
        {

            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_monthly_report"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dates = model.DateRange.Split(new[] { "to" }, StringSplitOptions.RemoveEmptyEntries);
                    cmd.Parameters.Add("from_in", OracleDbType.Varchar2).Value = dates[0].Trim();
                    cmd.Parameters.Add("to_in ", OracleDbType.Varchar2).Value = dates[1].Trim();
                    cmd.Parameters.Add("monthly_report_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("owner_report_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("owner_dt_report_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataset = new DataSet();
                        //var datatable = new DataTable();
                        //svar dataReader = cmd.ExecuteReader();
                        dataAdapter.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllOpenIncidents::Exception::" + ex.Message);
                return null;
            }
        }

        public DataSet Weektrend(ViewModel model)
        {

            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_weekly_trend_report"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dates = model.DateRange.Split(new[] { "to" }, StringSplitOptions.RemoveEmptyEntries);
                    cmd.Parameters.Add("from_in", OracleDbType.Varchar2).Value = dates[0].Trim();
                    cmd.Parameters.Add("to_in ", OracleDbType.Varchar2).Value = dates[1].Trim();
                    cmd.Parameters.Add("total_ticket_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("prutech_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("idea_mpls_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("idea_nws_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataset = new DataSet();
                        //var datatable = new DataTable();
                        //svar dataReader = cmd.ExecuteReader();
                        dataAdapter.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllOpenIncidents::Exception::" + ex.Message);
                return null;
            }
        }

        public DataSet Monthtrend(ViewModel model)
        {

            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_monthly_trend_report"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dates = model.DateRange.Split(new[] { "to" }, StringSplitOptions.RemoveEmptyEntries);
                    cmd.Parameters.Add("from_in", OracleDbType.Varchar2).Value = dates[0].Trim();
                    cmd.Parameters.Add("to_in ", OracleDbType.Varchar2).Value = dates[1].Trim();
                    cmd.Parameters.Add("total_ticket_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("prutech_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("idea_mpls_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("idea_nws_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataset = new DataSet();
                        //var datatable = new DataTable();
                        //svar dataReader = cmd.ExecuteReader();
                        dataAdapter.Fill(dataset);

                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllOpenIncidents::Exception::" + ex.Message);
                return null;
            }
        }

        public DataSet Datasets()
        {
            //filename = "name1,name2,name3";
            DataTable hai = new DataTable();
            DataTable table2 = new DataTable();

            DataColumn dc11 = new DataColumn("ID", typeof(Int32));
            DataColumn dc12 = new DataColumn("Name", typeof(string));
            DataColumn dc13 = new DataColumn("City", typeof(string));

            DataColumn dc16 = new DataColumn("ID", typeof(Int32));
            DataColumn dc14 = new DataColumn("Name", typeof(string));
            DataColumn dc15 = new DataColumn("City", typeof(string));

            hai.Columns.Add(dc11);
            hai.Columns.Add(dc12);
            hai.Columns.Add(dc13);

            hai.Rows.Add(111, "Amit Kumar", "Jhansi");
            hai.Rows.Add(222, "Rajesh Tripathi", "Delhi");
            hai.Rows.Add(333, "Vineet Saini", "Patna");
            hai.Rows.Add(444, "Deepak Dwij", "Noida");

            table2.Columns.Add(dc16);
            table2.Columns.Add(dc14);
            table2.Columns.Add(dc15);

            table2.Rows.Add(11, "Amit ", "si");
            table2.Rows.Add(2, "Rpathi", "Dhi");
            table2.Rows.Add(33, "Vineet Sai", "atna");
            table2.Rows.Add(44, "Deepakj", "Nda");

            DataSet dset = new DataSet();
            dset.Tables.Add(hai);
            dset.Tables.Add(table2);

            return dset;

        }

        public SelectList incidentRaisedBy()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_inc_raised_by"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("inc_raised_by_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllOwner :: Exception :: " + ex.Message);
                return null;
            }
        }


        #endregion

        #region Qrc

        public DataTable GetAllOpenQrc(bool download, string fileName)
        {

            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_qrc_list_open_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("qrc_list_open_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        var dataReader = cmd.ExecuteReader();
                        dataAdapter.Fill(dataTable);
                        if (download)
                        {
                            ExcelExtension _xlx = new ExcelExtension();
                            _xlx.ExportToCSV(dataReader, fileName);
                        }
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllOpenIncidents::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable GetAllQrc(ViewModel model, bool download, string fileName)
        {
            try
            {

                using (OracleCommand cmd = new OracleCommand("ims_qrc_list_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dates = model.DateRange.Split(new[] { "to" }, StringSplitOptions.RemoveEmptyEntries);
                    cmd.Parameters.Add("list_from_in", OracleDbType.Varchar2).Value = dates[0].Trim();
                    cmd.Parameters.Add("list_to_in ", OracleDbType.Varchar2).Value = dates[1].Trim();
                    cmd.Parameters.Add("qrc_list_cursor ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        var dataReader = cmd.ExecuteReader();
                        dataAdapter.Fill(dataTable);
                        if (download)
                        {
                            ExcelExtension _xlx = new ExcelExtension();
                            _xlx.ExportToCSV(dataReader, fileName);
                            //_xlx.ExportToCSV(dataTable, "");
                            //_xlx.ExportTo_CSV(dataReader, fileName,"1");
                        }
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllQrc::Exception::" + ex.Message);
                return null;
            }
        }

        public SelectList GetAllTypeQrc()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_type"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_type_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllTypeQrc :: Exception :: " + ex.Message);
                return null;
            }
        }

        public SelectList GetAllCusNameQrc()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_customer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_customer_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllCusNameQrc :: Exception :: " + ex.Message);
                return null;
            }
        }

        public SelectList GetAllLocationsQrc()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_location"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_location_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllLocationsQrc :: Exception :: " + ex.Message);
                return null;
            }
        }

        public SelectList GetAllComStatusQrc()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_comp_status"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_comp_status_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllComStatusQrc :: Exception :: " + ex.Message);
                return null;
            }
        }

        public int RegistrationQrc(QrcModel model)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_qrc_reg_prc"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("id", OracleDbType.Int32).Value = model.QrcId;
                    cmd.Parameters.Add("alert_id", OracleDbType.Int32).Value = model.AlertId;
                    cmd.Parameters.Add("system_id", OracleDbType.Int32).Value = model.SystemId;
                    cmd.Parameters.Add("complaint_date", OracleDbType.Varchar2).Value = model.ComplaintDate;
                    cmd.Parameters.Add("complaint_type", OracleDbType.Int32).Value = model.ComplaintType;
                    cmd.Parameters.Add("cust_id", OracleDbType.Int32).Value = model.CusName;
                    cmd.Parameters.Add("type", OracleDbType.Int32).Value = model.Type;
                    cmd.Parameters.Add("problem", OracleDbType.Varchar2).Value = model.ProblemStmt;

                    cmd.Parameters.Add("loc_id", OracleDbType.Int32).Value = model.Location;
                    cmd.Parameters.Add("rdn1", OracleDbType.Varchar2).Value = model.Rdn;
                    cmd.Parameters.Add("status1", OracleDbType.Int32).Value = model.SelectedStatus;
                    cmd.Parameters.Add("rca1", OracleDbType.Varchar2).Value = model.Rca;
                    cmd.Parameters.Add("solution1", OracleDbType.Varchar2).Value = model.Solution;
                    cmd.Parameters.Add("close_date", OracleDbType.Varchar2).Value = model.ClosingDate;
                    cmd.Parameters.Add("qrc_reg_status_out", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    int status = Convert.ToInt32(cmd.Parameters["qrc_reg_status_out"].Value.ToString());
                    if (status == 1)
                    {
                        return status;
                    }

                }

            }
            catch (Exception ex) { LogWriter.Write("DataAccess.IncidentDb.RegistrationQrc :: Exception :: " + ex.Message); }
            return 0;
        }

        public DataTable GetQrcDetails(int id)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_qrc_row_details_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("qr_id ", OracleDbType.Int32).Value = id;

                    cmd.Parameters.Add("qrc_row_details_cursor ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetIncidentDetails::Exception::" + ex.Message);
                return null;
            }
        }

        public SelectList GetAllComplaintTypeQrc()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_comp_type"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_comp_type_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select --", Value = "" });

                        return dt.ToSelectList(customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllComplaintTypeQrc :: Exception :: " + ex.Message);
                return null;
            }
        }

        #endregion

        #region flipkart

        public DataTable GetFlipDetails(int id)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_flip_alert_row_details_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("fa_id ", OracleDbType.Int32).Value = id;

                    cmd.Parameters.Add("flip_alert_row_details_cursor ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetFlipDetails::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable GetAllFlipkartAlerts()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_flip_alert_list_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("flip_alert_list_cursor ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllFlipkartAlerts::Exception::" + ex.Message);
                return null;
            }
        }

        public bool DeleteFlipAlerts(string id)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_flip_alert_del_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("fl_id", OracleDbType.Int32).Value = id;


                    cmd.Parameters.Add("flip_alert_del_status", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    int status = Convert.ToInt32(cmd.Parameters["flip_alert_del_status"].Value.ToString());
                    if (status == 1)
                    {

                        return true;
                    }

                }

            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.DeleteFlipAlerts :: Exception :: " + ex.Message);
            }
            return false;
        }

        #endregion

        #region System Alert



        public DataTable GetAllSystemAlerts()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_alert_list_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("alert_list_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllSystemAlerts::Exception::" + ex.Message);
                return null;
            }
        }

        public bool DeleteSysAlerts(string id)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_alert_del_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("al_id", OracleDbType.Int32).Value = id;


                    cmd.Parameters.Add("alert_del_status", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    int status = Convert.ToInt32(cmd.Parameters["alert_del_status"].Value.ToString());
                    if (status == 1)
                    {

                        return true;
                    }

                }

            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.DeleteFlipAlerts :: Exception :: " + ex.Message);
            }
            return false;
        }

        #endregion

        #region Customer 

        public bool CreateCustomer(CusModel model)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_add_customer_prc"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("cname", OracleDbType.Varchar2).Value = model.CusName;
                    cmd.Parameters.Add("circle", OracleDbType.Int32).Value = model.Location;
                    cmd.Parameters.Add("rdn1", OracleDbType.Varchar2).Value = model.Rdn;
                    cmd.Parameters.Add("tfn1", OracleDbType.Varchar2).Value = model.Tfn;

                    cmd.Parameters.Add("add_customer_status_out", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    int status = Convert.ToInt32(cmd.Parameters["add_customer_status_out"].Value.ToString());
                    if (status == 1)
                    {

                        return true;
                    }

                }

            }
            catch (Exception ex) { LogWriter.Write("DataAccess.IncidentDb.RegistrationQrc :: Exception :: " + ex.Message); }
            return false;
        }

        #endregion

        #region Owner

        public int CreateOwner(CusModel model)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_add_owner_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_owner ", OracleDbType.Varchar2).Value = model.OwnerName;
                    cmd.Parameters.Add("add_owner_status", OracleDbType.Int32).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    return Convert.ToInt32(cmd.Parameters["add_owner_status"].Value.ToString());
                }


            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.RegistrationQrc :: Exception :: " + ex.Message);
                return 0;
            }
        }

        #endregion


    }
}
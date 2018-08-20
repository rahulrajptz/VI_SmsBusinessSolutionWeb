using Oracle.DataAccess.Client;
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
    public class ProjectTrackerDb
    {

        public DataTable GetRdnsAutoComp()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_all_pt_rdn"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_pt_rdn_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        return dt;

                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllCustomer :: Exception :: " + ex.Message);
                return null;
            }
        }

        public DataTable GetTfnsAutoComp()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_all_tfn"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("all_pt_tfn_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        return dt;

                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.IncidentDb.GetAllCustomer :: Exception :: " + ex.Message);
                return null;
            }
        }

        public SelectList Locations()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_location"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("all_location_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("all_location_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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

        public SelectList Status()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_all_pt_status"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("all_pt_status_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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

        public SelectList RfpStatus()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_all_rfp_status"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("all_pt_rfp_status_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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

        public SelectList InternalUat()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_all_internal_uat"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("all_pt_internal_uat_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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

        public SelectList InternalUatResultList()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_all_int_uat_res"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("all_int_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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

        public SelectList TypeofSolution()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_all_type_of_sol"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("all_type_of_sol_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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

        public int RegistrationProjectTracker(ProjectTracker model)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_reg_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_id", OracleDbType.Varchar2).Value = model.Id;
                    cmd.Parameters.Add("v_request_from", OracleDbType.Varchar2).Value = model.RequestFrom;

                    cmd.Parameters.Add("v_project_name", OracleDbType.Varchar2).Value = model.ProjectName;
                    cmd.Parameters.Add("v_rdn", OracleDbType.Varchar2).Value = model.RDN;
                    cmd.Parameters.Add("v_circle", OracleDbType.Int32).Value = model.Circle;
                    cmd.Parameters.Add("v_status", OracleDbType.Int32).Value = model.Status;
                    cmd.Parameters.Add("v_project_receipt_dt", OracleDbType.Varchar2).Value = model.ProjectRecipientDate;
                    cmd.Parameters.Add("v_msc_config_dt", OracleDbType.Varchar2).Value = model.MSCConfigDate;
                    cmd.Parameters.Add("v_project_com_dt", OracleDbType.Varchar2).Value = model.ProjectCompletionDate;
                    cmd.Parameters.Add("v_app_config_dt", OracleDbType.Varchar2).Value = model.AppConfigDate;

                    cmd.Parameters.Add("v_rfp_rereceipt_dt", OracleDbType.Varchar2).Value = model.RerecipientofRfp;
                    cmd.Parameters.Add("v_eid_commit_dt", OracleDbType.Varchar2).Value = model.EIDCommitDate;
                    cmd.Parameters.Add("v_int_uat_result", OracleDbType.Int32).Value = model.InternalUatresult;
                    //cmd.Parameters.Add("v_eid_commit_to_client_dt", OracleDbType.Varchar2).Value = model.EIDCommitToClient;
                    //cmd.Parameters.Add("v_solution_dt", OracleDbType.Varchar2).Value = model.SolutionConfirmtoIdea;
                    cmd.Parameters.Add("v_welcome_email_dt", OracleDbType.Varchar2).Value = model.DateWelcomeEmail;

                    cmd.Parameters.Add("v_tfn", OracleDbType.Varchar2).Value = model.TFN;
                    cmd.Parameters.Add("v_imsi", OracleDbType.Varchar2).Value = model.IMSI;
                    cmd.Parameters.Add("v_customer_spoc", OracleDbType.Varchar2).Value = model.CustomerSpoc;
                    cmd.Parameters.Add("v_cust_email", OracleDbType.Varchar2).Value = model.CustomerEmail;
                    cmd.Parameters.Add("v_cust_num", OracleDbType.Varchar2).Value = model.CustomerNumber;
                    cmd.Parameters.Add("v_idea_spoc", OracleDbType.Varchar2).Value = model.IdeaSpoc;

                    cmd.Parameters.Add("v_internal_uat", OracleDbType.Int32).Value = model.InternalUat;
                    cmd.Parameters.Add("v_type_of_solution", OracleDbType.Int32).Value = model.TypeOfSolution;
                    cmd.Parameters.Add("v_rfp_status", OracleDbType.Int32).Value = model.RfpStatus;


                    cmd.Parameters.Add("v_his_status", OracleDbType.Varchar2).Value = model.cbx;
                    cmd.Parameters.Add("v_pending_reason", OracleDbType.Varchar2).Value = model.Remarks;
                    cmd.Parameters.Add("v_otherRemarks", OracleDbType.Varchar2).Value = model.OtherRemarks;

                    cmd.Parameters.Add("n_status", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    int status = Convert.ToInt32(cmd.Parameters["n_status"].Value.ToString());
                    if (status == 1)
                    {
                        return status;
                    }

                }

            }
            catch (Exception ex) { LogWriter.Write("DataAccess.IncidentDb.RegistrationQrc :: Exception :: " + ex.Message); }
            return 0;
        }

        public DataTable ProjectTrackerView(ProjectViewModel model, bool download, string fileName)
        {
            try
            {

                using (OracleCommand cmd = new OracleCommand("pt_list_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (model.Daterange != null)
                    {
                        var dates = model.Daterange.Split(new[] { "to" }, StringSplitOptions.RemoveEmptyEntries);
                        cmd.Parameters.Add("list_from_in", OracleDbType.Varchar2).Value = dates[0].Trim();
                        cmd.Parameters.Add("list_to_in", OracleDbType.Varchar2).Value = dates[1].Trim();
                    }
                    else
                    {
                        cmd.Parameters.Add("list_from_in", OracleDbType.Varchar2).Value = null;
                        cmd.Parameters.Add("list_to_in ", OracleDbType.Varchar2).Value = null;
                    }
                    cmd.Parameters.Add("v_Circle", OracleDbType.Varchar2).Value = model.Circleall;
                    cmd.Parameters.Add("v_tfn", OracleDbType.Varchar2).Value = model.TFNS;
                    cmd.Parameters.Add("v_rdn", OracleDbType.Varchar2).Value = model.RDNS;


                    cmd.Parameters.Add("pt_list_cursor_out ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


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
                LogWriter.Write("DataAccess.IncidentDb.GetAllQrc::Exception::" + ex.Message);
                return null;
            }
        }

        public DataTable GetProjectTrackerById(string id)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_row_dtls_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("id_in ", OracleDbType.Int32).Value = Convert.ToInt32(id);

                    cmd.Parameters.Add("pt_row_dtls_cursor ", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


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

        public DataTable RdnDetails(string RdnNum)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_dtls_from_rdn"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_rdn", OracleDbType.Varchar2).Value = RdnNum;

                    cmd.Parameters.Add("dtls_from_rdn_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


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

        public DataTable GetHistory(string id)
        {

            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_project_history_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("project_id_in", OracleDbType.Varchar2).Value = id;

                    cmd.Parameters.Add("project_history_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


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

        public DataTable DownLoadHistory(string id)
        {
            string fileName = "History";
            try
            {
                using (OracleCommand cmd = new OracleCommand("pt_get_project_history_prc"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("project_id_in", OracleDbType.Varchar2).Value = id;

                    cmd.Parameters.Add("project_history_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter dataAdapter = new OracleDataAdapter { SelectCommand = cmd };
                        var dataTable = new DataTable();
                        var dataReader = cmd.ExecuteReader();
                        dataAdapter.Fill(dataTable);
                        if ((dataTable.Rows[0][1]).ToString() != null)
                        {
                            fileName = fileName + (dataTable.Rows[0][1]).ToString();
                        }
                        ExcelExtension _xlx = new ExcelExtension();
                        _xlx.ExportToCSV(dataReader, fileName);
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

        public SelectList LocationsProject()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("ims_get_all_rep_location"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("all_rep_location_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dt = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dt);

                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Select Circle--", Value = "" });

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

    }
}
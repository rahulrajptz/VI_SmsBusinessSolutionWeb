using Templateprj.Helpers;
using Templateprj.Models;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.DataAccess
{
    public class AccountDbPrcs
    {
        CryptoAlg _EncDec = new CryptoAlg();
        Random _rnd = new Random();

        public int Login(LoginModel model)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_login"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_username_in", OracleDbType.Varchar2).Value = model.Username.Trim().ToLower();
                    cmd.Parameters.Add("v_password_in", OracleDbType.Varchar2).Value = _EncDec.GetHashSha1(model.Password.Trim());
                    cmd.Parameters.Add("n_appln_id_in", OracleDbType.Int32).Value = GlobalValues.AppId;
                    cmd.Parameters.Add("v_appln_name_in", OracleDbType.Varchar2).Value = GlobalValues.AppName;

                    cmd.Parameters.Add("v_circle_out", OracleDbType.Varchar2, 5).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_userid_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_pwd_required_out", OracleDbType.Int16).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_loginid_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_roleid_out", OracleDbType.Int16).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_rolename_out", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_email_out", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("d_started_out", OracleDbType.Date).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_status_out", OracleDbType.Int16).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    int status = Convert.ToInt32(cmd.Parameters["n_status_out"].Value.ToString());
                    if (status == 1)
                    {
                        int passFlag = Convert.ToInt32(cmd.Parameters["n_pwd_required_out"].Value.ToString());

                        HttpContext.Current.Session["Username"] = model.Username.Trim().ToTitleCase();
                        HttpContext.Current.Session["UserID"] = cmd.Parameters["n_userid_out"].Value.ToString();
                        HttpContext.Current.Session["PasswordFlag"] = passFlag;
                        HttpContext.Current.Session["LoginID"] = cmd.Parameters["n_loginid_out"].Value.ToString();
                        HttpContext.Current.Session["RoleID"] = cmd.Parameters["n_roleid_out"].Value.ToString();
                        HttpContext.Current.Session["EmailID"] = cmd.Parameters["v_email_out"].Value.ToString();
                        HttpContext.Current.Session["RoleName"] = cmd.Parameters["v_rolename_out"].Value.ToString();
                        HttpContext.Current.Session["StartDate"] = ((OracleDate)cmd.Parameters["d_started_out"].Value).Value;

                        if (cmd.Parameters["n_roleid_out"].Value.ToString() == "1")
                        {
                            //Administrator
                            HttpContext.Current.Session["DBConString"] = GlobalValues.ConnStr;
                        }
                        else
                        {
                            try
                            {
                                var con = GlobalValues.LocationDBs.Single(c => c.Locatn == cmd.Parameters["v_circle_out"].Value.ToString());
                                HttpContext.Current.Session["DBConString"] = con.ConStr;
                                HttpContext.Current.Session["CircleID"] = cmd.Parameters["v_circle_out"].Value.ToString();
                            }
                            catch (Exception e)
                            {
                                LogWriter.Write("DataAccess.AccountDbPrcs.Login :: Unable to get connection string from cirlce :: CircleOut:" + cmd.Parameters["v_circle_out"].Value.ToString() + " :: Exception :: " + e.Message);
                                HttpContext.Current.Session.Clear();
                                return -1;
                            }
                        }
                        return passFlag;
                    }
                    else
                    {
                        LogWriter.Write("DataAccess.AccountDbPrcs.Login :: Login Failed :: StatusOut:" + status);
                        HttpContext.Current.Session.Clear();
                        return -status;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.Login :: Exception :: " + ex.Message);
                HttpContext.Current.Session.Clear();
                return -1;
            }
        }

        public int Logout(int mode, string loginID = null)
        {
            if (loginID == null)
                loginID = HttpContext.Current.Session["LoginID"].ToString();

            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_logout"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("n_login_id_in", OracleDbType.Varchar2).Value = loginID;
                    cmd.Parameters.Add("n_mode_in", OracleDbType.Varchar2).Value = mode;

                    cmd.Parameters.Add("n_status_out", OracleDbType.Int16).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    return Convert.ToInt32(cmd.Parameters["n_status_out"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.LogOut :: Exception :: " + ex.Message);
                return -1;
            }
        }

        public int ResendOTP(out int mailId, out string OTP)
        {
            mailId = 0;
            OTP = _rnd.Next(100000, 999999).ToString();
            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_resend_otp"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("n_appln_id_in", OracleDbType.Int32).Value = GlobalValues.AppId;
                    cmd.Parameters.Add("n_user_id_in", OracleDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    cmd.Parameters.Add("n_type_in", OracleDbType.Int32).Value = HttpContext.Current.Session["OTPMethod"] == null ? "1" : HttpContext.Current.Session["OTPMethod"].ToString();
                    cmd.Parameters.Add("v_otp_in", OracleDbType.Varchar2).Value = OTP;

                    cmd.Parameters.Add("n_mid_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_status_out", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    mailId = Convert.ToInt32(cmd.Parameters["n_mid_out"].Value.ToString());

                    return Convert.ToInt32(cmd.Parameters["n_status_out"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.ResendOTP :: Exception :: " + ex.Message);
                return -1;
            }
        }

        public int FirstTimeChangePassword(FirstTimeLoginModel model, out string mailID)
        {
            mailID = "";
            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_first_time_login"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("n_userid_in", OracleDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    cmd.Parameters.Add("v_newpswd_in", OracleDbType.Varchar2).Value = _EncDec.GetHashSha1(model.NewPassword.Trim());
                    cmd.Parameters.Add("n_questid_in", OracleDbType.Int32).Value = model.SelectedSecurityQuestion;
                    cmd.Parameters.Add("v_answer_in", OracleDbType.Varchar2).Value = _EncDec.EncryptDes(model.Answer.Trim().ToLower());
                    cmd.Parameters.Add("n_appln_id_in", OracleDbType.Int32).Value = GlobalValues.AppId;

                    cmd.Parameters.Add("v_email_out", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_mid_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_status_out", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    int status = Convert.ToInt32(cmd.Parameters["n_status_out"].Value.ToString());
                    if (status == 1)
                    {
                        mailID = cmd.Parameters["v_email_out"].Value.ToString();
                        return Convert.ToInt32(cmd.Parameters["n_mid_out"].Value.ToString());
                    }
                    else
                    {
                        return -status;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.FirstTimeChangePassword :: Exception :: " + ex.Message);
                return -1;
            }
        }

        public int VerifySecurityAns(VerifySecurityQnModel model)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_verify_answer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("n_user_id_in", OracleDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    cmd.Parameters.Add("n_questid_in", OracleDbType.Int32).Value = model.SelectedSecurityQuestion;
                    cmd.Parameters.Add("v_answer_in", OracleDbType.Varchar2).Value = _EncDec.EncryptDes(model.Answer.Trim().ToLower());
                    cmd.Parameters.Add("n_appln_id_in", OracleDbType.Int32).Value = GlobalValues.AppId;

                    cmd.Parameters.Add("n_status_out", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteScalar();
                    }

                    return Convert.ToInt32(cmd.Parameters["n_status_out"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.VerifySecurityAns :: Exception :: " + ex.Message);
                return -1;
            }
        }

        public int AuthenticateUser(AuthenticateUserModel model, out string OTP, out int mid, out string mailId)
        {
            //For Reset/Forgot password Step 1
            mailId = "";
            mid = 0;
            OTP = _rnd.Next(100000, 999999).ToString();
            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_authenticate_user"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    string username;
                    if (HttpContext.Current.Session["Username"] != null)
                    {
                        username = HttpContext.Current.Session["Username"].ToString();
                    }
                    else
                    {
                        username = model.Username;
                    }

                    cmd.Parameters.Add("v_uname_in", OracleDbType.Varchar2).Value = username.ToLower().Trim();
                    cmd.Parameters.Add("n_questid_in", OracleDbType.Int32).Value = model.SelectedSecurityQuestion;
                    cmd.Parameters.Add("v_answer_in", OracleDbType.Varchar2).Value = _EncDec.EncryptDes(model.Answer.Trim().ToLower());
                    if (string.IsNullOrWhiteSpace(model.OldPwd))
                        cmd.Parameters.Add("v_old_pswd_in", OracleDbType.Varchar2).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("v_old_pswd_in", OracleDbType.Varchar2).Value = _EncDec.GetHashSha1(model.OldPwd.Trim());
                    cmd.Parameters.Add("v_otp_in", OracleDbType.Varchar2).Value = OTP;
                    cmd.Parameters.Add("n_appln_id_in", OracleDbType.Int32).Value = GlobalValues.AppId;
                    cmd.Parameters.Add("v_appln_name_in", OracleDbType.Varchar2).Value = GlobalValues.AppName;

                    cmd.Parameters.Add("v_email_out", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_mid_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_user_id_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_status_out", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    int status = Convert.ToInt32(cmd.Parameters["n_status_out"].Value.ToString());
                    if (status == 1)
                    {
                        mid = Convert.ToInt32(cmd.Parameters["n_mid_out"].Value.ToString());
                        mailId = cmd.Parameters["v_email_out"].Value.ToString();

                        HttpContext.Current.Session["UserID"] = cmd.Parameters["n_user_id_out"].Value.ToString();
                        if (HttpContext.Current.Session["RoleID"] == null)
                        { //Forgot password case
                            HttpContext.Current.Session["RoleID"] = -1;
                            HttpContext.Current.Session["Username"] = username.ToTitleCase();
                        }
                    }
                    return status;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.AuthenticateUser :: Exception :: " + ex.Message);
                return -1;
            }
        }

        public int ChangePassword(ChangePasswordModel model, out int mid, out string mailID)
        {
            mid = 0; mailID = "";
            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_change_password"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("n_userid_in", OracleDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    cmd.Parameters.Add("v_otp_in", OracleDbType.Varchar2).Value = model.OTP.Trim();
                    cmd.Parameters.Add("v_newpswd_in", OracleDbType.Varchar2).Value = _EncDec.GetHashSha1(model.NewPassword.Trim());
                    cmd.Parameters.Add("n_appln_id_in", OracleDbType.Int32).Value = GlobalValues.AppId;
                    cmd.Parameters.Add("v_appln_name_in", OracleDbType.Varchar2).Value = GlobalValues.AppName;

                    cmd.Parameters.Add("v_email_out", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_mid_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_status_out", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    int status = Convert.ToInt32(cmd.Parameters["n_status_out"].Value.ToString());
                    if (status == 1)
                    {
                        mid = Convert.ToInt32(cmd.Parameters["n_mid_out"].Value.ToString());
                        mailID = cmd.Parameters["v_email_out"].Value.ToString();
                    }
                    return status;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.ChangePassword :: Exception :: " + ex.Message);
                return -1;
            }
        }

        public SelectList GetSecurityQuestion()
        {
            int userID = -1;
            if (HttpContext.Current.Session["UserID"] != null)
                userID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_get_all_question"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("n_userid_in", OracleDbType.Int32).Value = userID;
                    cmd.Parameters.Add("c_ref_out", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_questid_out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        OracleDataAdapter da = new OracleDataAdapter("", con);
                        DataTable dtsecQs = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dtsecQs);

                        string selectedQnId = cmd.Parameters["n_questid_out"].Value.ToString();
                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-- Please select a question --" });

                        return dtsecQs.ToSelectList(selectedQnId, customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.GetSecurityQuestion :: Exception :: " + ex.Message);
                return null;
            }
        }

        public MailServerModel GetMailServerDetails()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("sec_mail_info"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("n_App_Id_In", OracleDbType.Varchar2).Value = GlobalValues.AppId;

                    cmd.Parameters.Add("v_Host_Ip_Out", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_Port_Out", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_Uname_Out", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_Password_Out", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_From_Address_Out", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_Display_Name_Out", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_Otp_Expire_Time_Out", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("n_Status_Out", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    if (cmd.Parameters["n_Status_Out"].Value.ToString() == "1")
                    {
                        MailServerModel model = new MailServerModel();
                        model.MailServerIP =  _EncDec.DecryptDes(cmd.Parameters["v_Host_Ip_Out"].Value.ToString());// cmd.Parameters["v_Host_Ip_Out"].Value.ToString();//
                        model.Port = Convert.ToInt32(_EncDec.DecryptDes(cmd.Parameters["v_Port_Out"].Value.ToString()));// Convert.ToInt32(cmd.Parameters["v_Port_Out"].Value.ToString());//
                        model.UserName = cmd.Parameters["v_Uname_Out"].Value.ToString();
                        model.Password = cmd.Parameters["v_Password_Out"].Value.ToString();
                        model.FromAddress = cmd.Parameters["v_From_Address_Out"].Value.ToString();
                        model.DisplayName = cmd.Parameters["v_Display_Name_Out"].Value.ToString();
                        model.OTPExpireTime = Convert.ToInt32(cmd.Parameters["n_Otp_Expire_Time_Out"].Value.ToString());
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.getMailServerDetails :: Exception :: " + ex.Message);
            }
            return null;
        }

        public void UpdateMailStatus(int mID, int Status)
        {
            try
            {
                using (OracleCommand occmd = new OracleCommand("sec_mail_update"))
                {
                    occmd.CommandType = CommandType.StoredProcedure;

                    occmd.Parameters.Add("n_mid_in", OracleDbType.Int32).Value = mID;
                    occmd.Parameters.Add("n_status_in", OracleDbType.Int32).Value = Status;
                    using (OracleConnection con = new OracleConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        occmd.Connection = con;
                        occmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDbPrcs.UpdateMailStatus :: Exception :: " + ex.Message);
            }
        }
    }
}
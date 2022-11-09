using MySql.Data.MySqlClient;
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
    public class AccountDbPrcs
    {
        CryptoAlg _EncDec = new CryptoAlg();
        Random _rnd = new Random();
        public int Login(LoginModel model, out string response)
        {
            response = "";
            //Sec_Login(IN v_Username_In varchar(5000), 
            //IN v_Password_In varchar(5000),
            //IN n_Appln_Id_In bigint, 
            //IN v_Appln_Name_In varchar(1000), 
            //OUT v_Circle_Out varchar(100),
            //OUT n_Userid_Out bigint, 
            //OUT n_Pwd_Required_Out bigint, 
            //OUT n_Loginid_Out bigint,
            //OUT n_Roleid_Out bigint, 
            //OUT v_Rolename_Out varchar(1000), 
            //OUT v_Email_Out varchar(1000),
            //OUT d_Started_Out date, 
            //OUT n_Status_Out int, 
            //OUT v_comapny_out varchar(100))

            //`Sec_Login_new`(IN v_Username_In varchar(5000), 
            //                IN v_Password_In varchar(5000), 
            //                IN n_Appln_Id_In bigint, 
            //                IN v_Appln_Name_In varchar(1000), 
            //                OUT v_Circle_Out varchar(100), 
            //                OUT n_Userid_Out bigint, 
            //                OUT n_Pwd_Required_Out bigint, 
            //                OUT n_Loginid_Out bigint, 
            //                OUT n_Roleid_Out bigint, 
           //                     OUT v_Rolename_Out varchar(1000), 
            //                OUT v_Email_Out varchar(1000), 
            //                OUT d_Started_Out date, 
            //                OUT n_Status_Out int, 
            //                    OUT v_comapny_out varchar(100),
            //                OUT v_Status_out varchar(200),
            //                Out v_acc_id int)

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Sec_Login_new"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@v_Username_In", MySqlDbType.VarChar, 5000).Value = model.Username.Trim().ToLower();
                    cmd.Parameters.Add("@v_Password_In", MySqlDbType.VarChar, 5000).Value = _EncDec.GetHashSha1(model.Password.Trim());
                    cmd.Parameters.Add("@n_Appln_Id_In", MySqlDbType.Int32).Value = GlobalValues.AppId;
                    cmd.Parameters.Add("@v_Appln_Name_In", MySqlDbType.VarChar, 1000).Value = GlobalValues.AppName;
                    cmd.Parameters.Add("@v_Circle_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Userid_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Pwd_Required_Out", MySqlDbType.Int16).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Loginid_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Roleid_Out", MySqlDbType.Int16).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Rolename_Out", MySqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Email_Out", MySqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@d_Started_Out", MySqlDbType.Date).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int16).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_comapny_out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_Status_out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@v_acc_id", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    int status = Convert.ToInt32(cmd.Parameters["@n_Status_Out"].Value.ToString());
                    response = (cmd.Parameters["@v_Status_out"].Value.ToString());

                    if (status == 1)
                    {
                        int passFlag = Convert.ToInt32(cmd.Parameters["@n_Pwd_Required_Out"].Value.ToString());

                        HttpContext.Current.Session["Username"] = model.Username.Trim().ToTitleCase();
                        HttpContext.Current.Session["UserID"] = cmd.Parameters["@n_Userid_Out"].Value.ToString();
                        HttpContext.Current.Session["PasswordFlag"] = passFlag;
                        HttpContext.Current.Session["LoginID"] = cmd.Parameters["@n_Loginid_Out"].Value.ToString();
                        HttpContext.Current.Session["RoleID"] = cmd.Parameters["@n_Roleid_Out"].Value.ToString();
                        HttpContext.Current.Session["EmailID"] = cmd.Parameters["@v_Email_Out"].Value.ToString();
                        HttpContext.Current.Session["RoleName"] = cmd.Parameters["@v_Rolename_Out"].Value.ToString();
                        HttpContext.Current.Session["StartDate"] = cmd.Parameters["@d_Started_Out"].Value;
                        HttpContext.Current.Session["AccountID"] = cmd.Parameters["@v_acc_id"].Value.ToString();
                        //Administrator
                        HttpContext.Current.Session["DBConString"] = GlobalValues.ConnStr;




                        return passFlag;
                    }
                    else
                    {
                        LogWriter.Write("DataAccess.AccountDb.Login :: Login Failed :: StatusOut:" + status);
                        HttpContext.Current.Session.Clear();
                        return -status;
                    }
                }
            }

            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.Login :: Exception :: " + ex.Message);
                HttpContext.Current.Session.Clear();
                return -1;
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

        public int FirstTimeChangePassword(FirstTimeLoginModel model, out string mailID)
        {
            mailID = "";
            //Sec_First_Time_Login`(IN n_Userid_In varchar(100),
            //IN v_Newpswd_In varchar(100),
            //IN n_Questid_In int,
            //IN v_Answer_In varchar(100),
            //IN n_Appln_Id_In int,
            //OUT v_Email_Out varchar(100),
            //OUT n_Mid_Out int,
            //OUT n_Status_Out int)

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Sec_First_Time_Login"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_Userid_In", MySqlDbType.VarChar, 100).Value = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    cmd.Parameters.Add("@v_Newpswd_In", MySqlDbType.VarChar, 100).Value = _EncDec.GetHashSha1(model.NewPassword.Trim());
                    cmd.Parameters.Add("@n_Questid_In", MySqlDbType.Int32).Value = model.SelectedSecurityQuestion;
                    cmd.Parameters.Add("@v_Answer_In", MySqlDbType.VarChar, 100).Value = _EncDec.EncryptDes(model.Answer.Trim());
                    cmd.Parameters.Add("@n_Appln_Id_In", MySqlDbType.Int32).Value = GlobalValues.AppId;

                    cmd.Parameters.Add("@v_Email_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Mid_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    int status = Convert.ToInt32(cmd.Parameters["@n_Status_Out"].Value.ToString());
                    if (status == 1)
                    {
                        mailID = cmd.Parameters["@v_Email_Out"].Value.ToString();
                        return Convert.ToInt32(cmd.Parameters["@n_Mid_Out"].Value.ToString());
                    }
                    else
                    {
                        return -status;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.FirstTimeChangePassword :: Exception :: " + ex.Message);
                return -1;
            }
        }


        public SelectList GetSecurityQuestion()
        {
            int userID = -1;
            if (HttpContext.Current.Session["UserID"] != null)
                userID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

            //Sec_Get_All_Question`(IN n_Userid_In int,
            //OUT n_Questid_Out int)

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Sec_Get_All_Question"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@n_Userid_In", MySqlDbType.Int32).Value = userID;
                    cmd.Parameters.Add("@n_Questid_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        MySqlDataAdapter da = new MySqlDataAdapter("", con);
                        DataTable dtsecQs = new DataTable();
                        da.SelectCommand = cmd;
                        da.Fill(dtsecQs);

                        string selectedQnId = cmd.Parameters["@n_Questid_Out"].Value.ToString();
                        List<SelectListItem> customList = new List<SelectListItem>();
                        customList.Add(new SelectListItem { Text = "-Security Question-" });

                        return dtsecQs.ToSelectList(/*selectedQnId,*/ customList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.GetSecurityQuestion :: Exception :: " + ex.Message);
                return null;
            }
        }

        public int Logout(int mode, string loginID = null, string userid = null)
        {

            //`Sec_Logout`(IN n_Login_Id_In int,
            //IN n_Mode_In int,
            //OUT n_Status_Out int)

            if (loginID == null)
                loginID = HttpContext.Current.Session["LoginID"].ToString();
            if (userid == null)
                userid = HttpContext.Current.Session["UserID"].ToString();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Sec_Logout"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_Login_Id_In", MySqlDbType.Int32).Value = loginID;
                    cmd.Parameters.Add("@n_Mode_In", MySqlDbType.Int32).Value = mode;
                    //cmd.Parameters.Add("@n_User_In", MySqlDbType.VarChar).Value = userid;
                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int16).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    return Convert.ToInt32(cmd.Parameters["@n_Status_Out"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.Logout :: Exception :: " + ex.Message);
                return -1;
            }

        }

        public int ResendOTP(out int mailId, out string OTP)
        {
            mailId = 0;
            OTP = _rnd.Next(100000, 999999).ToString();

            //Sec_Resend_Otp`(IN n_Appln_Id_In int,
            //IN n_User_Id_In int,
            //IN v_Otp_In varchar(100),
            //OUT n_Mid_Out int,
            //OUT n_Status_Out int)

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Sec_Resend_Otp"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_Appln_Id_In", MySqlDbType.Int32).Value = GlobalValues.AppId;
                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["UserID"].ToString();
                    //cmd.Parameters.Add("@n_Type_In", MySqlDbType.Int32).Value = HttpContext.Current.Session["OTPMethod"] == null ? "1" : HttpContext.Current.Session["OTPMethod"].ToString();
                    cmd.Parameters.Add("@v_Otp_In", MySqlDbType.VarChar, 100).Value = OTP;

                    cmd.Parameters.Add("@n_Mid_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    mailId = Convert.ToInt32(cmd.Parameters["@n_Mid_Out"].Value.ToString());

                    return Convert.ToInt32(cmd.Parameters["@n_Status_Out"].Value.ToString());
                }
            }

            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.ResendOTP :: Exception :: " + ex.Message);
                return -1;
            }


        }

        public int VerifySecurityAns(VerifySecurityQnModel model)
        {

            //`Sec_Verify_Answer`(IN n_User_Id_In bigint,
            //IN n_Questid_In bigint,
            //IN v_Answer_In varchar(100),
            //IN n_Appln_Id_In int,
            //OUT n_Status_Out int)

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Sec_Verify_Answer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_User_Id_In", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    cmd.Parameters.Add("@n_Questid_In", MySqlDbType.Int32).Value = model.SelectedSecurityQuestion;
                    cmd.Parameters.Add("@v_Answer_In", MySqlDbType.VarChar, 100).Value = _EncDec.EncryptDes(model.Answer.Trim());
                    cmd.Parameters.Add("@n_Appln_Id_In", MySqlDbType.Int32).Value = GlobalValues.AppId;

                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteScalar();
                    }

                    return Convert.ToInt32(cmd.Parameters["@n_Status_Out"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.VerifySecurityAns :: Exception :: " + ex.Message);
                return -1;
            }

        }

        public int AuthenticateUser(AuthenticateUserModel model, out string OTP, out int mid, out string mailId,out string response)
        {
            //For Reset/Forgot password Step 1
            mailId = "";
            mid = 0;
            OTP = _rnd.Next(100000, 999999).ToString();
            response = "";


            MySqlConnection con = null;
            try
            {

                //`Sec_Authenticate_User`(IN v_username_In varchar(100),
                //IN n_Questid_In bigint,
                //IN v_Answer_In varchar(1000),
                //IN v_Old_Pswd_In varchar(1000),
                //IN v_Otp_In varchar(1000),
                //IN n_Appln_Id_In bigint(11),
                //IN v_Appln_Name_In varchar(1000),
                //OUT v_Email_Out varchar(1000),
                //OUT n_Mid_Out bigint(11),
                //OUT n_User_Id_Out bigint(11),
                //OUT n_Status_Out int,
                //OUT V_response varchar(1000))

                MySqlCommand cmd = new MySqlCommand();
                con = new MySqlConnection(GlobalValues.ConnStr);
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "Sec_Authenticate_User";
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

                cmd.Parameters.Add("@v_username_In", MySqlDbType.VarChar).Value = username.ToLower().Trim();
                cmd.Parameters.Add("@n_Questid_In", MySqlDbType.Int32).Value = model.SelectedSecurityQuestion;
                cmd.Parameters.Add("@v_Answer_In", MySqlDbType.VarChar).Value = _EncDec.EncryptDes(model.Answer.Trim());
                if (string.IsNullOrWhiteSpace(model.OldPwd))
                    cmd.Parameters.Add("@v_Old_Pswd_In", MySqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@v_Old_Pswd_In", MySqlDbType.VarChar).Value =/* _EncDec.GetHashSha1*/(model.OldPwd.Trim());
                cmd.Parameters.Add("@v_Otp_In", MySqlDbType.VarChar).Value = OTP;
                cmd.Parameters.Add("@n_Appln_Id_In", MySqlDbType.Int32).Value = GlobalValues.AppId;
                cmd.Parameters.Add("@v_Appln_Name_In", MySqlDbType.VarChar).Value = GlobalValues.AppName;

                cmd.Parameters.Add("@v_Email_Out", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@n_Mid_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@n_User_Id_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@V_response", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                con.Close();
                int status = Convert.ToInt32(cmd.Parameters["@n_Status_Out"].Value.ToString());
                response = cmd.Parameters["@V_response"].Value.ToString();

                if (status == 1)
                {
                    mid = Convert.ToInt32(cmd.Parameters["@n_Mid_Out"].Value.ToString());
                    mailId = cmd.Parameters["@v_Email_Out"].Value.ToString();

                    HttpContext.Current.Session["UserID"] = cmd.Parameters["@n_User_Id_Out"].Value.ToString();
                    if (HttpContext.Current.Session["RoleID"] == null)
                    { //Forgot password case
                        HttpContext.Current.Session["RoleID"] = -1;
                        HttpContext.Current.Session["Username"] = username.ToTitleCase();
                    }
                }
                return status;


            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.AuthenticateUser :: Exception :: " + ex.Message);
                if (con != null)
                    con.Close();

                return -1;
            }

        }

        public int ChangePassword(ChangePasswordModel model, out int mid, out string mailID)
        {
            mid = 0; mailID = "";

            try
            {

                //sec_change_password(n_userid_in     IN NUMBER,
                //                                  v_otp_in        IN VARCHAR2,
                //                                  v_newpswd_in    IN VARCHAR2,
                //                                  n_appln_id_in   IN NUMBER,
                //                                  v_appln_name_in IN VARCHAR2,
                //                                  v_email_out     OUT VARCHAR2,
                //                                  n_mid_out       OUT NUMBER,
                //                                  n_status_out    OUT NUMBER)

                using (MySqlCommand cmd = new MySqlCommand("Sec_Change_Password"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@n_Userid_In", MySqlDbType.Int32).Value = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    cmd.Parameters.Add("@v_Otp_In", MySqlDbType.VarChar).Value = model.OTP.Trim();
                    cmd.Parameters.Add("@v_Newpswd_In", MySqlDbType.VarChar).Value = _EncDec.GetHashSha1(model.NewPassword.Trim());
                    cmd.Parameters.Add("@n_Appln_Id_In", MySqlDbType.Int32).Value = GlobalValues.AppId;
                    cmd.Parameters.Add("@v_Appln_Name_In", MySqlDbType.VarChar).Value = GlobalValues.AppName;

                    cmd.Parameters.Add("@v_Email_Out", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Mid_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@n_Status_Out", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }

                    int status = Convert.ToInt32(cmd.Parameters["@n_Status_Out"].Value.ToString());
                    if (status == 1)
                    {
                        mid = Convert.ToInt32(cmd.Parameters["@n_Mid_Out"].Value.ToString());
                        mailID = cmd.Parameters["@v_Email_Out"].Value.ToString();
                    }
                    return status;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.ChangePassword :: Exception :: " + ex.Message);
                return -1;
            }

        }

        public void UpdateMailStatus(int mID, int Status)
        {
            //`Sec_Mail_Update`(IN n_Mid_In int,
            //IN n_Status_In int)




            try
            {
                using (MySqlCommand occmd = new MySqlCommand("Sec_Mail_Update"))
                {
                    occmd.CommandType = CommandType.StoredProcedure;

                    occmd.Parameters.Add("@n_Mid_In", MySqlDbType.Int32).Value = mID;
                    occmd.Parameters.Add("@n_Status_In", MySqlDbType.Int32).Value = Status;
                    using (MySqlConnection con = new MySqlConnection(GlobalValues.ConnStr))
                    {
                        con.Open();
                        occmd.Connection = con;
                        occmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write("DataAccess.AccountDb.UpdateMailStatus :: Exception :: " + ex.Message);
            }

        }

    }
}
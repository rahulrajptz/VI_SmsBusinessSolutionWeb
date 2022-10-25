using Templateprj.Filters;
using System;
using System.Web.Mvc;
using Templateprj.DataAccess;
using Templateprj.Helpers;
using Templateprj.Models;
using CaptchaMvc.HtmlHelpers;

namespace Templateprj.Controllers
{
    public partial class AccountController : Controller
    {
        private readonly AccountDbPrcs _prc = new AccountDbPrcs();
        private readonly MailSender _mailSender = new MailSender();
        // GET: Account
        public virtual ActionResult Index() => RedirectToAction("Login");
        [HttpGet]
        [AllowAnonymous]
        public string AbandonSession()
        {
            if (Session["LoginID"] != null)
                _prc.Logout(2);

            Session.Clear();
            return "";
        }
        [HttpGet]
        [AuthorizeUser]
        public string KeepALive(string id) => "";
        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Login()
        {
            // Logout 
            if (Session["LoginID"] != null)
                _prc.Logout(0);

            Session.Clear();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Login(LoginModel model)
        {
            if (Session["LoginID"] != null)
                _prc.Logout(0);
            Session.Clear();

            if (ModelState.IsValid)
            {
                int nextAction = _prc.Login(model,out string response);
              

                switch (nextAction)
                {
                    case -9:
                        ViewBag.ErrorMsg = response;
                        break;
                    case -5:
                        ViewBag.ErrorMsg = "Some error occured !";
                        break;
                    case -1:
                        ViewBag.ErrorMsg = "Service Unavailable !";
                        break;
                    case 0:  // Login success
                        {
                            if (Session["RoleID"].ToString() == "4" || Session["RoleID"].ToString() == "2") //admin & support
                                //return RedirectToAction(MVC.Home.Insights());
                                return RedirectToAction(MVC.Campaign.BulkSms());
                            
                            else
                                //return RedirectToAction(MVC.Home.Insights());
                                return RedirectToAction(MVC.Campaign.BulkSms());

                        }
                    case 1:  // First time login/Password changed from db by developer -- email/phone may verified(verified or not checking is by verifyaccount action)
                        return RedirectToAction(MVC.Account.FirstTimeLogin());
                        //return RedirectToAction("CamelView", "Camel");
                    case 3:  // Password expired
                    case 10: // Password expires today      		  |
                    case 11: // Password expires tomarrow					|  user can skip
                    case 12: // Password expires in 2 days				|  reset password
                    case 13: // Password expires in 3 days				|
                        return RedirectToAction(MVC.Account.ResetPassword());
                    case 7:  // Already LogedIn, Ask Security Question
                        return RedirectToAction(MVC.Account.SecurityQuestion());
                    case 15:  // Account details not updated
                        return RedirectToAction(MVC.Management.Account());

                    default:
                        return View("Unauthorized");
                }
            }
            return View();
        }
        [HttpGet]
      //  [AuthorizeUser]
        public virtual ActionResult FirstTimeLogin()
        {
            var model = new FirstTimeLoginModel();
            model.SecurityQuestions = _prc.GetSecurityQuestion();

            return View("FirstTimeLogin", model);
        }
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public virtual ActionResult FirstTimeLogin(FirstTimeLoginModel model)
        {
            if (ModelState.IsValid)
            {
                string mailId;
                int mId = _prc.FirstTimeChangePassword(model, out mailId);

                if (mId > 0)
                {
                    //Success -- Send PWD Changed Mail
                    //MailSender mailSender = new MailSender();
                    //string emailBody = mailSender.ComposeMailBody(MailType.FirstTime);
                    //mailSender.SendEmail(mId, mailId, GlobalValues.AppName + "- Welcome", emailBody);

                    TempData["MsgBody"] = "Your Credentials are succesfully changed. Please login again to continue";
                    TempData["MsgHead"] = "Success!";
                    //TempData["MsgSuccess"] = "Your Credentials are succesfully changed. Please login again to continue";

                    return RedirectToAction("Login", "Account");
                }
                else if (mId == -9)
                {
                    TempData["ErrorMsg"] = "User not found!";
                }
                else if (mId == -2)
                {
                    TempData["ErrorMsg"] = "New and old passwords can not be same";
                }
                else
                {
                    TempData["ErrorMsg"] = "Something went wrong";
                }
            }

            model.SecurityQuestions = _prc.GetSecurityQuestion();
            return View("FirstTimeLogin", model);
        }
        [HttpGet]
        [AuthorizeUser]
        public virtual ActionResult SecurityQuestion()
        {
            VerifySecurityQnModel model = new VerifySecurityQnModel();
            model.SecurityQuestions = _prc.GetSecurityQuestion();
            model.SelectedSecurityQuestion = Convert.ToInt32(model.SecurityQuestions.SelectedValue);

            return View(model);
        }
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public virtual ActionResult SecurityQuestion(VerifySecurityQnModel model)
        {
            if (ModelState.IsValid)
            {
                int status = _prc.VerifySecurityAns(model);
                if (status == 1)
                {
                    Session["PasswordFlag"] = 0;
                    if (Session["RoleID"].ToString() == "1" || Session["RoleID"].ToString() == "2") //admin & support
                        //return RedirectToAction(MVC.Home.Insights());
                        return RedirectToAction(MVC.Campaign.BulkSms());

                    // else if (Session["RoleID"].ToString() == "4") { return RedirectToAction(MVC.Reports.MasterReport()); }
                    else
                        //return RedirectToAction(MVC.Home.Insights());
                        return RedirectToAction(MVC.Campaign.BulkSms());

                }
                else
                {
                    ViewBag.ErrorMsg = "Wrong Answer!";
                }
            }
            model.SecurityQuestions = _prc.GetSecurityQuestion();
            model.SelectedSecurityQuestion = Convert.ToInt32(model.SecurityQuestions.SelectedValue);

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult ForgotPassword()
        {
            var model = new AuthenticateUserModel();
            model.SecurityQuestions = _prc.GetSecurityQuestion();

            ViewBag.Head = "Forgot Password";
            return View("AuthenticateUser", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ForgotPassword(AuthenticateUserModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Head = "Forgot Password";
                model.SecurityQuestions = _prc.GetSecurityQuestion();
                //if (!this.IsCaptchaValid("Invalid Captcha"))
                //{
                //    return View("AuthenticateUser", model);
                //}
                //else
                //{
                    int mid;
                    string mailId, otp, response;
                    model.OldPwd = "";
                    int status = _prc.AuthenticateUser(model, out otp, out mid, out mailId, out response);

                    if (status == 1)
                    {
                        Session["Email"] = mailId;
                        string mailBody = _mailSender.ComposeMailBody(MailType.ForgotPwdOTP, otp);
                        _mailSender.SendEmail(mid, mailId, GlobalValues.AppName + "- Forgot Password - OTP", mailBody);

                        TempData["Head"] = "An OTP has sent to your contact " + mailId.ToMaskedString();

                        return RedirectToAction("ChangePassword", "Account");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = response;
                    }
               // }
            }


            return View("AuthenticateUser", model);
        }
        [HttpGet]
        //[AuthorizeUser]
        public virtual ActionResult ChangePassword() => View();
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                int mid; string mailID;
                int status = _prc.ChangePassword(model, out mid, out mailID);
                if (status == 1)
                {
                    string mailBody = _mailSender.ComposeMailBody(MailType.PwdChanged);
                    _mailSender.SendEmail(mid, mailID, GlobalValues.AppName + "- Password Changed", mailBody);

                    TempData["MsgBody"] = "Your Credentials are succesfully changed. Please login again to continue";
                    TempData["MsgHead"] = "Success!";

                    return RedirectToAction("Login", "Account");
                }
                else if (status == 5)
                {
                    TempData["MsgBody"] = "OTP has Expired. Please login again to continue";
                    TempData["MsgHead"] = "Failed!";

                    return RedirectToAction("Login", "Account");
                }
                else if (status == 2)
                {
                    ViewBag.ErrorMsg = "Invalid OTP!";
                }
                else if (status == 9)
                {
                    ViewBag.ErrorMsg = "Old and new password are same!";
                }
                else
                {
                    ViewBag.ErrorMsg = "Could not complete request.";
                }
            }

            return View("ChangePassword");
        }
        [HttpPost]
        [AuthorizeUser]
        public virtual JsonResult ResendOtp()
        {
            int mailID; string OTP;
            int status = _prc.ResendOTP(out mailID, out OTP);
            if (status == 1)
            {
                MailSender SM = new MailSender();
                string emailBody = SM.ComposeMailBody(MailType.VerifyOTP, OTP);
                bool isSuccess = SM.SendEmail(mailID, Session["Email"].ToString(), GlobalValues.AppName + "- Resend OTP", emailBody);
                if (isSuccess)
                    return Json(new { Msg = "New OTP send to your mail id", Head = "Success" });
                else
                    return Json(new { Msg = "Unable to send mail. Please try again later.", Head = "Error" });
            }
            else if (status == 3)
            {
                return Json(new { Msg = "OTP has been sent too many times, please try after sometime.", Head = "Error" });
            }
            else
            {
                return Json(new { Msg = "Something went wrong", Head = "Error" });
            }
        }
        [AuthorizeUser]
        public virtual ActionResult ResetPassword()
        {
            var model = new AuthenticateUserModel();
            model.Username = ExtensionClass.RandomString(15);
            model.SecurityQuestions = _prc.GetSecurityQuestion();
            model.SelectedSecurityQuestion = Convert.ToInt32(model.SecurityQuestions.SelectedValue);
            BindResetPassword();

            return View("AuthenticateUser", model);
        }
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ResetPassword(AuthenticateUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (!this.IsCaptchaValid("Invalid Captcha"))
                {
                    BindResetPassword();
                    model.Username = ExtensionClass.RandomString(15);
                    model.SecurityQuestions = _prc.GetSecurityQuestion();
                    model.SelectedSecurityQuestion = Convert.ToInt32(model.SecurityQuestions.SelectedValue);

                    return View("AuthenticateUser", model);
                }
                else
                {
                    int mid; string otp, emailId, response;
                    int status = _prc.AuthenticateUser(model, out otp, out mid, out emailId,out response);

                    if (status == 1)
                    {
                        string mailBody = _mailSender.ComposeMailBody(MailType.ResetPwdOTP, otp);
                        _mailSender.SendEmail(mid, emailId, GlobalValues.AppName + "- Change Password - OTP", mailBody);

                        TempData["Head"] = "An OTP has sent to your contact " + emailId.ToMaskedString();

                        return RedirectToAction("ChangePassword", "Account");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Authentication Failed!";
                    }
                }
            }

            model.SecurityQuestions = _prc.GetSecurityQuestion();
            BindResetPassword();

            return View("AuthenticateUser", model);
        }
        private void BindResetPassword()
        {
            if (Session["PasswordFlag"].ToString() == "10")
            {
                ViewBag.Head = "Password Expires Soon";
                ViewBag.Desc = "Your password expires today. Please reset.";
                ViewBag.CanLater = true;
            }
            else if (Session["PasswordFlag"].ToString() == "11")
            {
                ViewBag.Head = "Password Expires Soon";
                ViewBag.Desc = "Your password will expire tomorrow. Please reset.";
                ViewBag.CanLater = true;
            }
            else if (Session["PasswordFlag"].ToString() == "12")
            {
                ViewBag.Head = "Password Expires Soon";
                ViewBag.Desc = "Your password will expire in 2 days. Please reset.";
                ViewBag.CanLater = true;
            }
            else if (Session["PasswordFlag"].ToString() == "13")
            {
                ViewBag.Head = "Password Expires Soon";
                ViewBag.Desc = "Your password will expire in 3 days. Please reset.";
                ViewBag.CanLater = true;
            }
            else if (Session["PasswordFlag"].ToString() == "3")
            {
                ViewBag.Head = "Password Expired";
                ViewBag.Desc = "Please reset to continue";
            }
            else
            {
                ViewBag.Head = "Change Password";
            }
        }
        public virtual ActionResult Register()
        {
            return View();
        }
        public virtual ActionResult LockScreen()
        {
            return View();
        }
        public virtual ActionResult Courstral()
        {
           
           
            return View();

        }
    }
}
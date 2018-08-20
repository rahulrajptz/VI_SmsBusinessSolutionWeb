using Templateprj.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Templateprj.Helpers
{
    public class GlobalValues
    {
        static CryptoAlg EncDec = new CryptoAlg();

        #region ApplicationId
        private static int appId = Convert.ToInt32(ConfigurationManager.AppSettings["AppId"].ToString());
        public static int AppId
        {
            get { return appId; }
        }
        #endregion

        #region ApplicationName
        private static string appName = ConfigurationManager.AppSettings["AppName"].ToString();
        public static string AppName
        {
            get { return appName; }
        }
        #endregion

        #region Key
        private static string key = EncDec.DecryptDes(ConfigurationManager.AppSettings["connectionkey"].ToString(), ConfigurationManager.AppSettings["formatchanger"].ToString());
        public static string Key
        {
            get
            {
                return key;
            }
        }
        #endregion

        #region Connection String
        private static string connStr = EncDec.DecryptDes(ConfigurationManager.ConnectionStrings["WebCStr"].ToString(), GlobalValues.Key);
        public static string ConnStr
        {
            get
            {
                return connStr;
            }
        }
        private static List<DBConnection> locationDbs = null;
        public static List<DBConnection> LocationDBs
        {
            get
            {
                if (locationDbs == null)
                {
                    string[] locations = ConfigurationManager.AppSettings["LocationDBs"].Split(',');
                    locationDbs = new List<DBConnection>();
                    string conStr;
                    foreach (string location in locations)
                    {
                        try
                        {
                            conStr = EncDec.DecryptDes(ConfigurationManager.ConnectionStrings[location + "_CStr"].ToString(), GlobalValues.Key);
                            locationDbs.Add(new DBConnection { ConStr = conStr, Locatn = location });
                        }
                        catch (Exception ex)
                        {
                            LogWriter.Write("GlobalValues.LocationDB :: Can not extract location db connection string, Location: " + location + "\n :: Exception :: " + ex.Message);
                            locationDbs = new List<DBConnection>();
                            break;
                        }
                    }
                }
                return locationDbs;
            }
        }

        #endregion

        #region LogPath
        private static string logPath = EncDec.DecryptDes(ConfigurationManager.AppSettings["LogPath"].ToString(), GlobalValues.Key);
        public static string LogPath
        {
            get { return logPath; }
        }
        #endregion

        #region Mail Server Details
        private static MailServerModel mailServerDetails = null;
        public static MailServerModel MailServerDetails
        {
            get
            {
                if (mailServerDetails == null)
                {
                    mailServerDetails = (new AccountDbPrcs()).GetMailServerDetails();
                }
                return mailServerDetails;
            }
        }
        #endregion

        #region Session AlertTime
        private static int sessionAlertTime = -1;
        public static int SessionAlertTime
        {
            get
            {
                if (sessionAlertTime == -1)
                {
                    try
                    {
                        sessionAlertTime = Convert.ToInt32((ConfigurationManager.AppSettings["SessionAlertTime"].ToString()));
                        sessionAlertTime = sessionAlertTime * 60000;
                    }
                    catch
                    {
                        sessionAlertTime = 60000;
                    }

                }
                return sessionAlertTime;
            }
        }
        #endregion

        #region AbsoluteUri
        private static string absoluteUri = ConfigurationManager.AppSettings["AbsoluteUri"].ToString();
        public static string AbsoluteUri
        {
            get { return absoluteUri; }
        }
        #endregion

        #region AbsoluteUri
        private static int maxReportPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["MaxReportPeriod"]);
        public static int MaxReportPeriod
        {
            get { return maxReportPeriod; }
        }
        #endregion

        //#region Max rows in excel sheet
        //private static int maxRowsExcel = -1;
        //public static int MaxRowsExcel
        //{
        //	get
        //	{
        //		if (maxRowsExcel == -1)
        //		{
        //			try
        //			{
        //				maxRowsExcel = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRowsExcel"].ToString());
        //			}
        //			catch
        //			{
        //				maxRowsExcel = 500000;
        //			}
        //		}
        //		return maxRowsExcel;
        //	}
        //}
        //#endregion
    }
    public class DBConnection
    {
        public string Locatn { get; set; }
        public string ConStr { get; set; }
    }
}
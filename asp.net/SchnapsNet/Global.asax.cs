using SchnapsNet.ConstEnum;
using SchnapsNet.Models;
using SchnapsNet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SchnapsNet
{
    public class Global : System.Web.HttpApplication
    {
        GlobalAppSettings globalVariable;
        object xlock, pLock;

        protected void Application_Start(object sender, EventArgs e)
        {
            string initMsg = String.Format("application started at {0} object sender = {2}, EventArgs e = {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                (sender == null) ? "(null)" : sender.ToString(),
                (e == null) ? "(null)" : e.ToString());
            Area23Log.Logger.Log(initMsg);
            Area23Log.Logger.Log("logging to logfile = " + Logger.LogFile);
        }


        protected void Application_Disposed(object sender, EventArgs e)
        {
        }

        //protected void Application_End(object sender, EventArgs e)
        //{
        //    string initMsg = String.Format("application ended at {0} object sender = {2}, EventArgs e = {2}",
        //        DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
        //        (sender == null) ? "(null)" : sender.ToString(),
        //        (e == null) ? "(null)" : e.ToString());
        //    Area23Log.Logger.Log(initMsg);
        //}

        protected void Application_Error(object sender, EventArgs e)
        {

        }


    //    protected void Session_Start(object sender, EventArgs e)
    //    {
    //        string initMsg = String.Format("new session started at {0} object sender = {2}, EventArgs e = {2}",
    //            DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
    //            (sender == null) ? "(null)" : sender.ToString(),
    //            (e == null) ? "(null)" : e.ToString());
    //        Area23Log.Logger.Log(initMsg);
    //    }


    //    protected void Session_End(object sender, EventArgs e)
    //    {
    //        string initMsg = String.Format("session ended at {0} object sender = {2}, EventArgs e = {2}",
    //            DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
    //            (sender == null) ? "(null)" : sender.ToString(),
    //            (e == null) ? "(null)" : e.ToString());
    //        Area23Log.Logger.Log(initMsg);
    //    }
    
    }
}
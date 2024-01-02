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
            Logger.Log(initMsg);
            Logger.Log("logging to logfile = " + Logger.LogFile);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            
        }
    }
}
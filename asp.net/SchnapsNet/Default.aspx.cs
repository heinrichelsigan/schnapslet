using SchnapsNet.ConstEnum;
using SchnapsNet.Models;
using SchnapsNet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace SchnapsNet
{
    public partial class Default : Area23BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MetaRefresh("SchnapsNet.aspx");

            // InitGlobalVariable();
            // Response.Redirect("SchnapsNet.aspx");
        }


        public virtual void MetaRefresh(string redirUrl)
        {
            headerMetaRefresh.Attributes["http-equiv"] = "refresh";
            headerMetaRefresh.Content = "4; " + redirUrl;
        }


       

    }
}
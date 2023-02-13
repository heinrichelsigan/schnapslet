    <%@ Page Language="C#" AutoEventWireup="true" %>
    <%@ Import namespace="Newtonsoft.Json" %>
    <%@ Import namespace="Newtonsoft.Json.Linq" %>
    <%@ Import namespace="Newtonsoft.Json.Bson" %>
    <%@ Import namespace="System" %>
    <%@ Import namespace="System.Collections.Generic" %>
    <%@ Import namespace="System.Linq" %>
    <%@ Import namespace="System.Reflection" %>
    <%@ Import namespace="System.Web"%>
    <%@ Import namespace="System.Diagnostics"%>
    <%@ Import namespace="System.Web.UI"  %>
    <%@ Import namespace="System.Web.UI.WebControls" %>

    <!DOCTYPE html>

    <html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>Deserialize JSON</title>
    </head>

    <script runat="server" language="C#">

        const string dlm = "-";
        void Page_Load(object sender, EventArgs e)
        {
            this.Literal1.Text = DateTime.UtcNow.ToString("yyyy") + dlm + DateTime.UtcNow.ToString("MM") + dlm + DateTime.UtcNow.ToString("dd") + "_" + 
                DateTime.UtcNow.ToShortTimeString() + " Deserialize json paths: ";

            if (!this.IsPostBack)
                this.TextBoxJson.Text = "";
        }

        void JsonDeserialize_Click(Object sender, EventArgs e)
        {
            string outs0 = String.Empty;
            string outi0 = String.Empty;
            string js0 = this.TextBoxJson.Text;

            if (string.IsNullOrEmpty(js0) || js0.Length < 8)
            {
                preOut.InnerText = "JSON string is null or shorter then 8 characters \r\n";
                return;
            }
            preOut.InnerText = "JSON length = " + js0.Length + " \r\n";

            try
            {
                JObject o0 = (JObject)JsonConvert.DeserializeObject(js0);
                try
                {
                    foreach (var jprop in o0.Properties())
                    {
                        outs0 += jprop.Type.ToString() + " \t" + jprop.Name + " \t" + jprop.Value.ToString() + "\r\n";
                    }
                }
                catch (Exception ex1)
                {
                    outs0 += "inner Exc\tMessage = " + ex1.Message + " \r\n\tException: " + ex1.ToString() + " \r\n";
                }

                JToken root = o0.Root;
                try
                {
                    if (o0 != null)
                    {
                        outs0 += GetJsonTreeObject(root, outi0, 0, false);
                    }
                }
                catch (Exception ex2)
                {
                    outs0 += "Exception\tMessage = " + ex2.Message + " \r\n\tException: " + ex2.ToString() + " \r\n";
                    throw new ApplicationException("Error when parsing json tree with reflection", ex2);
                }
            }
            catch (Exception ex0)
            {
                outs0 += "Exception in JsonConvert.DeserializeObject(jsonSring): \r\n";
                outs0 += "\tMessage = " + ex0.Message + " \r\n\tException: " + ex0.ToString() + " \r\n";
            }
            preOut.InnerText += outs0;
        }

        void LinkButtonJSON_Click(object sender, EventArgs e)
        {
            string outs1 = String.Empty;
            string outi1 = String.Empty;
            string js1 = this.TextBoxJson.Text;
            if (!string.IsNullOrEmpty(js1))
            {
                JObject o1 = (JObject)JsonConvert.DeserializeObject(js1);
                if (o1 != null)
                {
                    outs1 += GetJsonTreeObject(o1.Root, outi1, 0);
                }
                preOut.InnerText = outs1;
            }
        }

        string GetJsonTreeObject(JToken o, string outp, int depth, bool html = false)
        {
            string NEWLINE = (html) ? "<br />\r\n" : "\r\n";
            string name = string.Empty;            
            string depthStr = ((depth > 99) ? "" : (depth < 10) ? "  " : " ") + depth.ToString();
            int jsninc = 1;

            try
            {
                Type type = o.GetType();
                JToken rootToken = o.Root;
                string path = o.Path;
                
                JContainer parent = (o.Parent != null) ? (JContainer)o.Parent : null;                
                if (parent != null && !String.IsNullOrEmpty(path) && parent.Path == path)
                    jsninc = 0;
                else
                {
                    if (depth > 0)
                        outp += depthStr.ToString() + "\t " + path + " \r\n";
                    else
                        outp += depthStr.ToString() + "\t " + "{" + type.ToString() + "} \r\n";
                }

                foreach (JToken jChildToken in o.Children())
                {
                    if (jChildToken.HasValues)
                    {
                        string oton = string.Empty;
                        outp += GetJsonTreeObject(jChildToken, oton, depth + jsninc);
                    }
                }
            }
            catch (Exception ex3)
            {
                outp += "Exception in Reflection type.GetFields() or GetProperties(): \r\n";
                outp += "\tMessage = " + ex3.Message + " \r\n\tException: " + ex3.ToString() + " \r\n";
            }
            return outp;
        }

        string Fortune(string filepath = "/usr/games/fortune", string args = "-a -l")
        {
            string consoleOutput = "No fortune today";
            try
            {
                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = "fortune";
                    string argTrys = (!string.IsNullOrWhiteSpace(args)) ? args : "";
                    compiler.StartInfo.Arguments = argTrys.ToString();
                    compiler.StartInfo.UseShellExecute = false;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    compiler.Start();

                    consoleOutput = compiler.StandardOutput.ReadToEnd();

                    compiler.WaitForExit();

                    return consoleOutput;
                }
            }
            catch (Exception exi)
            {
                return "Exception: " + exi.Message;
            }
        }

    </script>

    <body>
        <form id="form1" runat="server">
            <div align="left" style="text-align: left; background-color:#dfdfdf; font-size: larger; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', 'Geneva', 'Verdana', 'sans-serif'">
                <asp:Literal ID="Literal1" runat="server"></asp:Literal> &nbsp; <a href="https://darkstar.work/mono/json/json.aspx">json.aspx example</a>
            </div>
            <hr />
            <div style="line-height: normal; height: 24pt; width: 100%; table-layout: fixed; inset-block-start: initial; nowrap">        
                <span style="width:40%; vertical-align:baseline; text-align: left; font-size: larger" align="left"><asp:LinkButton ID="LinkButtonJSON" runat="server"  ToolTip="Deserialize only paths in json" Text="Deserialize json paths"  OnClick="LinkButtonJSON_Click"></asp:LinkButton></span>
            </div>
            <div>
                <asp:TextBox ID="TextBoxJson" runat="server" TextMode="MultiLine" ToolTip="Put your JSON string here" Width="100%" Height="320px"></asp:TextBox>
            </div>
            <div style="nowrap; line-height: normal; height: 24pt; width: 100%; table-layout: fixed; inset-block-start: initial">        
                <span style="width:40%; vertical-align:bottom; text-align: left" align="left"><asp:Button ID="ButtonDeserialize" runat="server" ToolTip="Click to deserialize JSON with paths" Text="Deserialize json" OnClick="JsonDeserialize_Click" /></span>&nbsp;            
            </div>
            <hr />
            <pre id="preOut" runat="server">
            </pre>
            <hr />
            <div align="right" style="text-align: right; background-color='#efefef'; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
                <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://@arkstar.work">darkstar.work</a>
            </div>
        </form>
    </body>
    </html>


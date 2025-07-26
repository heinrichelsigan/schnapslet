<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SchnapsNet.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Schnaps.Net</title>
    <link rel="stylesheet" href="res/schnapsnet.css" />
    <meta name="author" content="Heinrich Elsigan" />  
    <meta id="headerMetaRefresh" runat="server" http-equiv="refresh" content="4; SchnapsNet.aspx" />
    <script type="text/javascript">
        var timer = setTimeout(function () {
            window.location = 'SchnapsNet.aspx'
        }, 6000);
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="SchnapsFooterDiv" class="SchnapsFooter" align="left">
                <a href="mailto:zen@area23.at">Heinrich Elsigan</a>, <a href="https://github.com/heinrichelsigan" target="_blank">github.com/heinrichelsigan</a>[<a href="https://github.com/heinrichelsigan/schnapslet" target="_blank">/schnapslet</a>], GNU General Public License 3.0,  [<a href="https://blog.area23.at" target="_blank">blog.</a>]<a href="https://area23.at" target="_blank">area23.at</a>
            </div>  
        </div>
    </form>
</body>
</html>

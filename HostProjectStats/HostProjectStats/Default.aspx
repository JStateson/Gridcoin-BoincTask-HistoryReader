<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HostProjectStats.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Statistics for BOINC project at a specified host</title>
</head>
<body>

      

    <form id="form1" runat="server">
         <p> 

    <asp:Panel ID="Panel1" runat="server" Height="860px" Width="806px">
        Browse to a project that interests you, say Milkyway, Select a computer, select tasks at that computer<br /> select VALID tasks and makes sure there are exactly 20. Then copy the url from your browser<br /> into the &quot;Paste the url&quot; box below and click &quot;CALCULATE&quot;. You can also CLEAR the statistics or<br /> select additional pages of data up to a total of 10 pages.&nbsp;&nbsp; This program cannot log in to a uses account
        <br />
        so you must enter a url that points to a host computer and NOT a list of user tasks.&nbsp; Sample urls are in<br /> the pull down box &quot;TEST HOST&#39;.&nbsp; They are the TOP computer (where available) that have the best<br /> statistics at the corresponding project. To demo what this program does, select a TEST HOST.&nbsp; To<br /> see the original data at web site click on &quot;REVIEW DATA&quot;.&nbsp; THIS NO LONGER WORKS ON<br /> PROJECTS THAT BLOCK ANONYMOUS ACCESS.&nbsp; Usefull on your own projects only.<br />
        <br />
        TEST HOST DEMO:
        <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged">
            <asp:ListItem Value="https://milkyway.cs.rpi.edu/milkyway/results.php?hostid=591552&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">Milkyway</asp:ListItem>
            <asp:ListItem Value="https://einsteinathome.org/host/12153266/tasks/4/0">Einstein</asp:ListItem>
            <asp:ListItem Value="https://boinc.thesonntags.com/collatz/results.php?hostid=148179&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">Collatz</asp:ListItem>
            <asp:ListItem Value="http://www.gpugrid.net/results.php?hostid=467730&amp;offset=0&amp;show_names=0&amp;state=3&amp;appid=">GpuGrid</asp:ListItem>
            <asp:ListItem Value="https://sech.me/boinc/Amicable/results.php?hostid=33751&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">Amicable</asp:ListItem>
            <asp:ListItem Value="http://asteroidsathome.net/boinc/results.php?hostid=512380&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">Astroids</asp:ListItem>
            <asp:ListItem Value="https://www.cosmologyathome.org/results.php?hostid=268457&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">Cosmology</asp:ListItem>
            <asp:ListItem Value="http://www.enigmaathome.net/results.php?hostid=220602&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">Enigma</asp:ListItem>
            <asp:ListItem Value="https://boinc.multi-pool.info/latinsquares/results.php?hostid=14170&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">Odka1</asp:ListItem>
            <asp:ListItem Value="https://lhcathome.cern.ch/lhcathome/results.php?hostid=10484460&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">LHC</asp:ListItem>
            <asp:ListItem Value="https://escatter11.fullerton.edu/nfs/results.php?hostid=880073&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=1">NFS</asp:ListItem>
            <asp:ListItem Value="http://pogs.theskynet.org/pogs/results.php?hostid=858228&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">POGS</asp:ListItem>
            <asp:ListItem Value="https://setiathome.berkeley.edu/results.php?hostid=7475713&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid=">SETI</asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnReview" runat="server" Text="REVIEW DATA" OnClick="btnReview_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="tb_num2read" runat="server" Width="28px">20</asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" BackColor="#FFCCFF" Text="Paste the url here"></asp:Label>
        :
        <asp:TextBox ID="ProjUrl" runat="server" Height="16px" Width="504px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="btnCalc" runat="server" OnClick="btnCalc_Click" Text="CALCULATE" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="CLEAR" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnAbout" runat="server" OnClick="Button1_Click" Text="ABOUT" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" BackColor="#33CCCC" Text="Number of pages to gather"></asp:Label>
        &nbsp;
        <asp:DropDownList ID="ddlNumPages" runat="server" Width="44px">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="lblProjName" runat="server" Text="UNKNOWN PROJECT"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; tested on the following:&nbsp;
        <asp:DropDownList ID="ddlTested" runat="server">
            <asp:ListItem>milkyway</asp:ListItem>
            <asp:ListItem>einstein</asp:ListItem>
            <asp:ListItem>gpugrid</asp:ListItem>
            <asp:ListItem>collatz</asp:ListItem>
            <asp:ListItem>latinsquares</asp:ListItem>
            <asp:ListItem>setiathome</asp:ListItem>
            <asp:ListItem>latinsquares</asp:ListItem>
            <asp:ListItem>Amicable</asp:ListItem>
            <asp:ListItem>Enigma</asp:ListItem>
            <asp:ListItem>LHC</asp:ListItem>
            <asp:ListItem>SETI</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <asp:TextBox ID="ResultsBox" runat="server" Height="532px" ReadOnly="True" TextMode="MultiLine" Width="436px" ToolTip="sample location: https://milkyway.cs.rpi.edu/milkyway/results.php?hostid=766466&amp;offset=0&amp;show_names=0&amp;state=4&amp;appid="></asp:TextBox>





    </asp:Panel>

    </p>
        <div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="GRCearned.About" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ABOUT</title>
    <style type="text/css">
        #form1 {
            height: 578px;
            width: 792px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        Copyright 2018 Joseph &quot;Beemer Biker&quot; Stateson<br />
                <br />
        <asp:Image ID="Image1" runat="server" Height="205px" ImageUrl="http://stateson.net/images/jyspic.JPG" Width="345px" />
        <br />
        <br />
        If these programs proves useful send a few gridcoins my way to<br />
        SLGw2i8evCmVQq5oqs2FVV6TsrsTKZbAiR<br />
        <br />
        Suggestions, Complements, Bug Reports:&nbsp;
                <a href="mailto:GRCcontact@stateson.net?subject=GRCemail">SendEmail</a>
            <br />
            All else:&nbsp;
                <a href="mailto:PEBCAK@127.0.0.1?subject=Where you are going it is not half full">SendEmail</a>&nbsp;&nbsp; The C# sources are at Github/BeemerBiker.<br />
        <br />
        Other programs you may like.&nbsp; If you got to this web page you ran one of them already.<br />
        <br />
        <asp:Button ID="btnGetProj" runat="server" Text="Host Project Analizer" OnClick="btnGetProj_Click" />
&nbsp; This calculates the average credit for the specified host and project<br />
        <br />
        <asp:Button ID="btnCPID" runat="server" OnClick="btnCPID_Click" Text="GRC earnings" />
&nbsp;This uses the gridcoinstats.eu web page to calculate the approximate GRC per project<br />
        <br />
        BTHistoryReader is a Windows app you must install on your PC.&nbsp; It analyzes the BoincTasks history files.<br />
        Build it using
        VS2017.&nbsp; Sample results are
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="https://github.com/BeemerBiker/Gridcoin/blob/master/BTHistoryReader/BTHistory_Demo.png" Target="_blank">here</asp:HyperLink>
        ,
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="https://github.com/BeemerBiker/Gridcoin/blob/master/BTHistoryReader/BTHistory_Demo1.png" Target="_blank">here</asp:HyperLink>
        ,
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="https://github.com/BeemerBiker/Gridcoin/blob/master/BTHistoryReader/BTHistory_Demo2.png" Target="_blank">here</asp:HyperLink>
        , and
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="https://github.com/BeemerBiker/Gridcoin/blob/master/BTHistoryReader/BTHistory_Usage.png" Target="_blank">here</asp:HyperLink>
&nbsp;Can be used to measure throughput.<br />
        A wiring diagram and pictures for an inexpensive wattmeter (P06S-20 or 100)&nbsp; are
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="https://github.com/BeemerBiker/Gridcoin/blob/master/HostProjectStats\wmeter_wiring.jpg" Target="_blank">here</asp:HyperLink>
        ,
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="https://github.com/BeemerBiker/Gridcoin/blob/master/HostProjectStats\wmeter_assembled.jpg" Target="_blank">here</asp:HyperLink>
        , and
        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="https://github.com/BeemerBiker/Gridcoin/blob/master/HostProjectStats\HP_Z400_3_S9000_5T.png" Target="_blank">here</asp:HyperLink>
&nbsp;<br />
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="GRCearned.About" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ABOUT</title>
    <style type="text/css">
        #form1 {
            height: 547px;
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
                <a href="mailto:PEBCAK@127.0.0.1?subject=Where you are going it is not half full">SendEmail</a>

        <br />
        <br />
        <br />
        Other programs you may like.&nbsp; If you got to this web page you ran one of them already.<br />
        <br />
        <asp:Button ID="btnGetProj" runat="server" Text="Host Project Analizer" OnClick="btnGetProj_Click" />
&nbsp; This calculates the average credit for the specified host and project<br />
        <br />
        <asp:Button ID="btnCPID" runat="server" OnClick="btnCPID_Click" Text="GRC earnings" />
&nbsp;This uses the gridcoinstats.eu web page to calculate the approximate GRC per project</form>
</body>
</html>

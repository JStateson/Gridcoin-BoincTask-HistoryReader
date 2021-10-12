<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GRCearned.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 714px; width: 804px">
            GRC EARNINGS ESTIMATOR<br />
            <br />
            This program calculates approximate earnings in GRC based on your project&#39;s magnitude. It also calculates an efficiency percentage that MIGHT show which projects are more efficient at paying GRC based on credit earned. Note that this can change over time. Go to 
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="https://www.gridcoinstats.eu" Target="_blank">GRIDCOINSTATS.EU</asp:HyperLink>
&nbsp;and enter your CPID. Then select your &quot;address&quot; and using your mouse, select the contents of your &quot;Active Projects&quot; table and paste into the box below, replaceing the sample shown. The Last Known Earnings is shown as 1000. Change that to your last payoff if you want, else leave it at 1000. Then click on CALCULATE. Click on CLEAR to erase the resuls or the sample. If you trust this program then enter your CPID in the box labeled CPID and click LOOKUP to avoid haveing to do the copy and paste. If you dont trust this program then enter someone else CPID into the box and click LOOKUP.&nbsp; Gridcoinstats shows all CPIDs in payouts.&nbsp; If you pick the biggest payout it will NOT be mine, that is for sure. Or
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl=" https://gridcoin.network">gridcoinNetwork</asp:HyperLink>
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" BackColor="#FFCCFF" Text="Error: Info:"></asp:Label>
&nbsp;<asp:TextBox ID="errbox" runat="server" Width="398px" Height="23px"></asp:TextBox>
           

            <br />
            <br />
            
        <asp:Button ID="btnLKUPcpid" runat="server" Text="LOOKUP" OnClick="btnLKUPcpid_Click" />

           
        &nbsp;<asp:TextBox ID="cpidValue" runat="server" Width="383px" Height="22px" ToolTip="put cpid here to lookup projects and magnitudes"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;
            <br />
            <br />
            <asp:Button ID="btnCalc" runat="server" OnClick="btnCalc_Click" Text="CALCULATE" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="LastKnown" runat="server" Width="83px" ToolTip="last known earnings">1000</asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="CLEAR" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="ABOUT" />
            <br />
            <br />
            <asp:TextBox ID="ResultsBox" runat="server" Height="271px" TextMode="MultiLine" Width="507px" BackColor="#33CCFF">Milkyway@home	6	4,247,094	340.33
Gpugrid	4	2,528,578	125.35
Einstein@home	9	1,942,292	99.16
AmicableNumbers	6	1,705,200	52.75
CollatzConjecture	14	1,473,407	34.05
TheskynetPogs	8	39,099	27.99
Seti@home	9	36,756	19.60
Odlk1	8	6,559	11.68
CitizenScienceGrid	0	742	0.10
Asteroids@home	0	112	0.03
Primegrid	0	285	0.01
Numberfields@home	0	2	0.00
Enigma@home	0	3	0.00</asp:TextBox>

           
        </div>
    </form>
</body>
</html>

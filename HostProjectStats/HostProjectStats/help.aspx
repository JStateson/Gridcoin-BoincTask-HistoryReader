<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="help.aspx.cs" Inherits="HostProjectStats.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        This program can be used to compare different GPUs and CPUs and obtain the cost in KWH you are contributing to the project.<br />
        For this program to work you must be able access a BOINC project.&nbsp; If you have enabled &quot;Remember Me&quot; on your project account<br />
        then, except for WCG, you can accesss your own data.&nbsp; Some projects have not restructed anomymous access so possibly you<br />
        could access the data without signing in.&nbsp; Normally, &quot;Top participants&quot; have allowed their statistics to be searched.<br />
        <br />
        When this program starts up, Milkyway is selected and one of the &quot;Top Computers&quot; is preset into the URL.&nbsp; If Milkyway is online<br />
        then clicking CALCULATE will provide a demo for accessing statistics.&nbsp; Click REVIEW DATA to see the actual data.<br />
        If Milkyway is offline why will get an OOPS error as I don&#39;t do a lot of error checking.<br />
        <br />
        <br />
        To calculate the number of watts it takes to do a credit (cost of your contribution to the project) you must know how many watts<br />
        your system is drawing.&nbsp; That can be obtain using an inexpensive A/C ampmeter or wattmeter<br />
        <br />
        Load Watts:&nbsp; number of watts the system is drawing (BOINC projects running)<br />
        Idle Watts:&nbsp;&nbsp;&nbsp; watts your PC uses when BOINC is running but not the projects (idle)
        <br />
        nDev:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Number of GPUs being used by the project -or- number of threads being used<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Do not run CPU tasks at same time as GPU tasks and vice-versa when testing.<br />
        nCon:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Number of concurrent GPU tasks. Set to 1 for any CPU testing<br />
        Wu&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Work units to obtain (leave at 20 unless you do not have that many)<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Note that WCG uses 15 on a page.<br />
        <br />
        Notes
        <br />
        (1)&nbsp;&nbsp;&nbsp; WCG cannot be accessed because they required username / password to be stored in a cookie and this program<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; does not implement that scheme.<br />
        <br />
        (2)&nbsp;&nbsp;&nbsp; Example of computation of KWH for credit (GPU or CPU)<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A single watt is a joule of energy that is given up in 1 second of work.<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Assume your system uses 120 watts at idle and 420 watts at full load<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; and has 3 GPUs working.&nbsp; Assume it takes 10 seconds to get a single<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; credit.&nbsp; Your system then generates 3 credits in 10 seconds assumeing<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; only 1 task on a GPU (or CPU) at a time.&nbsp; Credit KWH calc explained:<br />
        <br />
        The 3 GPUs consume 100 watts each per second.&nbsp; Since it takes 10 seconds to get a<br />
        single credit you willl expend 100 joules for 10 seconds which is equivalent to 1000 joules<br />
        total energy sustained over exactly one second to get 1 credit.&nbsp; Since a watt is a joule<br />
        per second then it would take 1000 joules to produce a single credit in a second (1 KW).<br />
        Divide by 3600 to get 0.277.. watt-hours and divide by another 1000 for KWH per credit.<br />
        To determine how many credits could be produced in a single KWH invert the above result<br />
        or 3600 * 1000 / 1000 = 3600 credits.&nbsp; ie:&nbsp; if the device is allowed to run until a single<br />
        kilowatt hour is consumed then 3600 credits will be generated.&nbsp; PM me is this does
        <br />
        not seem correct.&nbsp; Sources are at Github\BeemerBiker<br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </form>
</body>
</html>

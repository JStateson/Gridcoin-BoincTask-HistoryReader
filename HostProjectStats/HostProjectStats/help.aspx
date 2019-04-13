<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="help.aspx.cs" Inherits="HostProjectStats.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #form1 {
            font-weight: 700;
        }
    </style>
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
        If Milkyway is offline one will get an OOPS error as I don&#39;t do a lot of error checking.<br />
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
        <br />
        (1)&nbsp;&nbsp;&nbsp; WCG cannot be accessed because they required username / password to be stored in a cookie and this program<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; does not implement that scheme.<br />
        <br />
        The following two notes assume 120 watts idle and 420 watts full load with 3 GPUs and only 1 task per GPU<br />
        and A single watt is a joule of energy that is given up in 1 second of work.<br />
        <br />
        (2)&nbsp;&nbsp;&nbsp; Example of computation of credit per second entire system.<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Assume you have 3 devices and it takes 10 seconds to get a single<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; credit.&nbsp; Your system then generates 3 credits in 10 seconds which<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; is 0.3 credits per second.&nbsp; Multiply that value by the number of<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; current tasks each device is running (just 1) total of 0.3 credits..<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; If the system is run for a full hour at 420 watts then 0.3 * 3600 gives<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1080 credits per hour.&nbsp; The 420 watt-hour can be converted to KWH<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; by dividing by 1000 for a total or 0.42 KWH to generate 1080 credits<br />
        <br />
        (3)&nbsp;&nbsp;&nbsp; Computation of KWH (cost of contribution) for each GPU (or CPU)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Assume your system uses 120 watts at idle and 420 watts at full load<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; and has 3 GPUs working with one task per GPU:<br />
        <br />
        The 3 GPUs each consume 100 watts.&nbsp;Doing the same calculation as<br />
        above,&nbsp; if the system is run for a fully hour at 100 watts then multiply<br />
        0.1 credits by 3600 to get 360 credits for that 100 watt-hour.<br />
        We can now calculate how many credits in a kilowatt hour by multiplying<br />
        by 10.&nbsp; This give 3600 credits per KWH per GPU.&nbsp; This value does not<br />
        include any idle wattage and can be used to compare to different GPUs<br />
        <br />
        Sources are at Github\BeemerBiker&nbsp; PM me if this does
        not seem correct.<br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </form>
</body>
</html>

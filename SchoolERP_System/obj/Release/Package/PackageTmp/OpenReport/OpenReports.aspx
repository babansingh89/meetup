﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenReports.aspx.cs" Inherits="SchoolERP_System.OpenReport.OpenReports" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register assembly="CrystalDecisions.Web" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
       Height="50px" Width="350px" GroupTreeStyle-ShowLines="False" 
       HasCrystalLogo="False" HasDrilldownTabs="False" HasDrillUpButton="False" 
       HasExportButton="False" HasPrintButton="False" HasToggleGroupTreeButton="False" 
       ShowAllPageIds="True" ReuseParameterValuesOnRefresh="true" 
       ToolPanelView="None" ViewStateMode="Enabled"   />
        </div>
    </form>
</body>
</html>

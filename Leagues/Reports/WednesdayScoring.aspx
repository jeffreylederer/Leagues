<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/Site1.Master" AutoEventWireup="true" CodeBehind="WednesdayScoring.aspx.cs" Inherits="Leagues.Reports.WednesdayScoring" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ForeColor="Red" ID="lblError" EnableViewState="False"></asp:Label>
    <asp:ScriptManager runat="server" ID="sc"></asp:ScriptManager>
    <asp:HyperLink runat="server" ID="hyReturn" Text="Return to Match page"></asp:HyperLink>
    <rsweb:ReportViewer ID="rv1" runat="server" AsyncRendering="false" OnReportError="Rv1_ReportError"
                        Width="800px" Height="1000px" ShowPrintButton="True" ShowExportControls="True">
        <LocalReport ReportPath="./Reports/ReportFiles/Standings.rdlc"  >
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>


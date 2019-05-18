<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/Site1.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="Leagues.Reports.Schedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><asp:Label runat="server" ID="lblLeague"></asp:Label> Schedule</h2>
       <asp:Button runat="server" ID="btnExport" Text="Export" OnClick="btnExport_Click"/>
</asp:Content>

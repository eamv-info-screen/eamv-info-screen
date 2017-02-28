<%@Page Title="Infoskærm" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ScreenPreview.aspx.cs" Inherits="ScreenPreview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="menu" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="Server">
    <div>
        <table class="events-table-preview table" runat="server" id="eventsTableForPreview">
            <tr>
                <th runat="server">Emne</th>
                <th runat="server">V/Hvem</th>
                <th runat="server">Tidspunkt</th>
                <th runat="server">Lokale</th>
            </tr>
        </table>
    </div>
</asp:Content>

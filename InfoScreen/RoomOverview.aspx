<%@ Page Title="Lokaleoversigt" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RoomOverview.aspx.cs" Inherits="RoomOverview" %>

<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Lokaleoversigt</h3>
        <div class="col-md-6 col-lg-offset-3">
            <div class="form-group white-select">
                <label>Afdeling</label>
                <select class="form-control selectpicker" title="Vælg afdeling" onchange="submitForm()" id="departmentsSelect" runat="server" />
            </div>
        </div>

        <div class="search-events-content text-center">
            <% if (rooms.Count > 0) { %>
            <% foreach (var roomEnity in rooms) { %>
            <div class="col-md-4">
                <ul class="event-content white-bg text-left">
                    <li><b>Navn</b> : <%: roomEnity.Identifier %></li>
                    <li><b>Afdeling</b> : <%: roomEnity.DepartmentEntity.Name %></li>
                    <li class="text-right">
                        <a class="btnDeleteRoom label label-danger" href="DeleteRoom.aspx?roomId=<%: roomEnity.Id %>">Slet</a>
                    </li>
                </ul>
            </div>
            <% } %>
            <% } else { %>
                Ingen rum.
            <% } %>
        </div>
    </div>

</asp:Content>


<%@ Page Title="Afdelings oversigt" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DepartmentOverview.aspx.cs" Inherits="RoomOverview" %>

<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Afdelings oversigt</h3>
        <div class="search-events-content text-center">
            <% if (departments.Count > 0) { %>
            <% foreach (var departmentEntity in departments) { %>
            <div class="col-md-3">
                <ul class="event-content white-bg text-left">
                    <li><b>Afdeling</b> : <%: departmentEntity.Name %></li>
                    <li class="text-right">
                        <a class="btnDeleteDepartment label label-danger" href="DeleteDepartment.aspx?departmentId=<%: departmentEntity.Id %>">Slet</a>
                    </li>
                </ul>
            </div>
            <% } %>
            <% } else { %>
                Ingen afdeling.
            <% } %>
        </div>
    </div>

</asp:Content>


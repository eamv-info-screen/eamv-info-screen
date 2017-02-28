<%@ Page Title="Bruger oversigt" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserOverview.aspx.cs" Inherits="UserOverview" %>

<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Bruger oversigt</h3>
        <div class="search-events-content text-center">
            <% if (userList.Count > 0) { %>
            <% foreach (var userEntity in userList) { %>
            <% if (userEntity.userId != HttpContext.Current.User.Identity.Name) { %>
            <div class="col-md-4">
                <ul class="event-content white-bg text-left">
                    <li><b>Brugernavn</b> : <%: userEntity.userId %></li>
                    <li><b>Rolle</b> : <%: userEntity.role %></li>
                    <%--<li><b>Password</b> : <%: userEntity.password %></li>--%>
                    <li class="text-right">
                        <a class="btnDeleteUser label label-danger" href="DeleteUser.aspx?userId=<%: userEntity.userId %>">Slet</a>
                    </li>
                </ul>
            </div>
            <% } %>
            <% } %>
            <% } else { %>
                Ingen bruger.
            <% } %>
        </div>
    </div>

</asp:Content>


<%@ Page Title="Søg aktiviteter" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Search" %>


<asp:Content ID="mainContent" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Søg aktiviteter</h3>
    </div>
    <div class="container events-list">
        <div class="col-md-12">
            <div class="col-md-4 col-md-offset-2">
                <div class="form-group white-select">
                    <label for="rooms">Afdeling *</label>
                    <select class="form-control selectpicker show-tick" id="departmentsSelect" runat="server">
                    </select>
                </div>
                <div class="form-group">
                    <label for="host">v/hvem</label>
                    <input type="text" class="form-control" id="host" placeholder="v/hvem" runat="server" />
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <label>
                        Vælg dato *
                        <asp:updatepanel id="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Calendar CssClass="event-calendar" ID="searchDate" OnDayRender="CalendarDayRender" runat="server">
                                        <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                                        <TitleStyle CssClass="month"></TitleStyle>
                                        <DayStyle CssClass="event-calendar-day"></DayStyle>
                                        <SelectedDayStyle CssClass="calendar-date-selected"></SelectedDayStyle>
                                    </asp:Calendar>
                                </ContentTemplate>
                            </asp:updatepanel>
                    </label>
                    &nbsp;
                </div>
            </div>
            <div class="col-md-9 col-md-offset-2">
                <button type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit" onclick="this.disabled=true; this.form.submit();">Søg</button>
                <button type="submit" id="clear" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormClear" onclick="this.disabled=true; this.form.submit();">Clear</button>
            </div>
        </div>

        <div class="search-events-content text-center">
            <% if (events.Count > 0) { %>
            <% foreach (var eventEntity in events) { %>
            <div class="col-md-3">
                <ul class="event-content white-bg text-left">
                    <% if (eventEntity.IsCanceled) {%>
                    <li><b class="canceled-event">Emne : <%: eventEntity.Topic %> [ AFLYST ]</b></li>
                    <% } else {%>
                    <li><b>Emne</b> : <%: eventEntity.Topic %></li>
                    <% } %>
                    <li><b>v/hvem</b> : <%: eventEntity.Host %></li>
                    <li><b>Tidspunk</b> : <%:eventEntity.FromDate.ToString("HH:mm")%> - <%:eventEntity.ToDate.ToString("HH:mm")%></li>
                    <% 
                        var rooms = "";
                        if (eventEntity.Rooms.Count > 0) {
                            foreach (var room in eventEntity.Rooms) {
                                rooms += room.Identifier + ", ";
                            }
                            rooms = rooms.Substring(0,rooms.Length - 2);
                        }
                    %>
                    <li><b>Lokale</b> : <%: rooms %></li>
                    <li><b>Afdeling</b> : <% if (eventEntity.DepartmentEntity != null) { %><%: eventEntity.DepartmentEntity.Name %><% } %></li>
                    <li><b>Dato</b> : <%: eventEntity.FromDate.ToString("dd-MM-yyyy") %></li>
                    <li class="text-right">
                        <% if (eventEntity.IsCanceled) { %>
                        <a href="CancelEvent.aspx?eventId=<%: eventEntity.Id %>&eventStatus=<%: eventEntity.IsCanceled %>" class="label label-warning" >Genoptag</a>
                        <% } else {%>
                        <a href="CancelEvent.aspx?eventId=<%: eventEntity.Id %>&eventStatus=<%: eventEntity.IsCanceled %>" class="label label-warning">Aflys</a>
                        <%}%>
                        <a href="CloneEvent.aspx?eventId=<%: eventEntity.Id %>" class="label label-warning">Klon</a>
                        <a href="EditEvent.aspx?eventId=<%: eventEntity.Id %>" class="label label-warning">Rediger</a>
                        <a class="btnDeleteEvent label label-danger" href="DeleteEvent.aspx?eventId=<%: eventEntity.Id %>">Slet</a>
                    </li>
                </ul>
            </div>
            <% } %>
            <% } else { %>
                Ingen aktiviteter.
            <% } %>
        </div>
    </div>
</asp:Content>



<%@ Page Title="Informationshåndtering" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InformationManagement.aspx.cs" Inherits="InformationManagement" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Informationshåndtering</h3>
        <div class="col-md-6">
            <label for="topic">Emne *</label>
            <asp:requiredfieldvalidator runat="server" id="emptyTopic" controltovalidate="topic" errormessage="Dette felt er obligatorisk" forecolor="Red" />
            <div class="form-group">
                <input type="text" class="form-control" id="topic" placeholder="Emne" runat="server" />
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label for="information">Information *</label>
                <asp:requiredfieldvalidator runat="server" id="emptyInformation" controltovalidate="input" errormessage="Dette felt er obligatorisk" forecolor="Red" />
                <input type="text" class="form-control" id="input" placeholder="Information" runat="server" />
            </div>
        </div>

        <div class="col-md-6 col-lg-offset-3">
            <div class="form-group white-select">
                <label for="rooms">Afdeling *</label>
                <select class="form-control selectpicker show-tick" id="departmentsSelect" runat="server">
                </select>
            </div>
        </div>

        <div class="col-md-12">
            <input type="hidden" value="" />
            <button id="submitButton" type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit" onclick="this.disabled=true; this.form.submit();">Tilføj</button>

            <div class="search-events-content text-center">
                <% if (informations.Count > 0) { %>
                <% foreach (var informationEntity in informations) { %>
                <div class="col-md-6">
                    <ul class="event-content white-bg text-left">
                        <% if (informationEntity.IsActive) {%>
                        <li><b class="deactivated-information">[ DEAKTIVERET ]</b> </li>
                        <li><b>Emne</b> : <%: informationEntity.Topic %></li>
                        <% } else {%>
                        <li><b>Emne</b> : <%: informationEntity.Topic %></li>
                        <% } %>
                        <li><b>Information</b> : <%: informationEntity.Input %></li>
                        <li><b>Afdeling</b> : <%: informationEntity.DepartmentEntity.Name %></li>
                        <li class="text-right">
                            <% if (informationEntity.IsActive) { %>
                            <a href="DeactivateInformation.aspx?informationId=<%: informationEntity.Id %>&informationStatus=<%: informationEntity.IsActive %>" class="btnDeactivateInformation label label-warning">Genoptag</a>
                            <% } else {%>
                            <a href="DeactivateInformation.aspx?informationId=<%: informationEntity.Id %>&informationStatus=<%: informationEntity.IsActive %>" class="btnDeactivateInformation label label-warning">Deaktiver</a>
                            <%}%>
                            <a class="btnDeleteInformation label label-danger" href="DeleteInformation.aspx?informationId=<%: informationEntity.Id %>">Fjern</a>
                        </li>
                    </ul>
                </div>
                <% } %>
                <% } else { %>
                Ingen informationer.
            <% } %>
            </div>
        </div>
    </div>
</asp:Content>

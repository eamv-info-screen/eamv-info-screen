<%@ Page Title="Opret lokale" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddRoom.aspx.cs" Inherits="AddRoom" %>

<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Opret lokale</h3>
        <div class="col-md-6">
            <label for="topic">Navn *</label>
            <asp:customvalidator id="alreadyExistValidator" display="Dynamic" controltovalidate="roomName" onservervalidate="AlreadyExistValidator" errormessage="Rummet eksiterer allerede" forecolor="Red" runat="server" />
            <asp:requiredfieldvalidator runat="server" id="emptyTopic" display="Dynamic" controltovalidate="roomName" errormessage="Dette felt er obligatorisk" forecolor="Red" />
            <div class="form-group">
                <input type="text" class="form-control" id="roomName" placeholder="Navn" runat="server" />
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group white-select">
                <label for="rooms">Afdeling *</label>
                <select class="form-control selectpicker show-tick" id="departmentsSelect" runat="server">
                </select>
            </div>
        </div>

        <div class="col-md-12">
            <input type="hidden" value="" />
            <button id="submitButton" type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit" onclick="this.disabled=true; this.form.submit();">Opret</button>
        </div>
    </div>
</asp:Content>


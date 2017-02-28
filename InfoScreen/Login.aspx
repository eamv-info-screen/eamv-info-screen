<%@ Page Title="Login" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<asp:Content ID="Menu" runat="server" ContentPlaceHolderID="menu"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Login</h3>
        <div class="col-md-7 col-lg-offset-3">
            <label for="main_userId">Brugernavn *</label>
            <asp:requiredfieldvalidator runat="server" id="emptyUserId" display="Dynamic" controltovalidate="userId" errormessage="Dette felt er obligatorisk" forecolor="Red" />
            <div class="form-group">
                <input type="text" class="form-control" id="userId" placeholder="Brugernavn" runat="server" />
            </div>
        </div>
        <div class="col-md-7 col-lg-offset-3">
            <label for="main_password">Adgangskode *</label>
            <asp:requiredfieldvalidator runat="server" id="emptyPassword" display="Dynamic" controltovalidate="password" errormessage="Dette felt er obligatorisk" forecolor="Red" />

            <input type="password" class="form-control" id="password" placeholder="Adgangskode" runat="server" />

            <%-- Husk mig:<asp:CheckBox ID="chkRememberMe" runat="server" />--%>

            <button id="submitButton" type="submit" class="btn pull-right btn-warning btn-login" runat="server" onserverclick="OnFormSubmit" onclick="this.disabled=true; this.form.submit();">Login</button>
        </div>
    </div>
</asp:Content>


<%@ Page Title="Opret bruger" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="CreateUser" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3">
        <h3 class="text-center">Opret bruger</h3>
        <div class="col-md-7 col-lg-offset-3">
            <div class="form-group">
                <label for="main_username">Brugernavn *</label>
                <asp:regularexpressionvalidator id="regexUserId" runat="server" display="Dynamic" errormessage="Minimum 2 karakter" controltovalidate="userId" validationexpression="^[a-zA-Z']{2,}$" forecolor="Red" />
                <asp:requiredfieldvalidator runat="server" display="Dynamic" id="emptyUserId" controltovalidate="userId" errormessage="Dette felt er obligatorisk" forecolor="Red" />
                <input type="text" class="form-control" id="userId" placeholder="Brugernavn" runat="server" />

            </div>
        </div>
        <div class="col-md-7 col-lg-offset-3">
            <div class="form-group">
                <label for="main_password">Password *</label>
                <asp:regularexpressionvalidator id="regexPassword" display="Dynamic" runat="server" errormessage="Minimum 4 karakter" controltovalidate="password" validationexpression="^[a-zA-Z0-9'@&#.\s]{4,}$" forecolor="Red" />

                <asp:requiredfieldvalidator runat="server" display="Dynamic" id="emptyPassword" controltovalidate="password" errormessage="Dette felt er obligatorisk" forecolor="Red" />
                <input type="text" class="form-control" id="password" placeholder="Password" runat="server" />
            </div>
        </div>

        <div class="col-md-7 col-lg-offset-3">
            <div class="form-group white-select">
                <label for="rooms">Rolle *</label>
                <select class="form-control selectpicker show-tick" id="rolesSelect" runat="server">
                </select>
            </div>
            <button id="submitButton" type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit" onclick="this.disabled=true; this.form.submit();">Opret</button>

        </div>
    </div>
</asp:Content>



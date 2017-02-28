<%@ Page Title="Opret afdeling" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddDepartment.aspx.cs" Inherits="AddDepartment" %>

<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Opret afdeling</h3>
        <div class="col-md-6  col-lg-offset-3">
            <asp:customvalidator id="alreadyExistValidator" display="Dynamic" controltovalidate="departmentName" onservervalidate="AlreadyExistValidator" errormessage="Afdelingen eksiterer allerede" forecolor="Red" runat="server" />
            <asp:requiredfieldvalidator runat="server" id="emptyTopic" display="Dynamic" controltovalidate="departmentName" errormessage="Dette felt er obligatorisk" forecolor="Red" />
            <label for="main_departmentName">Navn *</label>
            <div class="input-group">
                <input type="text" class="form-control" id="departmentName" placeholder="Navn" runat="server" />
                <span class="input-group-btn">
                    <button id="submitButton" type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit" onclick="this.disabled=true; this.form.submit();">Opret</button>
                </span>
            </div>
        </div>
    </div>
</asp:Content>


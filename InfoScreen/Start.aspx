<%@ Page Title="Skærmshow" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Start.aspx.cs" Inherits="Start" %>

<asp:Content ID="Content1" ContentPlaceHolderID="menu" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Vælg afdeling til skærmshow</h3>
        <div class="col-md-8 col-lg-offset-2">
            <div class="form-group white-select">
                <label for="department">Afdeling *</label>
                <div class="input-group">
                    <asp:RequiredFieldValidator runat="server" ID="emptyTopic" title="Vælg afdeling" Display="Dynamic" ControlToValidate="departmentsSelect" ErrorMessage="Dette felt er obligatorisk" ForeColor="Red" />
                    <select class="form-control selectpicker show-tick" id="departmentsSelect" runat="server">
                    </select>
                    <span class="input-group-btn">
                        <button id="submitButton" type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit" onclick="this.disabled=true; this.form.submit();" >Vis skærmshow</button>
                    </span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

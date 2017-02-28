<%@ Page Title="Se infoskærm" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ScreenPreviewChecker.aspx.cs" Inherits="ScreenPreviewChecker" %>


<asp:Content ID="mainContent" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Se infoskærm</h3>
        <div class="col-md-6 col-lg-offset-3">
            <div class="form-group white-select">
                <label for="rooms">Afdeling *</label>
                <select class="form-control selectpicker show-tick" id="departmentsSelect" runat="server">
                </select>
            </div>

            <div class="form-group">
                <label>Vælg dato *</label>
                <asp:calendar cssclass="event-calendar" id="searchDate" runat="server" ondayrender="CalendarDayRender">
                        <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                        <TitleStyle CssClass="month"></TitleStyle>
                        <DayStyle CssClass="event-calendar-day"></DayStyle>
                        <SelectedDayStyle CssClass="calendar-date-selected"></SelectedDayStyle>
                    </asp:calendar>
            </div>
            <div class="form-group">
                <button type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit_Preview" onclick="this.disabled=true; this.form.submit();">Preview </button>

                <%--
                    <button type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit_Aftenshold">Se aftenshold</button>
                    <button type="submit" class="btn pull-left btn-warning" runat="server" onserverclick="OnFormSubmit_Dagshold">Se dagshold</button>--%>
            </div>
        </div>
</asp:Content>

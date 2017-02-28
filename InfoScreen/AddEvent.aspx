<%@ Page Title="Opret aktivitet" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddEvent.aspx.cs" Inherits="AddEvent" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="main" runat="Server">
    <div class="col-md-6 col-lg-offset-3 edit-event-content">
        <h3 class="text-center">Opret aktivitet</h3>
        <div class="col-md-6">
            <div class="form-group">
                <label for="topic">Emne *</label>
                <asp:requiredfieldvalidator runat="server" id="emptyTopic" controltovalidate="topic" errormessage="Dette felt er obligatorisk" forecolor="Red" />
                <input type="text" class="form-control" id="topic" placeholder="Emne" runat="server" />
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label for="host">v/hvem</label>
                <input type="text" class="form-control" id="host" placeholder="v/hvem" runat="server" />
            </div>
        </div>
        <div class="col-md-6 col-lg-offset-3">
            <div class="form-group">
                <label>Start dato</label><span runat="server" id="startDatoWarning" style="color: red; visibility: hidden;"> Advarsel! Datoen ligger langt ude i fremtiden.</span>
                <asp:calendar cssclass="event-calendar" id="startDate" runat="server" ondayrender="Calendar_DayRender1">
                        <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                        <TitleStyle CssClass="month"></TitleStyle>
                        <DayStyle CssClass="event-calendar-day"></DayStyle>
                        <SelectedDayStyle CssClass="calendar-date-selected"></SelectedDayStyle>
                    </asp:calendar>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label for="startTime">Start tidspunkt *</label>
                <asp:customvalidator id="CustomValidator1" display="Dynamic" controltovalidate="startTime" onservervalidate="BeforeNowValidator" errormessage="er før nuværende tidspunkt" forecolor="Red" runat="server" />
                <asp:requiredfieldvalidator runat="server" id="emptyStartTime" display="Dynamic" controltovalidate="startTime" errormessage="Dette felt er obligatorisk" forecolor="Red" />
                <input type="text" id="startTime" class="form-control timepickerStart" name="timepickerStart" runat="server" />
            </div>

            <div class="form-group white-select">
                <label for="rooms">Afdeling *</label>
                <select class="form-control selectpicker show-tick" id="departmentsSelect" onchange="submitForm()" runat="server">
                </select>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label for="endTime">Slut tidspunkt *</label>
                <asp:customvalidator id="endTimeValidator" display="Dynamic" controltovalidate="endTime" onservervalidate="EndTimeValidator" errormessage="skal være efter start tidspunkt" forecolor="Red" runat="server" />
                <asp:requiredfieldvalidator runat="server" display="Dynamic" id="emptyEndTime" controltovalidate="endTime" errormessage="Dette felt er obligatorisk" forecolor="Red" />
                <input type="text" id="endTime" class="form-control timepickerEnd" name="timepickerEnd" runat="server" />
            </div>
            <div class="form-group white-select">
                <label for="rooms">Lokale *</label>
                <asp:requiredfieldvalidator runat="server" id="notChosenRoom" controltovalidate="roomsSelect" errormessage="Dette felt er obligatorisk" forecolor="Red" />
                <select class="form-control selectpicker" data-none-selected-text="Vælg lokale" multiple id="roomsSelect" runat="server">
                </select>
            </div>
        </div>

        <div class="col-md-6 col-lg-offset-3">
            <h3 class="text-center">Adskillige datoer/dage</h3>
        </div>

        <div class="col-md-6 col-lg-offset-3">
            <div class="form-group">
                <div class="form-group white-select">
                    <label for="rooms">Gentagelse</label>
                    <select class="form-control selectpicker" data-none-selected-text="Vælg gentagelses periode" multiple id="eventRepetitionSelect" runat="server">
                        <option value="0">Hver dag</option>
                        <option value="1">Hver mandag</option>
                        <option value="2">Hver tirsdag</option>
                        <option value="3">Hver onsdag</option>
                        <option value="4">Hver torsdag</option>
                        <option value="5">Hver fredag</option>
                    </select>
                </div>
            </div>
        </div>

        <div class="col-md-6 col-lg-offset-3">
            <div class="form-group">
                <label>Slut dato</label>
                <asp:calendar cssclass="event-calendar" id="endDate" runat="server" ondayrender="Calendar_DayRender2">
                        <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                        <TitleStyle CssClass="month"></TitleStyle>
                        <DayStyle CssClass="event-calendar-day"></DayStyle>
                        <SelectedDayStyle CssClass="calendar-date-selected"></SelectedDayStyle>
                    </asp:calendar>
            </div>
        </div>
        <div class="col-md-12">
            <input type="hidden" value="" />
            <button id="submitButton" type="submit" class="btn pull-right btn-warning" runat="server" onserverclick="OnFormSubmit" onclick="this.disabled=true; this.form.submit();">Opret</button>
        </div>
    </div>
</asp:Content>


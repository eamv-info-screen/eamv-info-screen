<%@ Page Title="Skærmshow" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DailyEvents.aspx.cs" Inherits="DailyEvents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="menu" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="Server">
    <div>
        <table class="events-table table" runat="server" id="eventsTable">
            <tr>
                <th runat="server">Emne</th>
                <th runat="server">V/Hvem</th>
                <th runat="server">Tidspunkt</th>
                <th runat="server">Lokale</th>
            </tr>
        </table>
    </div>

    <div class="marquee"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="extraScripts" runat="server">
    <script src="js/jquery.simplemarquee.js"></script>
    <script>
        (function ($) {
            var marquee, url, data, simpleMarqueeSettings, depId;
            marquee = $(".marquee");
            depId = location.search != "" ? location.search.split("?")[1].split("=")[1] : 1;
            data = { departmentId: depId };
            url = "JsonInformations.aspx";
            simpleMarqueeSettings = {
                speed: 150,
                handleHover: false,
                cycles: 1
            }

            marquee.width(window.innerWidth);

            fetchAllInformations();

            marquee.on("finish", function () {
                fetchAllInformations(true);
            });

            function createMarqueeContent(marquee, data) {
                var span, infos = "";
                marquee.html("");
                span = $("<span>");
                if (data.length == 0) {
                    span.text("");
                    marquee.append(span);
                } else {
                    for (var i = 0; i < data.length; i++) {
                        span = $("<span>");
                        span.text(data[i].Input).addClass("marqueeContent");
                        marquee.append(span);
                    }
                }

            }

            function fetchAllInformations(update) {
                $.get(url, data, function (response) {
                    createMarqueeContent(marquee, response);
                    marquee.simplemarquee(simpleMarqueeSettings);
                });
            }
        }(jQuery));
    </script>
</asp:Content>
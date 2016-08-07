$(function () {

    showLoginSectionOnClick();

    var chat = $.connection.chatHub;

    chat.client.showLoggedInStatus = function () {
        $("#loginnavv").text("Logout");
        $("#application").show();
    };

    chat.client.sendMessageToAllClients = function (name, message) {
        var encodedName = $("<div />").text(name).html();
        var encodedMsg = $("<div />").text(message).html();

        $("#discussion").append("<div><strong>" + encodedName + "</strong>:&nbsp;&nbsp;" + encodedMsg + "</div>");
    };

    $("#message").focus();

    $.connection.hub.start().done(function () {
        $("#message").enterKey(function () {
            chat.server.send($("#username").val(), $("#message").val());
            $("#message").val("").focus();
        });

        $("#login").click(function () {
            chat.server.login($("#username").val());
        });
    });
});

var showLoginSectionOnClick = function () {
    $("#loginnavv").click(function () {
        $("#welcome").hide();
    });
};

$.fn.enterKey = function (fnc) {
    return this.each(function () {
        $(this).keypress(function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == "13") {
                fnc.call(this, ev);
            }
        });
    });
}
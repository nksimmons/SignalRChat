$(function () {

    showLoginSectionOnClick();

    var chat = $.connection.chatHub;

    chat.client.showLoggedInStatus = function () {
        $("#loginnavv").text("Logout").attr("data-toggle", "");
        $("#application").show();
        $("#discussioncontainer").show();
        $("#login-status").val("1");
    };

    chat.client.sendMessageToAllClients = function (name, message) {
        var encodedName = $("<div />").text(name).html();
        var encodedMsg = $("<div />").text(message).html();

        $("#discussion").append("<div><strong>" + encodedName + "</strong>:&nbsp;&nbsp;" + encodedMsg + "</div>");
        var objDiv = document.getElementById("discussioncontainer");
        objDiv.scrollTop = objDiv.scrollHeight;
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
        var loginStatus = $("#login-status").val();
        if (loginStatus == 1) {
            window.location.reload(true);
        };
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
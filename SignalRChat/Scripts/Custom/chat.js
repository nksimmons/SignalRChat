$(function() {

    showLoginSectionOnClick();

    var chat = $.connection.chatHub;

    chat.client.manipulateDOMForLoggedInStatus = function () {
        $("#loginnavv").text("Logout").attr("data-toggle", "");
        $("#application").show();
        $("#discussioncontainer").show();
        $("#login-status").val("1");
    }

    $("#username").enterKey(function() {
        $("#login").trigger("click");
    });

    chat.client.sendMessage = function(name, message) {
        var encodedName = $("<div />").text(name).html();
        var encodedMsg = $("<div />").text(message).html();
        $("#discussion").append("<div><strong>" + encodedName + "</strong>:&nbsp;&nbsp;" + encodedMsg + "</div>");
        var objDiv = document.getElementById("discussioncontainer");
        objDiv.scrollTop = objDiv.scrollHeight;
    };

    chat.client.showConnectedClients = function (name) {
        var encodedName = $("<div />").text(name).html();
        $("#client-list").append("<p class='client-name' id='client-name-" + encodedName + "'>" + encodedName + "</p>");
    };

    chat.client.alertAtMaxCapacity = function () {
        $.connection.hub.stop();
        alert("We are at max capacity. Please try again later.");
        window.location.reload(true);
    };

    chat.client.removeDisconnectedClientFromClientList = function(nameOfClient) {
        var clientNameDomId = "#client-name-" + nameOfClient;
        $(clientNameDomId).remove();
    };

    $("#message").focus();

    $.connection.hub.start().done(function() {
        $("#message").enterKey(function() {
            chat.server.send($("#username").val(), $("#message").val());
            $("#message").val("").focus();
        });

        $("#login").click(function() {
            chat.server.login($("#username").val());
        });
    });
});

var showLoginSectionOnClick = function() {
    $("#loginnavv")
        .click(function() {
            $("#welcome").hide();
            $("#client-list-container").show();
            var loginStatus = $("#login-status").val();
            if (loginStatus == 1) {
                $.connection.hub.stop();
                alert("You have been logged out.");
                window.location.reload(true);
            };
        });
};

$.fn.enterKey = function(fnc) {
    return this.each(function() {
        $(this).keypress(function(ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == "13") {
                fnc.call(this, ev);
            }
        });
    });
};
﻿$("#BtnLogin").click(function () {
    var username = $("#username").val();
    var password = $("#password").val();

    $.ajax({
        type: "POST",
        url: "/api/user/login",
        data: JSON.stringify({ username, password }),
        contentType: "application/json",
        success: function (data) {
            alert(data);
            console.log(data);
        },
        error: function (error) {
            console.log(error.responseText);
        }
    });
});
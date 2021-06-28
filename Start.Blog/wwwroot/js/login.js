$("#BtnLogin").click(function () {
    var username = $("#username").val();
    var password = $("#password").val();

    $.ajax({
        type: "POST",
        url: "/api/user/login",
        data: JSON.stringify({ username, password }),
        contentType: "application/json",
        success: function (data) {
            alert(data);
            window.location.href = "/Post/Index";
        },
        error: function (error) {
            alert(error.responseText);
        }
    });
});
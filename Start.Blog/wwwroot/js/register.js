$("#BtnRegister").click(function () {
    var username = $("#username").val();
    var password = $("#password").val();
    var confirmPassword = $("#confirmPassword").val();

    $.ajax({
        type: "POST",
        url: "/api/user/register",
        data: JSON.stringify({ username, password, confirmPassword }),
        contentType: "application/json",
        success: function (data) {
            alert(data);
        },
        error: function (error) {
            alert(error.responseText);
        }
    });
});
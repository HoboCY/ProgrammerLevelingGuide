$("#BtnRegister").click(function () {
    var username = $("#username").val();
    var password = $("#password").val();
    var confirmPassword = $("#confirmPassword").val();

    axios.post("/api/user/register", {
        username: username,
        password: password,
        confirmPassword: confirmPassword
    }).then(function (response) {
        alert(response.data);
        window.location.href = "/user/login";
    }).catch(function (error) {
        alert(error.response.data);
    });
});
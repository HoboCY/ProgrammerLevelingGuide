$("#BtnLogin").click(function () {
    var username = $("#username").val();
    var password = $("#password").val();

    axios.post("/api/user/login", {
        username: username,
        password: password
    }).then(function (response) {
        alert(response.data);
        window.location.href = "/post/index";
    }).catch(function (error) {
        alert(error.response.data);
    });
});
$("#create").click(function () {
    var title = $("#title").val();
    var content = $("#content").val();

    axios.post("/api/post",
        {
            title,
            content
        })
        .then(function (response) {
            alert("Created success.");
            window.location.href = "/post/manage";
        }).catch(function (error) {
            alert(error.response.data);
        });
});
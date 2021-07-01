$("#update").click(function () {
    var id = $("#update").attr("data-id");

    var title = $("#title").val();
    var content = $("#content").val();

    axios.put(`/api/post/${id}`,
        {
            title,
            content
        })
        .then(function (response) {
            console.log(response);
            alert("Updated success.");
            window.location.href = "/post/manage";
        }).catch(function (error) {
            alert(error.response.data);
        });
});
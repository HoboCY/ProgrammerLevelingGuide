$(".delete").click(function () {
    var confirmed = confirm("confirm delete?");
    if (confirmed) {
        var id = this.dataset.id;
        axios.delete(`/api/post/${id}`)
            .then(function (response) {
                alert("Deleted success");
                window.location.href = "/post/manage";
            }).catch(function (error) {
                alert(error.response.data);
            });
    }
});

$(".edit").click(function () {
    var id = this.dataset.id;
    window.location.href = `/post/update/${id}`;
});

$(".create").click(function () {
    console.log(123);
    window.location.href = "/post/create";
})
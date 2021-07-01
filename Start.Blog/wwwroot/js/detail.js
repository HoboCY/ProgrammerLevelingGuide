$("#submit").click(function () {
    console.log(123);
    var content = $("#comment").val();
    var id = this.dataset.id;
    axios.post(`/api/post/${id}/comment`,
        {
            Content: content
        })
        .then(function (response) {
            alert("Commented success.");
            window.location.href = `/post/detail/${id}`;
        })
        .catch(function (error) {
            alert(error.response.data);
        });
});
$(".post-item").click(function () {
    var id = this.dataset.id;
    window.location.href = `/post/detail/${id}`;
});

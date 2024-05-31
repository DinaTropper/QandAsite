
$(() => {

    setInterval(() => {
        getLikes();
    }, 1000);

    $("#like-question").on('click', function () {
        const id = $(this).data('question-id');
        //var liked = isAlreadyLiked();
        //if (!liked) {
        $.post("/home/incrementLikes", { id }, function () {
            $("#like-question").addClass('text-danger');
            getLikes();
        });
        /* }*/
    });



    function getLikes() {
        const id = $("#id").val();
        $.get("/home/getlikes", { id }, function (likes) {
            $("#likes-count").text(likes);
        });
    }

    //function isAlreadyLiked() {
    //    var id = $("#id").val();
    //    $.get("/question/isLiked", { id }, function (liked) {
    //        return liked;
    //    });
    //}
});
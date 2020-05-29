function UpdateComment(url, ID) {
    $.get(url, { id: ID }, function (data, textStatus, jqXHR) {
        if (typeof data.error === 'undefined') {
            $(".CommentPH").html(data);
        }

        $('.Counter').text(Number($('.Comments-Counter').text()) + 1);
    });
}
$(function () {
    $("textarea").keydown(function () {
        var length = $(this).val().length
        $("#textCounter").text(length + 1);
    });
    $("i").click(function () {
        $("input").click();
    });
    $(".send").click(function () {
        var fd = new FormData;
        fd.append('uploadedFile', $("input").prop('files')[0]);
        fd.append('text', $("textarea").val());
        fd.append('articleID', $(".ArticleID").prop('id'));

        if ($("textarea").val().length > 1 | $("input").prop('files')[0] != null) {
            function setProgress(e) {
                if (e.lengthComputable) {
                    var complete = e.loaded / e.total;
                    $(".progress-bar").css('width', Math.floor(complete * 100)+"%");
                }
            }
            $.ajax({
                xhr: function () {
                    var xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", setProgress, false);
                    xhr.addEventListener("progress", setProgress, false);
                    return xhr;
                },
                url: '/Articles/AddComment',
                data: fd,
                processData: false,
                contentType: false,
                async: true,
                type: 'POST',
                success: function (respond, textStatus, jqXHR) {
                    $('textarea').val("");
                    if (typeof respond.error === 'undefined') {
                        UpdateComment('/Articles/UpdateComments', $(".ArticleID").prop('id'));
                    }
                }
            });
        }

        else
            alert("Комментарий не может быть пустым!");
    });
    
    $(".delete").click(function () {
        $.post($(this).attr('data'), { 'id': $(this).attr('id') }, function (data) {
            location.reload();
        });
    });
    $("input").change(function () {
        $("#pBar").css('display', 'flex');
    });
});
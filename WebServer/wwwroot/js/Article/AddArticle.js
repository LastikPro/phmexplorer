var Type;
$(".fa-download").click(function (params) {
    $("#Img").click();
});
$(".dropdown-item").click(function () { Type = $(this).attr('id'); $(".dropdown-toggle").text($(this).attr('id')); });
$(".send").click(function (params) {
    var fd = new FormData;
    fd.append('header', $("#header").val());
    fd.append('uploadedFile', $("#Img").prop('files')[0]);
    fd.append('Type', Type);
    fd.append('Adress', $("#Adress").val());
    fd.append('text', $("textarea").val());
    $.ajax({
        url: '/Articles/AddArticle',
        data: fd,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (respond, textStatus, jqXHR) {
            $(location).attr('href', respond);
        }
    });
});
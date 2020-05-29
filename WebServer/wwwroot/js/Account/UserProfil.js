var sex = 0;
var doc = document.getElementById("MyLoads");
var ModelEmail = doc.dataset.src;
$(".sex").click(function () {
    if ($(this).prop('id') === "sex1") {
        $(this).css("background-color", "green");
        $("#sex2").css("background-color", "gray");
        sex = 1;
    }
    if ($(this).prop('id') === "sex2") {
        $(this).css("background-color", "green");
        $("#sex1").css("background-color", "gray");
        sex = 2;
    }
});
$("i").click(function () {
    $("input").click();
});
$("#choos").change(function () { $("#pBar").css('display', 'flex'); });
$('.editbtn').click(function () {
    $('.main').css('display', 'none');
    $('.edit').css('display', 'flex');
});
$('.save').click(function () {
    var fd = new FormData;

    if ($("#Login").val().length > 1 && $("#Login").val() != ModelEmail)
        if (confirm("При изменении логина вам нужно будет заново войти в аккаунт!")) { fd.append('Login', $("#Login").val()); }
    fd.append('uploadedFile', $("#choos").prop('files')[0]);
    fd.append('Password', $("#Password").val());
    fd.append('Name', $("#Name").val());
    fd.append('Sex', sex);
    fd.append('BD', $("#BD").val());
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
        url: '/Account/UserProfil',
        data: fd,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (respond, textStatus, jqXHR) {
            if (typeof respond.error === 'undefined') {
                $('.main').css('display', 'flex');
                $('.edit').css('display', 'none');
            }
        }
    });

});
$('.back').click(function () {
    $('.main').css('display', 'flex');
    $('.edit').css('display', 'none');
    });
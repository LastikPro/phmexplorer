$(".col-2").click(function (params) {
    $(".selecktor").css('margin-left', '600px');
    $(".Register").css('display', 'block');
    $(".Login").css('display', 'none');
});
$(".col-3").click(function (params) {
    $(".selecktor").css('margin-left', '400px');
    $(".Login").css('display', 'block');
    $(".Register").css('display', 'none');
});
$("#InputEmailL").change(function () {
    $.get('/Account/Img', { 'data': $(this).val() }, function (params) {
        $("#circle").prop('src', params);
    });
});
$("#Button2").mouseenter(function () {
    if ($("#InputPassword1").val() != $("#InputPassword2").val()) {
        alert("Пароли не совпадают!");
        $("#InputPassword1").val("");
        $("#InputPassword2").val("");
    }
    if (!$("#InputEmail").val().includes("@"))
        alert("Email должен содержать: @");
    if ($("#InputPassword1").val() === "")
        alert("Строка пароля пуста!");
});
$("#Button1").click(function () {
    event.preventDefault();
    $.post('/Account/Login', { 'Email': $("#InputEmailL").val(), 'Password': $("#InputPassword").val(), '__RequestVerificationToken': $('input[type="hidden"]').val() }, function (response) {
        if (response != "OK") {
            $(".response").html(response);
        }
        else
            $(location).attr('href', "/Account/UserProfil");
    });
});
$(".fa-image").click(function () { $(".upImg").click(); });
$(".upImg").change(function () { $('.bar').css('display', 'flex'); });
$("#Button2").click(function () {
    event.preventDefault();
    var token = $('input[type="hidden"]');
    console.log(token);
    var fd = new FormData;
    fd.append('uploadedFile', $(".upImg").prop('files')[0]);
    fd.append('Email', $("#InputEmail").val());
    fd.append('Password', $("#InputPassword1").val());
    fd.append('__RequestVerificationToken', $('input[type="hidden"]').val());
    fd.append('Name', $("#InputName").val());
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
        url: '/Account/Register',
        data: fd,
        async: true,
        processData: false,
        contentType: false,
        type: 'POST',

        success: function (respond, textStatus, jqXHR) {
            if (respond != "OK") {
                $(".response").html(respond);
            }
            else {
                $(location).attr('href', "/Account/UserProfil");
            }
        }
    });
});
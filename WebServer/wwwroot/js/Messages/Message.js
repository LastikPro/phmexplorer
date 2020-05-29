var progress;
function sort(Obj) {
    var lists = $(Obj).get();
    var buff;
    for (var j = 0; j < lists.length / 2; j++)
        for (var i = 0 + j; i < lists.length - 1 - j; i++) {
            buff = $(lists[lists.length - 1 - i]).html();
            $(lists[lists.length - 1 - i]).html($(lists[i]).html());
            $(lists[i]).html(buff);
        }
}
function target(Obj, type) {
    var list = $(Obj).get();
    for (var i = 0; i < list.length; i++) {
        if (type === 1) {
            if ($(list[i]).prop("class").includes("active")) {
                $(list[i]).css("display", "table-row");
            } else {
                $(list[i]).css("display", "none");


            }
        }
        if (type === 2) {
            if ($(list[i]).prop("class").includes("active")) {
                $(list[i]).css("display", "none");
            } else {
                $(list[i]).css("display", "table-row");
            }
        }
        if (type === 0) {
            $(list[i]).css("display", "table-row");
        }
    }
}
function search(Obj, params) {
    var lists = $(Obj).get();
    for (var i = 0; i < lists.length; i++) {
        if ($(lists[i]).text().toLowerCase().includes(params.toLowerCase())) {
            $(lists[i]).css("display", "table-row");
        }
        else {
            $(lists[i]).css("display", "none");
        }
    }
}
function event(Obj, params) {
    var lists = $(Obj).get();
    for (var i = 0; i < lists.length; i++) {
        if ($(lists[i]).children('').children("input").prop("checked") === true) {
            if (params === 0) {
                $(lists[i]).children('').find(".star").prop("class", "fa fa-star");
                $.post("/Messages/Mark", { 'id': $(lists[i]).attr("id"), 'action': '0' });

            }
            if (params === 1) {
                $(lists[i]).children('').find(".fa-star").prop("class", "star");
                $.post("/Messages/Mark", { 'id': $(lists[i]).attr("id"), 'action': '1' });
            }
            if (params === 2) {
                $.post("/Messages/Delete", { 'id': $(lists[i]).attr("id") }, function (data) {
                    if (typeof data.error === 'undefined') {
                    
                        location.reload();
                    }
                });
            }
            if (params === 3) {
                $(lists[i]).prop("class", $(lists[i]).prop("class") + " active");
                $(lists[i]).children('').find(".col-9").text("Прочитано");
                $.post("/Messages/Viewed", { 'id': $(lists[i]).attr("id"), 'action': '0' });

            }
            if (params === 4) {
                $(lists[i]).prop("class", Obj.replace(".", ""));
                $(lists[i]).children('').find(".col-9").text("Не прочитано");
                $.post("/Messages/Viewed", { 'id': $(lists[i]).attr("id"), 'action': '1' });
            }
        }
    }
}
function send(fd) {
    function setProgress(e) {
        if (e.lengthComputable) {
            var complete = e.loaded / e.total;
            progress = Math.floor(complete * 100) + "%";
            $(".progress-bar").css('width', progress);
        }
    }
$.ajax({
    xhr: function () {
        var xhr = new window.XMLHttpRequest();
        xhr.upload.addEventListener("progress", setProgress, false);
        xhr.addEventListener("progress", setProgress, false);
        return xhr;
    },
    url: '/Messages/Send',
    data: fd,
    processData: false,
    contentType: false,
    async: true,
    type: 'POST',
    success: function (respond, textStatus, jqXHR) {
        $('textarea').val("");
        if (typeof respond.error === 'undefined') {
            if (progress === "100%")
                location.reload();
        }
    }
});
};

$("textarea").keydown(function () {
    var length = $(this).val().length;
    $("#textCounter").text(length + 1);
});
$("i").click(function () {
    $("input").click();
});
$("#uploadImage").change(function () { $('#pBar').css('display', 'flex'); });
$(".sendbtn").click(function () {
    var fd = new FormData;
    fd.append('uploadedFile', $("#uploadImage").prop('files')[0]);
    fd.append('text', $("textarea").val());
    fd.append('addressee', $("#addressee").val());
    fd.append('Header', $("#HeaderInp").val());
    fd.append('Name', $("#Who").val());
    if (typeof $('#Who').val() != "undefined") {
        if ($("#Who").val().length > 1)
            if ($("textarea").val().length > 1 | $("#uploadImage").prop('files')[0] != null) {
                send(fd);
            }
            else
                alert("Сообщение не может быть пустым!");
        else
            alert("Представтесь!");
    }
    else
    $.get('/Messages/Validrecipient', { 'Email': $("#addressee").val() }, function (data, textStatus, jqXHR) {
        if (data === 'ok') {                    
            if ($("textarea").val().length > 1 | $("#uploadImage").prop('files')[0] != null) {              
                send(fd);
            }
            else
                alert("Сообщение не может быть пустым!");
        }
        else alert("Получатель не найден!");
    });
});

$(".sortE").click(function () {
    target(".list1", 0);
    sort(".list1");
});
$(".all").click(function () {
    $(this).css("background-color", "yellow");
    $(".read").css("background-color", "grey");
    $(".notread").css("background-color", "grey");
    target(".list1", 0);
});
$(".read").click(function () {
    $(".all").css("background-color", "grey");
    $(this).css("background-color", "yellow");
    $(".notread").css("background-color", "grey");
    target(".list1", 1);
});
$(".notread").click(function () {
    $(".all").css("background-color", "grey");
    $(".read").css("background-color", "grey");
    $(this).css("background-color", "yellow");
    target(".list1", 2);
});
$("#SearchButton1").click(function () {
    search(".list1", $("#SearchArea1").val());
});
$(".favorites").click(function () {
    event(".list1", 0);
});
$(".readed").click(function () {
    event(".list1", 3);
});
$(".notreaded").click(function () {
    event(".list1", 4);
});
$(".delfromfavorites").click(function () {
    event('.list1', 1);
});
$(".delete").click(function () {
    event('.list1', 2);
});
$(".list1").click(function () {

});

$(".sortS").click(function () {
    target(".list2", 0);
    sort(".list2");
});
$(".all2").click(function () {
    $(this).css("background-color", "yellow");
    $(".read2").css("background-color", "grey");
    $(".notread2").css("background-color", "grey");
    target(".list2", 0);
});
$(".read2").click(function () {
    $(".all2").css("background-color", "grey");
    $(this).css("background-color", "yellow");
    $(".notread2").css("background-color", "grey");
    target(".list2", 1);
});
$(".notread2").click(function () {
    $(".all2").css("background-color", "grey");
    $(".read2").css("background-color", "grey");
    $(this).css("background-color", "yellow");
    target(".list2", 2);
});
$("#SearchButton2").click(function () {
    search(".list2", $("#SearchArea2").val());
});
$(".favorites2").click(function () {
    event(".list2", 0);
});
$(".readed2").click(function () {
    event(".list2", 3);
});
$(".notreaded2").click(function () {
    event(".list2", 4);
});
$(".delfromfavorites2").click(function () {
    event('.list2', 1);
});
$(".delete2").click(function () {
    event('.list2', 2);
});
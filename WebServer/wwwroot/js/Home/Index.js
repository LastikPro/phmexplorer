
function DisplayManager(params) {
    var lists = $(".tableitem").get();
    for (var i = 0; i < lists.length; i++) {
        if (params != "none") {
            if (!$(lists[i]).children('').first().attr('class').includes(params)) {
                $(lists[i]).css("display", "none");
            }
            else {
                $(lists[i]).css("display", "table-row");
            }
        }
        else {
            $(lists[i]).css("display", "table-row");
        }

    }
}
function Search(params) {
    var lists = $(".tableitem").get();
    for (var i = 0; i < lists.length; i++) {
        if ($(lists[i]).text().toLowerCase().includes(params.toLowerCase())) {
            $(lists[i]).css("display", "table-row");
        }
        else {
            $(lists[i]).css("display", "none");
        }
    }
}
function setTarget(ClassVar, Obj) {
    var lists = $(ClassVar).get();
    for (var i = 1; i < lists.length; i++) {
        $(lists[i]).attr('class', $(lists[0]).attr('class'));
    }
    if (!$(Obj).attr('id').includes('none'))
        $(Obj).attr('class', $(lists[0]).attr('class') + " Target");
}
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

$(function () {
    $(".submenuitemClass").click(function () {
        setTarget(".submenuitemClass", this);
        DisplayManager($(this).attr('id'));
    });
    $("#SearchButton").click(function () {
        Search($("input").val());
    });
    $('.Time').click(function () {
        DisplayManager("none");
        sort('.tableitem');
        if ($(this).attr('id') === "NewFirst") {
            $(this).text('Сначало старые');
            $(this).attr('id', 'OldFirst');
        }
        else {
            $(this).text("Сначало новые");
            $(this).attr('id', 'NewFirst');
        }
    });
    $('.CloseBtn').click(function () {
        $('.Event').css("display", "none");
        $('.OpenBtn').css("display", "inline");
    });
    $('.OpenBtn').click(function () {
        $(this).css("display", "none");
        $('.Event').css("display", "table");
    });
    $(".del").click(function () {
        $.post($(this).attr('href'), { 'id': $(this).attr('id') }, function (data) {
            location.reload();
        });
    });
});
 
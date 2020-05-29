function switching(params) {
    var lists = $(".tableitem").get();
    for (var i = 0; i < lists.length; i++) {
        if (params != "none") {
            if ($(lists[i]).attr('class').includes(params)) {
                $(lists[i]).css('display', 'none');
            }
            else {
                $(lists[i]).css('display', 'table-row');
            }
        }
        else {
            $(lists[i]).css('display', 'table-row');
        }

    }
};
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
};
$(function () {
    $('#item1').click(function (event) {
        $(this).css('background-color', 'green');
        $('#item3').css('background-color', 'grey');
        $('#item2').css('background-color', 'grey');
        switching('none');
    });

    $('#item2').click(function (event) {
        $(this).css('background-color', 'green');
        $('#item3').css('background-color', 'grey');
        $('#item1').css('background-color', 'grey');
        switching('ofline');
    });

    $('#item3').click(function (event) {
        $(this).css('background-color', 'green');
        $('#item1').css('background-color', 'grey');
        $('#item2').css('background-color', 'grey');
        switching('online');
    });

    $("#SearchButton").click(function () {
        Search($("input").val());
    });
    $(".del").click(function () {
        $.post($(this).attr('href'), { 'name': $(this).attr('id') }, function (data) {
            location.reload();
        });
    });
});
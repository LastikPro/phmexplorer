
$(".OverlayBtn").mouseover(function () {

    $(this).children('').last().css("animation-duration", '0.6s');
    $(this).children('').last().css("visibility", 'visible');
    $(this).children('').last().css("animation-name", 'OverlayUnderliningOver');
});
$(".OverlayBtn").mouseout(function () {
    $(this).children('').last().css("visibility", 'hidden');
    $(this).children('').last().css("animation-name", 'OverlayUnderliningOut');
});
$('.Header_Ellement_body').mouseover(function () {
    $(this).children('.text-right').children('').attr('style', ' transform: translate(40px,0px) rotate(180deg)');
});

$('.Header_Ellement_body').mouseout(function () {
    $(this).children('.text-right').children('').attr('style', ' transform: translate(30px,0px) ');
});

$('#Ellement1').mouseover(function () {
    $('#Skill_Body').css('animation-name', 'Slider_Right');
    $('#Skill_Body').css('animation-duration', '0.9s');
    $('#Skill_Body').css('transform', 'translateX(0px)');
});

$('#Ellement1').mouseout(function () {
    $('#Skill_Body').css('animation-name', 'Slider_Left');
    $('#Skill_Body').css('animation-duration', '0.7s');
    $('#Skill_Body').css('transform', 'translateX(1180px)');
});

$('#Ellement2').mouseover(function () {
    $('#Project_Body').css('animation-name', 'Slider_Right');
    $('#Project_Body').css('animation-duration', '0.9s');
    $('#Project_Body').css('transform', 'translateX(0px)');
});

$('#Ellement2').mouseout(function () {
    $('#Project_Body').css('animation-name', 'Slider_Left');
    $('#Project_Body').css('animation-duration', '0.7s');
    $('#Project_Body').css('transform', 'translateX(1180px)');
});
$('#Ellement3').mouseover(function () {
    $('#About_Body').css('animation-name', 'Slider_Right');
    $('#About_Body').css('animation-duration', '0.9s');
    $('#About_Body').css('transform', 'translateX(0px)');
});

$('#Ellement3').mouseout(function () {
    $('#About_Body').css('animation-name', 'Slider_Left');
    $('#About_Body').css('animation-duration', '0.7s');
    $('#About_Body').css('transform', 'translateX(1180px)');
});
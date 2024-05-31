/*global $, document, window, setTimeout, navigator, console, location*/
//$(document).ready(function () {

//    'use strict';

//    var usernameError = true;
//    var passwordError = true;

//    // Detect browser for css purpose
//    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
//        $('.form form label').addClass('fontSwitch');
//    }

//    // Label effect
//    $('input').focus(function () {

//        $(this).siblings('label').addClass('active');
//    });

//    // Form validation
//    $('input').blur(function () {

//        // User Name
//        if ($(this).hasClass('name')) {
//            if ($(this).val().length === 0) {
//                $(this).siblings('span.error').text('Porfavor escriba su nombre').fadeIn().parent('.form-group').addClass('hasError');
//                usernameError = true;
//            } else if ($(this).val().length > 1 && $(this).val().length <= 6) {
//                $(this).siblings('span.error').text('Porfavor escriba su nombre').fadeIn().parent('.form-group').addClass('hasError');
//                usernameError = true;
//            } else {
//                $(this).siblings('.error').text('').fadeOut().parent('.form-group').removeClass('hasError');
//                usernameError = false;
//            }
//        }
     

//        // PassWord
//        if ($(this).hasClass('pass')) {
//            if ($(this).val().length < 8) {
//                $(this).siblings('span.error').text('Porfavor escriba su contraseña').fadeIn().parent('.form-group').addClass('hasError');
//                passwordError = true;
//            } else {
//                $(this).siblings('.error').text('').fadeOut().parent('.form-group').removeClass('hasError');
//                passwordError = false;
//            }
//        }

//        // label effect
//        if ($(this).val().length > 0) {
//            $(this).siblings('label').addClass('active');
//        } else {
//            $(this).siblings('label').removeClass('active');
//        }
//    });


//    // form switch
//    $('a.switch').click(function (e) {
//        $(this).toggleClass('active');
//        e.preventDefault();

//        if ($('a.switch').hasClass('active')) {
//            $(this).parents('.form-peice').addClass('switched').siblings('.form-peice').removeClass('switched');
//        } else {
//            $(this).parents('.form-peice').removeClass('switched').siblings('.form-peice').addClass('switched');
//        }
//    });


//    // Form submit
//    $('form.signup-form').submit(function (event) {
//        event.preventDefault();

//        if (usernameError == true || emailError == true || passwordError == true || passConfirm == true) {
//            $('.name, .pass').blur();
//        } else {
//            $('.signup, .login').addClass('switched');

//            setTimeout(function () { $('.signup, .login').hide(); }, 700);
//            setTimeout(function () { $('.brand').addClass('active'); }, 300);
//            setTimeout(function () { $('.heading').addClass('active'); }, 600);
//            setTimeout(function () { $('.success-msg p').addClass('active'); }, 900);
//            setTimeout(function () { $('.success-msg a').addClass('active'); }, 1050);
//            setTimeout(function () { $('.form').hide(); }, 700);
//        }
//    });

//    // Reload page
//    $('a.profile').on('click', function () {
//        location.reload(true);
//    });


//});
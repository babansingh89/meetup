var errorDiv = $('#errorBlock').find('div.alert.alert-danger.display-none');
var successDiv = $('#errorBlock').find('div.alert.alert-success.display-none');

var ModalerrorDiv = $('#ErrorBlock_M').find('div.alert.alert-danger.display-none');
var ModalsuccessDiv = $('#SuccessBlock_M').find('div.alert.alert-success.display-none');

function HideSuccErrDiv() {
    successDiv.hide();
    errorDiv.hide();
}
function ModalHideSuccErrDiv() {
    //ModalsuccessDiv.hide();
    //ModalerrorDiv.hide();
    $("#ErrorBlock_M").hide();
    $("#SuccessBlock_M").hide();
}

function FieldError(elementID, msg) {
    var errorSpanLength = $('#' + elementID + "-error").length;
    if (errorSpanLength == 0) {
        var errorElement = $("<span>").attr("id", elementID + "-error").addClass("help-block help-block-error").html(msg);
        errorElement.insertAfter($('#' + elementID));
        $('#' + elementID).closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
    }
    else {
        $('#' + elementID + "-error").html(msg);
        $('#' + elementID).closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
    }
}
function FieldSuccess(elementID, msg) {
    var errorSpanLength = $('#' + elementID + "-error").length;
    if (errorSpanLength > 0) {
        $('#' + elementID + "-error").remove();
        $('#' + elementID).closest('.form-group').removeClass('has-error').addClass('has-success'); // set error class to the control group
    }
}
function FormError(message, type, parrentID) {
    if (type == 'Error') {
        if ($('.alert-danger').length > 0) {
            $('.alert-danger').find('span').text(message);
        }
        else {
            var pageErrorElement = $("<div>").addClass("alert alert-danger display-block").html("<button class='close' data-dismiss='alert'></button> <span>" + message + "</span>");
            //pageErrorElement.insertAfter($(parrentID));
            $(parrentID).append(pageErrorElement);
        }
    }
    else if (type == 'Success') {
        if ($('.alert-success').length > 0) {
            $('.alert-success').find('span').text(message);
        }
        else {
            var pageErrorElement = $("<div>").addClass("alert alert-success display-block").html("<button class='close' data-dismiss='alert'></button> <span>" + message + "</span>");
            //pageErrorElement.insertAfter($(parrentID));
            $(parrentID).append(pageErrorElement);
        }
    }
}

function FormSuccessError(Message, Status, parrentID) {
    successDiv.hide();
    errorDiv.hide();
    if (Status == "Error") {
        FormError(Message, Status, parrentID);
        errorDiv.show();
    }
    else if (Status == "Success") {
        FormError(Message, Status, parrentID);
        successDiv.show();
    }
    $("html, body").animate({ scrollTop: $('#div_Scroll').offset().top }, "slow");
}


function ModalPopupError(message, type, parrentID) {
    ModalHideSuccErrDiv();
    if (type == 'Error') {
        if ($('.alert-danger').length > 0) {
            $('.alert-danger').find('span').text(message);
        }
        else {
            var pageErrorElement = $("<div>").addClass("alert alert-danger display-block").html("<button class='close' data-dismiss='alert'></button> <span>" + message + "</span>");
            //pageErrorElement.insertAfter($(parrentID));
            $(parrentID).append(pageErrorElement);
        }
        $("#ErrorBlock_M").show();
    }
    else if (type == 'Success') {
        if ($('.alert-success').length > 0) {
            $('.alert-success').find('span').text(message);
        }
        else {
            var pageErrorElement = $("<div>").addClass("alert alert-success display-block").html("<button class='close' data-dismiss='alert'></button> <span>" + message + "</span>");
            //pageErrorElement.insertAfter($(parrentID));
            $(parrentID).append(pageErrorElement);
        }
        $("#SuccessBlock_M").show();
    }
}
function FieldErrorForDropDown(elementID, msg) {
    var errorSpanLength = $('#' + elementID + "-error").length;

    if (errorSpanLength == 0) {
        var errorElement = $("<span>")
            .attr("id", elementID + "-error")
            .addClass("help-block help-block-error")
            .html(msg);
        errorElement.insertBefore($('#' + elementID));
        $('#' + elementID).closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
        $('#' + elementID).closest('.col-md-6').removeClass('has-success').addClass('has-error'); // set error class to the control col-md-6
    }
    else {
        $('#' + elementID + "-error").html(msg);
        $('#' + elementID).closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
        $('#' + elementID).closest('.col-md-6').removeClass('has-success').addClass('has-error'); // set error class to the control col-md-6
    }
}
function FieldErrorOnClass(elementID, msg, classname) {
    var errorSpanLength = $('#' + elementID + "-error").length;
    if (errorSpanLength == 0) {
        var errorElement = $("<span>")
            .attr("id", elementID + "-error")
            .addClass("help-block help-block-error")
            .html(msg);
        errorElement.insertAfter($('#' + elementID));
        $('#' + elementID).closest(classname).removeClass('has-success').addClass('has-error'); // set error class to the control group
    }
    else {
        $('#' + elementID + "-error").html(msg);
        $('#' + elementID).closest(classname).removeClass('has-success').addClass('has-error'); // set error class to the control group
    }
}
function FieldErrorForDropDownOnClass(elementID, msg, classname) {
    var errorSpanLength = $('#' + elementID + "-error").length;

    if (errorSpanLength == 0) {
        var errorElement = $("<span>")
            .attr("id", elementID + "-error")
            .addClass("help-block help-block-error")
            .html(msg);
        errorElement.insertBefore($('#' + elementID));
        $('#' + elementID).closest(classname).removeClass('has-success').addClass('has-error'); // set error class to the control group
    }
    else {
        $('#' + elementID + "-error").html(msg);
        $('#' + elementID).closest(classname).removeClass('has-success').addClass('has-error'); // set error class to the control group
    }
}
$('body').on('focus', ".Decimal", function () {
    $(this).keypress(function (event) {
        return isDecimal(event, this)
    });
})
$('body').on('focus', ".Numeric", function () {
    $(this).keypress(function (event) {
        return isNumber(event, this)
    });
})
$('body').on('focus', ".Email", function () {
    $(this).keypress(function (event) {
        return isEmail(event, this)
    });
})
$(document).on('keyup', "input[type=text]", function () {

    var id = $(this).attr('id');
    var errorSpanLength = $('#' + id + "-error").length;
    if (errorSpanLength > 0) {
        $('#' + id + "-error").remove();
        $('#' + id).closest('.form-group').removeClass('has-error'); // set error class to the control group
        $('#' + id).closest('.col-md-6').removeClass('has-error'); // set error class to the control group
    }

});
$(document).on('keyup', "textarea", function () {

    var id = $(this).attr('id');
    var errorSpanLength = $('#' + id + "-error").length;
    if (errorSpanLength > 0) {
        $('#' + id + "-error").remove();
        $('#' + id).closest('.form-group').removeClass('has-error'); // set error class to the control group
    }

});
$(document).on('change', 'select', function () {

    var id = $(this).attr('id');
    var errorSpanLength = $('#sp' + id + "-error").length;
    if (errorSpanLength > 0) {
        $('#sp' + id + "-error").remove();
        $('#sp' + id).closest('.form-group').removeClass('has-error'); // set error class to the control group
        $('#sp' + id).closest('.col-md-6').removeClass('has-error'); // set error class to the control group
    }

});

function isEmail(evt, element) {
    if (isValidEmailAddress(element)) {
        alert(element);
        FieldError(element, 'Please Enter Correct Email ID')
        return false;
    } else {
        return true;
    }
}

function isValidEmailAddress(emailAddress) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);
}

function isDecimal(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode < 48 || charCode > 57))
        return false;

    return true;
}

function ShowCustomMessage(status, CustomMessage) {
    if (status == "Success") {
        Show_Hide_AlertDiv();
        $('#ModalSuccessAlert').append('<p>' + CustomMessage + '</p>').show();
    }
    else {
        Show_Hide_AlertDiv();
        $('#ModalErrorAlert').append('<p>' + CustomMessage + '</p>').show();
    }
    $('#AlertModal').modal();
}
function Show_Hide_AlertDiv() {
    $('#ModalSuccessAlert').empty();
    $('#ModalSuccessAlert').hide();
    $('#ModalErrorAlert').empty();
    $('#ModalErrorAlert').hide();
}

function fnValidatePAN(panno) {
    if (panno != "") {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = panno.substring(3, 4);
        if (panno.search(panPat) == -1) {
            //alert("Invalid Pan No");
            return false;
        }
    }
}

function IsValidEmail(_email) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(_email)) {
        return true;
    }
    else {
        return false;
    }
}

function IsValidPhone(_phone) {
    var regex = /^[0-9-+()]*$/;
    if (regex.test(_phone)) {
        return true;
    }
    else {
        return false;
    }
}
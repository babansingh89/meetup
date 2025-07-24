function ajaxindicatorstart(text) {
    if (jQuery('body').find('#resultLoading').attr('id') != 'resultLoading') {
        jQuery('body').append('<div id="resultLoading" style="display:none"><div><img src="../../Content/Image/ajax-loader5.gif" style="height: 100px"><div>' + text + '</div></div><div class="bg"></div></div>');
    }

    jQuery('#resultLoading').css({
        'width': '100%',
        'height': '100%',
        'position': 'fixed',
        'z-index': '10000000',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto'
    });

    jQuery('#resultLoading .bg').css({
        'background': '#000000',
        'opacity': '0.7',
        'width': '100%',
        'height': '100%',
        'position': 'absolute',
        'top': '0'
    });

    jQuery('#resultLoading>div:first').css({
        'width': '150px',
        'height': '75px',
        'text-align': 'center',
        'position': 'fixed',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto',
        'font-size': '16px',
        'z-index': '10',
        'color': '#ffffff'

    });

    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeIn(300);
    jQuery('body').css('cursor', 'wait');
}

function ajaxindicatorstop() {
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeOut(300);
    jQuery('body').css('cursor', 'default');
}


jQuery(document).ajaxStart(function () {
    //show ajax indicator
    ajaxindicatorstart('Please wait..');
}).ajaxError(function (event, xhr, settings) {
    if (xhr.status == 401) {
        alert("Sorry, your session has expired. Please login again to continue");
        window.location.href = "/Login";
    }
    else if (xhr.status == 500) {
        //alert("Error");
        window.location.href = "/Error";
    }
    else {
        alert("An error occurred: " + xhr.status + "Error: " + xhr.statusText);
    }
}).ajaxStop(function () {
    //hide ajax indicator
    ajaxindicatorstop();
});
$(document).keydown(function (e) {
    // ESCAPE key pressed
    if (e.keyCode == 27) {
        ajaxindicatorstop();
    }
});
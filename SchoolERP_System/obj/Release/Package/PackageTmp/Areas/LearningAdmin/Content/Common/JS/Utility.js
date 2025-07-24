function bindTable(url, data, table, withorwithoutheader) {
    alert(url);
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        success: function (data) {
            if (data.length > 0) {
                var data1 = data.replace(/(\r\n|\n|\r)/gm, "");
                var d = JSON.parse('[' + data1 + ']');
                //var d = JSON.parse('[' + data + ']');
                if (withorwithoutheader == 0) //withorwithoutheader=0 --> for With Out Header
                    makeTable(table, d);
                else if (withorwithoutheader == 1) //withorwithoutheader=1 --> for With Header
                {
                    makeTableWithDynamicHeader(table, d, 10)
                }
            }
            else {
                //makeTablewithNodata(table, 'No records found..');
            }
        },

        error: function (msg) {

            alert(msg.responseText);
        }
    });
}
function makeTableWithDynamicHeader(container, data) {
    $('#' + container + ' tbody').remove();
    $('#' + container + ' thead').remove();
    $.each(data, function (rowIndex, r) {
        var row = $("<tr/>");
        var thead = $("<thead/>");

        if (rowIndex == 0) {
            $.each(r, function (colIndex, c) {
                row.append($("<th/>").text(c));
            });
            thead.append(row);
            $('#' + container).append(thead);
        }
        else {
            $.each(r, function (colIndex, c) {
                //row.append($("<t" + (rowIndex == 0 ? "h" : "d") + "/>").text(c))
                row.append($("<td/>").text(c));
            });
            $('#' + container).append(row);
        }
    });

    $('#' + container).dataTable().fnDestroy();
    $('#' + container).dataTable({ "aLengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]], "bPaginate": true, "iDisplayLength": 10, "sPaginationType": "full_numbers" });
}
function makeTableWithDynamicHeader(container, data, pagedata) {
    $('#' + container + ' tbody').remove();
    $('#' + container + ' thead').remove();
    $.each(data, function (rowIndex, r) {
        var row = $("<tr/>");
        var thead = $("<thead/>");

        if (rowIndex == 0) {
            $.each(r, function (colIndex, c) {
                row.append($("<th/>").text(c));
            });
            thead.append(row);
            $('#' + container).append(thead);
        }
        else {
            $.each(r, function (colIndex, c) {
                //row.append($("<t" + (rowIndex == 0 ? "h" : "d") + "/>").text(c))
                row.append($("<td/>").text(c));
            });
            $('#' + container).append(row);
        }
    });

    $('#' + container).dataTable().fnDestroy();
    $('#' + container).dataTable({ "aLengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]], "bPaginate": true, "iDisplayLength": -1, "sPaginationType": "full_numbers" });
}



function makeTable(container, data) {
    $('#' + container + ' tbody').remove();
    $.each(data, function (rowIndex, r) {
        var row = $("<tr/>");

        $.each(r, function (colIndex, c) {
            // row.append($("<t" + (rowIndex == 0 ? "h" : "d") + "/>").text(c))
            row.append($("<td/>").text(c));
        });
        $('#' + container).append(row);
    });
    dataTableinit(container, true, 10);
    //$('#' + container).dataTable().fnDestroy();
    //$('#' + container).dataTable({ "aLengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]], "bPaginate": true, "iDisplayLength": 10, "sPaginationType": "full_numbers" });
}
$(document).ready(function () {
    var loc = window.location;
    var pathName = loc.pathname.substring(loc.pathname.lastIndexOf('/') + 1);
    var path = loc.pathname;
    var arr = path.split('/');
    $('#' + arr[1]).addClass('active open');
    if ($('#' + arr[1]).find("a").find("span.arrow").length > 0) {
        $('#' + arr[1]).find("a").find("span.arrow").addClass('open');
        var viewname = arr[2];
        if (arr[2].indexOf("?") >= 0) {
            viewname = arr[2].substring(arr[2].indexOf("?"));
        }
        $('#' + arr[2]).parent("ul").parent('li').addClass("active open");
        $('#' + arr[1]).find("ul").find('#' + arr[2]).addClass('active open');
    }
});
$('button#reloadDashboard').on('click', function () {
    getDashBoardData();
})
$(window).load(function () {
    jQuery("#loader").fadeOut();
    jQuery("#loader-wrapper").delay(1000).fadeOut("slow");
});



function percentage(max, total) {
    var percent = parseFloat(max / 100);
    var returnpercent = parseFloat(total / percent);
    //console.log('max: ' + max);
    //console.log('Total: ' + total);
    //console.log('Percent: ' + percent);
    //console.log(parseFloat(returnpercent));
    return returnpercent;
}

function PageError(msg, successerror) {
    if (successerror == "Success") {
        $('#errordiv').show();
        $('#errorextend').html("");
        if ($('.alert-success').length > 0)
            $('#errorextend').removeClass('alert-success');
        if ($('.alert-danger').length > 0)
            $('#errorextend').removeClass('alert-danger');
        $('#errorextend').addClass('alert-success');
        $('#errorextend').html(msg);
        $('html, body').animate({ scrollTop: 0 }, 500);
    }
    else {
        $('#errordiv').show();
        $('#errorextend').html("");
        if ($('.alert-success').length > 0)
            $('#errorextend').removeClass('alert-success');
        if ($('.alert-danger').length > 0)
            $('#errorextend').removeClass('alert-danger');
        $('#errorextend').addClass('alert-danger');
        $('#errorextend').html(msg);
        $('html, body').animate({ scrollTop: 0 }, 500);
    }
}
function PageErrorHide() {
    $('#errordiv').hide();
    $('#errorextend').html("");
    if ($('.alert-success').length > 0)
        $('#errorextend').removeClass('alert-success');
    if ($('.alert-danger').length > 0)
        $('#errorextend').removeClass('alert-danger');
}
function ModalError(msg, successerror) {
    if (successerror == "Success") {
        $('#modalerrordiv').show();
        $('#modalerror').html("");
        if ($('.alert-success').length > 0)
            $('#modalerror').removeClass('alert-success');
        if ($('.alert-danger').length > 0)
            $('#modalerror').removeClass('alert-danger');
        $('#modalerror').addClass('alert-success');
        $('#modalerror').html(msg);
    }
    else {
        $('#modalerrordiv').show();
        $('#modalerror').html("");
        if ($('.alert-success').length > 0)
            $('#modalerror').removeClass('alert-success');
        if ($('.alert-danger').length > 0)
            $('#modalerror').removeClass('alert-danger');
        $('#modalerror').addClass('alert-danger');
        $('#modalerror').html(msg);
    }
}
function ModalErrorHide() {
    $('#modalerrordiv').hide();
    $('#modalerror').html("");
    if ($('.alert-success').length > 0)
        $('#modalerror').removeClass('alert-success');
    if ($('.alert-danger').length > 0)
        $('#modalerror').removeClass('alert-danger');
}
function ExcelError(msg) {
    $('#errordiv').show();
    $('#errorextend').html("");
    if ($('.alert-success').length > 0)
        $('#errorextend').removeClass('alert-success');
    if ($('.alert-danger').length > 0)
        $('#errorextend').removeClass('alert-danger');
    $('#errorextend').addClass('alert-danger');
    var message = msg.split('/');
    $.each(message, function (key, value) {
        $('#errorextend').append($('<p/>').html(value));
    });
    $('html, body').animate({ scrollTop: 0 }, 500);
    //$('#errorextend').html(msg);
}
function ClearPageFields(field) {
    $.each(field, function (i, d) {

        $('#' + d).val('');
    });
}

function LoadMenu() {
    var roleID = "";
    $.ajax({
        type: "POST",
        url: '/Common/Menu',
        data: '{"roleID":"' + roleID + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        cache: false,
        success: function (data) {

            if (data.length > 0) {
                $('#menu').html(data);
            }
            else {
                //makeTablewithNodata(table, 'No records found..');
            }
        },
        error: function (msg) {
            //console.log(msg);
            //alert(msg.responseText);
        }
    });
}

function isNull(value) {
    if (value == "")
        return 0;
    else
        return value;
}

// Calculate Days
function GetTodayDate() {
    var d = new Date();
    var monthNo = d.getMonth() + 1;
    var day = d.getDate();
    var output = d.getFullYear() + '/' + (('' + monthNo).length < 2 ? '0' : '') + monthNo + '/' + (('' + day).length < 2 ? '0' : '') + day;
    var objDate = new Date(output),
        locale = "en-us",
        month = objDate.toLocaleString(locale, { month: "short" });
    output = (('' + day).length < 2 ? '0' : '') + day + '-' + month + '-' + d.getFullYear();
    return output;
}

function calculateDates(NewDate) {
    var startDay = convertDate(GetTodayDate());
    var endDay = convertDate(NewDate);
    var millisecondsPerDay = 1000 * 60 * 60 * 24;
    var millisBetween = endDay.getTime() - startDay.getTime();
    var days = millisBetween / millisecondsPerDay;
    return days;
}

function convertDate(str) {
    var parts = str.split("-");
    return new Date(ConvertToMonth(parts[1]) + "/" + parts[0] + "/" + parts[2]);
}

function ConvertToMonth(month) {
    var str = "JanFebMarAprMayJunJulAugSepOctNovDec";
    var mnthNo = (str.indexOf(month) / 3 + 1);
    mnthNo = ((('' + mnthNo).length < 2 ? '0' : '') + mnthNo);
    return mnthNo;
}


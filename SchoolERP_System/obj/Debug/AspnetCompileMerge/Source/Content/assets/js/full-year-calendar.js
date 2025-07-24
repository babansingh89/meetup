
(function ($) {
    'use strict';

    var PresentYr = new Date().getFullYear();
    var NextYr = (new Date().getFullYear() + 1);
    var StartYear = PresentYr + '-04-01';
    var EndYear = NextYr + '-03-31';

    var _default = {
        yearStart: StartYear,
        yearEnd: EndYear,
        //yearStart: new Date(),
        //yearEnd: new Date().setFullYear(new Date().getFullYear() + 1),
        weekDaysHolidays: [0],
        publicHolidays: [],
        niwds: [],
        halfDays: [],
        noSchools: [],
        testEvents: [],
        P: [],
        A: []
    };

    $.fn.fullYearCalendar = function (options) {
        options = $.extend(_default, options);
        this.each(function (index, element) {
            var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            var content = $('<div class="row fyc-calendar"></div>');
            var monthContainer = ""; var monthName = ""; var monthIndex = 0;
            var yearStart = moment(options.yearStart);
            //var yearStart = moment(options.yearStart);
            var yearEnd = moment(options.yearEnd);
            while (yearEnd.isAfter(yearStart)) {
                monthIndex = yearStart.month();
                monthName = months[monthIndex];
                monthContainer = $('<div class="col-sm-12 col-md-12 fyc-hide table-responsive" id="' + monthName + '"></div>');
                monthContainer.append(_getMonthTable(monthIndex, monthName, yearStart, options));
                content.append(monthContainer);
                yearStart.add(1, 'months');
            }
            $(element).append(content);
        });
        return {
            addHoliday: _addHoliday
        }
    }

    function _getMonthTable(monthIndex, monthName, yearStart, options) {
        var year = yearStart.year();
        var table = '<table class="table fyc-calendar-table"><thead><tr><th colspan="7" class="text-center">' + monthName + ' ' + year + '</th></tr>';
        table += '<tr><th>Mon</th><th>Tue</th><th>Wed</th><th>Thu</th><th>Fri</th><th>Sat</th><th>Sun</th></tr></thead>';
        var tbody = _getMonthTableBody(monthIndex, year, options);
        table += tbody + '</table>';
        return table;
    }

    function _getMonthTableBody(monthIndex, year, options) {
        var tbody = '<tbody><tr>';
        var day = 1;
        var counter = 0;
        var numberOfDays = moment('' + year + '-' + (monthIndex + 1), 'YYYY-MM').daysInMonth();
        var firstDayOfMonth = moment([year, monthIndex]).day();
        if (firstDayOfMonth === 0) {
            firstDayOfMonth = 7;
        }
        firstDayOfMonth--;
        while (firstDayOfMonth > 0) {
            tbody += '<td></td>';
            firstDayOfMonth--;
            counter++;
        }
        while (day <= numberOfDays) {
            if (counter > 6) {
                counter = 0;
                tbody += '</tr><tr>';
            }
            var currentDate = moment('' + year + '-' + (monthIndex + 1) + '-' + day, 'YYYY-MM-DD');
            var currentDateFormated = currentDate.format('YYYY-MM-DD');
            var classList = 'fyc-day ';
            if (options.weekDaysHolidays.indexOf(currentDate.day()) > -1) {
                classList += 'fyc-holiday ';
            }
            if (options.publicHolidays.indexOf(currentDateFormated) > -1) {
                classList += 'fyc-public-holiday ';
            }
            if (options.niwds.indexOf(currentDateFormated) > -1) {
                classList += 'fyc-niwd-holiday ';
            }
            if (options.halfDays.indexOf(currentDateFormated) > -1) {
                classList += 'fyc-halfDay-holiday ';
            }
            if (options.noSchools.indexOf(currentDateFormated) > -1) {
                classList += 'fyc-noSchools-holiday ';
            }
            if (options.testEvents.indexOf(currentDateFormated) > -1) {
                classList += 'fyc-testEvents-holiday ';
            }
            if (options.P.indexOf(currentDateFormated) > -1) {
                classList += 'fyc-P-holiday ';
            }
            if (options.A.indexOf(currentDateFormated) > -1) {
                classList += 'fyc-A-holiday ';
            }
            tbody += '<td data-fyc-date=' + currentDateFormated + ' class="' + classList + '">' + day + '</td>';
            counter++;
            day++;
        }
        return tbody + '</tr></tbody>';
    }

    function _addHoliday(data) {
        var className = "";
        switch (data.type) {
            case 'publicHolidays':
                className = "fyc-public-holiday";
                break;
            case 'niwds':
                className = "fyc-niwd-holiday";
                break;
            case 'halfDays':
                className = "fyc-halfDay-holiday";
                break;
            case 'noSchools':
                className = "fyc-noSchools-holiday";
                break;
            case 'testEvents':
                className = "fyc-testEvents-holiday";
                break;
            case 'P':
                className = "fyc-P-holiday";
                break;
            case 'A':
                className = "fyc-A-holiday";
                break;

            default:
                break;
        }
        var fromdate = moment(data.from, 'YYYY-MM-DD');
        if (data.to != undefined) {
            var todate = moment(data.to, 'YYYY-MM-DD');
            while (todate >= fromdate) {
                $('td[data-fyc-date=' + fromdate.format('YYYY-MM-DD') + ']').addClass(className);
                fromdate = fromdate.add(1, 'days');
            }
        } else {
            $('td[data-fyc-date=' + data.from + ']').addClass(className);
        }
    }

}(jQuery));

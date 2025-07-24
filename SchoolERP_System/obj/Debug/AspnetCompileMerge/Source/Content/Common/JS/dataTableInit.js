function dataTableinit(tableID, isDestroy, length) {
    if (isDestroy == true) {
        $('#' + tableID).dataTable().fnDestroy();
    }

    var table = $('#' + tableID);
    table.dataTable({

        // Internationalisation. For more info refer to http://datatables.net/manual/i18n
        "language": {
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            },
            "emptyTable": "No data available in table",
            "info": "Showing _START_ to _END_ of _TOTAL_ records",
            "infoEmpty": "No records found",
            "infoFiltered": "(filtered1 from _MAX_ total records)",
            "lengthMenu": "Show _MENU_",
            "search": "Search:",
            "zeroRecords": "No matching records found",
            "paginate": {
                "previous": "Prev",
                "next": "Next",
                "last": "Last",
                "first": "First"
            }
        },

        // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
        // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
        // So when dropdowns used the scrollable div should be removed. 
        //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

        "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
        "pagingType": "bootstrap_extended",

        "lengthMenu": [
            [5, 10, 15, 20, -1],
            [5, 10, 15, 20, "All"] // change per page values here
        ],
        // set the initial value
        "pageLength": length,
        "columnDefs": [{  // set default column settings
            'orderable': false,
            'targets': [0]
        }, {
            "searchable": true,
            "targets": [0]
        }],

        "order": [
            [1, "asc"]
        ] // set first column as a default sort by asc        
    });
}

function dataTableOpeninit(tableID, isDestroy, length) {

    if (isDestroy == true) {
        $('#' + tableID).dataTable().fnDestroy();
    }
    var table = $('#' + tableID);

    table.dataTable({
        "searching": false,
        "paging": false,
        "ordering": false,
        // Internationalisation. For more info refer to http://datatables.net/manual/i18n
        "language": {
            //"aria": {
            //    "sortAscending": ": activate to sort column ascending",
            //    "sortDescending": ": activate to sort column descending"
            //},
            "emptyTable": "No data available in table",
            "info": "Showing _START_ to _END_ of _TOTAL_ records",
            "infoEmpty": "No records found",
            "infoFiltered": "(filtered1 from _MAX_ total records)",
            "lengthMenu": "Show _MENU_",
            "search": "Search:",
            "zeroRecords": "No matching records found"//,
            //"paginate": {
            //    "previous": "Prev",
            //    "next": "Next",
            //    "last": "Last",
            //    "first": "First"
            //}
        },
        "rowCallback": function (row, data, index) {
            //check to see if row is expanded
            if (!$(row).attr('role') || $(row).attr('role') != 'row' || $(row).hasClass('parent')) {
                return;
            }
            //add class to expand row
            $(row).addClass('parent');
        },
        // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
        // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
        // So when dropdowns used the scrollable div should be removed. 
        //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

        "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
        "pagingType": "bootstrap_extended",

        //"lengthMenu": [
        //    [5, 10, 15, 20, -1],
        //    [5, 10, 15, 20, "All"] // change per page values here
        //],
        // set the initial value
        "pageLength": length,
        //"columnDefs": [{  // set default column settings
        //    'orderable': false,
        //    'targets': [0]
        //}, {
        //    "searchable": false,
        //    "targets": [0]
        //}],

        "order": [
            [1, "asc"]
        ] // set first column as a default sort by asc        
    });

    $('#' + tableID + ' tr').find('td:first').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
    });
    $("<style type='text/css'> table.dataTable.dtr-inline.collapsed>tbody>tr>td:first-child:before, table.dataTable.dtr-inline.collapsed>tbody>tr>th:first-child:before { display: none !important; } </style>").appendTo("head");


    $("<style type='text/css'> table.dataTable.dtr-inline.collapsed>tbody>tr>td:first-child:before, table.dataTable.dtr-inline.collapsed>tbody>tr>th:first-child:before { display: none !important; } " +
        "tbody tr {background-color: #fff !important;}" +
        "tbody tr:nth-child(4n+1) {background-color: #eef1f5 !important;}" +
        "tbody tr:nth-child(4n+2) {background-color: #eef1f5 !important;}" +
        "table.dataTable>tbody>tr:nth-child(4n+1):hover {background: #eef1f5 !important;}" +
        "table.dataTable>tbody>tr:nth-child(4n+2):hover {background: #eef1f5 !important;}" +
        "  </style>").appendTo("head");

}
function dataTableinitWithoutSortSearch(tableID, isDestroy, length) {

    if (isDestroy == true) {
        $('#' + tableID).dataTable().fnDestroy();
    }
    var table = $('#' + tableID);

    table.dataTable({
        "searching": false,
        "paging": false,
        "ordering": false,
        // Internationalisation. For more info refer to http://datatables.net/manual/i18n
        "language": {
            //"aria": {
            //    "sortAscending": ": activate to sort column ascending",
            //    "sortDescending": ": activate to sort column descending"
            //},
            "emptyTable": "No data available in table",
            "info": "Showing _START_ to _END_ of _TOTAL_ records",
            "infoEmpty": "No records found",
            "infoFiltered": "(filtered1 from _MAX_ total records)",
            "lengthMenu": "Show _MENU_",
            "search": "Search:",
            "zeroRecords": "No matching records found"//,
            //"paginate": {
            //    "previous": "Prev",
            //    "next": "Next",
            //    "last": "Last",
            //    "first": "First"
            //}
        },
        "rowCallback": function (row, data, index) {
            //check to see if row is expanded
            if (!$(row).attr('role') || $(row).attr('role') != 'row' || $(row).hasClass('parent')) {
                return;
            }
            //add class to expand row
            $(row).addClass('parent');
        },
        // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
        // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
        // So when dropdowns used the scrollable div should be removed. 
        //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

        "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
        "pagingType": "bootstrap_extended",

        //"lengthMenu": [
        //    [5, 10, 15, 20, -1],
        //    [5, 10, 15, 20, "All"] // change per page values here
        //],
        // set the initial value
        "pageLength": length,
        //"columnDefs": [{  // set default column settings
        //    'orderable': false,
        //    'targets': [0]
        //}, {
        //    "searchable": false,
        //    "targets": [0]
        //}],

        "order": [
            [1, "asc"]
        ] // set first column as a default sort by asc        
    });

   
    
}
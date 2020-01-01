/*
$(".addproduct").click(function() {
    var element = $("#parentDiv").find(".myClassNameOfInterest");
    
    close-modal
    //do something fired 5 times});
}


$(document).on("click", "#close-modal", function (event) {
    $('#add-update-modal').modal('hide');
});

$("#close-modal").click(function () {
    alert('button clicked');
    }
);
*/

function ShowAllUsersChange() {
    var table = $('#pagedResultsDataTable').DataTable();

    table.ajax.reload(function (json) {

    });
}

function ChangeDateFilter() {


    var url = $("#table-actions").data("setreportdatefilterurl");
    var formData = { customReportId: $('#SelectedCustomReport').val(), dateFilter: $('#DateFilterIds').find('option:selected').val() };

    $.ajax({
        type: "POST",
        url: url,
        cache: false,
        data: formData,
        success: function (data) {
            if (data.result === true) {

                var table = $('#pagedResultsDataTable').DataTable();
                table.ajax.reload(function (json) {
                });

            }
            else {
                if (data.errorMessage !== null) {
                    $('#mainheader').notify(data.errorMessage, { "status": "danger", "pos": "top-right" });
                }
            }
        },
        complete: function () {
            //
        }
    });

}


function Delete(id) {
    var url = $("#table-actions").data("deleteurl");
    CallServerByUrlAndId(id, url,true);
}

function RandomiseCustomer(id) {
    var url = $("#table-actions").data("randomisecustomer");
    CallServerByUrlAndId(id, url,false);
}

function CallServerByUrlAndId(id,url, isDelete) {

    if ($('#errorMessage').hasClass('hidden') === false) {
        $('#errorMessage').addClass('hidden');
    }

    if ($('#successMessage').hasClass('hidden') === false) {
        $('#successMessage').addClass('hidden');
    }

    if ($('#anonSuccessMessage').length > 0) {
        if ($('#anonSuccessMessage').hasClass('hidden') === false) {
            $('#anonSuccessMessage').addClass('hidden');
        }
    }
    

    if ($('#updatedSuccessMessage').length > 0) {
        if ($('#updatedSuccessMessage').hasClass('hidden') === false) {
            $('#updatedSuccessMessage').addClass('hidden');
        }
    }

    var isDeleting = isDelete;
    
    var formData = { id: id };

    $.ajax({
        type: "POST",
        url: url,
        cache: false,
        data: formData,
        success: function (data) {
            if (data.result === true) {
                var table = $('#pagedResultsDataTable').DataTable();
                table.ajax.reload(function (json) {
                });

                if (isDeleting) {
                    if ($('#successMessage').hasClass('hidden') === true) {
                        $('#successMessage').removeClass('hidden');
                    }
                }
                else {
                    if ($('#anonSuccessMessage').hasClass('hidden') === true) {
                        $('#anonSuccessMessage').removeClass('hidden');
                    }
                }
                
            }
            else {
                if (data.errorMessage !== null) {
                    if ($('#errorMessage').hasClass('hidden') === true) {
                        $('#errorMessage').removeClass('hidden');
                    }

                    var $summary = $('#errorMessage').find("[data-valmsg-summary=true]");

                    // find the unordered list
                    var $ul = $summary.find("ul");

                    // Clear existing errors from DOM by removing all element from the list
                    $ul.empty();

                    // Add error to the list
                    $("<li />").html(data.errorMessage).appendTo($ul);
                }
            }
        },
        complete: function () {
            //
        }
    });

}

function CompleteTask(id) {

    if ($('#errorMessage').hasClass('hidden') === false) {
        $('#errorMessage').addClass('hidden');
    }

    if ($('#successMessage').hasClass('hidden') === false) {
        $('#successMessage').addClass('hidden');
    }

    if ($('#anonSuccessMessage').length > 0) {
        if ($('#anonSuccessMessage').hasClass('hidden') === false) {
            $('#anonSuccessMessage').addClass('hidden');
        }
    }

    if ($('#updatedSuccessMessage').length > 0) {
        if ($('#updatedSuccessMessage').hasClass('hidden') === false) {
            $('#updatedSuccessMessage').addClass('hidden');
        }
    }
    
    var url = $("#table-actions").data("completedurl");
    var formData = { id: id };

    $.ajax({
        type: "POST",
        url: url,
        cache: false,
        data: formData,
        success: function (data) {
            if (data.result === true) {
                var table = $('#pagedResultsDataTable').DataTable();
                table.ajax.reload(function (json) {
                });

                if ($('#updatedSuccessMessage').length > 0) {
                    if ($('#updatedSuccessMessage').hasClass('hidden') === true) {
                        $('#updatedSuccessMessage').removeClass('hidden');
                    }
                }
                

            }
            else {
                if (data.errorMessage !== null) {
                    if ($('#errorMessage').hasClass('hidden') === true) {
                        $('#errorMessage').removeClass('hidden');
                    }

                    var $summary = $('#errorMessage').find("[data-valmsg-summary=true]");

                    // find the unordered list
                    var $ul = $summary.find("ul");

                    // Clear existing errors from DOM by removing all element from the list
                    $ul.empty();

                    // Add error to the list
                    $("<li />").html(data.errorMessage).appendTo($ul);
                }
            }
        },
        complete: function () {
            //
        }
    });

}

function InitDataTable() {
    if ($('#pagedResultsDataTable').length < 1) {
        return;
    }

    var isAbleToAnonymise = false;
    if ($('#CanAnonymise').length > 0) {
        if ($('#CanAnonymise').val() === "True") {
            isAbleToAnonymise = true;
        }
    }

    var editUrl = $("#table-actions").data("editurl") + '/';
    var randomiseCustomerUrl = $("#table-actions").data("randomisecustomer") + '?id=';
    var deviceTypeUrl = $("#table-actions").data("devicetypeurl") + '/';
    var brandEditUrl = $("#table-actions").data("brandediturl") + '/';
    var playUrl = $("#table-actions").data("playurl") + '/';
    var settingsUrl = $("#table-actions").data("settingsurl") + '/';
    var searchcolumns = $("#table-actions").data("searchcolumns");

    var showjobdetailUrl = $("#table-actions").data("showjobdetailurl") + '?jobNumber=';
    /*
    let datatableTranslations = $("#dataTableTranslations");
    let gridviewsearch = datatableTranslations.data("gridviewsearch");
    let gridviewrecordsperpage = datatableTranslations.data("gridviewrecordsperpage");
    let gridviewpageofpages = datatableTranslations.data("gridviewpageofpages");
    let gridviewnothingfound = datatableTranslations.data("gridviewnothingfound");
    let gridviewnorecords = datatableTranslations.data("gridviewnorecords");
    let gridviewrecordfilter = datatableTranslations.data("gridviewrecordfilter");
    let gridviewfirst = datatableTranslations.data("gridviewfirst");
    let gridviewlast = datatableTranslations.data("gridviewlast");
    let gridviewnext = datatableTranslations.data("gridviewnext");
    let gridviewprevious = datatableTranslations.data("gridviewprevious");
    let editWord = datatableTranslations.data("edit");
    let deleteWord = datatableTranslations.data("delete");
    let duplicateWord = datatableTranslations.data("duplicate");
    let completedWord = datatableTranslations.data("completed");
    var playWord = datatableTranslations.data("play");
    var viewWord = datatableTranslations.data("view");
    var settingsWord = datatableTranslations.data("settings");
    var randomiseCustomerWord = datatableTranslations.data("randomisecustomer");
    */
    let editWord = "edit"; 
    let deleteWord = "delete"; 
    let duplicateWord = "duplicate"; 
    let completedWord = "completed";
    
    var viewWord = "view";
    
    var randomiseCustomerWord = "randomise customer";

    var urlstring = $("#pagedResultsDataTable").data("url");

    var columnOrdering = true;
    var recordsperpage = 10;
    if ($('#pagedResultsDataTable').hasClass('gridview')) {
        columnOrdering = false;
        recordsperpage = 25;
    }

    var dtInstance = $('#pagedResultsDataTable').DataTable({
        'responsive': true,
        'paging': true,  // Table pagination
        'ordering': columnOrdering,  // Column ordering 
        'info': true,  // Bottom left status text
        'iDisplayLength': recordsperpage,

        "bServerSide": true,
        "processing": true,
        "sAjaxSource": urlstring,
        //"bProcessing": false,
        "aoColumns": getDataTableColumns(),

        
        "fnServerParams": function (aoData) {
            if ($("#DateFrom").length)
            {
                aoData.push({ "name": "DateFrom", "value": $("#DateFrom").val() });
            }
            if ($("#DateTo").length)
            {
                aoData.push({ "name": "DateTo", "value": $("#DateTo").val() });
            } 

            if ($("#SelectedProduct").length) {
                aoData.push({ "name": "SelectedProduct", "value": $("#SelectedProduct").val() });
            }

            if ($("#UserName").length) {
                aoData.push({ "name": "UserName", "value": $("#UserName").val() });
            }
        },

        "footerCallback": function (row, data, start, end, display) {
            if ($('#PageColumnTotals').length > 0) {
                var api = this.api(); //, data;
                var cols = $('#PageColumnTotals').val().split(",");
                for (var colnum = 0; colnum < cols.length; colnum++) {
                    showFooter(api, cols[colnum], row, data, start, end, display);
                }

                PopulateTotals(api);
            }
        },

        // Text translation options
        // Note the required keywords between underscores (e.g _MENU_)
        "oLanguage": {
            "oPaginate": {
                //sFirst: gridviewfirst,
                //sLast: gridviewlast,
                //sNext: gridviewnext,
                //sPrevious: gridviewprevious
            },
            //sSearch: gridviewsearch, //'Search all columns: ',
            //sLengthMenu: gridviewrecordsperpage, // '_MENU_ records per page',
            //info: gridviewpageofpages, //'Showing page _PAGE_ of _PAGES_',
            //zeroRecords: gridviewnothingfound, //'Nothing found - sorry',
            //infoEmpty: gridviewnorecords, //'No records available',
            //infoFiltered: gridviewrecordfilter //'(filtered from _MAX_ total records)'
        }


    });

    $('a.toggle-vis').on('click', function (e) {
        e.preventDefault();

        // Get the column API object
        var column = dtInstance.column($(this).attr('data-column'));

        // Toggle the visibility
        column.visible(!column.visible());
    });

    $.fn.dataTable.ext.errMode = 'none';
    $('#pagedResultsDataTable')
    .on('error.dt', function (e, settings, techNote, message) {

        if (settings.jqXHR.status === 401) {
            window.location = settings.jqXHR.statusText;
        }
    }).DataTable();
    //Click handlers
    $('#pagedResultsDataTable tbody').on('click', 'a', function (e) {
        var $row = $(this).closest('tr');
        // Get row data
        var data = dtInstance.row($row).data();
        var id = data[0];
        let action = jQuery(this).data('action');

        switch (action) {
            case 'randomisecustomer':
                {
                    let msg = "Confirm Randomise";// $("#dataTableTranslations").data("confirmanonymise");
                    if (confirm(msg)) {
                        RandomiseCustomer(id);
                    }
                }
                break;
            case 'delete':
                {
                    let msg = "Confirm Delete"; //$("#dataTableTranslations").data("confirmdelete");
                    if (confirm(msg)) {
                        Delete(id);
                    }
                }
                break;
            case 'showUserPermission':
                {
                    var url = $("#table-actions").data("editurl");
                    ShowHtmlModalView(id, url);
                }
                
                break;
            
            default:
                {
                    let url = $("#table-actions").data("editurl") + '/' + data[0];
                    window.location.href = url; // + '/' + data[0];
                }
                break;
        }
    });

    $('body').delegate('#pagedResultsDataTable tbody tr', 'click', function (e) {

        var $this = $(this);
        if ($this.hasClass('clicked')) {

            //here is your code for double click
            var $row = $(this).closest('tr');

            // Get row data
            var data = dtInstance.row($row).data();
            var id = data[0];

            $('#Id').val(id);
            if ($("#frmEdit").length < 1) {
                return false;
            }
            var form = $("#frmEdit");
            if (!form.valid()) {
                return false;
            }
            form.submit();
            return;

        } else {
            $this.addClass('clicked');

            setTimeout(function () {
                $this.removeClass('clicked');
            }, 500);
        }

    });

    //Handles jquery datatable exception messages
    $.fn.dataTable.ext.errMode = 'none';
    $('#pagedResultsDataTable').on('error.dt', function (e, settings, techNote, message) {
        console.log('An error has been reported by DataTables: ', message);
        $('#mainheader').notify("Error reported by DataTables:<br /><br />" + message,
                { "status": "danger" });
    });

    var inputSearchClass = 'datatable_input_col_search';
    var columnInputs = $('tfoot .' + inputSearchClass);

    // On input keyup trigger filtering
    columnInputs
      .keyup(function () {
          var dtTable = $('#pagedResultsDataTable').dataTable();
          dtTable.fnFilter(this.value, columnInputs.index(this));
      });

    function getDataTableColumns() {

        var randomiselink = '';
        if (isAbleToAnonymise) {
            randomiselink = '| <a data-action="randomisecustomer" href="#">' + randomiseCustomerWord + '</a>';
        }
        
        var result;
        switch (searchcolumns) {
            
            
            
            
            case 'libraryUser':
                result = [
                    {
                        "sName": "Id",
                        "visible": false,
                        "searchable": false,
                        "aTargets": [0]
                    },
                    {
                        "sName": "Name",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                data = '<div class="has-hidden-block"><a href="' + editUrl + row[0] + '">' + data + '</a><div class="hidden-block"><a href="' + editUrl + row[0] + '">' + editWord + '</a> | <a data-action="delete" href="#">' + deleteWord + '</a> ' + randomiselink + ' </div></div>';
                            }
                            return data;
                        }
                    },
                    {
                        "sName": "Address"
                    },
                    {
                        "sName": "Phone"
                    },
                    {
                        "sName": "MobilePhoneNumber"
                    },
                    {
                        "sName": "Email"
                    },
                    {
                        "sName": "ModifiedBy"
                    },
                    {
                        "sName": "DateModified",
                        "sType": "date-euro"
                    }
                ];
                break;
            case 'librarybook':
                result = [
                    {
                        "sName": "Id",
                        "visible": false,
                        "searchable": false,
                        "aTargets": [0]
                    },
                    {
                        "sName": "ISBN",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                data = '<div class="has-hidden-block"><a href="' + editUrl + row[0] + '">' + data + '</a><div class="hidden-block"><a href="' + editUrl + row[0] + '">' + editWord + '</a> | <a data-action="delete" href="#">' + deleteWord + '</a></div></div>';
                            }
                            return data;
                        }
                    },
                    {
                        "sName": "Author"
                    },
                    {
                        "sName": "Title"
                    },
                    {
                        "sName": "Number"
                    },
                    {
                        "sName": "IsLost"
                    },
                    {
                        "sName": "IsStolen"
                    },
                    {
                        "sName": "ModifiedBy"
                    },
                    {
                        "sName": "DateModified",
                        "sType": "date-euro"
                    }
                ];
                break;
            case 'librarybookstatus':
                result = [
                    {
                        "sName": "Id",
                        "visible": false,
                        "searchable": false,
                        "aTargets": [0]
                    },
                    {
                        "sName": "DateCheckedOut",
                        "sType": "date-euro",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                data = '<div class="has-hidden-block"><a href="' + editUrl + row[0] + '">' + data + '</a><div class="hidden-block"><a href="' + editUrl + row[0] + '">' + editWord + '</a> | <a data-action="delete" href="#">' + deleteWord + '</a></div></div>';
                            }
                            return data;
                        }
                    },
                    {
                        "sName": "DateReturned",
                        "sType": "date-euro",
                    },
                    {
                        "sName": "ISBN"
                    },
                    {
                        "sName": "Title"
                    },
                    {
                        "sName": "Number"
                    },
                    {
                        "sName": "LibraryUserName"
                    },
                    {
                        "sName": "Address"
                    },
                    {
                        "sName": "ModifiedBy"
                    },
                    {
                        "sName": "DateModified",
                        "sType": "date-euro"
                    }
                ];
                break;
            case 'setting':
                result = [
                    {
                        "sName": "Id",
                        "visible": false,
                        "searchable": false,
                        "aTargets": [0]
                    },
                    {
                        //| <a data-action="delete" href="#">' + deleteWord + '</a>
                        "sName": "SettingName",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                data = '<div class="has-hidden-block"><a href="' + editUrl + row[0] + '">' + data + '</a><div class="hidden-block"><a href="' + editUrl + row[0] + '">' + editWord + '</a> </div></div>';
                            }
                            return data;
                        }
                    },
                    {
                        "sName": "SettingValue"
                    },
                    {
                        "sName": "ModifiedBy"
                    },
                    {
                        "sName": "DateModified",
                        "sType": "date-euro",
                        "data": 4
                    }
                ];
                break;
            case 'userpermission':
                result = [
                    {
                        "sName": "Id",
                        "visible": false,
                        "searchable": false,
                        "aTargets": [0]
                    },
                    {
                        "sName": "Permission",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                data = '<div class="has-hidden-block"><a href="#" data-action="showUserPermission" >' + data + '</a><div class="hidden-block"><a data-action="showUserPermission" href="#">' + editWord + '</a></div></div>';
                                /*
                                data = '<div class="has-hidden-block"><a href="' + editUrl + row[0] + '">' + data + '</a><div class="hidden-block"><a href="' + editUrl + row[0] + '">' + editWord + '</a></div></div>';
                                */
                            }
                            return data;
                        }
                    },
                    {
                        "sName": "SettingValue"
                    },
                    {
                        "sName": "ModifiedBy"
                    },
                    {
                        "sName": "DateModified",
                        "sType": "date-euro"
                    }
                ];
                break;
            case 'appuser':
                result = [
                    {
                        "sName": "UserName",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                if ($("#ShowAllUsers").length) {
                                    data = '<div class="has-hidden-block"><a href="' + editUrl + "?id=" + row[0] + '&showAllUsers=' + $("#ShowAllUsers").is(':checked').toString() + '">' + data + '</a><div class="hidden-block"><a href="' + editUrl + "?id=" + row[0] + '&showAllUsers=' + $("#ShowAllUsers").is(':checked').toString() + '">' + editWord + '</a> | <a data-action="delete" href="#">' + deleteWord + '</a></div></div>';
                                } else {
                                    data = '<div class="has-hidden-block"><a href="' + editUrl + "?id=" + row[0] + '">' + data + '</a><div class="hidden-block"><a href="' + editUrl + "?id=" + row[0] + '">' + editWord + '</a> | <a data-action="delete" href="#">' + deleteWord + '</a></div></div>';
                                }
                                
                            }
                            return data;
                        },
                        "aTargets": [0]
                    },
                    {
                        "sName": "EmailAddress"

                    },
                    {
                        "data": 3
                    }
                ];
                break;
            default:
                break;
        }
        return result;
    }
}

(function (window, document, $, undefined) {

    $(function () {
        InitDataTable();
             
    });
    
})(window, document, window.jQuery);

function ProductChange() {
    var e = document.getElementById("ProductIds");
    $("#SelectedProduct").val(e.options[e.selectedIndex].value);

    var table = $('#pagedResultsDataTable').DataTable();

    table.ajax.reload(function (json) {

    });
}

function JobStatusChange() {
    var e = document.getElementById("JobStatusIds");
    $("#SelectedJobStatus").val(e.options[e.selectedIndex].value);

    var table = $('#pagedResultsDataTable').DataTable();

    table.ajax.reload(function (json) {

    });
}

function CustomReportChange() {
    var e = document.getElementById("CustomReportIds");

    //var index = e.selectedIndex;
    var val = e.options[e.selectedIndex].value;

    var url = $("#table-actions").data("changecustomreporturl");
    url = url + "?id=" + val;
    
    window.location.href = url;
}

function CustomReportCategoryChange() {

    var e = document.getElementById("CategoryIds");

    //var index = e.selectedIndex;
    var val = e.options[e.selectedIndex].value;

    var url = $("#table-actions").data("changecustomreportcategoryurl");
    url = url + "?id=" + val;

    window.location.href = url;
}

function showFooter(api, colNum, row, data, start, end, display) {


    // Remove the formatting to get integer data for summation
    var intVal = function (i) {
        return typeof i === 'string' ?
            i.replace(/[\$,]/g, '') * 1 :
            typeof i === 'number' ?
                i : 0;
    };

    // Total over all pages
    //total = api
    //    .column(colNum)
    //    .data()
    //    .reduce(function (a, b) {
    //        return intVal(a) + intVal(b);
    //    }, 0);

    // Total over this page
    pageTotal = api
        .column(colNum, { page: 'current' })
        .data()
        .reduce(function (a, b) {
            return intVal(a) + intVal(b);
        }, 0);

    // Update footer
    $(api.column(colNum).footer()).html(
        //'$' + pageTotal + ' ( $' + total + ' total)'
        pageTotal.toFixed(2) //+ ' ( ' + total.toFixed(2) + ' total)'
    );


}

function PopulateTotals(api) {

    $('#SearchText').val($('.dataTables_filter input').val());

    var url = $("#table-actions").data("totals");
    var form = $("#frmDownload");
    if (!form.valid()) {
        return false;
    }

    var formVM = form.serialize();

    $.ajax({
        type: "POST",
        url: url,
        cache: false,
        data: formVM,
        success: function (data) {

            for (var i = 0; i < data.totals.length; i++) {

                $('tr:eq(1) th:eq(' + data.totals[i].columnNumber + ')', api.table().footer()).html(data.totals[i].total);

            }

            /*
            if (data.result === true) {

                
               
                //$('tr:eq(1) th:eq(7)', api.table().footer()).html('hello');
            }
            else {
                if (data.errorMessage !== null) {
                    if ($('#errorMessage').hasClass('hidden') === true) {
                        $('#errorMessage').removeClass('hidden');
                    }

                    var $summary = $('#errorMessage').find("[data-valmsg-summary=true]");

                    // find the unordered list
                    var $ul = $summary.find("ul");

                    // Clear existing errors from DOM by removing all element from the list
                    $ul.empty();

                    // Add error to the list
                    $("<li />").html(data.errorMessage).appendTo($ul);
                }
            }
            */
        },
        complete: function () {
            //
        }
    });

}

function PDAssignedToChange() {

    var e = document.getElementById("AssignedToIds");
    $("#SelectedAssignedFilter").val(e.options[e.selectedIndex].value);

    var table = $('#pagedResultsDataTable').DataTable();

    table.ajax.reload(function (json) {

    });

}

function PDCompletedFilterChange() {

    var e = document.getElementById("CompletedFilterIds");
    $("#SelectedCompletedFilter").val(e.options[e.selectedIndex].value);

    var table = $('#pagedResultsDataTable').DataTable();

    table.ajax.reload(function (json) {

    });

}

function AssignedToChange() {

    var e = document.getElementById("SEAssignedToIds");
    $("#SelectedAssignedTo").val(e.options[e.selectedIndex].value);

    var table = $('#pagedResultsDataTable').DataTable();

    table.ajax.reload(function (json) {

    });

}

function SalesActionSalesActionFilterChange() {
    var e = document.getElementById("SESalesActionFilterIds");
    $("#SelectedSalesActionFilter").val(e.options[e.selectedIndex].value);

    var table = $('#pagedResultsDataTable').DataTable();

    table.ajax.reload(function (json) {

    });
}


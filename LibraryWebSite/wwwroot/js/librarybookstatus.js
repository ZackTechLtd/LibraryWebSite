jQuery.extend(true, jQuery.fn.datetimepicker.defaults, {
    icons: {
        time: 'far fa-clock',
        date: 'far fa-calendar',
        up: 'fas fa-arrow-up',
        down: 'fas fa-arrow-down',
        previous: 'fas fa-chevron-left',
        next: 'fas fa-chevron-right',
        today: 'fas fa-calendar-check',
        clear: 'far fa-trash-alt',
        close: 'far fa-times-circle'
    }
});

(function (window, document, $, undefined) {

    $(function () {

        SetupDates("StrDateCheckedOut", "selectedDateCheckedOut", "DateCheckedOut");
        SetupDates("StrDateReturned", "selectedStrDateReturned", "DateReturned");
        
    });

})(window, document, window.jQuery);

/*
$('.ajax-typeahead').typeahead({
    source: function (query, process) {
        return $.ajax({
            url: $(this)[0].$element[0].dataset.link,
            type: 'get',
            data: { query: query },
            dataType: 'json',
            success: function (json) {
                return typeof json.options == 'undefined' ? false : process(json.options);
            }
        });
    }
});
*/


$("#LibraryBookTitle").typeahead({
    source: function (query, process) {
        var references = [];
        let map = {};

        var url = $("#typeahead-actions").data("bookurl");

        var formData = { searchText: query };

        $.ajax({
            type: "POST",
            url: url,
            cache: false,
            data: formData,
            success: function (retval) {

                if (retval.result === true) {

                    // Loop through and push to the array
                    $.each(retval.lstBooks, function (i, item) {
                        map[item] = item;
                        //alert(contact.EmailAddress);
                        references.push(item);
                    });

                    // Process the details
                    process(references);
                }

            },

            error: function (data) {
                console.log("error Typeahead");
                //if (data.errorMessage !== null) {
                //    console.log(data.errorMessage);
                //    $('#mainheader').notify(data.errorMessage, { "status": "danger", "pos": "top-right" });
                //}
            }
        });


    },
    afterSelect: function (item) {
        $('#LibraryBookCode').val(item.code);
    }
    
});

//
$("#LibraryUserName").typeahead({
    source: function (query, process) {
        var references1 = [];
        let map1 = {};

        var url1 = $("#typeahead-actions").data("userurl");

        var formData1 = { searchText: query };

        $.ajax({
            type: "POST",
            url: url1,
            cache: false,
            data: formData1,
            success: function (retval) {

                if (retval.result === true) {

                    // Loop through and push to the array
                    $.each(retval.lstUsers, function (i, item) {
                        map1[item] = item;
                        //alert(contact.EmailAddress);
                        references1.push(item);
                    });

                    // Process the details
                    process(references1);
                }

            },

            error: function (data) {
                console.log("error Typeahead");
                //if (data.errorMessage !== null) {
                //    console.log(data.errorMessage);
                //    $('#mainheader').notify(data.errorMessage, { "status": "danger", "pos": "top-right" });
                //}
            }
        });


    },
    afterSelect: function (item) {
        $('#LibraryUserCode').val(item.code);
    }
    
});
//

function SetDateNow(elementName) {

    var $ele = $('#' + elementName);
    var date = new Date();
    var datePickerObject = $ele.data("DateTimePicker");

    if (typeof datePickerObject !== "undefined") {
        // it's already been Initialize . Just update the date.
        datePickerObject.date(date);
    }
}

function SetupDates(strDefaultDateName, datePickerName, dateParameter) {

    var originalDate = $('#' + strDefaultDateName).val();

    //version 4 date time picker
    $('#' + datePickerName).datetimepicker({
        locale: 'en-gb',
        showTodayButton: true,
        defaultDate: originalDate,
        //format: 'DD-MM-YYYY HH:mm',
        //maxDate: maximumDate,
        useCurrent: true,
        showClear: true,
        toolbarPlacement: 'top',
        icons: {
            time: 'fa fa-clock-o',
            date: 'fa fa-calendar',
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down',
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-crosshairs',
            clear: 'fa fa-trash'
        }
    }).on('dp.change', function (event) {
        if (event.date == false) {
            $('#' + dateParameter).val("");
        } else {
            $('#' + dateParameter).val(event.date.format('YYYY-MM-DD HH:mm'));
        }


    });

}

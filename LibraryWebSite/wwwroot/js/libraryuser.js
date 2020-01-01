
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
        
        SetupGDPRDates("StrInformedDate", "selectedGDPRInformedDate", "GDPRInformedDate");
        SetupGDPRDates("StrContactByPostConsentDate", "selectedLibraryUserByPostConsentDate", "LibraryUserByPostConsentDate");
        SetupGDPRDates("StrContactByEmailConsentDate", "selectedLibraryUserByEmailConsentDate", "LibraryUserByEmailConsentDate");
        SetupGDPRDates("StrContactByPhoneConsentDate", "selectedLibraryUserByPhoneConsentDate", "LibraryUserByPhoneConsentDate");
        SetupGDPRDates("StrContactBySMSConsentDate", "selectedLibraryUserBySMSConsentDate", "LibraryUserBySMSConsentDate");


        $('#LibraryUserByPost').change(function () {
            var isChecked = $(this).is(":checked");
            var dateinput = $('#LibraryUserByPostConsentDate').val();
            if (isChecked && dateinput.length < 1) {
                SetDateNow('selectedLibraryUserByPostConsentDate');
            }
            
        });

        $('#LibraryUserByEmail').change(function () {
            var isChecked = $(this).is(":checked");
            var dateinput = $('#LibraryUserByEmailConsentDate').val();
            if (isChecked && dateinput.length < 1) {
                SetDateNow('selectedLibraryUserByEmailConsentDate');
            }

        });

        $('#LibraryUserByPhone').change(function () {
            var isChecked = $(this).is(":checked");
            var dateinput = $('#LibraryUserByPhoneConsentDate').val();
            if (isChecked && dateinput.length < 1) {
                SetDateNow('selectedLibraryUserByPhoneConsentDate');
            }

        });

        $('#LibraryUserBySMS').change(function () {
            var isChecked = $(this).is(":checked");
            var dateinput = $('#LibraryUserBySMSConsentDate').val();
            if (isChecked && dateinput.length < 1) {
                SetDateNow('selectedLibraryUserBySMSConsentDate');
            }

        });
        

    });

})(window, document, window.jQuery);

//https://stackoverflow.com/questions/30891880/set-date-in-bootstrap-datetimepicker-input

function SetDateNow(elementName) {

    var $ele = $('#' + elementName);
    var date = new Date();
    var datePickerObject = $ele.data("DateTimePicker");

    if (typeof datePickerObject !== "undefined") {
        // it's already been Initialize . Just update the date.
        datePickerObject.date(date);
    }
}

function SetupGDPRDates(strDefaultDateName, datePickerName, dateParameter ) {

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

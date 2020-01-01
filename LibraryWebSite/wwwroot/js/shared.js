//used for the company drop down in TopNav so option can be selected
$(document).on('click', '.click-inside', function (e) {
    e.stopPropagation();
});

$(document).on('click', '#device-help', function (e) {
    e.stopPropagation();
    $("#one-button").fadeOut(150, function () {
        $("#two-button").fadeIn(150, function () {
            // Animation complete
        });
    });
});

function IsEmpty(value) {
    return value === undefined || value === null;
}

function displayOverlay(text) {
    $("<table id='overlay'><tbody><tr><td>" + text + "</td></tr></tbody></table>").css({
        "position": "fixed",
        "top": 0,
        "left": 0,
        "width": "100%",
        "height": "100%",
        "background-color": "rgba(0,0,0,.5)",
        "z-index": 10000,
        "vertical-align": "middle",
        "text-align": "center",
        "color": "#fff",
        "font-size": "30px",
        "font-weight": "bold",
        "cursor": "wait"
    }).appendTo("body");
}

function removeOverlay() {
    $("#overlay").remove();
}


$('#CompanyIds').change(function () {
    var selectedVal = $('#CompanyIds option:selected').attr('value');

    var url = $('#changecompany').data("url") + "?companycode=" + selectedVal;
    window.location = url;
});

function ShowHtmlModalView(id, url) {
    // add id parameter for update functionality
    if (id !== null) {
        url = url + '/' + id;
    }

    $.get(url, function (data) {
        $('#add-update-modal-container').html(data);
        $('#add-update-modal').modal('show');
    }).done(function (data) {
        // ensure validation runs for ajax loaded content
        $.validator.unobtrusive.parse("#add-update-modal");
    });
}

// displays notification stored in session-storage
// handles notification following page redirect
function displayStoredNotification() {
    var notification = sessionStorage.getItem("wl-crm-notification");

    if (notification !== null) {
        $('#mainheader').notify(notification, { "status": "success", "pos": "top-right" });
        sessionStorage.removeItem("wl-crm-notification");
    }
}

function ShowTrackingModalDialog(id) {

    if (id === undefined) {
        return false;
    }

    var url = $('#ShowTrackingDlg').data("url");
    if (id !== null) {
        url = url + '/' + id;
    }

    $.get(url, function (data) {
        $('#add-update-modal-container').html(data);
        $('#add-update-modal').modal('show');
    }).done(function (data) {
        // ensure validation runs for ajax loaded content
        $.validator.unobtrusive.parse("#add-update-modal");

        doRadial();

    });

}

function doRadial() {
    var $scroller = $(window),
       inViewFlagClass = 'js-is-in-view'; // a classname to detect when a chart has been triggered after scroll

    $('[data-classyloader]').each(initClassyLoader);

    function initClassyLoader() {

        var $element = $(this),
            options = $element.data();

        // At lease we need a data-percentage attribute
        if (options) {
            if (options.triggerInView) {

                $scroller.scroll(function () {
                    checkLoaderInVIew($element, options);
                });
                // if the element starts already in view
                checkLoaderInVIew($element, options);
            }
            else
                startLoader($element, options);
        }
    }
    function checkLoaderInVIew(element, options) {
        var offset = -20;
        startLoader(element, options); //MAB

        /*if( ! element.hasClass(inViewFlagClass) &&
            $.Utils.isInView(element, {topoffset: offset}) ) {
          startLoader(element, options);
        }*/
    }
    function startLoader(element, options) {
        element.ClassyLoader(options).addClass(inViewFlagClass);
    }
}

function IgnoreEmptyString(value, append) {
    if (value === undefined || value === null || value === " ") {
        return '';
    }
    else {
        if (append === undefined) {
            append = ' ';
        }
        append = append || "";
        return value + append;
    }
}

function getHasChanges() {
    var hasChanges = false;

    $(":input:not(:button):not([type=hidden])").each(function () {
        if ((this.type === "text" || this.type === "textarea" || this.type === "hidden") && this.defaultValue !== this.value) {
            hasChanges = true;
            return false;
        }
        else {
            if ((this.type === "radio" || this.type === "checkbox") && this.defaultChecked !== this.checked) {
                hasChanges = true;
                return false;
            }
            else {
                if (this.type === "select-one" || this.type === "select-multiple") {
                    for (var x = 0; x < this.length; x++) {
                        if (this.options[x].selected !== this.options[x].defaultSelected) {
                            hasChanges = true;
                            return false;
                        }
                    }
                }
            }
        }
    });

    return hasChanges;
}

function acceptChanges() {
    $(":input:not(:button):not([type=hidden])").each(function () {
        if (this.type === "text" || this.type === "textarea" || this.type === "hidden") {
            this.defaultValue = this.value;
        }
        if (this.type === "radio" || this.type === "checkbox") {
            this.defaultChecked = this.checked;
        }
        if (this.type === "select-one" || this.type === "select-multiple") {
            for (var x = 0; x < this.length; x++) {
                this.options[x].defaultSelected = this.options[x].selected;
            }
        }
    });
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}


class Tools {

    static BackFunction = null;

    static GuidEmpty() {
        return "00000000-0000-0000-0000-000000000000";
    }

    static AddDialog(dialog) {
        if (typeof dialog.Init != "undefined") {
            Session.Dialogs.push(dialog);
        } else {
            console.warn("Dialog doesn't have Init function!");
        }
    }

    static PushNotification(type, text) {

        var top = $(window).height() - 150;
        var width = $(window).width();

        if (type === "success") {
            $("#notify_success").css("left", "-" + width + "px");
            $("#notify_success").css("top", top + "px");
            $("#notify_success_text").html(text);
            $("#notify_success").css("visibility", "visible");
            $("#notify_success").css("opacity", "1");

            $("#notify_success").animate({
                left: 0
            }, 'slow', function () {
                window.setTimeout(function () {
                    $("#notify_success").animate({ opacity: 0 }, 1000);
                }, 1500);
            });
        }

        if (type === "fail") {
            $("#notify_fail").css("left", "-" + width + "px");
            $("#notify_fail").css("top", top + "px");
            $("#notify_fail_text").html(text);
            $("#notify_fail").css("visibility", "visible");
            $("#notify_fail").css("opacity", "1");

            $("#notify_fail").animate({
                left: 0
            }, 'slow', function () {
                window.setTimeout(function () {
                    $("#notify_fail").animate({ opacity: 0 }, 1000);
                }, 3500);
            });
        }
    }

    static ShowLoading() {
        $.blockUI({
            message: $('div.block-ui'),
            fadeIn: 100,
            //fadeOut: 200,
            //timeout: 2000,
            showOverlay: true,
            centerY: true,
            css: {
                //width: '350px',
                //top: '10px',
                //left: '',
                //right: '10px',
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .6,
                color: '#fff'
            }
        });
    }

    static HideLoading() {
        window.setTimeout($.unblockUI, 500);
    }

    static GoBack() {
        if (Tools.BackFunction.constructor.name == "Back") {
            Tools.BackFunction.Invoke();
        }
    }

    static SwichDialog(pageName, params, isBack) {

        if (typeof isBack == "undefined") {
            Tools.backData.push({ pageName: pageName, params: params });
        }

        if (Session.CurrentPage === pageName) {
            console.log("Page '" + pageName + "' already opened!");
            return;
        }

        var activeDialog = $(".dialog:visible").first();
        var width = $(window).width() + 10;
        var newDialog = $("#" + pageName);

        newDialog.css('left', '-' + width + 'px');
        newDialog.css('position', 'absolute');
        newDialog.css('width', width + "px");

        newDialog.show();

        activeDialog.css('position', 'absolute');
        activeDialog.css('top', '60px');
        activeDialog.css('left', '0px');
        activeDialog.css('width', width + "px");


        activeDialog.animate({
            left: width
        }, 'slow', function () {
            activeDialog.hide();
            activeDialog.css('position', 'relative');
            activeDialog.css('top', 'auto');
            activeDialog.css('left', 'auto');
            activeDialog.css('width', "auto");
        });

        newDialog.animate({
            left: 0
        }, 'slow', function () {
            newDialog.css('position', 'relative');
            newDialog.css('top', 'auto');
            newDialog.css('left', 'auto');
            newDialog.css('width', "auto");
        });

        Session.CurrentPage = pageName;
        var dialogs = Session.Dialogs;
        for (let i = 0; i < dialogs.length; i++) {
            if (dialogs[i].name == pageName) {
                $("#main_title").html(dialogs[i].title);
                Tools.BackFunction = dialogs[i].GetBack();
                dialogs[i].Init(params);
                break;
            }
        }
    }
}
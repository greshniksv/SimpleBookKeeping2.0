﻿class Tools {

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

    static ShowLoading() {
        $.blockUI({
            message: $('div.block-ui'),
            fadeIn: 700,
            fadeOut: 700,
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
        $.unblockUI();
    }

    static backData = []
    static GoBack() {
        if (Tools.backData.length < 2) {
            return;
        }

        var cur = Tools.backData[Tools.backData.length - 2];
        var items = [];

        for (let i = 0; i <= Tools.backData.length - 2; i++) {
            items.push(Tools.backData[i]);
        }
        Tools.backData = items;

        Tools.SwichDialog(cur.pageName, cur.params, true);
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
                dialogs[i].Init(params);
                break;
            }
        }
    }
}
class AuthDialog {

    static Show() {
        $.blockUI({
            message: $('#login_form'),
            css: { width: '80%', left: '10%' }
        });
    }

    static Login() {

        var username = $("#auth_username").val();
        var password = $("#auth_password").val();

        var onSuccess = function () {
            Tools.SwichDialog("home_dialog");
            HomeDialog.Init();
        };

        var onError = function () {
            $("#auth_loader").hide();
            $("#auth_button").show();
        };

        var auth = new Auth();
        auth.getToken(username, password, onSuccess, onError);

        $("#auth_loader").show();
        $("#auth_button").hide();
    }
}
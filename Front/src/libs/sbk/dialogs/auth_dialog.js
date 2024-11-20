class AuthDialog {

    static Show() {
        $.blockUI({
            message: $('#login_form'),
            css: { width: '80%', left: '10%' }
        });
    }

    static Login(username, password, onSuccess, onError) {
        var auth = new Auth();
        auth.getToken(username, password, onSuccess, onError);
    }
}
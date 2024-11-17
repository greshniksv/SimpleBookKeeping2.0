class Auth {

    constructor() {
        this.Token = null;
    }

    getToken(userName, password) {

        $.ajax({
            url: BASE_HOST + '/connect/token',
            method: 'post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: "grant_type=password&username=" + userName + "&password=" + password + "&client_id=client",
            success: function (data) {
                this.Token = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                // xhr.status - status code
                alert("Error AUTH: " + xhr + "Code:" + xhr.status);
            }
        });
    }

    refreshToken() {
        $.ajax({
            url: BASE_HOST + '/connect/token',
            method: 'post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: "grant_type=refresh_token&client_id=client&refresh_token=" + this.Token.refresh_token,
            success: function (data) {
                this.Token = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                // xhr.status - status code
                alert("Error AUTH: " + xhr + "Code:" + xhr.status);
            }
        });
    }
}
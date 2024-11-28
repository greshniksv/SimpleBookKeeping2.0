class Auth {

    static getToken(userName, password, onSuccess, onError) {

        $.ajax({
            url: AUTH_BASE_HOST + '/connect/token',
            method: 'post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: "grant_type=password&username=" + userName + "&password=" + password + "&client_id=client",
            success: function (data) {
                data["created"] = new Date();
                localStorage.setItem('auth', JSON.stringify(data));

                if (typeof onSuccess != "undefined") {
                    onSuccess();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                // xhr.status - status code
                console.log("Error AUTH: " + xhr + "Code:" + xhr.status);

                if (typeof onError != "undefined") {
                    onError();
                }
            }
        });
    }

    static refreshToken(onSuccess, onError) {

        var data = JSON.parse(localStorage.getItem('auth'));

        if (typeof data == "undefined") {
            console.log("refreshToken. There is no 'auth' in localStorage");
            return;
        }

        var refresh_token = data.refresh_token;

        if (typeof refresh_token == "undefined") {
            console.log("refreshToken. There is no 'refresh_token' in 'auth' in localStorage");
            return;
        }

        $.ajax({
            url: AUTH_BASE_HOST + '/connect/token',
            method: 'post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: "grant_type=refresh_token&client_id=client&refresh_token=" + refresh_token,
            success: function (data) {
                data["created"] = new Date();
                localStorage.setItem('auth', JSON.stringify(data));

                if (typeof onSuccess != "undefined") {
                    onSuccess();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                // xhr.status - status code
                console.log("Error AUTH: " + xhr + "Code:" + xhr.status);

                if (typeof onError != "undefined") {
                    onError();
                }
            }
        });
    }
}
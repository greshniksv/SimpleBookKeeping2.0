class AjaxRequestEngine {

    static Execute(request) {
        if (!request instanceof AjaxRequest) {
            console.log("Request has invalid type: " + request.constructor.name);
            return;
        }

        var token = JSON.parse(localStorage.getItem('auth')).access_token;
        if (typeof token == "undefined" || token == null) {
            Tools.SwichDialog("auth_dialog");
            return;
        }

        $.ajax({
            url: BASE_HOST + request.url,
            method: request.method,
            dataType: request.dataType,
            contentType: request.contentType,
            data: request.data,
            headers: { "Authorization": "Bearer " + token },
            success: function (data) {

                if (typeof request.onSuccess != "undefined") {
                    request.onSuccess(data);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {

                if (xhr.status == 401) { // Unauthorized
                    Auth.refreshToken(function () {

                        var token = JSON.parse(localStorage.getItem('auth')).access_token;
                        if (typeof token == "undefined" || token == null) {
                            Tools.SwichDialog("auth_dialog");
                            return;
                        }

                        $.ajax({
                            url: BASE_HOST + request.url,
                            method: request.method,
                            dataType: request.dataType,
                            contentType: request.contentType,
                            data: request.data,
                            headers: { "Authorization": "Bearer " + token },
                            success: function (data) {

                                if (typeof request.onSuccess != "undefined") {
                                    request.onSuccess(data);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {

                                if (xhr.status == 401) { // Unauthorized
                                    Tools.SwichDialog("auth_dialog");
                                    return;
                                }

                                if (typeof request.onError != "undefined") {
                                    request.onError(data);
                                }
                            }
                        });

                    }, function () {
                        Tools.SwichDialog("auth_dialog");
                        return;
                    });
                }

                if (typeof request.onError != "undefined") {
                    request.onError(data);
                }
            }
        });
    }
}
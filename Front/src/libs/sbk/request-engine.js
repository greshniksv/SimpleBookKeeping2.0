class RequestEngine {

    Execute(request) {
        if (request instanceof Request) {
            console.log("Request has invalid type: " + request.constructor.name);
            return;
        }

        var token = JSON.parse(localStorage.getItem('auth')).access_token;

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

                if (typeof request.onError != "undefined") {
                    request.onError(data);
                }
            }
        });
    }
}
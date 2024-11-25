class AjaxRequest {
    constructor() {
        this.url = undefined;
        this.method = undefined;
        this.dataType = undefined;
        this.contentType = undefined;
        this.data = undefined;
        this.onSuccess = undefined;
        this.onError = undefined;
    }

    static Post(url, data, onSuccess, onError) {
        var request = new AjaxRequest();
        request.url = url;
        request.method = 'post'
        request.dataType = "json";
        request.contentType = "application/json";
        request.data = data;
        request.onSuccess = onSuccess;
        request.onError = onError;
        return request;
    }

    static Get(url, onSuccess, onError) {
        var request = new AjaxRequest();
        request.url = url;
        request.method = 'get';
        request.dataType = "json";
        request.contentType = "application/json";
        request.data = undefined;
        request.onSuccess = onSuccess;
        request.onError = onError;
        return request;
    }
}
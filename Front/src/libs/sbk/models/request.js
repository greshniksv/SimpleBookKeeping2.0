class Request {
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
        var request = new Request();
        request.url = url;
        request.method = 'post';
        request.dataType = "json";
        request.contentType = "application/json";
        request.data = data;
        request.onSuccess = onSuccess;
        request.onError = onError;
        return request;
    }
}
class Auth {

    getToken() {

        /*
         POST /connect/token
CONTENT-TYPE application/x-www-form-urlencoded

    client_id=client1&
    client_secret=secret&
    grant_type=client_credentials&
    scope=scope1
         */

        $.ajax({
            url: BASE_HOST +'/connect/token',
            method: 'post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: "grant_type=password&username=admin&password=admin&client_id=client",
            success: function (data) {
                alert(data);    /* выведет "Текст" */
                
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(data);

            }
        });
    }

    sayHi() {
        alert("Hello world!");
    }
}
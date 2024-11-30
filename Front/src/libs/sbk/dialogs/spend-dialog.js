class SpendDialog extends DialogBase {

    static name = "spend_dialog";
    static title = "Траты";
    static spendId = null;

    static Init(spendId) {
        console.log("Init plan");

        if (typeof spendId == "undefined") {
            console.error("Spend not exist");
            return;
        }

        SpendDialog.spendId = spendId;
        Tools.ShowLoading();
        var req = AjaxRequest.Get("/v1/Spend/" + spendId, function (data) {
            console.log("loading spends");
            Tools.HideLoading();

            if (typeof data.result == "undefined") {
                console.error(data);
                return;
            }

            SpendDialog.LoadingSpends(data.result);
        }, function () { Tools.HideLoading(); });

        AjaxRequestEngine.Execute(req);
    }

    static GetBack() {
        return new Back("home_dialog");
    }


    /**
     {
        "costId": "256a392e-c39c-43a6-b527-bf04e6134226",
        "costName": "Еда",
        "detailId": "0a0591ba-be35-4f6d-b07e-e26005eb2c0d",
        "date": "2024-11-29T06:00:00+03:00",
        "value": 2000,
        "spends": [
            {
                "id": "7acbdb31-517c-4c9e-8454-ec9237e994d1",
                "userId": "bea54459-3abb-4150-bfd8-ba68c6d5870c",
                "costDetail": {
                    "id": "0a0591ba-be35-4f6d-b07e-e26005eb2c0d",
                    "date": "2024-11-29T06:00:00+03:00",
                    "value": 2000
                },
                "value": 8000,
                "comment": "string",
                "image": ""
            }
        ]
    },
     */

    static LoadingSpends(spend) {

        var number = 0;
        var name = spend[0].costName;
        $("#main_title").html(SpendDialog.title + ": " + name);
        $(".accordion-item").remove();

        $.each(spend, function (i, v) {
            var date = new Date(v.date);
            var detailId = v.detailId;
            number += 10;

            var spendItem = "";
            $.each(v.spends, function (item, value) {

                var comment = $.base64.encode(value.comment);
                spendItem += `
                    <div class="mb-3">
                        <div class="col-auto">
                            <div class="input-group mb-2" data-costId='`+ v.costId + `' data-detailId='` + v.detailId + `' 
                                        data-spendid='` + value.id + `' data-value='` + value.value + `' data-comment='` + comment + `' >

                                <textarea class="input-group-text me-1 w-75 text-wrap text-start" autocomplete='off' > ` + value.comment + ` </textarea>
                                <input type="text" class="form-control ms-1" autocomplete='off' value="` + value.value + `" placeholder="">
                            </div>
                        </div>
                    </div>
                `;
            });

            var model = `
                <div class="accordion-item">
                    <h2 class="accordion-header" id="flush-headingOne">
                        <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
                                data-bs-target="#flush-collapse`+ number + `" aria-expanded="false" aria-controls="flush-collapse` + number + `">
                            `+ date.toLocaleDateString("ru-RU") + `
                        </button>
                    </h2>
                    <div id="flush-collapse`+ number + `" class="accordion-collapse collapse" aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushExample">
                        <div class="accordion-body" style="padding: 5px;">
                            <div class="form-control">
                                `+ spendItem + `
                                <div class="mb-3">
                                    <div class="col-auto">
                                        <div class="input-group mb-2" data-costId='`+ v.costId + `' data-detailId='` + v.detailId + `' 
                                                data-spendid='` + Tools.GuidEmpty() + `' data-value='' data-comment='' >
                                            <textarea class="input-group-text me-1 w-75 text-wrap text-start" autocomplete='off'> </textarea>
                                            <input type="text" class="form-control ms-1" autocomplete='off' value="" placeholder="Сумма">
                                        </div>
                                    </div>
                                </div>

                                <div class="d-grid gap-2 pt-1">
                                    <button type="button" onclick="SpendDialog.Save('`+ v.detailId + `')" class="btn btn-success p-2">Сохранить</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                `;

            $("#spend_list").append(model);
        });


        $('#flush-collapse10').collapse('show');
    }

    static Save(detailId) {

        const regexp = new RegExp("^[0-9]*$");
        var models = [];

        $("div[data-detailid=" + detailId + "]").each(function (i, v) {

            var obj = $(v);
            var costId = obj.attr("data-costId");
            var detailId = obj.attr("data-detailId");
            var spendId = obj.attr("data-spendid");
            var oldValue = obj.attr("data-value");
            var oldComment = $.base64.decode(obj.attr("data-comment"));
            var newValue = obj.find("input[type=text]").val();
            var newComment = obj.find("textarea").val();

            if (oldValue.trim() != newValue.trim() || oldComment.trim() != newComment.trim()) {

                var model = {
                    "costId": costId,
                    "id": spendId,
                    "costDetailId": detailId,
                    "value": newValue.trim(),
                    "comment": newComment.trim(),
                    "image": ""
                };

                if ((model.value == "" && model.comment != "") ||
                    (model.comment == "" && model.value != "")) {
                    Tools.PushNotification("fail", "Сумма не может быть пустой!");
                    return;
                }

                if (!regexp.test(model.value)) {
                    Tools.PushNotification("fail", "Сумма может содержать только цифры!");
                    return;
                }

                models.push(model);
            }
        });

        $.each(models, function (i, model) {

            if (model.id == Tools.GuidEmpty()) {
                // Create

                var req = AjaxRequest.Post("/v1/Spend", model, function (data) {
                    SpendDialog.Init(SpendDialog.spendId);
                });

                AjaxRequestEngine.Execute(req);

            } else {

                if (model.comment == "" && model.value == "") {

                    // Delete
                    var req = AjaxRequest.Delete("/v1/Spend/" + model.id, function (data) {
                        SpendDialog.Init(SpendDialog.spendId);
                    });

                    AjaxRequestEngine.Execute(req);

                } else {

                    // Update
                    var req = AjaxRequest.Put("/v1/Spend", model, function (data) {
                        SpendDialog.Init(SpendDialog.spendId);
                    });

                    AjaxRequestEngine.Execute(req);
                }
            }
        });
    }



}

Tools.AddDialog(SpendDialog);
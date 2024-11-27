class NewCostDialog extends DialogBase {

    static name = "new_cost_dialog";
    static planId = undefined;
    static costId = undefined;


    static Init(model) {
        console.log("Init new cost. Plan:" + model.plan + ", Cost:" + model.cost);

        NewCostDialog.planId = model.plan;
        NewCostDialog.costId = model.cost;

        $('#cost_generator').collapse('hide');

        $("#cost_accordion").find("button[data-toggle=collapse]").on("click", function () {
            $(".collapse").collapse('hide');
            var id = $(this).attr("data-target");
            $(id).collapse('show');
        });

        if (typeof model.cost == "undefined") {
            NewCostDialog.costId = Tools.GuidEmpty();
            $("#cost_costid").val(Tools.GuidEmpty());
        }

        if (typeof NewCostDialog.planId == "undefined") {
            console.error("plain id is null");
            return;
        }

        if (typeof model.cost != "undefined") {

            Tools.ShowLoading();
            var req = AjaxRequest.Get("/v1/Cost/" + NewCostDialog.costId, function (data) {
                console.log("loading costs");
                Tools.HideLoading();

                if (typeof data.result == "undefined") {
                    console.error(data);
                    return;
                }

                NewCostDialog.LoadingCosts(data.result);
            }, function () { Tools.HideLoading(); });

            AjaxRequestEngine.Execute(req);

        } else {

            Tools.ShowLoading();
            var req = AjaxRequest.Get("/v1/Cost/generate/" + NewCostDialog.planId, function (data) {
                console.log("generate costs");
                Tools.HideLoading();

                if (typeof data.result == "undefined") {
                    console.error(data);
                    return;
                }

                NewCostDialog.LoadingCosts(data.result);
            }, function () { Tools.HideLoading(); });

            AjaxRequestEngine.Execute(req);
        }


       

    }

    static LoadingCosts(costs) {

        /*
        {
          "result": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "planId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string",
            "costDetails": [
              {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "date": "2024-11-27T09:52:33.751Z",
                "value": 5000000
              }
            ]
          }
        }
        */


        $("#cost_planid").val(costs.planId);
        $("#cost_name").val(costs.name);

        $("div.cost-details-item").remove();
        $.each(costs.costDetails, function (i, v) {

            var date = new Date(v.date);

            var item = `
                <div class="mb-3 cost-details-item">
                    <div class="col-auto">
                        <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <div class="input-group-text"> `+ date.toLocaleDateString("ru-RU") + ` </div>
                            </div>
                            <input type="text" code='` + v.id + `' date='` + v.date + `' value='` + v.value + `' class="cost-detail form-control" placeholder="">
                        </div>
                    </div>
                </div>
                `;

            $("#cost_details").append(item);
        });
        
    }
    static Delete() {
        var req = AjaxRequest.Delete("/v1/Cost/" + NewCostDialog.costId, function (data) {
            Tools.SwichDialog("cost_dialog", NewCostDialog.planId);
        });

        AjaxRequestEngine.Execute(req);
    }


    static Save() {

        var costDetails = [];

        $(".cost-detail").each(function (i, v) {
            costDetails.push({
                "id": $(v).attr("code"),
                "date": $(v).attr("date"),
                "value": $(v).val()
            });
        });

        var model = {
            "id": NewCostDialog.costId,
            "planId": NewCostDialog.planId,
            "name": $("#cost_name").val(),
            "costDetails": costDetails
        };

        var req = AjaxRequest.Post("/v1/Cost", model, function (data) {

            Tools.SwichDialog("cost_dialog", NewCostDialog.planId); 
        });

        AjaxRequestEngine.Execute(req);
    }

    static Cancel() {
        Tools.SwichDialog("cost_dialog", NewCostDialog.planId);
    }
}

Tools.AddDialog(NewCostDialog);
class CostDialog extends DialogBase {

    static name = "cost_dialog";
    static title = "Расходы";
    static planId = undefined;

    static Init(planId) {
        console.log("Init cost:" + planId);

        CostDialog.planId = planId;

        if (typeof planId == "undefined") {
            console.error("Plan not exist");
            return;
        }

        Tools.ShowLoading();
        var req = AjaxRequest.Get("/v1/Cost/byPlan/" + planId, function (data) {
            console.log("loading users");
            Tools.HideLoading();

            if (typeof data.result == "undefined") {
                console.error(data);
                return;
            }

            CostDialog.LoadingCosts(data.result);
        }, function () { Tools.HideLoading(); });

        AjaxRequestEngine.Execute(req);
    }

    static GetBack() {
        return new Back("new_plan_dialog", CostDialog.planId)
    }

    static GoToNewCosts() {
        Tools.SwichDialog("new_cost_dialog", { "plan": CostDialog.planId, "cost": undefined });
    }

    static EditCost(costId) {
        Tools.SwichDialog("new_cost_dialog", { "plan": CostDialog.planId, "cost": costId });
    }

    static LoadingCosts(costs) {

        /**
        {
          "result": [
            {
              "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
              "planId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
              "name": "string",
              "costDetails": [
                {
                  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                  "date": "2024-11-27T10:58:24.427Z",
                  "value": 5000000
                }
              ]
            }
          ]
        }
         */


        $("#cost_list a").remove();
        var totalSum = 0;

        $.each(costs, function (i, v) {

            var sum = 0;
            $.each(v.costDetails, function (i, v) {
                sum += v.value;
            });

            totalSum += sum;

            var item = `
            <a href="#" onclick="CostDialog.EditCost('`+ v.id + `')" class="cost-item list-group-item list-group-item-action" aria-current="true">
                <div class="d-flex w-100 justify-content-between">
                    <h5 class="mb-1">` + v.name + `</h5>
                    <small> * </small>
                </div>
                <small>Баланс: `+ sum.toLocaleString() + ` руб.</small>
            </a>
            `;

            $("#cost_list").append(item);
        });

        $("#total_sum").html("Сумма: " + totalSum.toLocaleString() + " руб");

        $('.cost-item').hover(
            function () { $(this).addClass('active') },
            function () { $(this).removeClass('active') }
        );

    }

}

Tools.AddDialog(CostDialog);
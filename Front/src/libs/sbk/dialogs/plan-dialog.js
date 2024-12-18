﻿class PlanDialog extends DialogBase {

    static name = "plan_dialog";
    static title = "Планы";

    static Init() {
        console.log("Init plan");

        Tools.ShowLoading();
        var req = AjaxRequest.Get("/v1/Plan", function (data) {
            console.log("loading users");
            Tools.HideLoading();

            if (typeof data.result == "undefined") {
                console.error(data);
                return;
            }

            PlanDialog.LoadingPlans(data.result);
        }, function () { Tools.HideLoading(); });

        AjaxRequestEngine.Execute(req);
    }

    static GetBack() {
        return new Back("settings_dialog");
    }

    static GoToNewPlan() {
        Tools.SwichDialog("new_plan_dialog");
    }

    static EditPlan(code) {
        Tools.SwichDialog("new_plan_dialog", code);
    }

    static LoadingPlans(plans) {

        function datediff(first, second) {
            return Math.round((second - first) / (1000 * 60 * 60 * 24));
        }

        $("#plan_list a").remove();

        $.each(plans, function (i, v) {

            var startDate = new Date(v.start);
            var endDate = new Date(v.end);
            var diff = datediff(startDate, endDate);

            var item = `
            <a href="#" onclick="PlanDialog.EditPlan('`+ v.id + `')" class="plan-item list-group-item list-group-item-action" aria-current="true">
            <div class="d-flex w-100 justify-content-between">

                <h5 class="mb-1">` + v.name + `</h5>
                <small>`+ diff + ` дней</small>
            </div>
            <p class="mb-1">Начиная с `+ startDate.toLocaleDateString("ru-RU") + ` по ` + endDate.toLocaleDateString("ru-RU") + `</p>
            <small>Баланс: `+ v.balance + ` руб.</small>
        </a>
            `;

            $("#plan_list").append(item);
        });

        $('.plan-item').hover(
            function () { $(this).addClass('active') },
            function () { $(this).removeClass('active') }
        );

    }

}

Tools.AddDialog(PlanDialog);
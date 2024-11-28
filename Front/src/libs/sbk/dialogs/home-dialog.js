class HomeDialog extends DialogBase {

    static name = "home_dialog";
    static title = "Главная";

    static Init() {
        console.log("Init home");

        Tools.ShowLoading();
        $('.navbar-nav>li>a:not([data-bs-toggle^="dropdown"])').on('click', function (i, v) {
            $('.navbar-collapse').collapse('hide');
        });

        $('.dropdown-menu>li>a:not([data-bs-toggle^="dropdown"])').on('click', function (i, v) {
            $('.navbar-collapse').collapse('hide');
        });

        var req = AjaxRequest.Get("/v1/PlanStatus", function (data) {
            console.log("loading plan");

            Tools.HideLoading();
            if (typeof data.result == "undefined") {
                console.error(data);
                return;
            }

            HomeDialog.LoadingForm(data.result[0]);
            HomeDialog.LoadingMenu(data.result[0]);

        }, function () { Tools.HideLoading(); });

        AjaxRequestEngine.Execute(req);
    }

    /**
     {
        "result": [
            {
                "id": "f44ffd79-4b8f-4854-9eff-67eda6bb2c1e",
                "name": "sdfdsf",
                "balance": 18000,
                "rest": 2343,
                "progress": 53,
                "balanceToEnd": 0,
                "costStatusModels": [
                    {
                        "id": "256a392e-c39c-43a6-b527-bf04e6134226",
                        "name": "Еда",
                        "balance": 18000
                    }
                ]
            }
        ]
    }
     */

    static LoadingForm(data) {
        $("#home_name").html("<h3>" + data.name + "</h3>");
        $("#home_balance").html("Баланс:" + data.balance);
        $("#home_progress").css("width", data.progress + "%");
        $("#home_datetime").html();
        $("#home_list a").remove();

        $.each(data.costStatusModels, function (i, v) {
            var item = `
             <a href="#" class="list-group-item list-group-item-action" aria-current="true">
                <div class="d-flex w-100 justify-content-between">
                    <h7 class="mb-1">`+ v.name + `</h7>
                    <small>`+ v.balance + ` руб</small>
                </div>
            </a>
            `;

            $("#home_list").append(item);
        });

    }

    static LoadingMenu(data) {

        $("#main_costs a").remove();

        $.each(data.costStatusModels, function (i, v) {
            var item = `
            <li>
                <a class="dropdown-item" onclick="HomeDialog.GoToSpend('`+ v.id + `')" href="#">` + v.name + `</a>
            </li>
            `;

            $("#main_costs").append(item);
        });

    }

    static GoToSpend(id) {

    }

}

Tools.AddDialog(HomeDialog);
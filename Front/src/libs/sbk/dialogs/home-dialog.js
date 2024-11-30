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

            HomeDialog.LoadingForm(data.result);
            HomeDialog.LoadingMenu(data.result);

        }, function () { Tools.HideLoading(); });

        AjaxRequestEngine.Execute(req);
    }

    /**
     {
      "result": [
        {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string",
          "balance": 0,
          "rest": 0,
          "progress": 0,
          "balanceToEnd": 0,
          "currentDateTime": "string",
          "costStatusModels": [
            {
              "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
              "name": "string",
              "balance": 0
            }
          ]
        }
      ]
    }
     */

    static LoadingForm(data) {

        $("#home_list a").remove();


        $.each(data, function (num, obj) {

            $(".main-plan-item").remove();

            var model = `

             <div class="main-plan-item form-control">

                <div class="row mb-1 pt-4">
                    <div class="col">
                        <div data-mdb-input-init class="form-outline">
                            <label class="form-label"> <h1> `+ obj.name +`</h1> </label>
                        </div>
                    </div>
                </div>


                <div class="row mb-4">
                    <div class="col">
                        <div data-mdb-input-init class="form-outline">
                            <label class="form-label"> В кошельке: `+ obj.balance +` </label>
                        </div>
                    </div>
                </div>

                <div class="row mb-4">
                    <div class="col">
                        <div data-mdb-input-init class="form-outline">
                            <label class="form-label"> Текущее время: `+ obj.currentDateTime +` </label>
                        </div>
                    </div>
                </div>

                <div class="progress">
                    <div style=" width: `+ obj.progress+`% " class="progress-bar" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100"></div>
                </div>

                <div id="home_list" class="list-group pt-4">

            `;

            $.each(obj.costStatusModels, function (i, v) {
                model += `
                 <a href="#" class="list-group-item list-group-item-action" aria-current="true">
                    <div class="d-flex w-100 justify-content-between">
                        <h7 class="mb-1">`+ v.name + `</h7>
                        <small>`+ v.balance + ` руб</small>
                    </div>
                </a>
                `;
            });

            model += `
                            </div>

            </div>
            `;

            $("#home_base").html(model);
        });

    }

    static LoadingMenu(data) {

        //navbarPlanCosts

        $(".main-plan-index").remove();

        $.each(data, function (num, obj) {

            var title = `
                <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                    `+ obj.name + `
                </a>
            `;

            var items = "";

            $.each(obj.costStatusModels, function (i, v) {

                items += `
                <li>
                    <a class="dropdown-item" onclick="HomeDialog.GoToSpend('`+ v.id + `')" href="#">` + v.name + `</a>
                </li>
                `;
            });

            var model = `

                <ul class="main-plan-index navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="#" onclick="Home()">Главная</a>
                    </li>
                    <li class="nav-item dropdown">
                      `+ title + `
                        <ul id="main_costs" class="dropdown-menu" aria-labelledby="navbarDropdown">
                           `+ items + `
                        </ul>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#" onclick="Settings()">Настроки</a>
                    </li>
                </ul>
            `;

            $("#navbarPlanCosts").append(model);
        });

    }

    static GoToSpend(id) {
        Tools.SwichDialog("spend_dialog", id);
    }

}

Tools.AddDialog(HomeDialog);
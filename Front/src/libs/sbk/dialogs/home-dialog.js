class HomeDialog extends DialogBase {

    static name = "home_dialog";

    static Init() {
        console.log("Init home");

        $('.navbar-nav>li>a:not([data-bs-toggle^="dropdown"])').on('click', function (i, v) {

            debugger;
            $('.navbar-collapse').collapse('hide');
        });

        $('.dropdown-menu>li>a:not([data-bs-toggle^="dropdown"])').on('click', function (i, v) {

            debugger;
            $('.navbar-collapse').collapse('hide');
        });

        var req = AjaxRequest.Get("/v1/PlanStatus", function (data) {
            console.log("loading plan");

            if (typeof data.result == "undefined") {
                console.error(data);
                return;
            }

            HomeDialog.LoadingForm(data.result);
        });

        AjaxRequestEngine.Execute(req);

        var req = AjaxRequest.Get("/v1/PlanCosts", function (data) {
            console.log("loading menu");

            if (typeof data.result == "undefined") {
                console.error(data);
                return;
            }

            HomeDialog.LoadingMenu(data.result);
        });

        AjaxRequestEngine.Execute(req);

        


    }

    static LoadingForm(data) {

    }

    static LoadingMenu() {

    }

}

Tools.AddDialog(HomeDialog);
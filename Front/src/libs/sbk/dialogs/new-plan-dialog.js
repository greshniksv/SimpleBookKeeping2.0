class NewPlanDialog extends DialogBase {

    static name = "new_plan_dialog";
    static title = "Новый план";
    static planId = undefined;

    static Init(planId) {
        console.log("Init new plan");

        $("#newpaln_id").val("");
        $("#newplan_start").val("");
        $("#newplan_end").val("");
        $("#newplan_name").val("");
        $("#newplan_balance").val("");
        $('#newplan_users').val(null).trigger('change');


        $('#newplan_users').select2({
            theme: "bootstrap-5",
            width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
            placeholder: $(this).data('placeholder'),
            closeOnSelect: false,
            allowClear: true
        });


        $("#new_plan_accordion").find("button[data-toggle=collapse]").on("click", function () {
            $(".collapse").collapse('hide');
            var id = $(this).attr("data-target");
            $(id).collapse('show');
        });


        
        $('#collapseOne').collapse('hide');
        $("#collapseTwo").collapse('hide');


        Tools.ShowLoading();
        // Loading user list
        var req = AjaxRequest.Get("/v1/User", function (data) {
            console.log("loading users");

            if (typeof data.result == "undefined") {
                console.error(data);
                return;
            }

            NewPlanDialog.LoadingUsers(data.result);

            // Loading existing plan
            if (typeof planId != "undefined") {
                NewPlanDialog.planId = planId;
                var req = AjaxRequest.Get("/v1/Plan/" + planId, function (data) {

                    Tools.HideLoading();
                    if (typeof data.result == "undefined") {
                        console.error(data);
                        return;
                    }

                    NewPlanDialog.LoadPlan(data.result);
                    $("#collapseTwo").collapse('show');
                }, function () { Tools.HideLoading(); });

                AjaxRequestEngine.Execute(req);
            } else {
                $('#collapseOne').collapse('show');
                Tools.HideLoading();
            }

        }, function () { Tools.HideLoading(); });

        AjaxRequestEngine.Execute(req);
    }

    static GoToCost() {
        Tools.SwichDialog("cost_dialog", NewPlanDialog.planId ); 
    }

    static LoadPlan(plan) {

        $("#newpaln_accordion").html("Изменить план: " + plan.name);
        $("#newpaln_id").val(plan.id);
        $("#newplan_start").val(plan.start.split('T')[0].replace("-", "/").replace("-", "/"));
        $("#newplan_end").val(plan.end.split('T')[0].replace("-", "/").replace("-", "/"));
        $("#newplan_name").val(plan.name);
        $("#newplan_balance").val(plan.balance);

        var arr = [];
        $.each(plan.userMembers, function (i, v) {
            arr.push(v);
        });

        $('#newplan_users').val(arr);
        $('#newplan_users').trigger('change');
    }

    static LoadingUsers(users) {

        $.each(users, function (i, v) {

            var data = {
                id: v.id,
                text: v.name
            };

            if ($('#newplan_users').find("option[value='" + data.id + "']").length == 0) {
                var newOption = new Option(data.text, data.id, false, false);
                $('#newplan_users').append(newOption).trigger('change');
            } 
        });
    }

    static AddNewPlan() {

        var userMembers = [];

        var items = $('#newplan_users').select2('data');
        $.each(items, function (i, v) {
            userMembers.push(v.id);
        });

        //$("#newplan_users").find('option:selected').each(function (i, v) {
        //    userMembers.push($(v).attr('id'));
        //});

        var planId = $("#newpaln_id").val();

        if (typeof planId == "undefined" || planId.length < 10) {
            planId = Tools.GuidEmpty();
        }

        var model = {
            "id": planId,
            "start": $("#newplan_start").val(),
            "end": $("#newplan_end").val(),
            "name": $("#newplan_name").val(),
            "balance": $("#newplan_balance").val(),
            "userMembers": userMembers
        }

        var req = AjaxRequest.Post("/v1/Plan", model, function (data) {

            if (typeof data.result == "undefined") {
                console.error(data);
                return;
            }

            Tools.SwichDialog("plan_dialog");
        });

        AjaxRequestEngine.Execute(req);
    }

    static Cancel() {
        Tools.SwichDialog("plan_dialog");
    }
}

Tools.AddDialog(NewPlanDialog);
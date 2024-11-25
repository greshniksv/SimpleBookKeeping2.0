class NewPlanDialog extends DialogBase {

    static name = "new_plan_dialog";

    static Init() {
        console.log("Init plan");

        //$('.datepicker').datepicker({
        //    format: 'dd/mm/yyyy',
        //    startDate: '-3d'
        //});

        $('#multiple-select-field').select2({
            theme: "bootstrap-5",
            width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
            placeholder: $(this).data('placeholder'),
            closeOnSelect: false,
        });
    }

  
}

Tools.AddDialog(NewPlanDialog);
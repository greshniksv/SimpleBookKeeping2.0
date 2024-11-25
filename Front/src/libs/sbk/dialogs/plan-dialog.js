class PlanDialog extends DialogBase {

    static name = "plan_dialog";

    static Init() {
        console.log("Init plan");
    }

    static GoToNewPlan() {
        Tools.SwichDialog("new_plan_dialog");
    }
  
}

Tools.AddDialog(PlanDialog);
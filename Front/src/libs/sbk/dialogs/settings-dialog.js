class SettingsDialog extends DialogBase {

    static name = "settings_dialog";
    static title = "Настройки";

    static Init() {
        console.log("Init plan");
    }

    static GoToPlan() {
        Tools.SwichDialog("plan_dialog");

    }
  
}

Tools.AddDialog(SettingsDialog);
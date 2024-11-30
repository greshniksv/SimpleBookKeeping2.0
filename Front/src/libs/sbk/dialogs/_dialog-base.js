class DialogBase {

    static name = "BASE_NAME";

    static title = "BASE_TITLE";

    static Init() {
        console.log("Init not implemented!");
    }

    static GetBack() {
        console.log("GetBack not implemented!");
    }
}

class Back {

    constructor(name, param) {
        this.name = name;
        this.param = param;
    }

    Invoke() {
        Tools.SwichDialog(this.name, this.param);
    }
}
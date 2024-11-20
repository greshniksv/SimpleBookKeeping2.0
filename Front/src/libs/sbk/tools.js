class Tools {
    static ShowLoading() {
        $.blockUI({
            message: $('div.block-ui'),
            fadeIn: 700,
            fadeOut: 700,
            //timeout: 2000,
            showOverlay: true,
            centerY: true,
            css: {
                //width: '350px',
                //top: '10px',
                //left: '',
                //right: '10px',
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .6,
                color: '#fff'
            }
        }); 
    }

    static HideLoading() {
        $.unblockUI();
    }

}
function SuccessMsg(msg) {
    $.notify(msg, {
        animate: {
            enter: 'animated fadeInRight',
            exit: 'animated fadeOutRight'            
        },
        type: 'success',        
        placement: {
            from: "bottom",
            align: "right"
        },
        autoHideDelay: 2000,
        showAnimation: "fadeIn",
        hideAnimation: "fadeOut",
        hideDuration: 700,
        arrowShow: false,
        className: "success"
    });
}

function ErrorMsg(msg) {
    $.notify(msg, {
       
        animate: {
            enter: 'animated fadeInRight',
            exit: 'animated fadeOutRight'            
        }, type: 'danger',
        placement: {
            from: "bottom",
            align: "right"
        },

        autoHideDelay: 2000,
        showAnimation: "fadeIn",
        hideAnimation: "fadeOut",
        hideDuration: 700,
        arrowShow: false,
        className: "fail"
    });
}

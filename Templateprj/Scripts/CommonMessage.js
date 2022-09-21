function SuccessMsg(msg) {
    $.notify(msg, {
        animate: {
            enter: 'animated fadeInRight',
            exit: 'animated fadeOutRight'
        }, type: 'success',
        placement: {
            from: "bottom",
            align: "right"
        },
        delay: 1000000
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
        delay: 1000000
    });
}

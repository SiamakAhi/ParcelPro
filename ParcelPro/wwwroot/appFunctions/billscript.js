
$(document).on('click', '.callform[data-url]', function () {

    let actionUrl = $(this).data('url');

    $.get(actionUrl).done(function (data) {

        appModal = $('#billmodal');
        appModal.html(data);
        appModal.modal('show');

    });
});

$(document).on('click', '.accAjaxSubmit', function () {
    /*  loder.style.display = "block";*/
    let frm = $(this).closest('form');

    /*let frm = $(this).parents('form');*/
    let actionUrl = frm.attr('action');
    let dataToSend = new FormData(frm.get(0));

    $.ajax({
        url: actionUrl,
        type: 'post',
        data: dataToSend,
        processData: false,
        contentType: false
    }).done(function (data) {
        let result = JSON.parse(data);

        if (result['Success'] === true) {

            if (result['ShowMessage'] === true) {
                Swal.fire({
                    title: 'کاربـر گـرامی !',
                    html: result['Message'],
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-success'
                    },
                    confirmButtonText: 'متوجه شدم',
                    buttonsStyling: false
                });
            }

            $(this).delay(200).queue(function () {
                if (parseInt(result['updateType']) === 1) {
                    if (result['returnUrl']) window.location.href = result['returnUrl'];
                }
                else {
                    if (result['returnUrl']) {
                        var getUrl = result['returnUrl'];
                        var target = result['updateTargetId'];

                        $.get(getUrl).done(function (newData) {
                            $(document).find(target).html(newData);
                            var modalTarge = $(document).find('#accmodal');
                            modalTarge.modal('hide');
                            $(document).find('.select2').select2();
                        });
                    }
                }
            })
        }
        else {
            Swal.fire({
                title: 'کاربـر گـرامی !',
                html: result['Message'],
                icon: 'warning',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                confirmButtonText: 'باشه',
                buttonsStyling: false
            });
        }


    }).always(function () {
        /*   loder.style.display = "none";*/
    });
});
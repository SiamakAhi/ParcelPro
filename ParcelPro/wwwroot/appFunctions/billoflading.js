var loder = document.getElementById("loader");



$('.callmodalbyform').on('click', function () {
    let frm = $(this).closest('form');
    let actionUrl = frm.attr('action');
    let dataToSend = new FormData(frm.get(0));

    $.ajax({
        url: actionUrl,
        type: 'post',
        data: dataToSend,
        processData: false,
        contentType: false
    }).done(function (data) {
        appModal = $('#appmodal');
        appModal.html(data);
        appModal.modal('show');

    }).always(function () {
       // loder.style.display = "none";
    });
    
});


$('#ddlSender').change(function () {
    const actionUrl = $(this).data('url');
    const dataToSend = $(this).val();
    $("#senderMobile").val('');
    $("#senderAddress").val('');

    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: { id: dataToSend },
        dataType: 'json',
    }).done(function (info) {
        $("#senderMobile").val(info.mobilePhone);
        $("#senderAddress").val(info.address);
        
    }).fail(function (xhr, status, error) {
        console.error('درخواست با خطا مواجه شد:', error);
    });
});

$('#ddlReciver').change(function () {
    const actionUrl = $(this).data('url');
    const dataToSend = $(this).val();
    $("#reciverMobile").val('');
    $("#reciverAddress").val('');

    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: { id: dataToSend },
        dataType: 'json',
    }).done(function (info) {
        $("#reciverMobile").val(info.mobilePhone);
        $("#reciverAddress").val(info.address);
        
    }).fail(function (xhr, status, error) {
        console.error('درخواست با خطا مواجه شد:', error);
    });
});




























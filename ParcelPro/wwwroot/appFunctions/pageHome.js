
$('#activeSeller').val('@ViewBag.ActiveSellerId').change();
$('#activeSellerPeriod').val('@ViewBag.ActiveSellerPeriod').change();


$('#activeSeller').change(function () {
    var id = $(this).val();
    let actionUrl = '@Url.Action("SettActiveSeller", "Home", new { Area = "" })';
    //alert(id);
    let title = 'در صورت تغییر شرکت فعال تمام فرمهای باز بسته خواهد شد. آیا ادامه می دهید ؟';

    if (confirm(title)) {
        $.ajax({
            url: actionUrl,
            type: "post",
            data: { sellerId: id },
        }).done(function (data) {
            location.reload();
        });
    }
})

$('#activeSellerPeriod').change(function () {
    var id = $(this).val();
    let actionUrl = '@Url.Action("SettActivePeriod", "Home", new { Area = "" })';
    //alert(id);
    let title = 'در صورت تغییر سال مالی فعال تمام فرمهای باز بسته خواهد شد. آیا ادامه می دهید ؟';

    if (confirm(title)) {
        $.ajax({
            url: actionUrl,
            type: "post",
            data: { periodId: id },
        }).done(function (data) {
            location.reload();
        });
    }
});

//................................

$('#selectTafsilGroup').change(function () {
    const actionUrl = $(this).data('url');
    const targetId = $(this).data('filltarget');
    const target = $(targetId);
    const dataToSend = $(this).val();

    //
    $('#selectTafsil').empty();
    $('#selectKol').empty();
    $('#selectMoein').empty();
    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: { groupId: dataToSend },
        dataType: 'json',
    }).done(function (response) {
        $.each(response, function (index, tafsil) {
            target.append($('<option>', {
                value: tafsil.id,
                text: tafsil.name
            }));
        });
    }).fail(function (xhr, status, error) {
        console.error('درخواست با خطا مواجه شد:', error);
    });
});
$('#selectTafsil').change(function () {
    const actionUrl = $(this).data('url');
    const targetId = $(this).data('filltarget');
    const target = $(targetId);
    const dataToSend = $(this).val();

    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: { items: dataToSend },
        dataType: 'json',
    }).done(function (response) {
        target.empty();
        $('#selectMoein').empty();
        $.each(response, function (index, kol) {
            target.append($('<option>', {
                value: kol.id,
                text: kol.kolName
            }));
        });
    }).fail(function (xhr, status, error) {
        console.error('درخواست با خطا مواجه شد:', error);
    });
});
$('#selectKol').change(function () {
    const actionUrl = $(this).data('url');
    const targetId = $(this).data('filltarget');
    const target = $(targetId);
    const kolIds = $(this).val();
    const tafsilIds = $('#selectTafsil').val();
    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: { tafsils: tafsilIds, kols: kolIds },
        dataType: 'json',
    }).done(function (response) {
        target.empty();
        $.each(response, function (index, moeins) {
            target.append($('<option>', {
                value: moeins.id,
                text: moeins.moeinName
            }));
        });
    }).fail(function (xhr, status, error) {
        console.error('درخواست با خطا مواجه شد:', error);
    });
});

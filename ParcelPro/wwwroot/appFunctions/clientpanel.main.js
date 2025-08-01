

$('.pagination a.page-link').on('click', function (e) {
    e.preventDefault(); 
    var currentPage = $(this).data('currentpage');
    var formId = $(this).data('formid');
    var form = $('#' + formId);
    form.find('input[name="filter.CurrentPage"]').val(currentPage);
    form.submit();
});

$('#selectProvince').change(function () {
    const actionUrl = $(this).data('url');
    const target = $('#SelectCity');
    const proviceId = $('#selectProvince').val();
    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: { pid: proviceId },
        dataType: 'json',
    }).done(function (response) {
        target.empty();
        target.append($('<option>', {
            value: 'null',
            text: 'همه'
        }));
        $.each(response, function (index, cities) {
            target.append($('<option>', {
                value: cities.cityId,
                text: cities.nameFa
            }));
        });
    }).fail(function (xhr, status, error) {
        console.error('درخواست با خطا مواجه شد:', error);
    });
});


//        public string BiilOdLadingNumber { get; set; }


$(document).on('click', '.getprint_app', function () {

    // ploder.style.display = "block";
    let frm = $(this).parents('form');
    let actionUrl = $(this).data('url');

    let ReciverId = frm.find('select[name="filter.ReciverId"]').val();
    let RoutId = frm.find('select[name="filter.RoutId"]').val();
    let Distributer = frm.find('select[name="filter.Distributer"]').val();
    let OriginCityId = frm.find('select[name="filter.OriginCityId"]').val();
    let DestinationCityId = frm.find('select[name="filter.DestinationCityId"]').val();
    let SettelmentType = frm.find('select[name="filter.SettelmentType"]').val();
    let BillStatus = frm.find('select[name="filter.BillStatus"]').val();
    let PaymentStatus = frm.find('select[name="filter.PaymentStatus"]').val();
    let IssuerUserName = frm.find('select[name="filter.IssuerUserName"]').val();
    let BiilOdLadingNumber = frm.find('input[name="filter.BiilOdLadingNumber"]').val();
    let strFromDate = frm.find('input[name="filter.strFromDate"]').val();
    let strUntilDate = frm.find('input[name="filter.strUntilDate"]').val();
    let ShowCancelation = frm.find('input[name="filter.ShowCancelation"]:checked').val();
    let personSearchtype = frm.find('input[name="filter.personSearchtype"]:checked').val();


    $.ajax({
        url: actionUrl,
        type: 'get',
        data: {
              _ReciverId: ReciverId
            , _RoutId: RoutId
            , _Distributer: Distributer
            , _OriginCityId: OriginCityId
            , _DestinationCityId: DestinationCityId
            , _SettelmentType: SettelmentType
            , _BillStatus: BillStatus
            , _PaymentStatus: PaymentStatus
            , _IssuerUserName: IssuerUserName
            , _BiilOdLadingNumber: BiilOdLadingNumber
            , _strFromDate: strFromDate
            , _strUntilDate: strUntilDate
            , _ShowCancelation: ShowCancelation
            , _personSearchtype: personSearchtype
        }
    }).done(function (data) {
        var newWindow = window.open();
        newWindow.document.write(data);
        newWindow.document.close();
    }).always(function () {
        ploder.style.display = "none";
    });
});


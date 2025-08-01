
// تابعی برای ارسال درخواست به سرور
function keepSessionAlive() {
    fetch('/Home/KeepSessionAlive', { method: 'GET' })
        .then(response => {
            if (!response.ok) {
                console.error('Error keeping session alive');
            }
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

$(document).on('click', '.printlabel', function () {
    alert();
});

// انتخاب دکمه نصب
const installButton = document.getElementById("installPWAButton");

// متغیر برای ذخیره رویداد beforeinstallprompt
let deferredPrompt;

// گوش دادن به رویداد beforeinstallprompt
window.addEventListener("beforeinstallprompt", (e) => {
    // جلوگیری از نمایش خودکار پیام نصب
    e.preventDefault();
    // ذخیره رویداد
    deferredPrompt = e;

    // نمایش دکمه نصب
    installButton.style.display = "block";

    // اضافه کردن کلیک روی دکمه نصب
    installButton.addEventListener("click", async () => {
        // نمایش پیام نصب
        deferredPrompt.prompt();
        // انتظار برای انتخاب کاربر
        const choiceResult = await deferredPrompt.userChoice;

        // بررسی نتیجه انتخاب کاربر
        if (choiceResult.outcome === "accepted") {
            console.log("User accepted the PWA installation.");
        } else {
            console.log("User dismissed the PWA installation.");
        }

        // مخفی کردن دکمه نصب
        installButton.style.display = "none";
        deferredPrompt = null;
    });
});


$('#themechange').change(function () {
    let uiId = $(this).val();

    $.get("/Home/uitoggle/id=" + uiId).done(function (ui) {
        alert();
    });
});

$(document).on('click', '.callform[data-url]', function () {

    let actionUrl = $(this).data('url');

    $.get(actionUrl).done(function (data) {

        appModal = $('#appmodal');
        appModal.html(data);
        appModal.modal('show');

        $(document).find('.flatpickr-input').flatpickr({
            locale: 'fa',
            altInput: true,
            altFormat: 'Y/m/d',
            allowInput: true,
            disableMobile: true
        });
    });
});
$(document).on('click', '.removeById', function () {
    /*loder.style.display = "block";*/

    let actionUrl = $(this).data('url');
    let Id = $(this).data('target');
    var confirmDialogText = $(this).attr('title');

    if (confirm(confirmDialogText)) {
        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: { id: Id }
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

                $(this).delay(1000).queue(function () {
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
            /*  loder.style.display = "none";*/
        });
    }
    else {
        /*loder.style.display = "none";*/
    }
});

//================================================================
$(document).on('input', '.numberInput', function () {
    var inputVal = $(this).val();
    // حذف هر کاراکتر غیر عددی
    inputVal = inputVal.replace(/[^\d]/g, '');
    // اضافه کردن جداکننده‌های هزارگان
    var formattedInputVal = inputVal.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    $(this).val(formattedInputVal);
});
$(document).on('change', '.numberInput', function () {
    var inputVal = $(this).val();
    if (inputVal == null || inputVal == '')
        $(this).val(0);
});

$(document).on('select2:open', function (e) {
    // پیدا کردن فیلد جستجوی Select2
    var searchField = $(e.target).data('select2').dropdown.$search;
    if (searchField) {
        // تمرکز و انتخاب متن در فیلد جستجو
        searchField.focus().select();
    }
});

$(document).on('keypress', '.gonext', function (e) {
    if (e.which === 13) {  // Enter key
        e.preventDefault();
        var inputs = $(this).parents('form').find(':input:visible');
        var idx = inputs.index(this);

        while (idx < inputs.length - 1) {
            idx++;  // حرکت به کنترل بعدی
            var nextInput = inputs[idx];

            // چک کردن اینکه کنترل بعدی readonly نباشد
            if (!$(nextInput).prop('readonly')) {
                $(nextInput).focus().select();
                break; // خروج از حلقه در صورت یافتن کنترل بدون readonly
            }
        }

        // اگر به آخرین کنترل رسید، دکمه ثبت را فشار دهد
        if (idx === inputs.length - 1) {
            $('.accAjaxSubmit').click();
        }
    }
});
$(document).on('keydown', '.flatpickr-input', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        e.preventDefault(); // جلوگیری از بسته شدن تقویم

        // رفتن به کنترل بعدی (فیلدی که در فرم بعد از این قرار دارد)
        let $next = $(this).closest('form').find(':input:visible:enabled:not([readonly])').eq(
            $(this).closest('form').find(':input:visible:enabled:not([readonly])').index(this) + 1
        );

        if ($next.length) {
            $next.focus().select(); // فوکوس و انتخاب متن
        }
    }
});


$('#accmodal').on('shown.bs.modal', function () {

    $(this).find('.firstSelect').focus();
    $(this).find('.firstSelect').select();
});
$(document).on('load', function () {

    $(this).find('.firstSelect').focus();
    $(this).find('.firstSelect').select();
});
$('#model-general-larg').on('shown.bs.modal', function () {

    $(this).find('.firstSelect').focus();
    $(this).find('.firstSelect').select();
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

$(document).on('click', '.confirmForm', function () {
    /*  loder.style.display = "block";*/
    let frm = $(this).closest('form');

    /*let frm = $(this).parents('form');*/
    let actionUrl = frm.attr('action');
    let title = $(this).attr('title');
    let dataToSend = new FormData(frm.get(0));

    if (confirm(title)) {
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
    }

});

$('.ajaxgetModal').on('click', function (e) {
    e.preventDefault();
    var form = $('#frmbilling');
    var formData = form.serialize();
    var actionUrl = form.attr('action');
    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: formData,
        success: function (response) {
            // بارگذاری PartialView در modal
            $('#appmodal').html(response);
            $('#appmodal').modal('show');

            $(document).find('#appmodal').find('.flatpickr-input').flatpickr({
                locale: 'fa',
                altInput: true,
                altFormat: 'Y/m/d',
                allowInput: true,
                disableMobile: true
            });
        },
        error: function (xhr, status, error) {
            alert("خطا در دریافت اطلاعات: " + error);
        }
    });
})

$('.modalByAjax').on('click', function (e) {
    e.preventDefault();
    var form = $(this).closest('form');
    var formData = form.serialize();
    var actionUrl = form.attr('action');

    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: formData,
        success: function (response) {
            // بارگذاری PartialView در modal
            $('#appmodal').html(response);
            $('#appmodal').modal('show');

            $(document).find('#appmodal').find('.flatpickr-input').flatpickr({
                locale: 'fa',
                altInput: true,
                altFormat: 'Y/m/d',
                allowInput: true,
                disableMobile: true
            });
        },
        error: function (xhr, status, error) {
            alert("خطا در دریافت اطلاعات: " + error);
        }
    });
})

$('.modalByAjax---').on('click', function (e) {
    e.preventDefault();
    var form = $(this).closest('form');
    var formData = form.find('select[name="personId"]').val();
    var actionUrl = form.attr('action');

    alert(formData);
    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: { personId: formData },
        success: function (response) {
            // بارگذاری PartialView در modal
            $('#appmodal').html(response);
            $('#appmodal').modal('show');

        },
        error: function (xhr, status, error) {
            alert("خطا در دریافت اطلاعات: " + error);
        }
    });
})

// ------------ Payment --------------------

// هندل کردن تغییر روش پرداخت (با delegation برای مدال)
$(document).on('change', '#OperationId', function () {
    let val = $(this).val();
    toggleFields(val);
});

// تابع ریست فیلدهای وابسته
function resetFields() {
    $(document).find('#PosId').val('').trigger('change');
    $(document).find('#BankAccountId').val('').trigger('change');
    $(document).find('#TransferNumber').val('');
}

// تابع نمایش/مخفی کردن فیلدها بر اساس نوع پرداخت
function toggleFields(type) {
    let pos = $(document).find('#dposId');
    let bank = $(document).find('#dbankAccountId');
    let transfer = $(document).find('#dtransferNumber');
    let submitBtn = $(document).find('.accAjaxSubmit');

    pos.hide();
    bank.hide();
    transfer.hide();

    resetFields();

    switch (parseInt(type)) {
        case 1: // نقدی
            submitBtn.text("ثبت دریافت وجه");
            break;
        case 2: // کارت‌خوان
            pos.show();
            transfer.show();
            submitBtn.text("ثبت دریافت وجه");
            break;
        case 3: // حواله بانکی
            transfer.show();
            bank.show();
            submitBtn.text("ثبت دریافت وجه");
            break;
        case 601: // لینک پرداخت
            submitBtn.text("ثبت موقت بارنامه و ارسال لینک پرداخت برای طرف حساب");
            break;
        default:
            submitBtn.text("ثبت دریافت وجه");
    }
}

// در صورتی که مدال به تازگی به DOM تزریق شده، باید بعد از نمایش، این تابع رو صدا بزنی
function initCashModalBehavior() {
    let opVal = $(document).find('#OperationId').val();
    toggleFields(opVal || 1);
}


//======================== Pagination =========================================
$('.pagination a.page-link').on('click', function (e) {
    e.preventDefault(); // جلوگیری از رفتار پیش‌فرض لینک
    var currentPage = $(this).data('currentpage');
    var formId = $(this).data('formid');
    var form = $('#' + formId);
    form.find('input[name="filter.CurrentPage"]').val(currentPage);
    form.submit();
});

//--------------------------------------------------------------
$('.selectparty').hide();

$('#select_SettelmentType').on('change', function () {
    let setteltype = $(this).val();
    if (setteltype == 3 || setteltype == 4)
        $('.selectparty').show(300);
    else {
        $('.selectparty').hide();
    }
});

$(document).on('click', '.printlabel', function () {
    alert();
    let url = $(this).data('url');

    $.ajax({
        url: url,
        type: 'post',
        dataType: 'json'
    }).done(function (result) {
        if (result.success !== true) {
            Swal.fire({
                title: 'خطا در چاپ',
                text: result.message || 'چاپ لیبل انجام نشد',
                icon: 'error',
                confirmButtonText: 'باشه',
                customClass: { confirmButton: 'btn btn-danger' },
                buttonsStyling: false
            });
        }
        // موفقیت نیازی به پیام ندارد
    }).fail(function () {
        Swal.fire({
            title: 'خطای سیستم',
            text: 'در اتصال به چاپگر یا سرور مشکلی پیش آمده است',
            icon: 'warning',
            confirmButtonText: 'باشه',
            customClass: { confirmButton: 'btn btn-warning' },
            buttonsStyling: false
        });
    });
});



$(document).on('click', '.bulkAction-callmodal', function () {

    let actionUrl = $(this).data('url');
    let tblId = $(this).data('tableid');
    var tbl = $(document).find('#' + tblId);
    var dataToSend = [];
    tbl.find('.tblRow:checked').each(function () {
        dataToSend.push($(this).val());
    })
    if (dataToSend.length == 0) {
        return;
    }
    $.ajax({
        url: actionUrl,
        type: 'post',
        data: { items: dataToSend },
    }).done(function (response) {
        var modalTarget = $(document).find('#appmodal');
        modalTarget.html(response);

        $(document).find('.flatpickr-input').flatpickr({
            locale: 'fa',
            altInput: true,
            altFormat: 'Y/m/d',
            allowInput: true,
        });
        modalTarget.modal('show');
    }).always(function () {
    });

});

$(document).on('change', '#selectTblRows', function () {
    var isChecked = this.checked;
    $(document).find('input[name="tableRow"]').prop('checked', isChecked);
});


$('#selectroute').on('change', function () {

    const actionUrl = $(this).data('url');
    const targetId = $(this).data('filltarget');
    const target = $(targetId);
    const dataToSend = $(this).val();

    $.ajax({
        url: actionUrl,
        type: 'POST',
        data: { Id: dataToSend },
        dataType: 'json',
    }).done(function (response) {
        target.empty();
        if (response && response.length > 0) {
            $.each(response, function (index, lst) {
                target.append($('<option>', {
                    value: lst.id,
                    text: lst.branchName
                }));
            });
        } else {
            target.append($('<option>', {
                value: '00000000-0000-0000-0000-000000000000', // Guid.Empty
                text: 'فاقد نمایند'
            }));
        }

    }).fail(function (xhr, status, error) {
        console.error('درخواست با خطا مواجه شد:', error);
        target.empty();
        target.append($('<option>', {
            value: '00000000-0000-0000-0000-000000000000',
            text: 'فاقد نمایند'
        }));
    });

});





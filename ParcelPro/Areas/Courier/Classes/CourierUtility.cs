
public static class CourierUtility
{

    public static string ToShipmentTypeName(this Int16 ShipmentCode)
    {
        string shipmentName = "";

        switch (ShipmentCode)
        {
            case 1:
                shipmentName = "زمینی";
                break;
            case 2:
                shipmentName = "هوایی";
                break;
            case 3:
                shipmentName = "دریایی";
                break;

            default:
                break;
        }

        return shipmentName;
    }

    public static string ToSettelmentTypeName(this int SettelmentTypeId)
    {
        string shipmentName = "";

        switch (SettelmentTypeId)
        {
            case 1:
                shipmentName = "نقدی";
                break;
            case 2:
                shipmentName = "پسکرایه";
                break;
            case 3:
                shipmentName = "اعتباری";
                break;

            default:
                break;
        }

        return shipmentName;
    }
    public static string ToSettelmentTypeName(this short? SettelmentTypeId)
    {
        string shipmentName = "نامشخص";
        if (!SettelmentTypeId.HasValue)
            return shipmentName;

        switch (SettelmentTypeId)
        {
            case 1:
                shipmentName = "نقدی";
                break;
            case 2:
                shipmentName = "پسکرایه";
                break;
            case 3:
                shipmentName = "اعتباری";
                break;

            default:
                break;
        }

        return shipmentName;
    }

    public static string ToRateImpactType(this Int16 InpactTypeId)
    {
        string shipmentName = "";

        switch (InpactTypeId)
        {
            case 1:
                shipmentName = "ثابت";
                break;
            case 2:
                shipmentName = "درصداز کل بارنامه";
                break;
            case 3:
                shipmentName = "درصد از مبلغ حمل بار";
                break;
            case 4:
                shipmentName = "محاسبه توسط کاربر";
                break;
            case 5:
                shipmentName = "محاسبه سیستمی";
                break;

            default:
                break;
        }

        return shipmentName;
    }

    //وضعیت بارنامه

    public static string ToParcelStatusName(this int statusId)
    {
        string statusName = "وضعیت نامشخص";

        switch (statusId)
        {
            case 1:
                statusName = "در حال صدور";
                break;
            case 2:
                statusName = "در انتظار پرداخت";
                break;
            case 3:
                statusName = "در انتظار جمع آوری";
                break;
            case 4:
                statusName = "در حال جمع آوری";
                break;
            case 5:
                statusName = "تأیید ورود به هاب مبدأ";
                break;
            case 6:
                statusName = "در حال ارسال به هاب شهر مقصد";
                break;
            case 7:
                statusName = "تأیید ورود به هاب شهر مقصد";
                break;
            case 8:
                statusName = "آماده توزیع (در انتظار تحویل به سفیر)";
                break;
            case 9:
                statusName = "تحویل سفیر جهت تحویل به گیرنده";
                break;
            case 10:
                statusName = "در انتظار پرداخت توسط گیرنده";
                break;
            case 11:
                statusName = "مرسوله تحویل گیرنده شد";
                break;
            case 12:
                statusName = "برگشت به هاب مقصد";
                break;
            case 13:
                statusName = "مفقود شده";
                break;
            case 14:
                statusName = "فاسد شده";
                break;
            case 15:
                statusName = "باطل شده";
                break;

            default:
                // اگر شناسه معتبر نباشد، پیش‌فرض "وضعیت نامشخص" بازگردانده می‌شود.
                break;
        }

        return statusName;
    }

    public static string ToBillOfLadingStatusName(this short statusCode)
    {
        string statusName = "";

        switch (statusCode)
        {
            case 1:
                statusName = "در حال صدور";
                break;
            case 2:
                statusName = "در انتظار پرداخت";
                break;
            case 3:
                statusName = "در انتظار جمع آوری";
                break;
            case 4:
                statusName = "در حال جمع آوری";
                break;
            case 5:
                statusName = "تأیید ورود به هاب مبدأ";
                break;
            case 6:
                statusName = "در حال ارسال به هاب شهر مقصد";
                break;
            case 7:
                statusName = "تأیید ورود به هاب شهر مقصد";
                break;
            case 8:
                statusName = "آماده توزیع (در انتظار تحویل به سفیر)";
                break;
            case 9:
                statusName = "تحویل سفیر جهت تحویل به گیرنده";
                break;
            case 10:
                statusName = "در انتظار پرداخت توسط گیرنده";
                break;
            case 11:
                statusName = "مرسوله تحویل گیرنده شد";
                break;
            case 12:
                statusName = "برگشت به هاب مقصد";
                break;
            case 13:
                statusName = "مفقود شده";
                break;
            case 14:
                statusName = "فاسد شده";
                break;
            case 15:
                statusName = "باطل شده";
                break;

            default:
                statusName = "وضعیت نامشخص";
                break;
        }

        return statusName;
    }

    public static string BillStatusToBtnCssClassName(this Int16 statusId)
    {
        string statusName = "";

        switch (statusId)
        {
            case 1:
                statusName = "btn-secondary";
                break;
            case 2:
                statusName = "btn-danger";
                break;
            case 3:
                statusName = "btn-info";
                break;
            case 4:
                statusName = "btn-success";
                break;
            case 5:
                statusName = "";
                break;
            case 6:
                statusName = "";
                break;
            case 7:
                statusName = "";
                break;
            case 8:
                statusName = ")";
                break;


            default:
                break;
        }

        return statusName;
    }

    public static string ToIsSetteledName(this bool IsSetteled)
    {
        if (IsSetteled)
            return "پرداخت شده";
        else
            return "پرداخت نشده";
    }

    public static string ToSimpleStatusName(this short status)
    {
        if (status < 11)
            return "تحویل نشده";
        else if (status == 11)
            return "تحویل شده";
        else if (status == 15)
            return "باطل شده";

        return "";
    }

}


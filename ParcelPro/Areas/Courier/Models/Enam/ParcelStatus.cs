
public enum ParcelStatus
{
    /// <summary>
    /// مرسوله در حال صدور است.
    /// </summary>
    InIssuance = 1,

    /// <summary>
    /// مرسوله در انتظار پرداخت است.
    /// </summary>
    AwaitingPayment = 2,

    /// <summary>
    /// مرسوله در انتظار جمع‌آوری است.
    /// </summary>
    AwaitingPickup = 3,

    /// <summary>
    /// مرسوله در حال جمع‌آوری است.
    /// </summary>
    BeingPickedUp = 4,

    /// <summary>
    /// مرسوله به هاب مبدأ وارد شده است.
    /// </summary>
    ConfirmedAtOriginHub = 5,

    /// <summary>
    /// مرسوله در حال ارسال به هاب شهر مقصد است.
    /// </summary>
    BeingSentToDestinationHub = 6,

    /// <summary>
    /// مرسوله به هاب شهر مقصد وارد شده است.
    /// </summary>
    ConfirmedAtDestinationHub = 7,

    /// <summary>
    /// مرسوله آماده توزیع است و منتظر تحویل به سفیر است.
    /// </summary>
    ReadyForDistribution = 8,

    /// <summary>
    /// مرسوله به سفیر تحویل شده است و در حال ارسال به گیرنده است.
    /// </summary>
    DeliveredToCourier = 9,

    /// <summary>
    /// مرسوله در انتظار پرداخت توسط گیرنده است.
    /// </summary>
    AwaitingRecipientPayment = 10,

    /// <summary>
    /// مرسوله با موفقیت به گیرنده تحویل داده شد.
    /// </summary>
    DeliveredToRecipient = 11,

    /// <summary>
    /// مرسوله به هاب مقصد برگشت داده شده است.
    /// </summary>
    ReturnedToDestinationHub = 12,

    /// <summary>
    /// مرسوله مفقود شده است.
    /// </summary>
    Lost = 13,

    /// <summary>
    /// مرسوله فاسد شده است.
    /// </summary>
    Damaged = 14
}



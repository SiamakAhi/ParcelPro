// تابع تولید QR کد
function generateQRCode(data) {
    const qrCanvas = document.getElementById('qr-code');
    QRCode.toCanvas(qrCanvas, data, {
        width: 100,
        height: 100,
        margin: 1,
        color: {
            dark: '#000000',
            light: '#ffffff'
        }
    }, function (error) {
        if (error) console.error(error);
    });
}

// تابع تولید بارکد
function generateBarcode(data) {
    const barcodeCanvas = document.getElementById('barcode');
    JsBarcode(barcodeCanvas, data, {
        format: "CODE128",
        width: 2,
        height: 50,
        displayValue: false,
        margin: 10
    });
}

// تابع چاپ بارنامه
function printWaybill() {
    const printWindow = window.open('', '_blank');
    printWindow.document.write(`
        <!DOCTYPE html>
        <html dir="rtl" lang="fa">
        <head>
            <meta charset="UTF-8">
            <title>بارنامه حمل کالا</title>
            <link rel="stylesheet" href="style.css">
            <style>
                body {
                    padding: 0;
                    margin: 0;
                    background-color: white;
                }
                .container {
                    box-shadow: none;
                    margin: 0;
                    padding: 10mm;
                }
                .action-buttons {
                    display: none;
                }
                @page {
                    size: A5 portrait;
                    margin: 0;
                }
            </style>
        </head>
        <body>
            ${document.querySelector('.container').innerHTML}
        </body>
        </html>
    `);
    printWindow.document.close();
    printWindow.focus();
    setTimeout(() => {
        printWindow.print();
        printWindow.close();
    }, 500);
}

// تابع اشتراک PDF
async function sharePDF() {
    try {
        const container = document.querySelector('.container');
        const canvas = await html2canvas(container, {
            scale: 2,
            useCORS: true,
            logging: false
        });

        const imgData = canvas.toDataURL('image/jpeg', 1.0);
        const pdf = new jspdf.jsPDF({
            orientation: 'portrait',
            unit: 'mm',
            format: 'a5'
        });

        const imgProps = pdf.getImageProperties(imgData);
        const pdfWidth = pdf.internal.pageSize.getWidth();
        const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

        pdf.addImage(imgData, 'JPEG', 0, 0, pdfWidth, pdfHeight);

        try {
            const blob = pdf.output('blob');
            const file = new File([blob], 'بارنامه.pdf', { type: 'application/pdf' });

            if (navigator.share) {
                await navigator.share({
                    title: 'بارنامه حمل کالا',
                    text: 'بارنامه حمل کالا',
                    files: [file]
                });
            } else {
                pdf.save('بارنامه.pdf');
            }
        } catch (error) {
            console.error('خطا در اشتراک PDF:', error);
            pdf.save('بارنامه.pdf');
        }
    } catch (error) {
        console.error('خطا در ایجاد PDF:', error);
        alert('خطا در ایجاد PDF. لطفاً دوباره تلاش کنید.');
    }
}

// تابع اصلی برای تولید QR و بارکد
function generateCodes() {
    // این مقادیر را می‌توانید به صورت داینامیک تغییر دهید
    const qrData = {
        waybillNumber: "5160-1126067",
        date: "1403/12/28",
        sender: "علی باوی",
        receiver: "آقای موسوی",
        amount: "27335000"
    };

    const barcodeData = "51601126067"; // شماره بارنامه

    // تولید QR کد
    generateQRCode(JSON.stringify(qrData));

    // تولید بارکد
    generateBarcode(barcodeData);
}

// اجرای تابع تولید کدها پس از لود صفحه
document.addEventListener('DOMContentLoaded', generateCodes); 
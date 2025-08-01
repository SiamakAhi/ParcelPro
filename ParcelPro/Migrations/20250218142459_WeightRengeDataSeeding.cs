using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class WeightRengeDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cu_RateWeightRanges",
                columns: new[] { "Id", "Courier_WeightFactorPercent", "EndWeight", "IATA_WeightFactorPercent", "StartWeight" },
                values: new object[,]
                {
                    { 5, 5m, 2.5, 5m, 2.0009999999999999 },
                    { 6, 10m, 3.0, 10m, 2.5009999999999999 },
                    { 7, 15m, 3.5, 15m, 3.0009999999999999 },
                    { 8, 20m, 4.0, 20m, 3.5009999999999999 },
                    { 9, 25m, 4.5, 25m, 4.0010000000000003 },
                    { 10, 30m, 5.0, 30m, 4.5010000000000003 },
                    { 11, 35m, 5.5, 35m, 5.0010000000000003 },
                    { 12, 40m, 6.0, 40m, 5.5010000000000003 },
                    { 13, 45m, 6.5, 45m, 6.0010000000000003 },
                    { 14, 50m, 7.0, 50m, 6.5010000000000003 },
                    { 15, 55m, 7.5, 55m, 7.0010000000000003 },
                    { 16, 60m, 8.0, 60m, 7.5010000000000003 },
                    { 17, 65m, 8.5, 65m, 8.0009999999999994 },
                    { 18, 70m, 9.0, 70m, 8.5009999999999994 },
                    { 19, 75m, 9.5, 75m, 9.0009999999999994 },
                    { 20, 80m, 10.0, 80m, 9.5009999999999994 },
                    { 21, 85m, 10.5, 85m, 10.000999999999999 },
                    { 22, 90m, 11.0, 90m, 10.500999999999999 },
                    { 23, 95m, 11.5, 95m, 11.000999999999999 },
                    { 24, 100m, 12.0, 100m, 11.500999999999999 },
                    { 25, 105m, 12.5, 105m, 12.000999999999999 },
                    { 26, 110m, 13.0, 110m, 12.500999999999999 },
                    { 27, 115m, 13.5, 115m, 13.000999999999999 },
                    { 28, 120m, 14.0, 120m, 13.500999999999999 },
                    { 29, 125m, 14.5, 125m, 14.000999999999999 },
                    { 30, 130m, 15.0, 130m, 14.500999999999999 },
                    { 31, 135m, 15.5, 135m, 15.000999999999999 },
                    { 32, 140m, 16.0, 140m, 15.500999999999999 },
                    { 33, 145m, 16.5, 145m, 16.001000000000001 },
                    { 34, 150m, 17.0, 150m, 16.501000000000001 },
                    { 35, 155m, 17.5, 155m, 17.001000000000001 },
                    { 36, 160m, 18.0, 160m, 17.501000000000001 },
                    { 37, 165m, 18.5, 165m, 18.001000000000001 },
                    { 38, 170m, 19.0, 170m, 18.501000000000001 },
                    { 39, 175m, 19.5, 175m, 19.001000000000001 },
                    { 40, 180m, 20.0, 180m, 19.501000000000001 },
                    { 41, 185m, 20.5, 185m, 20.001000000000001 },
                    { 42, 190m, 21.0, 190m, 20.501000000000001 },
                    { 43, 195m, 21.5, 195m, 21.001000000000001 },
                    { 44, 200m, 22.0, 200m, 21.501000000000001 },
                    { 45, 205m, 22.5, 205m, 22.001000000000001 },
                    { 46, 210m, 23.0, 210m, 22.501000000000001 },
                    { 47, 215m, 23.5, 215m, 23.001000000000001 },
                    { 48, 220m, 24.0, 220m, 23.501000000000001 },
                    { 49, 225m, 24.5, 225m, 24.001000000000001 },
                    { 50, 230m, 25.0, 230m, 24.501000000000001 },
                    { 51, 235m, 25.5, 235m, 25.001000000000001 },
                    { 52, 240m, 26.0, 240m, 25.501000000000001 },
                    { 53, 245m, 26.5, 245m, 26.001000000000001 },
                    { 54, 250m, 27.0, 250m, 26.501000000000001 },
                    { 55, 255m, 27.5, 255m, 27.001000000000001 },
                    { 56, 260m, 28.0, 260m, 27.501000000000001 },
                    { 57, 265m, 28.5, 265m, 28.001000000000001 },
                    { 58, 270m, 29.0, 270m, 28.501000000000001 },
                    { 59, 275m, 29.5, 275m, 29.001000000000001 },
                    { 60, 280m, 30.0, 280m, 29.501000000000001 },
                    { 61, 285m, 30.5, 285m, 30.001000000000001 },
                    { 62, 290m, 31.0, 290m, 30.501000000000001 },
                    { 63, 295m, 31.5, 295m, 31.001000000000001 },
                    { 64, 300m, 32.0, 300m, 31.501000000000001 },
                    { 65, 305m, 32.5, 305m, 32.000999999999998 },
                    { 66, 310m, 33.0, 310m, 32.500999999999998 },
                    { 67, 315m, 33.5, 315m, 33.000999999999998 },
                    { 68, 320m, 34.0, 320m, 33.500999999999998 },
                    { 69, 325m, 34.5, 325m, 34.000999999999998 },
                    { 70, 330m, 35.0, 330m, 34.500999999999998 },
                    { 71, 335m, 35.5, 335m, 35.000999999999998 },
                    { 72, 340m, 36.0, 340m, 35.500999999999998 },
                    { 73, 345m, 36.5, 345m, 36.000999999999998 },
                    { 74, 350m, 37.0, 350m, 36.500999999999998 },
                    { 75, 355m, 37.5, 355m, 37.000999999999998 },
                    { 76, 360m, 38.0, 360m, 37.500999999999998 },
                    { 77, 365m, 38.5, 365m, 38.000999999999998 },
                    { 78, 370m, 39.0, 370m, 38.500999999999998 },
                    { 79, 375m, 39.5, 375m, 39.000999999999998 },
                    { 80, 380m, 40.0, 380m, 39.500999999999998 },
                    { 81, 385m, 40.5, 385m, 40.000999999999998 },
                    { 82, 390m, 41.0, 390m, 40.500999999999998 },
                    { 83, 395m, 41.5, 395m, 41.000999999999998 },
                    { 84, 400m, 42.0, 400m, 41.500999999999998 },
                    { 85, 405m, 42.5, 405m, 42.000999999999998 },
                    { 86, 410m, 43.0, 410m, 42.500999999999998 },
                    { 87, 415m, 43.5, 415m, 43.000999999999998 },
                    { 88, 420m, 44.0, 420m, 43.500999999999998 },
                    { 89, 425m, 44.5, 425m, 44.000999999999998 },
                    { 90, 430m, 45.0, 430m, 44.500999999999998 },
                    { 91, 435m, 45.5, 435m, 45.000999999999998 },
                    { 92, 440m, 46.0, 440m, 45.500999999999998 },
                    { 93, 445m, 46.5, 445m, 46.000999999999998 },
                    { 94, 450m, 47.0, 450m, 46.500999999999998 },
                    { 95, 455m, 47.5, 455m, 47.000999999999998 },
                    { 96, 460m, 48.0, 460m, 47.500999999999998 },
                    { 97, 465m, 48.5, 465m, 48.000999999999998 },
                    { 98, 470m, 49.0, 470m, 48.500999999999998 },
                    { 99, 475m, 49.5, 475m, 49.000999999999998 },
                    { 100, 480m, 50.0, 480m, 49.500999999999998 },
                    { 101, 485m, 50.5, 485m, 50.000999999999998 },
                    { 102, 490m, 51.0, 490m, 50.500999999999998 },
                    { 103, 495m, 51.5, 495m, 51.000999999999998 },
                    { 104, 500m, 52.0, 500m, 51.500999999999998 },
                    { 105, 505m, 52.5, 505m, 52.000999999999998 },
                    { 106, 510m, 53.0, 510m, 52.500999999999998 },
                    { 107, 515m, 53.5, 515m, 53.000999999999998 },
                    { 108, 520m, 54.0, 520m, 53.500999999999998 },
                    { 109, 525m, 54.5, 525m, 54.000999999999998 },
                    { 110, 530m, 55.0, 530m, 54.500999999999998 },
                    { 111, 535m, 55.5, 535m, 55.000999999999998 },
                    { 112, 540m, 56.0, 540m, 55.500999999999998 },
                    { 113, 545m, 56.5, 545m, 56.000999999999998 },
                    { 114, 550m, 57.0, 550m, 56.500999999999998 },
                    { 115, 555m, 57.5, 555m, 57.000999999999998 },
                    { 116, 560m, 58.0, 560m, 57.500999999999998 },
                    { 117, 565m, 58.5, 565m, 58.000999999999998 },
                    { 118, 570m, 59.0, 570m, 58.500999999999998 },
                    { 119, 575m, 59.5, 575m, 59.000999999999998 },
                    { 120, 580m, 60.0, 580m, 59.500999999999998 },
                    { 121, 585m, 60.5, 585m, 60.000999999999998 },
                    { 122, 590m, 61.0, 590m, 60.500999999999998 },
                    { 123, 595m, 61.5, 595m, 61.000999999999998 },
                    { 124, 600m, 62.0, 600m, 61.500999999999998 },
                    { 125, 605m, 62.5, 605m, 62.000999999999998 },
                    { 126, 610m, 63.0, 610m, 62.500999999999998 },
                    { 127, 615m, 63.5, 615m, 63.000999999999998 },
                    { 128, 620m, 64.0, 620m, 63.500999999999998 },
                    { 129, 625m, 64.5, 625m, 64.001000000000005 },
                    { 130, 630m, 65.0, 630m, 64.501000000000005 },
                    { 131, 635m, 65.5, 635m, 65.001000000000005 },
                    { 132, 640m, 66.0, 640m, 65.501000000000005 },
                    { 133, 645m, 66.5, 645m, 66.001000000000005 },
                    { 134, 650m, 67.0, 650m, 66.501000000000005 },
                    { 135, 655m, 67.5, 655m, 67.001000000000005 },
                    { 136, 660m, 68.0, 660m, 67.501000000000005 },
                    { 137, 665m, 68.5, 665m, 68.001000000000005 },
                    { 138, 670m, 69.0, 670m, 68.501000000000005 },
                    { 139, 675m, 69.5, 675m, 69.001000000000005 },
                    { 140, 680m, 70.0, 680m, 69.501000000000005 },
                    { 141, 685m, 70.5, 685m, 70.001000000000005 },
                    { 142, 690m, 71.0, 690m, 70.501000000000005 },
                    { 143, 695m, 71.5, 695m, 71.001000000000005 },
                    { 144, 700m, 72.0, 700m, 71.501000000000005 },
                    { 145, 705m, 72.5, 705m, 72.001000000000005 },
                    { 146, 710m, 73.0, 710m, 72.501000000000005 },
                    { 147, 715m, 73.5, 715m, 73.001000000000005 },
                    { 148, 720m, 74.0, 720m, 73.501000000000005 },
                    { 149, 725m, 74.5, 725m, 74.001000000000005 },
                    { 150, 730m, 75.0, 730m, 74.501000000000005 },
                    { 151, 735m, 75.5, 735m, 75.001000000000005 },
                    { 152, 740m, 76.0, 740m, 75.501000000000005 },
                    { 153, 745m, 76.5, 745m, 76.001000000000005 },
                    { 154, 750m, 77.0, 750m, 76.501000000000005 },
                    { 155, 755m, 77.5, 755m, 77.001000000000005 },
                    { 156, 760m, 78.0, 760m, 77.501000000000005 },
                    { 157, 765m, 78.5, 765m, 78.001000000000005 },
                    { 158, 770m, 79.0, 770m, 78.501000000000005 },
                    { 159, 775m, 79.5, 775m, 79.001000000000005 },
                    { 160, 780m, 80.0, 780m, 79.501000000000005 },
                    { 161, 785m, 80.5, 785m, 80.001000000000005 },
                    { 162, 790m, 81.0, 790m, 80.501000000000005 },
                    { 163, 795m, 81.5, 795m, 81.001000000000005 },
                    { 164, 800m, 82.0, 800m, 81.501000000000005 },
                    { 165, 805m, 82.5, 805m, 82.001000000000005 },
                    { 166, 810m, 83.0, 810m, 82.501000000000005 },
                    { 167, 815m, 83.5, 815m, 83.001000000000005 },
                    { 168, 820m, 84.0, 820m, 83.501000000000005 },
                    { 169, 825m, 84.5, 825m, 84.001000000000005 },
                    { 170, 830m, 85.0, 830m, 84.501000000000005 },
                    { 171, 835m, 85.5, 835m, 85.001000000000005 },
                    { 172, 840m, 86.0, 840m, 85.501000000000005 },
                    { 173, 845m, 86.5, 845m, 86.001000000000005 },
                    { 174, 850m, 87.0, 850m, 86.501000000000005 },
                    { 175, 855m, 87.5, 855m, 87.001000000000005 },
                    { 176, 860m, 88.0, 860m, 87.501000000000005 },
                    { 177, 865m, 88.5, 865m, 88.001000000000005 },
                    { 178, 870m, 89.0, 870m, 88.501000000000005 },
                    { 179, 875m, 89.5, 875m, 89.001000000000005 },
                    { 180, 880m, 90.0, 880m, 89.501000000000005 },
                    { 181, 885m, 90.5, 885m, 90.001000000000005 },
                    { 182, 890m, 91.0, 890m, 90.501000000000005 },
                    { 183, 895m, 91.5, 895m, 91.001000000000005 },
                    { 184, 900m, 92.0, 900m, 91.501000000000005 },
                    { 185, 905m, 92.5, 905m, 92.001000000000005 },
                    { 186, 910m, 93.0, 910m, 92.501000000000005 },
                    { 187, 915m, 93.5, 915m, 93.001000000000005 },
                    { 188, 920m, 94.0, 920m, 93.501000000000005 },
                    { 189, 925m, 94.5, 925m, 94.001000000000005 },
                    { 190, 930m, 95.0, 930m, 94.501000000000005 },
                    { 191, 935m, 95.5, 935m, 95.001000000000005 },
                    { 192, 940m, 96.0, 940m, 95.501000000000005 },
                    { 193, 945m, 96.5, 945m, 96.001000000000005 },
                    { 194, 950m, 97.0, 950m, 96.501000000000005 },
                    { 195, 955m, 97.5, 955m, 97.001000000000005 },
                    { 196, 960m, 98.0, 960m, 97.501000000000005 },
                    { 197, 965m, 98.5, 965m, 98.001000000000005 },
                    { 198, 970m, 99.0, 970m, 98.501000000000005 },
                    { 199, 975m, 99.5, 975m, 99.001000000000005 },
                    { 200, 980m, 100.0, 980m, 99.501000000000005 },
                    { 201, 985m, 100.5, 985m, 100.001 },
                    { 202, 990m, 101.0, 990m, 100.501 },
                    { 203, 995m, 101.5, 995m, 101.001 },
                    { 204, 1000m, 102.0, 1000m, 101.501 },
                    { 205, 1005m, 102.5, 1005m, 102.001 },
                    { 206, 1010m, 103.0, 1010m, 102.501 },
                    { 207, 1015m, 103.5, 1015m, 103.001 },
                    { 208, 1020m, 104.0, 1020m, 103.501 },
                    { 209, 1025m, 104.5, 1025m, 104.001 },
                    { 210, 1030m, 105.0, 1030m, 104.501 },
                    { 211, 1035m, 105.5, 1035m, 105.001 },
                    { 212, 1040m, 106.0, 1040m, 105.501 },
                    { 213, 1045m, 106.5, 1045m, 106.001 },
                    { 214, 1050m, 107.0, 1050m, 106.501 },
                    { 215, 1055m, 107.5, 1055m, 107.001 },
                    { 216, 1060m, 108.0, 1060m, 107.501 },
                    { 217, 1065m, 108.5, 1065m, 108.001 },
                    { 218, 1070m, 109.0, 1070m, 108.501 },
                    { 219, 1075m, 109.5, 1075m, 109.001 },
                    { 220, 1080m, 110.0, 1080m, 109.501 },
                    { 221, 1085m, 110.5, 1085m, 110.001 },
                    { 222, 1090m, 111.0, 1090m, 110.501 },
                    { 223, 1095m, 111.5, 1095m, 111.001 },
                    { 224, 1100m, 112.0, 1100m, 111.501 },
                    { 225, 1105m, 112.5, 1105m, 112.001 },
                    { 226, 1110m, 113.0, 1110m, 112.501 },
                    { 227, 1115m, 113.5, 1115m, 113.001 },
                    { 228, 1120m, 114.0, 1120m, 113.501 },
                    { 229, 1125m, 114.5, 1125m, 114.001 },
                    { 230, 1130m, 115.0, 1130m, 114.501 },
                    { 231, 1135m, 115.5, 1135m, 115.001 },
                    { 232, 1140m, 116.0, 1140m, 115.501 },
                    { 233, 1145m, 116.5, 1145m, 116.001 },
                    { 234, 1150m, 117.0, 1150m, 116.501 },
                    { 235, 1155m, 117.5, 1155m, 117.001 },
                    { 236, 1160m, 118.0, 1160m, 117.501 },
                    { 237, 1165m, 118.5, 1165m, 118.001 },
                    { 238, 1170m, 119.0, 1170m, 118.501 },
                    { 239, 1175m, 119.5, 1175m, 119.001 },
                    { 240, 1180m, 120.0, 1180m, 119.501 },
                    { 241, 1185m, 120.5, 1185m, 120.001 },
                    { 242, 1190m, 121.0, 1190m, 120.501 },
                    { 243, 1195m, 121.5, 1195m, 121.001 },
                    { 244, 1200m, 122.0, 1200m, 121.501 },
                    { 245, 1205m, 122.5, 1205m, 122.001 },
                    { 246, 1210m, 123.0, 1210m, 122.501 },
                    { 247, 1215m, 123.5, 1215m, 123.001 },
                    { 248, 1220m, 124.0, 1220m, 123.501 },
                    { 249, 1225m, 124.5, 1225m, 124.001 },
                    { 250, 1230m, 125.0, 1230m, 124.501 },
                    { 251, 1235m, 125.5, 1235m, 125.001 },
                    { 252, 1240m, 126.0, 1240m, 125.501 },
                    { 253, 1245m, 126.5, 1245m, 126.001 },
                    { 254, 1250m, 127.0, 1250m, 126.501 },
                    { 255, 1255m, 127.5, 1255m, 127.001 },
                    { 256, 1260m, 128.0, 1260m, 127.501 },
                    { 257, 1265m, 128.5, 1265m, 128.001 },
                    { 258, 1270m, 129.0, 1270m, 128.501 },
                    { 259, 1275m, 129.5, 1275m, 129.001 },
                    { 260, 1280m, 130.0, 1280m, 129.501 },
                    { 261, 1285m, 130.5, 1285m, 130.001 },
                    { 262, 1290m, 131.0, 1290m, 130.501 },
                    { 263, 1295m, 131.5, 1295m, 131.001 },
                    { 264, 1300m, 132.0, 1300m, 131.501 },
                    { 265, 1305m, 132.5, 1305m, 132.001 },
                    { 266, 1310m, 133.0, 1310m, 132.501 },
                    { 267, 1315m, 133.5, 1315m, 133.001 },
                    { 268, 1320m, 134.0, 1320m, 133.501 },
                    { 269, 1325m, 134.5, 1325m, 134.001 },
                    { 270, 1330m, 135.0, 1330m, 134.501 },
                    { 271, 1335m, 135.5, 1335m, 135.001 },
                    { 272, 1340m, 136.0, 1340m, 135.501 },
                    { 273, 1345m, 136.5, 1345m, 136.001 },
                    { 274, 1350m, 137.0, 1350m, 136.501 },
                    { 275, 1355m, 137.5, 1355m, 137.001 },
                    { 276, 1360m, 138.0, 1360m, 137.501 },
                    { 277, 1365m, 138.5, 1365m, 138.001 },
                    { 278, 1370m, 139.0, 1370m, 138.501 },
                    { 279, 1375m, 139.5, 1375m, 139.001 },
                    { 280, 1380m, 140.0, 1380m, 139.501 },
                    { 281, 1385m, 140.5, 1385m, 140.001 },
                    { 282, 1390m, 141.0, 1390m, 140.501 },
                    { 283, 1395m, 141.5, 1395m, 141.001 },
                    { 284, 1400m, 142.0, 1400m, 141.501 },
                    { 285, 1405m, 142.5, 1405m, 142.001 },
                    { 286, 1410m, 143.0, 1410m, 142.501 },
                    { 287, 1415m, 143.5, 1415m, 143.001 },
                    { 288, 1420m, 144.0, 1420m, 143.501 },
                    { 289, 1425m, 144.5, 1425m, 144.001 },
                    { 290, 1430m, 145.0, 1430m, 144.501 },
                    { 291, 1435m, 145.5, 1435m, 145.001 },
                    { 292, 1440m, 146.0, 1440m, 145.501 },
                    { 293, 1445m, 146.5, 1445m, 146.001 },
                    { 294, 1450m, 147.0, 1450m, 146.501 },
                    { 295, 1455m, 147.5, 1455m, 147.001 },
                    { 296, 1460m, 148.0, 1460m, 147.501 },
                    { 297, 1465m, 148.5, 1465m, 148.001 },
                    { 298, 1470m, 149.0, 1470m, 148.501 },
                    { 299, 1475m, 149.5, 1475m, 149.001 },
                    { 300, 1480m, 150.0, 1480m, 149.501 },
                    { 301, 1485m, 150.5, 1485m, 150.001 },
                    { 302, 1490m, 151.0, 1490m, 150.501 },
                    { 303, 1495m, 151.5, 1495m, 151.001 },
                    { 304, 1500m, 152.0, 1500m, 151.501 },
                    { 305, 1505m, 152.5, 1505m, 152.001 },
                    { 306, 1510m, 153.0, 1510m, 152.501 },
                    { 307, 1515m, 153.5, 1515m, 153.001 },
                    { 308, 1520m, 154.0, 1520m, 153.501 },
                    { 309, 1525m, 154.5, 1525m, 154.001 },
                    { 310, 1530m, 155.0, 1530m, 154.501 },
                    { 311, 1535m, 155.5, 1535m, 155.001 },
                    { 312, 1540m, 156.0, 1540m, 155.501 },
                    { 313, 1545m, 156.5, 1545m, 156.001 },
                    { 314, 1550m, 157.0, 1550m, 156.501 },
                    { 315, 1555m, 157.5, 1555m, 157.001 },
                    { 316, 1560m, 158.0, 1560m, 157.501 },
                    { 317, 1565m, 158.5, 1565m, 158.001 },
                    { 318, 1570m, 159.0, 1570m, 158.501 },
                    { 319, 1575m, 159.5, 1575m, 159.001 },
                    { 320, 1580m, 160.0, 1580m, 159.501 },
                    { 321, 1585m, 160.5, 1585m, 160.001 },
                    { 322, 1590m, 161.0, 1590m, 160.501 },
                    { 323, 1595m, 161.5, 1595m, 161.001 },
                    { 324, 1600m, 162.0, 1600m, 161.501 },
                    { 325, 1605m, 162.5, 1605m, 162.001 },
                    { 326, 1610m, 163.0, 1610m, 162.501 },
                    { 327, 1615m, 163.5, 1615m, 163.001 },
                    { 328, 1620m, 164.0, 1620m, 163.501 },
                    { 329, 1625m, 164.5, 1625m, 164.001 },
                    { 330, 1630m, 165.0, 1630m, 164.501 },
                    { 331, 1635m, 165.5, 1635m, 165.001 },
                    { 332, 1640m, 166.0, 1640m, 165.501 },
                    { 333, 1645m, 166.5, 1645m, 166.001 },
                    { 334, 1650m, 167.0, 1650m, 166.501 },
                    { 335, 1655m, 167.5, 1655m, 167.001 },
                    { 336, 1660m, 168.0, 1660m, 167.501 },
                    { 337, 1665m, 168.5, 1665m, 168.001 },
                    { 338, 1670m, 169.0, 1670m, 168.501 },
                    { 339, 1675m, 169.5, 1675m, 169.001 },
                    { 340, 1680m, 170.0, 1680m, 169.501 },
                    { 341, 1685m, 170.5, 1685m, 170.001 },
                    { 342, 1690m, 171.0, 1690m, 170.501 },
                    { 343, 1695m, 171.5, 1695m, 171.001 },
                    { 344, 1700m, 172.0, 1700m, 171.501 },
                    { 345, 1705m, 172.5, 1705m, 172.001 },
                    { 346, 1710m, 173.0, 1710m, 172.501 },
                    { 347, 1715m, 173.5, 1715m, 173.001 },
                    { 348, 1720m, 174.0, 1720m, 173.501 },
                    { 349, 1725m, 174.5, 1725m, 174.001 },
                    { 350, 1730m, 175.0, 1730m, 174.501 },
                    { 351, 1735m, 175.5, 1735m, 175.001 },
                    { 352, 1740m, 176.0, 1740m, 175.501 },
                    { 353, 1745m, 176.5, 1745m, 176.001 },
                    { 354, 1750m, 177.0, 1750m, 176.501 },
                    { 355, 1755m, 177.5, 1755m, 177.001 },
                    { 356, 1760m, 178.0, 1760m, 177.501 },
                    { 357, 1765m, 178.5, 1765m, 178.001 },
                    { 358, 1770m, 179.0, 1770m, 178.501 },
                    { 359, 1775m, 179.5, 1775m, 179.001 },
                    { 360, 1780m, 180.0, 1780m, 179.501 },
                    { 361, 1785m, 180.5, 1785m, 180.001 },
                    { 362, 1790m, 181.0, 1790m, 180.501 },
                    { 363, 1795m, 181.5, 1795m, 181.001 },
                    { 364, 1800m, 182.0, 1800m, 181.501 },
                    { 365, 1805m, 182.5, 1805m, 182.001 },
                    { 366, 1810m, 183.0, 1810m, 182.501 },
                    { 367, 1815m, 183.5, 1815m, 183.001 },
                    { 368, 1820m, 184.0, 1820m, 183.501 },
                    { 369, 1825m, 184.5, 1825m, 184.001 },
                    { 370, 1830m, 185.0, 1830m, 184.501 },
                    { 371, 1835m, 185.5, 1835m, 185.001 },
                    { 372, 1840m, 186.0, 1840m, 185.501 },
                    { 373, 1845m, 186.5, 1845m, 186.001 },
                    { 374, 1850m, 187.0, 1850m, 186.501 },
                    { 375, 1855m, 187.5, 1855m, 187.001 },
                    { 376, 1860m, 188.0, 1860m, 187.501 },
                    { 377, 1865m, 188.5, 1865m, 188.001 },
                    { 378, 1870m, 189.0, 1870m, 188.501 },
                    { 379, 1875m, 189.5, 1875m, 189.001 },
                    { 380, 1880m, 190.0, 1880m, 189.501 },
                    { 381, 1885m, 190.5, 1885m, 190.001 },
                    { 382, 1890m, 191.0, 1890m, 190.501 },
                    { 383, 1895m, 191.5, 1895m, 191.001 },
                    { 384, 1900m, 192.0, 1900m, 191.501 },
                    { 385, 1905m, 192.5, 1905m, 192.001 },
                    { 386, 1910m, 193.0, 1910m, 192.501 },
                    { 387, 1915m, 193.5, 1915m, 193.001 },
                    { 388, 1920m, 194.0, 1920m, 193.501 },
                    { 389, 1925m, 194.5, 1925m, 194.001 },
                    { 390, 1930m, 195.0, 1930m, 194.501 },
                    { 391, 1935m, 195.5, 1935m, 195.001 },
                    { 392, 1940m, 196.0, 1940m, 195.501 },
                    { 393, 1945m, 196.5, 1945m, 196.001 },
                    { 394, 1950m, 197.0, 1950m, 196.501 },
                    { 395, 1955m, 197.5, 1955m, 197.001 },
                    { 396, 1960m, 198.0, 1960m, 197.501 },
                    { 397, 1965m, 198.5, 1965m, 198.001 },
                    { 398, 1970m, 199.0, 1970m, 198.501 },
                    { 399, 1975m, 199.5, 1975m, 199.001 },
                    { 400, 1980m, 200.0, 1980m, 199.501 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 293);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 294);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 295);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 296);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 297);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 298);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 299);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 314);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 315);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 316);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 317);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 318);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 319);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 323);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 324);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 325);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 326);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 327);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 328);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 329);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 330);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 333);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 334);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 335);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 336);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 337);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 338);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 339);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 340);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 341);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 342);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 343);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 344);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 345);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 346);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 347);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 348);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 349);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 350);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 351);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 352);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 353);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 354);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 355);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 356);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 357);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 358);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 359);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 360);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 361);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 362);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 363);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 364);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 365);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 366);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 367);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 372);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 373);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 374);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 375);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 376);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 377);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 378);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 379);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 380);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 381);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 382);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 383);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 384);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 385);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 386);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 387);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 388);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 389);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 390);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 391);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 392);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 393);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 394);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 395);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 396);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 397);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 398);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 399);

            migrationBuilder.DeleteData(
                table: "Cu_RateWeightRanges",
                keyColumn: "Id",
                keyValue: 400);
        }
    }
}

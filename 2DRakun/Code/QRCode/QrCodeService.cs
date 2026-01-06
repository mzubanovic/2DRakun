using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2DRakun.Code._2DBarCode
{
    public static class QrCodeService
    {
        public static byte[] GenerateQrCode(string payload)
        {
            using (var generator = new QRCodeGenerator())
            {
                var data = generator.CreateQrCode(
                    payload,
                    QRCodeGenerator.ECCLevel.M);

                using (var qrCode = new PngByteQRCode(data))
                {
                    return qrCode.GetGraphic(10);
                }
            }
        }

        public static string GenerateQrCodeBase64(string payload)
        {
            var bytes = GenerateQrCode(payload); // tvoja metoda za byte[]
            return "data:image/png;base64," + Convert.ToBase64String(bytes);
        }
    }
}
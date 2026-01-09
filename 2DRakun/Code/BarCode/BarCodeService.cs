
using ZXing;
using ZXing.Common;
using ZXing.PDF417;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;

namespace _2DRakun.Code._2DBarCode
{
    public static class BarCodeService
    {

        /// <summary>
        /// Generates a PDF417 2D barcode image from the specified payload string.
        /// The barcode is encoded using UTF-8 character set with automatic ECI enabled,
        /// allowing proper encoding of special characters (e.g. Croatian diacritics).
        /// </summary>
        /// <param name="payload">
        /// The HUB3A-formatted payload string to be encoded into the PDF417 barcode.
        /// </param>
        /// <returns>
        /// A byte array containing the generated PDF417 barcode image in PNG format.
        /// </returns>
        public static byte[] GeneratePdf417Barcode(string payload)
        {
            var options = new EncodingOptions
            {
                Width = 280,
                Height = 240,
                Margin = 25,
                PureBarcode = true
            };

            options.Hints[EncodeHintType.CHARACTER_SET] = "UTF-8";
            options.Hints[EncodeHintType.PDF417_AUTO_ECI] = true;

            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.PDF_417,
                Options = options
            };

            var pixelData = writer.Write(payload);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                var bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppRgb);

                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(
                        pixelData.Pixels,
                        0,
                        bitmapData.Scan0,
                        pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Generates a PDF417 2D barcode from the specified payload and returns it
        /// as a Base64-encoded PNG image string suitable for embedding in HTML or PDF documents.
        /// </summary>
        /// <param name="payload">
        /// The HUB3A-formatted payload string to be encoded into the PDF417 barcode.
        /// </param>
        /// <returns>
        /// A Base64-encoded string representing the generated PDF417 barcode PNG image.
        /// </returns>
        public static string GeneratePdf417BarcodeBase64(string payload)
        {
            var bytes = GeneratePdf417Barcode(payload);
            return Convert.ToBase64String(bytes);
        }
    }
}
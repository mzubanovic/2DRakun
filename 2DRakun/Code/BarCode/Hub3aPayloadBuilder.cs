using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace _2DRakun.Code
{
    public static class Hub3aPayloadBuilder
    {
        public static string Build(
            string receiverName,
            string receiverStreet,
            string receiverCity,
            string receiverCountry,
            string receiverIban,
            decimal amount,
            string model,
            string reference,
            string description)
        {
            var sb = new StringBuilder();

            sb.AppendLine("HRVHUB30");
            sb.AppendLine("EUR");
            sb.AppendLine(amount.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine(receiverName);
            sb.AppendLine(receiverStreet);
            sb.AppendLine(receiverCity);
            sb.AppendLine(receiverCountry);
            sb.AppendLine(receiverIban);
            sb.AppendLine("HR" + model);
            sb.AppendLine(reference);
            sb.AppendLine(description);

            return sb.ToString();
        }
    }
}
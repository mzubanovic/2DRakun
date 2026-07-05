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

            // Specifikacija zahtijeva zarez kao decimalni separator
            // i duljinu od 15 znakova, lijevo popunjenu nulama.
            string formattedAmount = amount.ToString("F2", CultureInfo.GetCultureInfo("hr-HR"))
                                           .Replace(".", ",")
                                           .PadLeft(15, '0');

            sb.AppendLine("HRVHUB30");
            sb.AppendLine("EUR");
            sb.AppendLine(formattedAmount);
            
            // Podaci o platitelju - mogu biti prazni
            sb.AppendLine(""); // Ime platitelja
            sb.AppendLine(""); // Adresa platitelja
            sb.AppendLine(""); // Mjesto platitelja

            // Podaci o primatelju
            sb.AppendLine(receiverName);
            sb.AppendLine(receiverStreet);
            sb.AppendLine(receiverCity);
            
            sb.AppendLine(receiverIban);
            sb.AppendLine(model); // Model npr. HR00
            sb.AppendLine(reference);
            sb.AppendLine(description);

            return sb.ToString();
        }
    }
}
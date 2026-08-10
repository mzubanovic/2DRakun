using _2DRakun.Code._2DBarCode;
using _2DRakun.Helpers;
using _2DRakun.Models;
using _2DRakun.Models.ViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2DRakun.Code
{

    public class InvoiceService
    {
        public static bool ValidateInvoiceModel(InvoiceViewModel model)
        {
            return model != null && model.Items != null && model.Items.Count > 0;
        }

        /// <summary>
        /// Saves the customer to the database (insert or update) and returns the customer ID.
        /// </summary>
        /// <param name="model">Invoice view model containing customer data</param>
        /// <param name="userId">ID of the user issuing the invoice</param>
        /// <returns>Customer db Id </returns>
        public static int SaveCustomer(InvoiceViewModel model, int userId)
        {
            var customer = new Customer
            {
                Name = model.CustomerName,
                Email = model.CustomerEmail,
                City = model.CustomerCity,
                Street = model.CustomerStreet,
                PostalCode = model.CustomerPostalCode,
                Oib = model.CustomerOib,
                Phone = model.CustomerPhone,
                UserId = userId
            };

            return CustomerHelper.InsertOrUpdateCustomer(customer);
        }


        /// <summary>
        /// Generates a HUB3A-compliant PDF417 2D barcode payload, encodes it as a Base64 string,
        /// and assigns it to the InvoiceViewModel for display.
        /// </summary>
        /// <param name="model">The invoice view model to which the barcode Base64 string will be assigned.</param>
        /// <param name="amount">The total invoice amount to encode in the barcode.</param>
        /// <param name="invoiceNumber">The invoice number/reference used as a payment reference.</param>
        /// <param name="user">The user issuing the invoice, providing receiver information.</param>
        public static void AddPdf417BarcodeToModel(InvoiceViewModel model, decimal amount, string invoiceNumber, User user)
        {
            // The total amount for the barcode must include VAT.
            var totalAmountWithVat = amount;

            var hubPayload = Hub3aPayloadBuilder.Build(
                receiverName: user.CompanyName,
                receiverStreet: user.Street,
                receiverCity: user.City,
                receiverIban: user.IBAN,
                amount: totalAmountWithVat,
                model: "HR00",
                reference: invoiceNumber,
                purposeCode: "OTHR", // Add purpose code
                description: "predračun " + invoiceNumber // Set description to invoice number
            );

            var base64Barcode = BarCodeService.GeneratePdf417BarcodeBase64(hubPayload);
            model.QrCodeBase64 = base64Barcode;
        }
    }
}

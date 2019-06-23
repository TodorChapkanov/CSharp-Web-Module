namespace Panda.ViewModels.Receipts
{
    public class ReceiptDetailsViewModel
    {
        public string ReceiptNumber { get; set; }

        public string IssuedOn { get; set; }

        public string DeliveryAddress { get; set; }

        public decimal PackageWeight  { get; set; }

        public string PackageDescription { get; set; }

        public string RecipientUserName { get; set; }

        public decimal Fee { get; set; }
    }
}


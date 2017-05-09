namespace MiniERP_desktop.Models
{
    public class Address
    {
        public string Title { get; set; }
        public string[] AddressLines { get; set; }
        public string VatNumber { get; set; }
        public string CompanyNumber { get; set; }

        public bool HasCompanyNumber => !string.IsNullOrEmpty(CompanyNumber);
        public bool HasVatNumber => !string.IsNullOrEmpty(VatNumber);

        public static Address Make(string title, string[] address, string company = null, string vat = null)
        {
            return new Address
            {
                Title = title,
                AddressLines = address,
                CompanyNumber = company,
                VatNumber = vat,
            };
        }
    }
}

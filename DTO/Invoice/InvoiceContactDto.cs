namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class InvoiceContactDto
    {
        public int PersonId { get; set; }

        public int? CompanyId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Email { get; set; }

        public string CallNumber { get; set; }

        public int EmailContactType { get; set; }

        public string MailAddress { get; set; }
    }
}
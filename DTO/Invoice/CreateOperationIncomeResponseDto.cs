using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice
{
    public class CreateOperationIncomeResponseDto
    {
        public List<InvoiceAddedIncomeDto> AddedIncomes { get; set; }

        public string Error { get; set; }
    }
}
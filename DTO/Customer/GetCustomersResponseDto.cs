using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class GetCustomersResponseDto
    {
        public int QueriedCnt { get; set; }

        public List<CreateCustomerResponseDto> ResultList { get; set; }
    }
}
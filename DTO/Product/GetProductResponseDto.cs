using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product
{
    public class GetProductResponseDto
    {
        public int QueriedCnt { get; set; }

        public List<CreateProductResponseDto> ResultList { get; set; }
    }
}
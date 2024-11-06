using System.Collections.Generic;

namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class GetCustomersRequestDto
    {
        #region Ctor

        public GetCustomersRequestDto()
        {
            AcquaintionTypeIDs = string.Empty;
            InteractionIDs = string.Empty;
            GenderIDs = string.Empty;
            OrderType = true;
            Value = string.Empty;
            Skip = 0;
            FilterCustomFields = new List<string>();
        }

        #endregion

        #region Properties

        public string AcquaintionTypeIDs { get; set; }

        public string InteractionIDs { get; set; }

        public string GenderIDs { get; set; }

        public bool OrderType { get; set; }

        public string Value { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public List<string> FilterCustomFields { get; set; }

        #endregion
    }
}

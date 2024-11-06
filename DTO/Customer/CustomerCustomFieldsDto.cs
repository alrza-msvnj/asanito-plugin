namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer
{
    public class CustomerCustomFieldsDto
    {
        public string NegotiationName { get; set; }

        public string Price { get; set; }

        public int Funnel { get; set; }

        public int Stage { get; set; }

        public int CommunicationType
        {
            get; set;
        }
        public int ProposalType { get; set; }

        public CustomerNegotiationRelationDto NegotiationRelation { get; set; }

        public CustomerNegotiationDetailDto NegotiationDetails { get; set; }

        public int SelectingPartners { get; set; }
    }
}
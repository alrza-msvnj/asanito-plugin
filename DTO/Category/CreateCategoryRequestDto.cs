namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Category
{
    public class CreateCategoryRequestDto
    {
        #region Ctor

        public CreateCategoryRequestDto()
        {
            ParentId = 0;
        }

        #endregion

        #region Properties

        public string Title { get; set; }

        public int ParentId { get; set; }

        public string Description { get; set; }

        public int Cencus { get; set; }

        #endregion
    }
}

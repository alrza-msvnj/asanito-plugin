namespace Tesla.Plugin.Widgets.CRM.Asanito.DTO.Category
{
    public class UpdateCategoryResponseDto
    {
        public CreateCategoryRequestDto SubCats { get; set; }

        public string Description { get; set; }

        public int ParentId { get; set; }

        public string ParentTitle { get; set; }

        public int Census { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }
    }
}
namespace MRSAPI.Models
{
    public class MenuInfoModel
    {
        List<Menu> MenuList { get; set; }
    }

    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public string IsActive { get; set; }
    }
}

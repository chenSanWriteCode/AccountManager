using SqlSugar;

namespace AccountManager.DB.Entity
{
    [SugarTable("Web_RoleRight")]
    public class RoleRight : BaseEntity
    {
        public string RoleId { get; set; }
        public string RightData { get; set; }
    }
}
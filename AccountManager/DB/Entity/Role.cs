using SqlSugar;

namespace AccountManager.DB.Entity
{
    [SugarTable("Web_Role")]
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        [SugarColumn(IsNullable = true)]
        public int RoleLevel { get; set; } = 2;
    }
}
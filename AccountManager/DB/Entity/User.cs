using SqlSugar;
namespace AccountManager.DB.Entity
{
    [SugarTable("Web_User")]
    public class User : BaseEntity
    {
        [SugarColumn(IsNullable = true)]
        public string UserName { get; set; }
        [SugarColumn(IsNullable = true)]
        public string NickName { get; set; }

        [SugarColumn(Length = 11)]
        public string PhoneNum { get; set; }

        public string Password { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? VIPLevel { get; set; } = 1;


    }
}
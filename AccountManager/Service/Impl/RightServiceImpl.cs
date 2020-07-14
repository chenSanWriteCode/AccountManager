using System.Collections.Generic;
using System.Text;
using AccountManager.DB;
using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service.Impl
{
    public class RightServiceImpl : BaseServiceImpl<Right>, IRightService
    {
        public List<Right> GetByTitle(string title, int pageIndex, int pageSize)
        {
            DBContext context = new DBContext();
            return context.RightDB.AsQueryable().Where(x => x.Title.Contains(title)).ToPageList(pageIndex, pageSize);
        }

        public Result<int> UpdateById(string id, Right value)
        {
            StringBuilder sql = new StringBuilder($"update web_right set href='{value.Href}',title='{value.Title}',parent={value.Parent},lastupdateby='{value.LastUpdateBy}' where id='{id}'");
            return base.Update(sql.ToString());

        }
    }
}
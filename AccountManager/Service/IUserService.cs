using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service
{
    public interface IUserService : IBaseService<User>
    {
        User GetByPhoneNum(string phoneNum);
        User GetByPhoneNum(string phoneNum, string password);

        User GetByNamePassw(string name, string password);

        Result<int> Update(string phoneNumOrId, User model);

        Result<int> DeleteByPhoneNum(string phoneNum);
    }
}
using Infrastructure;

namespace AccountManager.Service
{
    public interface IAccountService
    {
        Result<AccessToken> LoginByNickName(string nickName, string password);
        Result<AccessToken> LoginByPhoneNum(string phoneNum, string password);
    }
}
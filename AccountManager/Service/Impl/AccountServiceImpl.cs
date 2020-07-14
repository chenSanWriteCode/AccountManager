using System;
using System.Linq;
using Infrastructure;
using Unity;

namespace AccountManager.Service.Impl
{
    public class AccountServiceImpl : IAccountService
    {
        [Dependency]
        public IUserService UserService { get; set; }
        [Dependency]
        public IUserRoleService URoleService { get; set; }

        public Result<AccessToken> LoginByNickName(string nickName, string password)
        {
            var user = UserService.GetByNamePassw(nickName, password);
            if (user != null)
            {
                var roleList = URoleService.GetByUserId(user.Id);
                var role = roleList.FirstOrDefault();
                return ResultUtil.CreateResult(new AccessToken
                {
                    ExpireTime = TimeUtil.GetTimeStamp(DateTime.UtcNow.AddHours(2)),
                    NickName = nickName,
                    Token = JWTUtil.GenerateToken(user.Id, role?.Id),
                    RoleName = role?.RoleName,
                    UserId = user.Id
                });
            }
            return ResultUtil.CreateResult<AccessToken>(msg: ConstInfo.ERR_Login);
        }
        public Result<AccessToken> LoginByPhoneNum(string phoneNum, string password)
        {
            var user = UserService.GetByPhoneNum(phoneNum, password);
            if (user != null)
            {
                var roleList = URoleService.GetByUserId(user.Id);
                var role = roleList.FirstOrDefault();
                return ResultUtil.CreateResult(new AccessToken
                {
                    ExpireTime = TimeUtil.GetTimeStamp(DateTime.UtcNow.AddHours(2)),
                    NickName = user.NickName,
                    Token = JWTUtil.GenerateToken(user.Id, role?.Id),
                    RoleName = role?.RoleName,
                    UserId = user.Id
                });
            }
            return ResultUtil.CreateResult<AccessToken>(msg: ConstInfo.ERR_Login);
        }
    }
}
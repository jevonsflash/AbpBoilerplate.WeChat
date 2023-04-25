using Abp.Authorization;
using WeChatPaySample.Authorization.Roles;
using WeChatPaySample.Authorization.Users;

namespace WeChatPaySample.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}

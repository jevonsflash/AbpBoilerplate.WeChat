using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using WeChatPaySample.Authorization.Roles;
using Abp.Authorization.Roles;
using Abp.UI;
using System.Linq;

namespace WeChatPaySample.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IRepository<UserLogin, long> userLoginRepository;

        public UserManager(
          RoleManager roleManager,
          UserStore store,
          IOptions<IdentityOptions> optionsAccessor,
          IPasswordHasher<User> passwordHasher,
          IEnumerable<IUserValidator<User>> userValidators,
          IEnumerable<IPasswordValidator<User>> passwordValidators,
          ILookupNormalizer keyNormalizer,
          IdentityErrorDescriber errors,
          IServiceProvider services,
          ILogger<UserManager<User>> logger,
          IPermissionManager permissionManager,
          IUnitOfWorkManager unitOfWorkManager,
          ICacheManager cacheManager,
          IRepository<OrganizationUnit, long> organizationUnitRepository,
          IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
          IOrganizationUnitSettings organizationUnitSettings,
          ISettingManager settingManager, 
          IRepository<UserLogin, long> userLoginRepository)
          : base(
              roleManager,
              store,
              optionsAccessor,
              passwordHasher,
              userValidators,
              passwordValidators,
              keyNormalizer,
              errors,
              services,
              logger,
              permissionManager,
              unitOfWorkManager,
              cacheManager,
              organizationUnitRepository,
              userOrganizationUnitRepository,
              organizationUnitSettings,
              settingManager,
              userLoginRepository)
        {
            this.userLoginRepository = userLoginRepository;
        }

        public string GetOpenIdByUserId(long userId, int? tenantId, string loginProvider)
        {
            var openId = "";

            var query = from userLogin in userLoginRepository.GetAll()
                        join user in Users on userLogin.UserId equals user.Id
                        where user.Id == userId &&
                        userLogin.LoginProvider == loginProvider &&
                        userLogin.TenantId == tenantId
                        select userLogin;

            var currentUserLogin = query.FirstOrDefault();
            if (currentUserLogin != null)
            {
                openId = currentUserLogin.ProviderKey;
            }
            else
            {
                throw new UserFriendlyException("此账号未绑定第三方账号");

            }
            return openId;
        }
    }
}

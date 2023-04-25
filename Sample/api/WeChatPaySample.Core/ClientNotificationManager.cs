using Abp;
using Abp.Domain.Services;
using Abp.UI;
using WeChat.MiniProgram.Services.SubscribeMessage;
using System.Reflection;
using Abp.Domain.Uow;
using System.Threading.Tasks;
using System.Collections.Generic;
using WeChatPaySample.Authorization.Users;
using System;

namespace WeChatPaySample
{
    public class NotificationTemplate
    {
        public string Url { get; set; }

        public Dictionary<string, string> WeChatTemplateKeyMaps { get; set; }

        public string WeChatTemplateId { get; set; }

        public string State { get; set; }
    }


    public class ClientNotificationManager : DomainService
    {
        private readonly UserManager userManager;
        private readonly SubscribeMessageService subscribeMessageService;
        private readonly string loginProvider = "WeChatAuthProvider";

        public ClientNotificationManager(
            UserManager userManager,
            SubscribeMessageService subscribeMessageService)
        {
            this.userManager = userManager;
            this.subscribeMessageService = subscribeMessageService;

        }

        public List<NotificationTemplate> MyProperty => new List<NotificationTemplate>()
            {
                new NotificationTemplate()
                {
                   //todo: set url for web
                    Url="/health/report/index",
                    WeChatTemplateId="gmD2y3vawWKr3xqq82DLl-jwhsRuTbBmu3qDrL8RJB4",
                    WeChatTemplateKeyMaps=new Dictionary<string, string>()
                    {
                        {"Name","thing4" },
                        {"Type","thing1"},
                        {"Status","phrase5"},
                        {"ReportTime","time2"},
                        {"Remark","thing3"},
                    },
                    State="trial",
                },
                new NotificationTemplate()
                {
                    //todo: set url for web
                    Url="/health/alarm/index",
                    WeChatTemplateId="MGQxso0ociz3H4eX862qXpT79JWn3gkW1wo-rVkL_tI",
                    WeChatTemplateKeyMaps=new Dictionary<string, string>()
                    {
                        {"ClientName","thing1" },
                        {"CreationTime","time2"},
                        {"Content","thing3"},
                    },
                    State="trial",
                },

                new NotificationTemplate()
                {
                    Url="/health/healthArchive/nst",
                    WeChatTemplateId="6kJ_jHarhTYABQeWpjOIUfrGLe5MUt1xEnx8KHtJYk0",
                    WeChatTemplateKeyMaps=new Dictionary<string, string>()
                    {
                        {"ClientName","thing11" },
                        {"Category","thing5"},
                        {"CreationTime","time30"},
                    },
                    State="trial",
                }
             };


        [UnitOfWork]
        private async Task SendWeChatSubscribeMessage(object args, NotificationTemplate currentTemplate, UserIdentifier[] userIds)
        {
            string url;
            SubscribeMessageData subscribeMessageData;
            long id;
            GetSubscribeMessageData(args, currentTemplate, out subscribeMessageData, out id);
            url = string.Format(currentTemplate.Url, id);

            foreach (var userId in userIds)
            {
                var sendToOpenId = string.Empty;
                try
                {

         
                    sendToOpenId = userManager.GetOpenIdByUserId(userId.UserId, userId.TenantId, loginProvider);
                    if (string.IsNullOrEmpty(sendToOpenId))
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    //未绑定第三方账号的user，忽略
                    continue;
                }

                var result = await subscribeMessageService.SendAsync(sendToOpenId, currentTemplate.WeChatTemplateId, url, subscribeMessageData, currentTemplate.State);
                if (result == null)
                {
                    continue;
                    //throw new UserFriendlyException("发送小程序订阅消息调用失败");
                }
                else
                {
                    if (result.ErrorCode == 43101 || result.ErrorCode == 40001)
                    {
                        //用户未开启接受订阅通知，忽略
                    }
                    else if (result.ErrorCode != 0)
                    {
                        throw new UserFriendlyException("发送小程序订阅消息调用失败", $"错误代码:{result.ErrorCode}, 错误信息:{result.ErrorMessage}");
                    }

                }
            }
        }

        [UnitOfWork]
        private void GetSubscribeMessageData(object args, NotificationTemplate currentTemplate, out SubscribeMessageData subscribeMessageData, out long id)
        {
            Type t = args.GetType();
            PropertyInfo[] pi = t.GetProperties();
            subscribeMessageData = new SubscribeMessageData();
            id = 0;
            foreach (PropertyInfo p in pi)
            {

                var key = $"${p.Name}$";
                var value = p.GetValue(args, null);
                if (value == null)
                {
                    continue;
                }
                if (p.Name == "Id" && p.PropertyType == typeof(long))
                {
                    id = (long)value;
                    continue;
                }
                var valueStr = "";
                if (p.PropertyType == typeof(DateTime))
                {
                    valueStr = ((DateTime)value).ToString("yyyy年MM月dd日 HH:mm");
                }
                else
                {
                    valueStr = value.ToString();

                }
                string weChatTemplateKey;
                if (currentTemplate.WeChatTemplateKeyMaps.TryGetValue(p.Name, out weChatTemplateKey))
                {
                    if (!string.IsNullOrEmpty(weChatTemplateKey))
                    {
                        subscribeMessageData.Add(weChatTemplateKey, new SubscribeMessageDataItem() { Value = valueStr });

                    }

                }

            }
        }


    }
}

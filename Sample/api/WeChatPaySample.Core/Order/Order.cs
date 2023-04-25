using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using WeChatPaySample.Authorization.Users;

#nullable disable

namespace WeChatPaySample.Order
{
    public class Order : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [Comment("关联用户")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }


        [Comment("订单号")]
        [StringLength(19)]
        public string OrderNumber { get; set; }

        [Comment("交易单号")]
        public string TradeNo { get; set; }

        [Comment("订单状态")]
        public string Status { get; set; }

        [Comment("错误描述")]
        public string ErrorMessage { get; set; }

        [Comment("订单类型")]
        public string Type { get; set; }


        [Comment("订单价格")]
        public decimal? Payment { get; set; }

        [Comment("交易完成时间")]
        public DateTime? EndTime { get; set; }

        [Comment("交易名称")]

        public string Body { get; set; }

        [Comment("商品描述")]

        public string Description { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using WeChatPaySample.Authorization.Users;
using WeChatPaySample.Users.Dto;

#nullable disable

namespace WeChatPaySample.Order
{
    [AutoMapTo(typeof(Order))]
    [AutoMapFrom(typeof(Order))]
    public class OrderDto : FullAuditedEntityDto<long>
    {
        public long UserId { get; set; }
        public UserDto User { get; set; }
        public string OrderNumber { get; set; }
        public string TradeNo { get; set; }
        public string Status { get; set; }

        public string ErrorMessage { get; set; }

        public string Type { get; set; }

        public decimal? Payment { get; set; }

        public DateTime? EndTime { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }

    }
}

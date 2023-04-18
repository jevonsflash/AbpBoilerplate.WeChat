using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WeChatPaySample.Common
{

    public class RefundModel
    {
        public string RefundNumber { get; set; }

        public decimal? Payment { get; set; }

        public decimal? TotalPayment { get; set; }

        public Order.Order Order { get; set; }



        public string Description { get; set; }


    }
}

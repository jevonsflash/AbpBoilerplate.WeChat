<template>
  <div class="content">
    <div class="text-area">
      <text class="title">{{ title }}</text>
    </div>
    <div>
      <div class="main-area">
        <text class="sub-title">使用帮助</text>
      </div>
      <div class="main-area">
        <ul>
          <li>请在电脑浏览器打开:<br />https://www.matoapp.net:3003/</li>
          <li>使用微信扫描Web端的小程序码</li>
          <li>等待微信小程序跳转至授权页面</li>
          <li>点击授权登录按钮，完成登录</li>
        </ul>
        <button v-on:click="handleCreateOrder">点击创建订单</button>
        <ul>
          <li v-for="(item, index) in orders" :key="index" class="order-item">
            {{ item.body }} <br />
            订单号：{{ item.orderNumber }} <br />
            金额：{{ item.payment }} <br />
            描述： {{ item.description }}<br />
            状态 {{ item.status }}<br />
            <button v-on:click="handlePay(item)">点击支付</button>
          </li>
        </ul>
      </div>
      <div class="main-area sub-title">
        <text>详情请查看系列博客⌈使用 Abp.Zero 搭建第三方登录模块⌋</text>
      </div>
      <div class="main-area">
        <text>https://blog.csdn.net/jevonsflash/</text>
      </div>
    </div>
  </div>
</template>

<script lang='ts'>
import { getCancelToken, request } from "../utils/ajaxRequire";
import md5 from "js-md5";
export default {
  data() {
    return {
      title: "微信支付 小程序sample",
      userId: null,
      loading: false,
      prefix: "https://localhost:44311",
      VUE_APP_WECHAT_APPID: "wx9b1b9f9c9c9c9c9c",
      VUE_APP_WECHAT_APIKEY: "wx9b1b9f9c9c9c9c9c",
      orders: [],
      saleOrders: [
        {
          body: "测试商品",
          description: "测试商品描述",
          payment: 0.01,
        },
      ],
    };
  },
  methods: {
    onLoad(option) {
      var token = uni.getStorageSync("token");
      var userId = uni.getStorageSync("userId");
      if (token == "" || userId == "") {
        uni.redirectTo({
          url: "/pages/login/wxLoginIndex",
        });
      }
      this.userId = userId;
      this.getOrders();
    },

    successMessage(value = "执行成功") {
      uni.showToast({
        title: value,
        icon: "success",
        duration: 1.5 * 1000,
      });
    },
    errorMessage(value = "执行错误") {
      uni.showToast({
        title: value,
        icon: "error",
        duration: 1.5 * 1000,
      });
    },

    async getOrders() {
      await request(
        `${this.prefix}/api/services/app/Order/GetAll`,
        "get",
        null
      ).then((re) => {
        this.orders = re.data.result.items;
      });
    },
 

    async handlePay(order) {
      var that = this;
      var prePayModel = {
        type: "订单支付",
        id: order.id,
        userId: this.userId,
        loginProvider: "WeChatAuthProvider",
      };

      await request(
        `${this.prefix}/api/services/app/Pay/PrePay`,
        "post",
        prePayModel
      ).then((re) => {
        var res = re.data.result;
        if (res == null) return;
        const appId: string = this.VUE_APP_WECHAT_APPID;
        const apiKey: string = this.VUE_APP_WECHAT_APIKEY;
        var timeStamp = String(Date.now());
        var paySingType = "MD5";
        var paySignOriginObject = {
          appId: appId,
          nonceStr: res.nonceStr,
          package: "prepay_id=" + res.prepayId,
          signType: paySingType,
          timeStamp: timeStamp,
          key: apiKey,
        };

        var array: Array<string> = new Array<string>();
        for (const key in paySignOriginObject) {
          if (Object.prototype.hasOwnProperty.call(paySignOriginObject, key)) {
            array.push([key, paySignOriginObject[key]].join("="));
          }
        }
        var paySignOriginString = array.join("&");

        var paySign = md5(paySignOriginString);
        var paymentObj: UniApp.RequestPaymentOptions = {
          provider: "wxpay",
          timeStamp: timeStamp,
          nonceStr: res.nonceStr,
          package: "prepay_id=" + res.prepayId,
          signType: paySingType,
          paySign: paySign.toUpperCase(),
          success: async function (c) {
            uni.showLoading({ title: "支付中" });
            //支付成功
            await request(
              `${that.prefix}/api/services/app/Pay/FinishPay`,
              "post",
              prePayModel
            ).then((re) => {
              var res = re.data.result;
              if (res.status == "已付款") {
                that.successMessage("支付成功");
                that.getOrders();
              } else {
                that.successMessage("尚未接到支付成功通知，等待结果");
              }
              uni.hideLoading();
            });
          },
          fail: function (err) {
            console.log("fail:" + JSON.stringify(err));
            that.errorMessage("支付失败");
          },
          orderInfo: "",
        };
        uni.requestPayment(paymentObj);
      });

      // return false;
      return true;
    },

    async handleCreateOrder() {
      this.loading = true;
      var model = this.saleOrders[0];
      (model as any).userId = this.userId;
      await request(
        `${this.prefix}/api/services/app/Order/Create`,
        "post",
        model
      )
        .then((re) => {
          this.successMessage("创建订单成功");
          this.getOrders();
        })
        .catch((c) => {
          var err = c.response?.data?.error?.message;
          if (err != null) {
            this.errorMessage(c.err);
          }
          setTimeout(() => {
            this.loading = false;
          }, 1.5 * 1000);
        });
    },
  },
};
</script>

<style>
.order-item {
  background-color: #efefef;
  padding: 10px;
  margin: 10px 0;
}

.content {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.logo {
  height: 200rpx;
  width: 200rpx;
  margin-top: 200rpx;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 50rpx;
}

.text-area {
  display: flex;
  justify-content: center;
}
.main-area {
  margin: 10px;
}
.sub-title {
  font-weight: bold;
  font-size: 36rpx;
  margin-top: 5px;
  color: #000000;
}
.title {
  font-size: 36rpx;
  color: #8f8f94;
}
</style>

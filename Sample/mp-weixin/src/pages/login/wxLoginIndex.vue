<template>
  <div class="content">
    <image class="logo" src="../../static/logo.png"></image>
    <div class="text-area">
      <text class="title">{{ title }}</text>
    </div>
    <div class="main-area">
      <button
        @click="handleWxLogin"
        :disabled="isInvalid || loading"
        class="sub-btn"
      >
        微信登录
      </button>
      <div><text v-show="loading">正在请求数据，请稍候</text></div>
    </div>
  </div>
</template>

<script lang='ts'>
import { getCancelToken, request } from "../utils/ajaxRequire";
import { URL_PREFIX, VUE_APP_WECHAT_APPID, VUE_APP_WECHAT_APIKEY } from "../page_configs";

export default {
  data() {
    return {
      title: "欢迎使用，请先登录系统",
      loading: false,
      isInvalid: false,

      loginForms: { userNameOrEmailAddress: "admin", password: "123qwe" },
    };
  },

  methods: {
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

    async handleWxLogin() {
      var currentForms = this.loginForms;
      this.loading = true;

      uni.login({
        provider: "weixin",
        success: async (loginRes) => {
          console.log(loginRes.authResult);
          var accessCode = loginRes.code;
          await request(
            `${URL_PREFIX}/api/services/app/MiniProgram/Login`,
            "post",
            {accessCode}
          )
            .then((re) => {
              var token = re.data.result.session_key;
              var userId = re.data.result.openid;
              uni.setStorageSync("token", token);
              uni.setStorageSync("userId", userId);
              uni.redirectTo({
                url: "/pages/index/index",
              });
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
      });
    },
  },
};
</script>

<style>
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

.main-area {
  margin-top: 150px;
  width: 100%;
}
.sub-btn {
  margin: 10px 15px;
  line-height: 50px;
}
.text-area {
  display: flex;
  justify-content: center;
}

.title {
  font-size: 36rpx;
  color: #8f8f94;
}
</style>

<template>
  <div class="content">
    <image class="logo" src="../../static/logo.png"></image>
    <div class="text-area">
      <text class="title">{{ title }}</text>
    </div>
    <div class="main-area">
      <div style="margin-left: auto; margin-right: auto; width: 200rpx">
        <input
          type="text"
          v-model="loginForms.userNameOrEmailAddress"
          placeholder="请输入用户名"
        />
        <input
          password
          type="text"
          v-model="loginForms.password"
          placeholder="请输入密码"
        />
      </div>
      <button
        @click="handleWxLogin"
        :disabled="isInvalid || loading"
        class="sub-btn"
      >
        登录
      </button>
      <div><text v-show="loading">正在请求数据，请稍候</text></div>
    </div>
  </div>
</template>

<script lang='ts'>
import { getCancelToken, request } from "../utils/ajaxRequire";

export default {
  data() {
    return {
      title: "欢迎使用，请先登录系统",
      loading: false,
      isInvalid: false,

      loginForms: { userNameOrEmailAddress: "admin", password: "123qwe" },
      prefix: "https://localhost:44311",
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
      await request(
        `${this.prefix}/api/TokenAuth/Authenticate`,
        "post",
        currentForms
      )
        .then((re) => {
          var token = re.data.result.accessToken;
          var userId = re.data.result.userId;
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

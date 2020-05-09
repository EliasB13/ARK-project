<template>
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-5 col-md-7">
        <div class="card">
          <div class="card-body px-lg-5 py-lg-5">
            <form role="form">
              <fg-input
                :placeholder="$t('loginPage.loginPlaceholder')"
                addon-left-icon="ti-key"
                v-model="loginInput"
              ></fg-input>

              <fg-input
                :placeholder="$t('loginPage.passwordPlaceholder')"
                addon-left-icon="ti-key"
                type="password"
                v-model="passwordInput"
              ></fg-input>

              <!-- <b-form-checkbox>Remember me</b-form-checkbox> -->

              <div class="text-center">
                <b-spinner v-if="showSpinner" class="mr-3 mt-4"></b-spinner>
                <p-button
                  v-else-if="!showSpinner"
                  @click="handleSignIn"
                  type="success"
                  class="mt-4"
                  >{{ $t("loginPage.signInBtn") }}</p-button
                >
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";

export default {
  name: "loginView",
  data() {
    return {
      loginInput: "",
      passwordInput: "",
      privateUserSelected: true,
      businessUserSelected: false,
      isSubmitted: false
    };
  },
  computed: {
    ...mapState({
      account: state => state.account.status,
      alert: state => state.alert
    }),
    isLoginValid() {
      if (this.loginInput === "" && !this.isSubmitted) return null;
      return this.loginInput.length > 4 && this.loginInput.length < 24;
    },
    getLoginError() {
      if (this.loginInput === "") return "";
      if (this.loginInput.length < 5 || this.loginInput.length > 23)
        return "Login must be more than 4 letters and less than 24";
      if (this.loginInput.charAt(0) >= "0" && this.loginInput.charAt(0) <= "9")
        return "Login can't starts with digit";
      if (!/^[a-zA-z0-9]+$/.test(this.loginInput))
        return "Login can contain only latin chars";
      return "";
    },
    isPasswordValid() {
      if (this.passwordInput === "" && !this.isSubmitted) return null;
      if (this.passwordInput.length < 6 || this.passwordInput.length > 23)
        return false;
      return true;
    },
    getPasswordError() {
      if (this.passwordInput === "" && !this.isSubmitted) return "";
      if (this.passwordInput.length < 6 || this.passwordInput.length > 23)
        return "Password must be more than 5 symbols and less than 24";
      return "";
    },
    showSpinner() {
      return this.account.loggingIn;
    }
  },
  created() {
    this.logout();
  },
  methods: {
    ...mapActions("account", ["login", "logout"]),
    ...mapActions("alert", ["clear"]),
    handleSignIn() {
      this.isSubmitted = true;
      const { loginInput, passwordInput } = this;
      if (loginInput && passwordInput) {
        this.login({
          login: loginInput,
          password: passwordInput
        });
      }
    },
    clearStoreAlerts() {
      this.clear();
    },
    businessUserClick() {
      if (this.privateUserSelected) {
        this.privateUserSelected = false;
        this.businessUserSelected = true;
      }
    },
    privateUserClick() {
      if (this.businessUserSelected) {
        this.businessUserSelected = false;
        this.privateUserSelected = true;
      }
    },
    updateProfile() {}
  }
};
</script>
<style></style>

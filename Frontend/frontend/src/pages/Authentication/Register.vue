<template>
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-5 col-md-7">
        <div class="card">
          <div class="card-body px-lg-5 py-lg-5">
            <form role="form">
              <fg-input
                placeholder="Login"
                v-model="loginInput"
                :valid="isLoginValid"
                :error="getLoginError"
              ></fg-input>

              <fg-input
                placeholder="Email"
                type="email"
                v-model="email"
                :valid="isEmailValid"
                :error="getEmailError"
              ></fg-input>
              <fg-input
                placeholder="Company name"
                type="text"
                v-model="companyName"
                :valid="isCompanyNameValid"
                :error="getCompanyNameError"
              ></fg-input>
              <fg-input
                placeholder="Password"
                type="password"
                v-model="password"
                :valid="isPasswordValid"
                :error="getPasswordError"
              ></fg-input>
              <fg-input
                placeholder="Password confirmation"
                type="password"
                v-model="passwordConfirmation"
                :valid="isPasswordConfirmationValid"
                :error="getPasswordConfirmationError"
              ></fg-input>

              <div class="text-center">
                <p-button @click="handleSignUp" type="danger" class="mt-4"
                  >Sign Up</p-button
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
  name: "register",
  data() {
    return {
      loginInput: "",
      email: "",
      password: "",
      companyName: "",
      passwordConfirmation: "",
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
    isCompanyNameValid() {
      if (this.companyName === "" && !this.isSubmitted) return null;
      return /^([^,.;+=()"#@!$%\^&*]*)$/.test(this.companyName);
    },
    isEmailValid() {
      if (this.email === "" && !this.isSubmitted) return null;
      var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return regex.test(String(this.email).toLowerCase());
    },
    isPasswordValid() {
      if (this.password === "" && !this.isSubmitted) return null;
      if (this.password.length < 6 || this.password.length > 23) return false;
      return true;
    },
    isPasswordConfirmationValid() {
      if (this.passwordConfirmation === "" && !this.isSubmitted) return null;
      if (
        this.passwordConfirmation.length < 5 &&
        this.passwordConfirmation.length > 24
      )
        return false;
      if (this.password != this.passwordConfirmation) return false;
      return true;
    },
    getLoginError() {
      if (this.loginInput === "") return "";
      if (this.loginInput.length < 5 || this.loginInput.length > 23)
        return "Wrong login length";
      if (this.loginInput.charAt(0) >= "0" && this.loginInput.charAt(0) <= "9")
        return "Login can starts only with digits";
      if (!/^[a-zA-z0-9]+$/.test(this.loginInput))
        return "Login can contain only latin symbols";
      return "";
    },
    getPasswordError() {
      if (this.password === "") return "";
      if (this.password.length < 6 || this.password.length > 23)
        return "Wrong password length";
      return "";
    },
    getPasswordConfirmationError() {
      if (this.password != this.passwordConfirmation)
        return "Password and password confirmation don't match";
      return "";
    },
    getEmailError() {
      if (this.email === "" && !this.isSubmitted) return null;
      var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      if (!regex.test(String(this.email).toLowerCase()))
        return "Wrong email format";
      return "";
    },
    getCompanyNameError() {
      if (this.firstName === "") return "";
      if (!/^([^,.;+=()"#@!$%\^&*]*)$/.test(this.firstName))
        return "Company name can contain only digits";
      return "";
    },
    showSpinner() {
      return this.account.registering;
    }
  },
  methods: {
    ...mapActions("account", ["register"]),
    ...mapActions("alert", ["clear"]),
    handleSignUp() {
      this.isSubmitted = true;
      const {
        loginInput,
        companyName,
        email,
        password,
        passwordConfirmation
      } = this;
      if (
        loginInput &&
        companyName &&
        email &&
        password &&
        passwordConfirmation
      ) {
        this.register({
          login: loginInput,
          email,
          companyName,
          password,
          passwordConfirmation
        });
      }
    }
  }
};
</script>
<style></style>

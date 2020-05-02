<template>
  <div class="wrapper">
    <div class="main-panel-auth">
      <top-navbar></top-navbar>

      <div class="header py-7 py-lg-8 my-5">
        <div class="container">
          <div class="header-body text-center mb-7">
            <div class="row justify-content-center">
              <div class="col-lg-5 col-md-6">
                <h1>Welcome!</h1>
                <p class="text-lead">Sign in with your account to start work!</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <dashboard-content @click.native="toggleSidebar"></dashboard-content>

      <footer class="footer">
        <div class="row align-items-center justify-content-xl-between px-3">
          <div class="col-xl-6">
            <div class="copyright text-center text-xl-left text-muted">
              &copy; {{ year }}
              <a
                href="https://github.com/EliasB13"
                class="font-weight-bold ml-1"
                target="_blank"
              >EliasB13</a>
            </div>
          </div>
          <div class="col-xl-6">
            <ul class="nav nav-footer justify-content-center justify-content-xl-end">
              <li class="nav-item" @click="changeLang('ua')">
                <div class="nav-link pointer">
                  <img class="thumbnail-locale mr-3" src="@/assets/img/locales/ua.png" />
                  Українська
                </div>
              </li>
              <li class="nav-item" @click="changeLang('en')">
                <div class="nav-link pointer">
                  <img class="thumbnail-locale mr-3" src="@/assets/img/locales/uk.png" />
                  English
                </div>
              </li>
            </ul>
          </div>
        </div>
      </footer>
    </div>
  </div>
</template>
<style lang="scss">
</style>
<script>
import { SlideYUpTransition } from "vue2-transitions";
import { mapState, mapActions } from "vuex";
import TopNavbar from "./AuthTopNavbar.vue";
import ContentFooter from "./ContentFooter.vue";
import DashboardContent from "./Content.vue";
import MobileMenu from "./MobileMenu";

export default {
  name: "auth-layout",
  components: {
    SlideYUpTransition,
    TopNavbar,
    ContentFooter,
    DashboardContent,
    MobileMenu
  },
  data() {
    return {
      year: new Date().getFullYear(),
      showMenu: false
    };
  },
  methods: {
    changeLang(lang) {
      this.$i18n.locale = lang;
    },
    toggleSidebar() {
      if (this.$sidebar.showSidebar) {
        this.$sidebar.displaySidebar(false);
      }
    }
  }
};
</script>

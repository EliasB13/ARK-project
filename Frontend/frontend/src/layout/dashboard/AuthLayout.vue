<template>
  <div class="wrapper">
    <div class="main-panel-auth">
      <top-navbar></top-navbar>

      <div class="header py-7 py-lg-8 my-5">
        <div class="container">
          <div class="header-body text-center mb-7">
            <div class="row justify-content-center">
              <div class="col-lg-5 col-md-6">
                <h1>{{ $t("authLayout.welcomeHeader") }}</h1>
                <p class="text-lead">{{ $t("authLayout.secondaryHeader") }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <dashboard-content @click.native="toggleSidebar"></dashboard-content>

      <content-footer></content-footer>
    </div>
  </div>
</template>
<style lang="scss"></style>
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

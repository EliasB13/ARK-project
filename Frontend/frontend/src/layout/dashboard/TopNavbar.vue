<template>
  <nav class="navbar navbar-expand-lg navbar-light">
    <div class="container-fluid">
      <a class="navbar-brand" href="#">{{ routeName }}</a>
      <button
        class="navbar-toggler navbar-burger"
        type="button"
        @click="toggleSidebar"
        :aria-expanded="$sidebar.showSidebar"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-bar"></span>
        <span class="navbar-toggler-bar"></span>
        <span class="navbar-toggler-bar"></span>
      </button>
      <div class="collapse navbar-collapse">
        <ul class="navbar-nav ml-auto">
          <drop-down
            class="nav-item"
            :title="user.login"
            title-classes="nav-link"
            icon="ti-bell"
          >
            <router-link class="dropdown-item" to="/profile">{{
              $t("layout.userProfile")
            }}</router-link>
            <div class="dropdown-divider"></div>
            <router-link class="dropdown-item" to="/login">{{
              $t("layout.logout")
            }}</router-link>
          </drop-down>
        </ul>
      </div>
    </div>
  </nav>
</template>
<script>
import { mapState, mapActions } from "vuex";

export default {
  data() {
    return {
      activeNotifications: false
    };
  },
  computed: {
    ...mapState({
      user: state => state.account.user
    }),
    routeName() {
      const { name } = this.$route;
      return this.capitalizeFirstLetter(name);
    }
  },
  methods: {
    ...mapActions("account", ["getAccountData"]),
    toggleSidebar() {
      if (this.$sidebar.showSidebar) {
        this.$sidebar.displaySidebar(false);
      }
    },
    capitalizeFirstLetter(string) {
      return string.charAt(0).toUpperCase() + string.slice(1);
    },
    toggleNotificationDropDown() {
      this.activeNotifications = !this.activeNotifications;
    },
    closeDropDown() {
      this.activeNotifications = false;
    },
    toggleSidebar() {
      this.$sidebar.displaySidebar(!this.$sidebar.showSidebar);
    },
    hideSidebar() {
      this.$sidebar.displaySidebar(false);
    }
  }
};
</script>
<style></style>

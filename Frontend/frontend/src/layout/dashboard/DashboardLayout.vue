<template>
  <div class="wrapper">
    <side-bar active-color="danger">
      <template slot="links">
        <sidebar-link to="/dashboard" name="Dashboard" icon="ti-stats-up" />
        <sidebar-link to="/cards" name="Person cards" icon="ti-credit-card" />
        <sidebar-link to="/roles" name="Roles" icon="ti-id-badge" />
        <sidebar-link to="/readers" name="Readers" icon="ti-rss-alt" />
        <sidebar-link to="/profile" name="Profile" icon="ti-user" />
      </template>
      <mobile-menu>
        <drop-down class="nav-item" title="Elias corp" title-classes="nav-link" icon="ti-bell">
          <a class="dropdown-item">User profile</a>

          <div class="dropdown-divider"></div>
          <a class="dropdown-item">Logout</a>
        </drop-down>
        <li class="divider"></li>
      </mobile-menu>
    </side-bar>
    <div class="main-panel">
      <top-navbar></top-navbar>

      <dashboard-content @click.native="toggleSidebar"></dashboard-content>

      <content-footer></content-footer>
    </div>
  </div>
</template>
<style lang="scss">
</style>
<script>
import TopNavbar from "./TopNavbar.vue";
import ContentFooter from "./ContentFooter.vue";
import DashboardContent from "./Content.vue";
import MobileMenu from "./MobileMenu";
export default {
  components: {
    TopNavbar,
    ContentFooter,
    DashboardContent,
    MobileMenu
  },
  methods: {
    toggleSidebar() {
      if (this.$sidebar.showSidebar) {
        this.$sidebar.displaySidebar(false);
      }
    }
  }
};
</script>

<template>
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
</template>
<script>
export default {
  data() {
    return {
      year: new Date().getFullYear(),
      showMenu: false
    };
  }
};
</script>
<style>
</style>

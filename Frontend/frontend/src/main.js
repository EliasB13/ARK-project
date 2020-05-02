import Vue from "vue";
import App from "./App";
import router from "./router/index";
import { store } from "./store";
import { BootstrapVue, IconsPlugin } from "bootstrap-vue";
import "bootstrap-vue/dist/bootstrap-vue.css";

import PaperDashboard from "./plugins/paperDashboard";
//import "vue-notifyjs/themes/default.css";

Vue.use(PaperDashboard);
Vue.use(BootstrapVue);

/* eslint-disable no-new */
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");

import Vue from "vue";
import Vuex from "vuex";

import { alertModule } from "./modules/alert.module";
import { account } from "./modules/account.module";

Vue.use(Vuex);

export const store = new Vuex.Store({
  modules: {
    alert: alertModule,
    account
  }
});

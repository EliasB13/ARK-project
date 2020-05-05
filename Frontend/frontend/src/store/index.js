import Vue from "vue";
import Vuex from "vuex";

import { alertModule } from "./modules/alert.module";
import { account } from "./modules/account.module";
import { cards } from "./modules/cards.module";
import { roles } from "./modules/roles.module";
import { readers } from "./modules/readers.module";

Vue.use(Vuex);

export const store = new Vuex.Store({
  modules: {
    alert: alertModule,
    account,
    cards,
    roles,
    readers
  }
});

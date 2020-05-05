import Vue from "vue";
import VueRouter from "vue-router";
import routes from "./routes";
Vue.use(VueRouter);

const router = new VueRouter({
  routes,
  mode: "history",
  linkActiveClass: "active"
});

router.beforeEach((to, from, next) => {
  const publicPages = ["/login", "/register"];
  const authRequired = !publicPages.includes(to.path);
  const userJson = localStorage.getItem("user");

  if (authRequired && !userJson) {
    return next("/login");
  }

  next();
});

export default router;

import DashboardLayout from "@/layout/dashboard/DashboardLayout.vue";
import NotFound from "@/pages/NotFoundPage.vue";

import Dashboard from "@/pages/Dashboard.vue";
import UserProfile from "@/pages/UserProfile.vue";
import Login from "@/pages/Authentication/Login.vue";
import Register from "@/pages/Authentication/Register.vue";
import AuthLayout from "@/layout/dashboard/AuthLayout.vue";
import Cards from "@/pages/Cards.vue";
import Readers from "@/pages/Readers.vue";
import Roles from "@/pages/Roles.vue";
import Role from "@/pages/Role.vue";
import ReadersStat from "@/pages/ReadersStatistic.vue";
import PersonsStat from "@/pages/PersonsStatistic.vue";
import ReaderStat from "@/pages/ReaderStatistic.vue";
import PersonStat from "@/pages/PersonStatistic.vue";

const routes = [
  {
    path: "/",
    component: DashboardLayout,
    redirect: "/dashboard",
    children: [
      {
        path: "dashboard",
        name: "dashboard",
        component: Dashboard
      },
      {
        path: "cards",
        name: "cards",
        component: Cards
      },
      {
        path: "roles",
        name: "roles",
        component: Roles
      },
      {
        path: "/role/:roleId",
        name: "role",
        component: Role,
        props: true
      },
      {
        path: "readers",
        name: "readers",
        component: Readers
      },
      {
        path: "profile",
        name: "profile",
        component: UserProfile
      },
      {
        path: "readersStat",
        name: "readers-statistic",
        component: ReadersStat
      },
      {
        path: "/readerStat/:readerId",
        name: "reader-statistic",
        component: ReaderStat,
        props: true
      },
      {
        path: "personsStat",
        name: "persons-statistic",
        component: PersonsStat
      },
      {
        path: "/personStat/:cardId",
        name: "person-statistic",
        component: PersonStat,
        props: true
      }
    ]
  },
  {
    path: "/",
    redirect: "login",
    component: AuthLayout,
    children: [
      {
        path: "/login",
        name: "login",
        component: Login
      },
      {
        path: "/register",
        name: "register",
        component: Register
      }
    ]
  },
  { path: "*", component: NotFound }
];

/**
 * Asynchronously load view (Webpack Lazy loading compatible)
 * The specified component must be inside the Views folder
 * @param  {string} name  the filename (basename) of the view to load.
function view(name) {
   var res= require('../components/Dashboard/Views/' + name + '.vue');
   return res;
};**/

export default routes;

import DashboardLayout from "@/layout/dashboard/DashboardLayout.vue";
// GeneralViews
import NotFound from "@/pages/NotFoundPage.vue";

// Admin pages
import Dashboard from "@/pages/Dashboard.vue";
import UserProfile from "@/pages/UserProfile.vue";
import Notifications from "@/pages/Notifications.vue";
import Icons from "@/pages/Icons.vue";
import Maps from "@/pages/Maps.vue";
import Typography from "@/pages/Typography.vue";
import TableList from "@/pages/TableList.vue";
import Login from "@/pages/Authentication/Login.vue";
import Register from "@/pages/Authentication/Register.vue";
import AuthLayout from "@/layout/dashboard/AuthLayout.vue";
import Cards from "@/pages/Cards.vue";
import Readers from "@/pages/Readers.vue";
import Roles from "@/pages/Roles.vue";
import Role from "@/pages/Role.vue";

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
        path: "readers",
        name: "readers",
        component: Readers
      },
      {
        path: "maps",
        name: "maps",
        component: Maps
      },
      {
        path: "profile",
        name: "profile",
        component: UserProfile
      },
      {
        path: "table-list",
        name: "table-list",
        component: TableList
      },
      {
        path: "/role/:roleId",
        name: "role",
        component: Role,
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

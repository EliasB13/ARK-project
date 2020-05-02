import { userService } from "../../services";
import Router from "../../router/index.js";
//import i18n from "../../localization/i18n";

const user = JSON.parse(localStorage.getItem("user"));
const state = user
  ? { status: { loggedIn: true }, user, userToUpdate: {}, publicProfile: {} }
  : { status: {}, user: null, userToUpdate: {}, publicProfile: {} };

const actions = {
  login({ dispatch, commit }, { login, password }) {
    commit("loginRequest", { login });
    userService.login(login, password).then(
      user => {
        commit("loginSuccess", user);
        Router.push("/dashboard");
      },
      error => {
        commit("loginFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  logout({ commit }) {
    userService.logout();
    commit("logout");
  },
  register({ dispatch, commit }, user) {
    commit("registerRequest", user);

    userService.register(user).then(
      user => {
        commit("registerSuccess", user);
        Router.push("/login");
        setTimeout(() => {
          dispatch("alert/success", "Register success".toString(), {
            root: true
          });
        });
      },
      error => {
        commit("registerFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  getAccountData({ commit, dispatch }) {
    commit("getAccountDataRequest");

    userService.getAccountData().then(
      accountData => {
        commit("getAccountDataSuccess", accountData);
      },
      error => {
        commit("getAccountDataFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },

  updateUser({ commit, dispatch }, user) {
    commit("updateUserRequest", user);

    userService.update(user).then(
      updatedUser => {
        commit("updateUserSuccess", updatedUser);
        dispatch("alert/success", "Update user success", {
          root: true
        });
      },
      error => {
        console.log(error);
        commit("updateUserFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  }
};

const mutations = {
  loginRequest(state, user) {
    state.status = { loggingIn: true };
    state.user = user;
  },
  loginSuccess(state, user) {
    state.status = { loggedIn: true };
    state.user = user;
  },
  loginFailure(state, error) {
    state.status = {};
    state.user = null;
    state.error = error;
  },

  logout(state) {
    state.status = {};
    state.user = null;
  },

  registerRequest(state, user) {
    state.status = { registering: true };
  },
  registerSuccess(state, user) {
    state.status = {};
  },
  registerFailure(state, error) {
    state.status = {};
    state.error = error;
  },

  getAccountDataRequest(state, isBusinessUser) {
    state.status = { ...state.status, accountDataLoading: true };
  },
  getAccountDataSuccess(state, accountData) {
    state.status = {
      ...state.status,
      accountDataLoading: false,
      accountDataLoaded: false
    };
    state.user = { ...state.user, ...accountData };
  },
  getAccountDataFailure(state, error) {
    state.status = {};
    state.error = error;
  },

  getPublicProfileRequest(state) {
    state.status = { ...state.status, publicProfileLoading: true };
  },
  getPublicProfileSuccess(state, profile) {
    (state.status = {
      ...state.status,
      publicProfileLoading: false,
      publicProfileLoaded: true
    }),
      (state.publicProfile = profile);
  },
  getPublicProfileFailure(state, error) {
    (state.status = {
      ...state.status,
      publicProfileLoading: false,
      publicProfileLoaded: false
    }),
      (state.publicProfile = {});
    state.error = error;
  },

  updateUserRequest(state, user) {
    state.status = { ...state.status, userUpdating: true };
    state.userToUpdate = user;
  },
  updateUserSuccess(state, updatedUser) {
    state.status = { ...state.status, userUpdating: false, userUpdated: true };
    state.user = { ...state.user, ...updatedUser };
  },
  updateUserFailure(state, error) {
    state.status = { ...state.status, userUpdating: false, userUpdated: false };
    state.userToUpdate = {};
    state.error = error;
  },

  orderCardRequest(state) {
    state.status = { ...state.status, orderingCard: true };
  },
  orderCardSuccess(state, rfid) {
    state.status = { ...state.status, orderingCard: false, orderedCard: true };
    state.user = { ...state.user, rfid: rfid };
  },
  orderCardFailure(state, error) {
    state.status = { ...state.status, orderingCard: false, orderedCard: false };
    state.error = error;
  }
};

export const account = {
  namespaced: true,
  state,
  actions,
  mutations
};

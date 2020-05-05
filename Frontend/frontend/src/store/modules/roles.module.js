import { rolesService } from "../../services";

const state = {
  roles: [],
  status: {},
  error: null,
  cardToAdd: null
};

const actions = {
  getRoles({ commit, dispatch }) {
    commit("getRolesRequest");

    rolesService.getRoles().then(
      roles => commit("getRolesSuccess", roles),
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("getRolesFailure", error);
      }
    );
  }
};

const mutations = {
  getRolesRequest(state) {
    state.status = { cardsLoading: true };
  },
  getRolesSuccess(state, roles) {
    state.status = { cardsLoaded: true };
    state.roles = roles;
  },
  getRolesFailure(state, error) {
    state.status = {};
    state.roles = [];
    state.error = error;
  }
};

export const roles = {
  namespaced: true,
  state,
  actions,
  mutations
};

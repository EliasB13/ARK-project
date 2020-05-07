import { rolesService } from "../../services";

const state = {
  role: [],
  status: {},
  error: null,
  cardToAdd: null
};

const actions = {
  getRoleById({ commit, dispatch }, id) {},
  getRoleEmployees({ commit, dispatch }, id) {},
  addCardToRole({ commit, dispatch }, dto) {},
  restrictReader({ commit, dispatch }, dto) {},
  unrestrictReader({ commit, dispatch }, dto) {}
};

const mutations = {
  // getRoleById
  getRoleByIdRequest(state) {
    state.status = { roleLoading: true };
  },
  getRoleByIdSuccess(state, role) {
    state.state = { roleLoading: true };
    state.role = role;
  },
  getRoleByIdFailure(state, error) {
    state.status = {};
    state.role = [];
    state.error = error;
  }
  // getRoleEmployees
  // addCardToRole
  // restrictReader
  // unrestrictReader
};

export const role = {
  namespaced: true,
  state,
  actions,
  mutations
};

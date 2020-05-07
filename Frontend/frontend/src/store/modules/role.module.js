import { rolesService } from "../../services";

const state = {
  role: {},
  status: {},
  error: null,
  cardToAdd: null,
  readerToRestrict: null,
  employees: [],
  readerToUnrestrict: null,
  restrictedReaders: []
};

const actions = {
  getRoleById({ commit, dispatch }, id) {
    commit("getRoleByIdRequest");

    rolesService.getRoleById(id).then(
      role => commit("getRoleByIdSuccess", role),
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("getRoleByIdFailure", error);
      }
    );
  },
  getRoleEmployees({ commit, dispatch }, id) {
    commit("getRoleEmployeesRequest");

    rolesService.getRoleEmployees(id).then(
      employees => commit("getRoleEmployeesSuccess", employees),
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("getRoleEmployeesFailure", error);
      }
    );
  },
  getRoleRestrictedReaders({ commit, dispatch }, id) {
    commit("getRoleRestrictedReadersRequest");

    rolesService.getRoleRestrictedReaders(id).then(
      readers => commit("getRoleRestrictedReadersSuccess", readers),
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("getRoleRestrictedReadersFailure", error);
      }
    );
  },
  addCardToRole({ commit, dispatch }, dto) {
    commit("addCardToRoleRequest", dto);
    debugger;
    rolesService.addCardToRole(dto.roleId, dto.cardId).then(
      () => {
        commit("addCardToRoleSuccess");
        dispatch("alert/success", "Card was successfully added to role", {
          root: true
        });
        dispatch("getRoleEmployees", dto.roleId);
      },
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("addCardToRoleFailure", error);
      }
    );
  },
  removeCardFromRole({ commit, dispatch }, dto) {
    commit("removeCardFromRoleRequest", dto);

    rolesService.removeCardFromRole(dto.roleId, dto.cardId).then(
      () => {
        commit("removeCardFromRoleSuccess", dto.cardId);
        dispatch("alert/success", "Card was successfully removed from role", {
          root: true
        });
      },
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("removeCardFromRoleFailure", error);
      }
    );
  },
  restrictReader({ commit, dispatch }, dto) {
    commit("restrictRequest", dto);

    rolesService.restrictReader(dto.readerId, dto.roleId).then(
      () => {
        commit("restrictSuccess");
        dispatch("getRoleRestrictedReaders", dto.roleId);
        dispatch("alert/success", "Readers was sucessfully restricted", {
          root: true
        });
      },
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("restrictFailure", error);
      }
    );
  },
  unrestrictReader({ commit, dispatch }, dto) {
    commit("unrestrictRequest", dto);

    rolesService.unrestrictReader(dto.readerId, dto.roleId).then(
      () => {
        commit("unrestrictSuccess", dto.readerId);
        dispatch("alert/success", "Readers was sucessfully restricted", {
          root: true
        });
      },
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("unrestrictFailure", error);
      }
    );
  }
};

const mutations = {
  // getRoleById
  getRoleByIdRequest(state) {
    state.status = { roleLoading: true };
  },
  getRoleByIdSuccess(state, role) {
    state.status = { roleLoaded: true };
    state.role = role;
  },
  getRoleByIdFailure(state, error) {
    state.status = {};
    state.role = {};
    state.error = error;
  },

  // getRoleEmployees
  getRoleEmployeesRequest(state) {
    state.status = { employeesLoading: true };
  },
  getRoleEmployeesSuccess(state, employees) {
    state.status = { employeesLoaded: true };
    state.employees = employees;
  },
  getRoleEmployeesFailure(state, error) {
    state.status = {};
    state.employees = [];
    state.error = error;
  },

  // getRoleRestrictedReadersRequest
  getRoleRestrictedReadersRequest(state) {
    state.status = { restrictedReadersLoading: true };
  },
  getRoleRestrictedReadersSuccess(state, readers) {
    state.status = { restrictedReadersLoaded: true };
    state.restrictedReaders = readers;
  },
  getRoleRestrictedReadersFailure(state, error) {
    state.status = {};
    state.restrictedReaders = [];
    state.error = error;
  },

  // addCardToRole
  addCardToRoleRequest(state, cardToAdd) {
    state.status = { cardAdding: true };
    state.cardToAdd = cardToAdd;
  },
  addCardToRoleSuccess(state) {
    state.status = { cardAdded: true };
  },
  addCardToRoleFailure(state, error) {
    state.status = {};
    state.cardToAdd = {};
    state.error = error;
  },

  // restrictReader
  restrictRequest(state, readerToRestrict) {
    state.status = { readerRestricting: true };
    state.readerToRestrict = readerToRestrict;
  },
  restrictSuccess(state) {
    state.status = { readerRestricted: true };
  },
  restrictFailure(state, error) {
    state.status = {};
    state.readerToRestrict = {};
    state.error = error;
  },

  // unrestrictReader
  unrestrictRequest(state, readerToUnrestrict) {
    state.status = { readerUnrestricting: true };
    state.readerToUnrestrict = readerToUnrestrict;
  },
  unrestrictSuccess(state, id) {
    state.status = { readerUnrestricted: true };
    state.restrictedReaders = state.restrictedReaders.filter(r => r.id !== id);
  },
  unrestrictFailure(state, error) {
    state.status = {};
    state.readerToUnrestrict = {};
    state.error = error;
  },

  // removeCardFromRole
  removeCardFromRoleRequest(state, id) {
    state.status = { cardRemoving: true };
  },
  removeCardFromRoleSuccess(state, id) {
    state.status = { cardRemoved: true };
    state.employees = state.employees.filter(e => e.personCardId !== id);
  },
  removeCardFromRoleFailure(state, error) {
    state.status = {};
    state.error = error;
  }
};

export const role = {
  namespaced: true,
  state,
  actions,
  mutations
};

import { rolesService } from "../../services";

const state = {
  roles: [],
  status: {},
  error: null,
  roleToAdd: null
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
  },
  addRole({ commit, dispatch }, role) {
    commit("addRoleRequest", role);

    rolesService.addRole(role).then(
      () => {
        commit("addRoleSuccess", role);
        dispatch("alert/success", "Role was successfully added", {
          root: true
        });
        dispatch("getRoles");
      },
      error => {
        commit("addRoleFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  deleteRole({ commit, dispatch }, id) {
    commit("deleteRoleRequest", id);

    rolesService.deleteRole(id).then(
      () => {
        commit("deleteRoleSuccess", id);
        dispatch("alert/success", "Role was successfully removed", {
          root: true
        });
      },
      error => {
        commit("deleteRoleFailure", { id, error: error.toString() });
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  }
};

const mutations = {
  getRolesRequest(state) {
    state.status = { rolesLoading: true };
  },
  getRolesSuccess(state, roles) {
    state.status = { rolesLoaded: true };
    state.roles = roles;
  },
  getRolesFailure(state, error) {
    state.status = {};
    state.roles = [];
    state.error = error;
  },

  // addRole
  addRoleRequest(state, role) {
    state.roleToAdd = role;
    state.status = { ...state.status, roleAdding: true };
  },
  addRoleSuccess(state, role) {
    state.status = { ...state.status, roleAdded: true, roleAdding: false };
    state.roles.push(role);
  },
  addRoleFailure(state, error) {
    state.status = { ...state.status, roleAdded: false, roleAdding: false };
    state.error = error;
  },

  // deleteRole
  deleteRoleRequest(state, id) {
    debugger;
    state.status = { ...state.status, roleRemoving: true };
    state.roles = state.roles.map(role =>
      role.id === id ? { ...role, roleRemoving: true } : role
    );
    debugger;
  },
  deleteRoleSuccess(state, id) {
    state.status = { ...state.status, roleRemoved: true, roleRemoving: false };
    state.roles = state.roles.filter(role => role.id !== id);
  },
  deleteRoleFailure(state, { id, error }) {
    state.status = { ...state.status, roleRemoved: false, roleRemoving: false };
    state.roles = state.roles.map(role => {
      if (role.id === id) {
        const { roleRemoving, ...roleCopy } = role;
        return { ...roleCopy, deleteError: error };
      }

      return role;
    });
  }
};

export const roles = {
  namespaced: true,
  state,
  actions,
  mutations
};

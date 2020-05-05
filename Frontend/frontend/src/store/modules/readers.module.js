import { readersService } from "../../services";

const state = {
  readers: [],
  status: {},
  error: null,
  itemToAdd: null
};

const actions = {
  getReaders({ commit, dispatch }) {
    commit("getReadersRequest");

    readersService.getReaders().then(
      readers => commit("getReadersSuccess", readers),
      error => {
        commit("getReadersFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  addReader({ commit, dispatch }, item) {
    commit("addReaderRequest", item);

    readersService.addReader(item).then(
      reader => {
        commit("addReaderSuccess", reader);
        dispatch("alert/success", "Reader sucessfully added", {
          root: true
        });
        dispatch("getReaders");
      },
      error => {
        commit("addReaderFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  deleteReader({ commit, dispatch }, id) {
    commit("removeReaderRequest", id);

    readersService.removeReader(id).then(
      () => {
        commit("removeReaderSuccess", id);
        dispatch("alert/success", "Reader was successfuly removed", {
          root: true
        });
        //dispatch("getReaders");
      },
      error => {
        commit("removeReaderFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  }
};

const mutations = {
  getReadersRequest(state) {
    state.status = { readersLoading: true };
  },
  getReadersSuccess(state, readers) {
    state.status = { readersLoading: false, readersLoaded: true };
    state.readers = readers;
  },
  getReadersFailure(state, error) {
    state.status = {};
    state.readers = [];
    state.error = error;
  },

  addReaderRequest(state) {
    state.status = { ...state.status, readerAdding: true };
  },
  addReaderSuccess(state, reader) {
    state.status = {
      ...state.status,
      readerAdding: false,
      readerAdded: true
    };
    state.readers.push(reader);
  },
  addReaderFailure(state, error) {
    state.status = {
      ...state.status,
      readerAdding: false,
      readerAdded: false
    };
    state.error = error;
  },

  removeReaderRequest(state, id) {
    state.status = {
      ...state.status,
      readerRemoving: true
    };
    state.readers = state.readers.map(r =>
      r.id === id ? { ...r, readerRemoving: true } : r
    );
  },
  removeReaderSuccess(state, id) {
    state.status = {
      ...state.status,
      readerRemoved: true,
      readerRemoving: false
    };
    state.readers = state.readers.filter(r => r.id !== id);
  },
  removeReaderFailure(state, { id, error }) {
    state.status = {
      ...state.status,
      readerRemoved: false,
      readerRemoving: false
    };
    state.readers = state.readers.map(r => {
      if (r.id === id) {
        const { readerRemoving, ...readerCopy } = r;
        return { ...readerCopy, deleteError: error };
      }

      return r;
    });
  }
};

export const readers = {
  namespaced: true,
  state,
  actions,
  mutations
};

import { cardsService } from "../../services";

const state = {
  cards: [],
  status: {},
  error: null,
  cardToAdd: null,
  statistic: {}
};

const actions = {
  getCards({ commit, dispatch }) {
    commit("getCardsRequest");

    cardsService.getCards().then(
      cards => commit("getCardsSuccess", cards),
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("getCardsFailure", error);
      }
    );
  },
  getCardStatistic({ commit, dispatch }, id) {
    commit("getCardsStatisticRequest");

    cardsService.getCardStatistic(id).then(
      cards => commit("getCardsStatisticSuccess", cards),
      error => {
        dispatch("alert/error", error.toString(), { root: true });
        commit("getCardsStatisticFailure", error);
      }
    );
  },
  addCard({ commit, dispatch }, card) {
    commit("addCardRequest", card);

    cardsService.addCard(card).then(
      () => {
        commit("addCardSuccess", card);
        dispatch("alert/success", "Card was successfully added", {
          root: true
        });
        dispatch("getCards");
      },
      error => {
        commit("addCardFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  deleteCard({ commit, dispatch }, id) {
    commit("deleteCardRequest", id);

    cardsService.deleteCard(id).then(
      () => {
        commit("deleteCardSuccess", id);
        dispatch("alert/success", "Card was successfully removed", {
          root: true
        });
        //dispatch("getCards");
      },
      error => {
        commit("deleteCardFailure", { id, error: error.toString() });
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  }
};

const mutations = {
  getCardsRequest(state) {
    state.status = { cardsLoading: true };
  },
  getCardsSuccess(state, cards) {
    state.status = { cardsLoaded: true };
    state.cards = cards;
  },
  getCardsFailure(state, error) {
    state.status = {};
    state.cards = [];
    state.error = error;
  },

  getCardStatisticRequest(state) {
    state.status = { cardStatisticLoading: true };
  },
  getCardStatisticSuccess(state, statistic) {
    state.status = { cardStatisticLoaded: true };
    state.statistic = statistic;
  },
  getCardStatisticFailure(state, error) {
    state.status = {};
    state.statistic = [];
    state.error = error;
  },

  addCardRequest(state, card) {
    state.cardToAdd = card;
    state.status = { ...state.status, cardAdding: true };
  },
  addCardSuccess(state, card) {
    state.status = { ...state.status, cardAdded: true, cardAdding: false };
    state.cards.push(card);
  },
  addCardFailure(state, error) {
    state.status = { ...state.status, cardAdded: false, cardAdding: false };
    state.error = error;
  },

  deleteCardRequest(state, id) {
    state.status = { ...state.status, cardRemoving: true };
    state.cards = state.cards.map(card =>
      card.id === id ? { ...card, cardRemoving: true } : card
    );
  },
  deleteCardSuccess(state, id) {
    state.status = { ...state.status, cardRemoved: true, cardRemoving: false };
    state.cards = state.cards.filter(card => card.personCardId !== id);
  },
  deleteCardFailure(state, { id, error }) {
    state.status = { ...state.status, cardRemoved: false, cardRemoving: false };
    state.cards = state.cards.map(card => {
      if (card.personCardId === id) {
        const { cardRemoving, ...cardCopy } = card;
        return { ...cardCopy, deleteError: error };
      }

      return card;
    });
  }
};

export const cards = {
  namespaced: true,
  state,
  actions,
  mutations
};

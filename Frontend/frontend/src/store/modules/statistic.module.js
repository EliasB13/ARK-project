import { statisticService } from "../../services";

const state = {
  fullStat: [],
  readerStat: {
    observations: []
  },
  fullCountStat: [],
  readerCountStat: {},
  personStat: [],
  error: null,
  status: {}
};

const actions = {
  getFullStat({ commit, dispatch }, { lowerBound, upperBound }) {
    commit("getFullStatRequest");

    statisticService.getFullStats(lowerBound, upperBound).then(
      readers => commit("getFullStatSuccess", readers),
      error => {
        commit("getFullStatFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  getFullCountStat({ commit, dispatch }, { lowerBound, upperBound }) {
    commit("getFullCountStatRequest");

    statisticService.getFullCountStats(lowerBound, upperBound).then(
      count => commit("getFullCountStatSuccess", count),
      error => {
        commit("getFullCountStatFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  getReaderStat({ commit, dispatch }, { readerId, lowerBound, upperBound }) {
    commit("getReaderStatRequest");

    statisticService.getReaderStats(readerId, lowerBound, upperBound).then(
      stat => commit("getReaderStatSuccess", stat),
      error => {
        commit("getReaderStatFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  getReaderCountStat(
    { commit, dispatch },
    { readerId, lowerBound, upperBound }
  ) {
    commit("getReaderCountRequest");

    statisticService.getReaderCountStats(readerId, lowerBound, upperBound).then(
      count => commit("getReaderCountSuccess", count),
      error => {
        commit("getReaderCountFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  },
  getPersonStat({ commit, dispatch }, { cardId, lowerBound, upperBound }) {
    commit("getPersonStatRequest");

    statisticService.getPersonStats(cardId, lowerBound, upperBound).then(
      stat => commit("getPersonStatSuccess", stat),
      error => {
        commit("getPersonStatFailure", error);
        dispatch("alert/error", error.toString(), { root: true });
      }
    );
  }
};

const mutations = {
  getFullStatRequest(state) {
    state.status = { fullStatLoading: true };
  },
  getFullStatSuccess(state, fullStat) {
    state.status = { fullStatLoaded: true };
    state.fullStat = fullStat;
  },
  getFullStatFailure(state, error) {
    state.status = {};
    state.fullStat = [];
    state.error = error;
  },

  getReaderStatRequest(state) {
    state.status = { readerStatLoading: true };
  },
  getReaderStatSuccess(state, readerStat) {
    state.status = { readerStatLoaded: true };
    state.readerStat = readerStat;
  },
  getReaderStatFailure(state, error) {
    state.status = {};
    state.readerStat = {};
    state.error = error;
  },

  getFullCountStatRequest(state) {
    state.status = { fullCountStatLoading: true };
  },
  getFullCountStatSuccess(state, count) {
    state.status = { fullCountStatLoaded: true };
    state.fullCountStat = count;
  },
  getFullCountStatFailure(state, error) {
    state.status = {};
    state.fullCountStat = {};
    state.error = error;
  },

  getReaderCountStatRequest(state) {
    state.status = { readerCountStatLoading: true };
  },
  getReaderCountStatSuccess(state, count) {
    state.status = { readerCountStatLoaded: true };
    state.readerCountStat = count;
  },
  getReaderCountStatFailure(state, error) {
    state.status = {};
    state.readerCountStat = {};
    state.error = error;
  },

  getPersonStatRequest(state) {
    state.status = { personStatLoading: true };
  },
  getPersonStatSuccess(state, readers) {
    state.status = { personStatLoaded: true };
    state.personStat = readers;
  },
  getPersonStatFailure(state, error) {
    state.status = {};
    state.personStat = [];
    state.error = error;
  }
};

const getters = {
  mappedStatsCards: state => {
    return state.fullCountStat
      .sort(
        (a, b) =>
          b.employeesObservationsCount +
          b.anonymObservationsCount -
          (a.employeesObservationsCount + a.anonymObservationsCount)
      )
      .slice(0, 4)
      .map((r, idx) => {
        let colors = ["warning", "success", "danger", "info"];
        return {
          type: colors[idx],
          icon: "ti-signal",
          title: r.name,
          value: r.employeesObservationsCount + r.anonymObservationsCount,
          footerText: "In last month",
          footerIcon: "ti-calendar"
        };
      });
  },
  prefChartData: state => {
    if (state.fullCountStat.length > 0) {
      let emplCount = state.fullCountStat.reduce(
        (acc, val) => acc + parseInt(val.employeesObservationsCount),
        0
      );
      let anonCount = state.fullCountStat.reduce(
        (acc, val) => acc + parseInt(val.anonymObservationsCount),
        0
      );

      let emplPercent = (emplCount / (emplCount + anonCount)) * 100;
      let anonPercent = (anonCount / (emplCount + anonCount)) * 100;

      let roundedEmplPercent =
        Math.round((emplPercent + Number.EPSILON) * 100) / 100;
      let roundedAnonPercent =
        Math.round((anonPercent + Number.EPSILON) * 100) / 100;
      return {
        data: {
          labels: [`${roundedEmplPercent}%`, `${roundedAnonPercent}%`],
          series: [roundedEmplPercent, roundedAnonPercent]
        },
        options: {}
      };
    }
  }
};

export const statistic = {
  namespaced: true,
  state,
  actions,
  mutations,
  getters
};

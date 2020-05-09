<template>
  <div>
    <stats-cards></stats-cards>

    <dash-charts></dash-charts>

    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled" label="loading"></b-spinner>
      <br />Loading
    </div>

    <div class="card p-2 px-4 table-row-hover pointer mt-2 mb-4" @click="readersStatClick">
      <b-row>
        <b-col cols="auto" align-self="center">
          <i class="ti-signal" style="color: blue"></i>
        </b-col>
        <b-col cols="auto" class="pl-0" align-self="center">
          <p class="my-3">Readers statistic</p>
        </b-col>
        <b-col align-self="center" class="text-right">
          <i class="ti-angle-right"></i>
        </b-col>
      </b-row>
    </div>

    <div class="card p-2 px-4 table-row-hover pointer mt-2 mb-4" @click="personsStatClick">
      <b-row>
        <b-col cols="auto" align-self="center">
          <i class="ti-id-badge" style="color: green"></i>
        </b-col>
        <b-col cols="auto" class="pl-0" align-self="center">
          <p class="my-3">Person statistic</p>
        </b-col>
        <b-col align-self="center" class="text-right">
          <i class="ti-angle-right"></i>
        </b-col>
      </b-row>
    </div>
  </div>
</template>
<script>
import { ChartCard } from "@/components/index";
import StatsCards from "@/components/Dashboard/StatsCards.vue";
import Chartist from "chartist";
import DashCharts from "@/components/Dashboard/Charts.vue";
import { mapState, mapActions } from "vuex";

export default {
  components: {
    ChartCard,
    StatsCards,
    DashCharts
  },
  computed: {
    ...mapState({
      status: state => state.statistic.status
    }),
    showSpinner() {
      return status.fullCountStatLoading;
    }
  },
  data() {
    return {};
  },
  methods: {
    readersStatClick() {
      this.$router.push({ name: "readers-statistic" });
    },
    personsStatClick() {
      this.$router.push({ name: "persons-statistic" });
    }
  }
};
</script>
<style>
</style>

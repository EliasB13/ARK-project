<template>
  <div class="row">
    <div class="col-md-6 col-xl-3" v-for="stats in mappedStatsCards" :key="stats.title">
      <stats-card>
        <div class="icon-big text-center" :class="`icon-${stats.type}`" slot="header">
          <i :class="stats.icon"></i>
        </div>
        <div class="numbers" slot="content">
          <p>{{stats.title}}</p>
          {{stats.value}}
        </div>
        <div class="stats" slot="footer">
          <i :class="stats.footerIcon"></i>
          {{stats.footerText}}
        </div>
      </stats-card>
    </div>
  </div>
</template>
<script>
import { StatsCard, ChartCard } from "@/components/index";
import Chartist from "chartist";
import { mapState, mapActions, mapGetters } from "vuex";

export default {
  name: "stats-cards",
  components: {
    StatsCard,
    ChartCard
  },
  data() {
    return {
      statsCards: [
        {
          type: "warning",
          icon: "ti-signal",
          title: "Capacity",
          value: "105GB",
          footerText: "Updated now",
          footerIcon: "ti-reload"
        },
        {
          type: "success",
          icon: "ti-signal",
          title: "Revenue",
          value: "$1,345",
          footerText: "Last day",
          footerIcon: "ti-calendar"
        },
        {
          type: "danger",
          icon: "ti-signal",
          title: "Errors",
          value: "23",
          footerText: "In the last hour",
          footerIcon: "ti-timer"
        },
        {
          type: "info",
          icon: "ti-signal",
          title: "Followers",
          value: "+45",
          footerText: "Updated now",
          footerIcon: "ti-reload"
        }
      ]
    };
  },
  computed: {
    ...mapState({
      fullCountStat: state => state.statistic.fullCountStat,
      status: state => state.statistic.status
    }),
    ...mapGetters("statistic", ["mappedStatsCards"])
  },
  methods: {
    ...mapActions("statistic", ["getFullCountStat"])
  },
  mounted() {
    if (!this.status.fullCountStatLoaded) {
      var currDate = new Date();
      currDate.setDate(currDate.getDate() - 31);
      this.getFullCountStat({
        lowerBound: currDate.toISOString(),
        upperBound: new Date().toISOString()
      });
    }
  }
};
</script>
<template>
  <div class="row">
    <div class="col-md-6 col-12">
      <chart-card
        :title="$t('dashboard.prefChartTitle')"
        :sub-title="$t('dashboard.prefChartSubTitle')"
        :chart-data="preferencesChart.data"
        chart-type="Pie"
        ref="prefChart"
      >
        <span slot="footer">
          <i class="ti-timer"></i> {{ $t("dashboard.date") }}
        </span>
        <div slot="legend">
          <i class="fa fa-circle text-info"></i> {{ $t("dashboard.empl") }}
          <i class="fa fa-circle text-warning"></i> {{ $t("dashboard.anon") }}
        </div>
      </chart-card>
    </div>

    <div class="col-md-6 col-12">
      <chart-card
        :title="$t('dashboard.activityChartTitle')"
        :sub-title="$t('dashboard.activityChartSubTitle')"
        :chart-data="activityChart.data"
        :chart-options="activityChart.options"
      >
        <span slot="footer">
          <i class="ti-timer"></i> {{ $t("dashboard.year") }}
        </span>
        <div slot="legend">
          <i class="fa fa-circle text-info"></i> {{ $t("dashboard.empl") }}
          <i class="fa fa-circle text-warning"></i> {{ $t("dashboard.anon") }}
        </div>
      </chart-card>
    </div>
  </div>
</template>
<script>
import { ChartCard } from "@/components/index";
import Chartist from "chartist";
import { mapState, mapActions, mapGetters } from "vuex";

export default {
  name: "dash-charts",
  components: {
    ChartCard
  },
  data() {
    return {
      activityChart: {
        data: {
          labels: [
            this.$t("dashboard.months.Jan"),
            this.$t("dashboard.months.Feb"),
            this.$t("dashboard.months.Mar"),
            this.$t("dashboard.months.Apr"),
            this.$t("dashboard.months.May"),
            this.$t("dashboard.months.Jun"),
            this.$t("dashboard.months.Jul"),
            this.$t("dashboard.months.Aug"),
            this.$t("dashboard.months.Sep"),
            this.$t("dashboard.months.Oct"),
            this.$t("dashboard.months.Nov"),
            this.$t("dashboard.months.Dec")
          ],
          series: [
            [542, 543, 520, 680, 653, 753, 326, 434, 568, 610, 756, 895],
            [230, 293, 380, 480, 503, 553, 600, 664, 698, 710, 736, 795]
          ]
        },
        options: {
          seriesBarDistance: 10,
          axisX: {
            showGrid: false
          },
          height: "245px"
        }
      },
      preferencesChart: {
        data: {
          labels: ["62%", "32%"],
          series: [62, 32]
        },
        options: {}
      }
    };
  },
  computed: {
    ...mapState({
      fullCountStat: state => state.statistic.fullCountStat,
      status: state => state.statistic.status
    }),
    ...mapGetters("statistic", ["prefChartData"])
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
    } else {
      this.preferencesChart = this.prefChartData;
      this.$refs.prefChart.updateChart();
    }
  },
  watch: {
    status: {
      handler: function(newV, oldV) {
        if (newV.fullCountStatLoaded) {
          this.preferencesChart = this.prefChartData;
          this.$refs.prefChart.updateChart();
        }
      },
      deep: true
    }
  }
};
</script>

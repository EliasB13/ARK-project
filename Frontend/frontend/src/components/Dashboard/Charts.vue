<template>
  <div class="row">
    <div class="col-md-6 col-12">
      <chart-card
        title="Employees / anonyms observations statistic"
        sub-title="All readers during last month"
        :chart-data="preferencesChart.data"
        chart-type="Pie"
        ref="prefChart"
      >
        <span slot="footer">
          <i class="ti-timer"></i> In the last month
        </span>
        <div slot="legend">
          <i class="fa fa-circle text-info"></i> Employees
          <i class="fa fa-circle text-warning"></i> Anonyms
        </div>
      </chart-card>
    </div>

    <div class="col-md-6 col-12">
      <chart-card
        title="Employees / anonyms year observations statistic"
        sub-title="All readers month by month"
        :chart-data="activityChart.data"
        :chart-options="activityChart.options"
      >
        <span slot="footer">
          <i class="ti-timer"></i> In the last year
        </span>
        <div slot="legend">
          <i class="fa fa-circle text-info"></i> Employees
          <i class="fa fa-circle text-warning"></i> Anonyms
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
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"
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
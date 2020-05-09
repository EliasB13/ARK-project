<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <h4 class="card-title">{{ $t("readerStatistic.title") }}</h4>
              <p class="card-category my-2">
                {{ $t("readerStatistic.subTitle") }}
              </p>
            </b-col>
          </b-row>
        </div>
        <div slot="raw-content" class="table-responsive">
          <hr />
          <paper-table
            :hoverable="true"
            :removingMode="removingMode"
            :data="fullCountStat"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>
      </card>
    </div>

    <div class="px-4">
      <i class="ti-info-alt mr-2"></i>
      <span>{{ $t("readerStatistic.info1") }}</span>
      <br />
      <div class="mt-2">
        <svg
          enable-background="new 0 0 506.1 506.1"
          height="16"
          viewBox="0 0 506.1 506.1"
          width="16"
          xmlns="http://www.w3.org/2000/svg"
          style="fill: indianred"
        >
          <path
            d="m489.609 0h-473.118c-9.108 0-16.491 7.383-16.491 16.491v473.118c0 9.107 7.383 16.491 16.491 16.491h473.119c9.107 0 16.49-7.383 16.49-16.491v-473.118c0-9.108-7.383-16.491-16.491-16.491z"
          />
        </svg>
        <span class="ml-2">{{ $t("readerStatistic.info2") }}</span>
      </div>
    </div>

    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled" label="loading"></b-spinner>
      <br />{{ $t("spinner") }}
    </div>
  </div>
</template>
<script>
import { PaperTable } from "@/components";
import { mapState, mapActions } from "vuex";

export default {
  components: {
    PaperTable
  },
  data() {
    return {
      removingMode: false,
      selectedCards: [],
      showModal: false
    };
  },
  computed: {
    ...mapState({
      statsStatus: state => state.statistic.status,
      fullCountStat: state => state.statistic.fullCountStat
    }),
    tableCols() {
      return [
        {
          column: "id",
          displayName: this.$t("readerStatistic.cols.number")
        },
        {
          column: "name",
          displayName: this.$t("readerStatistic.cols.name")
        },
        {
          column: "description",
          displayName: this.$t("readerStatistic.cols.description")
        },
        {
          column: "isEntrance",
          displayName: this.$t("readerStatistic.cols.isEntrance")
        },
        {
          column: "employeesObservationsCount",
          displayName: this.$t(
            "readerStatistic.cols.employeesObservationsCount"
          )
        },
        {
          column: "anonymObservationsCount",
          displayName: this.$t("readerStatistic.cols.anonymObservationsCount")
        }
      ];
    },
    showSpinner() {
      return this.statsStatus.fullCountStatLoading;
    }
  },
  methods: {
    ...mapActions("statistic", ["getFullCountStat"]),
    click(item) {
      this.$router.push({
        name: "reader-statistic",
        params: { readerId: item.id }
      });
    }
  },
  mounted() {
    var currDate = new Date();
    currDate.setDate(currDate.getDate() - 31);
    this.getFullCountStat({
      lowerBound: currDate.toISOString(),
      upperBound: new Date().toISOString()
    });
  }
};
</script>
<style></style>

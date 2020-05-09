<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <h4 class="card-title">Readers statistics</h4>
              <p class="card-category my-2">Here you can explore your complete readers statistics</p>
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
      <span>Select reader to see all observations it registered</span>
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
        <span
          class="ml-2"
        >Rows marked with this color mean that observation was restricted (user came to restricted area)</span>
      </div>
    </div>

    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled" label="loading"></b-spinner>
      <br />Loading
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
      tableCols: [
        {
          column: "id",
          displayName: "Number"
        },
        {
          column: "name",
          displayName: "Name"
        },
        {
          column: "description",
          displayName: "Description"
        },
        {
          column: "isEntrance",
          displayName: "Is entrance"
        },
        {
          column: "employeesObservationsCount",
          displayName: "Employees observations count"
        },
        {
          column: "anonymObservationsCount",
          displayName: "Anonyms observations count"
        }
      ],
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
<style>
</style>

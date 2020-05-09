<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <h4 class="card-title">{{ readerStat.name }}</h4>
              <p class="card-category my-2">{{ readerStat.description }}</p>
            </b-col>
          </b-row>
        </div>
        <div slot="raw-content" class="table-responsive">
          <hr />
          <paper-table
            :removingMode="removingMode"
            :data="readerStat.observations"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>
      </card>
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
  props: {
    readerId: [Number, String]
  },
  data() {
    return {
      tableCols: [
        {
          outer: "person",
          column: "name",
          displayName: "Name",
          isInner: true
        },
        {
          outer: "person",
          column: "surname",
          displayName: "Surname",
          isInner: true
        },
        {
          column: "time",
          displayName: "Time"
        },
        {
          outer: "person",
          column: "isEmployee",
          displayName: "Is employee",
          isInner: true
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
      readerStat: state => state.statistic.readerStat
    }),
    showSpinner() {
      return this.statsStatus.readerStatLoading;
    }
  },
  methods: {
    ...mapActions("statistic", ["getReaderStat"]),
    click(item) {}
  },
  mounted() {
    var currDate = new Date();
    currDate.setDate(currDate.getDate() - 31);
    this.getReaderStat({
      readerId: this.readerId,
      lowerBound: currDate.toISOString(),
      upperBound: new Date().toISOString()
    });
  }
};
</script>
<style>
</style>

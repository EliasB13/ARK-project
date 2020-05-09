<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col>
              <h4 class="card-title">{{ personName }}</h4>
              <p class="card-category my-2">{{ personSurname }}</p>
            </b-col>
          </b-row>
        </div>
        <div slot="raw-content" class="table-responsive">
          <hr />
          <paper-table :data="personStat" :columns="tableCols" @click="click"></paper-table>
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
    cardId: [String, Number]
  },
  data() {
    return {
      tableCols: [
        {
          column: "id",
          displayName: "Number"
        },
        {
          outer: "reader",
          isInner: true,
          column: "name",
          displayName: "Name"
        },
        {
          column: "time",
          displayName: "Time"
        }
      ],
      personName: "",
      personSurname: ""
    };
  },
  computed: {
    ...mapState({
      personStat: state => state.statistic.personStat,
      status: state => state.statistic.status
    }),
    showSpinner() {
      return this.status.personStatLoading;
    }
  },
  methods: {
    ...mapActions("statistic", ["getPersonStat"]),

    click(item) {}
  },
  mounted() {
    var currDate = new Date();
    currDate.setDate(currDate.getDate() - 31);
    this.getPersonStat({
      cardId: this.cardId,
      lowerBound: currDate.toISOString(),
      upperBound: new Date().toISOString()
    });
  },
  watch: {
    status: {
      handler: function(newV, oldV) {
        if (this.status.personStatLoaded) {
          this.personName = this.personStat[0].person.name;
          this.personSurname = this.personStat[0].person.surname;
        }
      },
      deep: true
    }
  }
};
</script>
<style>
</style>

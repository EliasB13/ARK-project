<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <h4 class="card-title">{{ $t("personStatistic.title") }}</h4>
              <p class="card-category my-2">
                {{ $t("personStatistic.subTitle") }}
              </p>
            </b-col>
          </b-row>
        </div>
        <div slot="raw-content" class="table-responsive">
          <hr />
          <paper-table
            :hoverable="true"
            :removingMode="removingMode"
            :data="cards"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>
      </card>
      <card-adding-modal :showAddingModal="showModal"></card-adding-modal>
    </div>

    <div class="px-4">
      <i class="ti-info-alt mr-2"></i>
      <span>{{ $t("personStatistic.info1") }}</span>
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
        <span class="ml-2">{{ $t("personStatistic.info2") }}</span>
      </div>
    </div>

    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled" label="loading"></b-spinner>
      <br />{{ $t("spinner") }}
    </div>
  </div>
</template>
<script>
import CardAddingModal from "../components/Modals/CardAddingModal.vue";
import { PaperTable } from "@/components";
import { mapState, mapActions } from "vuex";

export default {
  components: {
    PaperTable,
    CardAddingModal
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
      cards: state => state.cards.cards,
      status: state => state.cards.status,
      roles: state => state.roles.roles,
      rolesStatus: state => state.roles.status
    }),
    tableCols() {
      return [
        {
          column: "personCardId",
          displayName: this.$t("personCardsPage.cols.number")
        },
        {
          column: "name",
          displayName: this.$t("personCardsPage.cols.name")
        },
        {
          column: "surname",
          displayName: this.$t("personCardsPage.cols.surname")
        },
        {
          column: "isEmployee",
          displayName: this.$t("personCardsPage.cols.isEmpl")
        },
        {
          column: "employeesRoleId",
          displayName: this.$t("personCardsPage.cols.role")
        },
        {
          column: "workingDayStartTime",
          displayName: this.$t("personCardsPage.cols.start")
        },
        {
          column: "workingDayEndTime",
          displayName: this.$t("personCardsPage.cols.end")
        }
      ];
    },
    showSpinner() {
      return (
        this.status.cardsLoading ||
        this.status.cardAdding ||
        this.status.cardRemoving ||
        this.rolesStatus.rolesLoading
      );
    },
    showEmptyList() {
      return this.status.itemsLoaded && this.items.length == 0;
    },
    showItems() {
      return this.status.itemsLoaded;
    }
  },
  methods: {
    ...mapActions("cards", ["getCards", "addCard", "deleteCard"]),
    ...mapActions("roles", ["getRoles"]),

    click(item) {
      this.$router.push({
        name: "person-statistic",
        params: { cardId: item.personCardId }
      });
    }
  },
  mounted() {
    this.getCards();
  },
  watch: {
    cards: function(oldValue, newValue) {
      this.getRoles();
      this.cards.map(e => {
        if (!e.isEmployee) {
          //e.isEmployee = "-";
          e.workingDayStartTime = "-";
          e.workingDayEndTime = "-";
        } else {
          //e.isEmployee = "+";
        }
        return e;
      });
    },
    roles: function(newValue, oldValue) {
      this.cards.map(e => {
        let role = this.roles.find(r => r.id == e.employeesRoleId);
        if (role != undefined)
          e.employeesRoleId = role.name == undefined ? "Anonymous" : role.name;
        return e;
      });
    }
  }
};
</script>
<style></style>

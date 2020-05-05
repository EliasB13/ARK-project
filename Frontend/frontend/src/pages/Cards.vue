<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <h4 class="card-title">Person cards</h4>
              <p class="card-category my-2">Here you can explore and manage your person cards</p>
            </b-col>
            <b-col v-else-if="removingMode">
              <h4 class="card-title">Delete card</h4>
            </b-col>
            <b-col cols="auto pr-0" align-self="center">
              <p-button v-if="!removingMode" type="success" @click="addClick">
                <i class="ti-plus"></i>
                Add
              </p-button>
              <p-button v-else-if="removingMode" type="success" @click="resetClick" outline>Reset</p-button>
            </b-col>
            <b-col cols="auto" align-self="center">
              <p-button type="danger" @click="deleteClick">
                <i class="ti-close"></i>
                Remove
              </p-button>
            </b-col>
          </b-row>
        </div>
        <div slot="raw-content" class="table-responsive">
          <hr />
          <paper-table
            :removingMode="removingMode"
            :data="cards"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>
      </card>
      <card-adding-modal :showAddingModal="showModal" @close-modal="closeModal"></card-adding-modal>
    </div>

    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled" label="loading"></b-spinner>
      <br />Loading
    </div>
  </div>
</template>
<script>
import CardAddingModal from "../components/CardAddingModal.vue";
import { PaperTable } from "@/components";
import { mapState, mapActions } from "vuex";

export default {
  components: {
    PaperTable,
    CardAddingModal
  },
  data() {
    return {
      tableCols: [
        {
          column: "personCardId",
          displayName: "Card number"
        },
        {
          column: "name",
          displayName: "Name"
        },
        {
          column: "surname",
          displayName: "Surname"
        },
        {
          column: "isEmployee",
          displayName: "Is employee"
        },
        {
          column: "employeesRoleId",
          displayName: "Role"
        },
        {
          column: "workingDayStartTime",
          displayName: "Working day starts at"
        },
        {
          column: "workingDayEndTime",
          displayName: "Working day ends at"
        }
      ],
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
    addClick() {
      this.showModal = true;
    },
    deleteClick() {
      if (!this.removingMode) this.removingMode = true;
      else {
        this.cards.forEach(c => {
          if (c.selected) this.deleteCard(c.personCardId);
        });
      }
    },
    click(item) {
      let card = this.cards.find(c => c.personCardId === item.personCardId);
      card.selected = !card.selected;
    },
    resetClick() {
      this.removingMode = false;
      this.cards.map(c => {
        c.selected = false;
        return c;
      });
    },
    closeModal(val) {
      debugger;
      this.showModal = false;
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
          e.isEmployee = "-";
          e.workingDayStartTime = "-";
          e.workingDayEndTime = "-";
        } else {
          e.isEmployee = "+";
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
<style>
</style>

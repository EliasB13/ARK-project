<template>
  <div>
    <b-modal v-model="showAddingModal" centered :showClose="false" size="lg" no-close-on-backdrop>
      <div slot="modal-header">Select employees to add</div>
      <template>
        <div class="scrollable-list">
          <paper-table
            :removingMode="true"
            :data="cards"
            :columns="tableCols"
            @click="click"
            ref="table"
          ></paper-table>
        </div>
      </template>
      <template slot="modal-footer">
        <p-button simple @click="closeModal">Close</p-button>
        <p-button type="success" class="ml-auto" @click="addReaderClick">Add employees</p-button>
      </template>
    </b-modal>
  </div>
</template>
<script>
import { mapState, mapActions } from "vuex";
import VueTimepicker from "vue2-timepicker/src/vue-timepicker.vue";
import { PaperTable } from "@/components";

export default {
  name: "employee-adding-modal",
  components: {
    VueTimepicker,
    PaperTable
  },
  data() {
    return {
      tableCols: [
        {
          column: "personCardId",
          displayName: "Number"
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
      ]
    };
  },
  props: {
    showAddingModal: Boolean,
    roleId: Number
  },
  computed: {
    ...mapState({
      status: state => state.role.status,
      cards: state => state.cards.cards,
      cardsStatus: state => state.cards.status
    }),
    showSpinner() {
      return this.cardsStatus.cardsLoading || this.status.cardAdding;
    }
  },
  methods: {
    ...mapActions("role", ["addCardToRole"]),
    ...mapActions("cards", ["getCards"]),
    addReaderClick() {
      this.cards.forEach(r => {
        if (r.selected)
          this.addCardToRole({ cardId: r.personCardId, roleId: this.roleId });
      });
      this.closeModal();
    },
    closeModal() {
      this.$parent.showModal = false;
      this.$refs.table.resetSelection();
      this.cards.map(r => {
        r.selected = false;
        return r;
      });
    },
    click(item) {
      let reader = this.cards.find(c => c.personCardId === item.personCardId);
      reader.selected = !reader.selected;
    }
  },
  watch: {
    showAddingModal: function(newV, oldV) {
      if (!newV) {
        this.closeModal();
      }
    }
  },
  mounted() {
    this.getCards();
  }
};
</script>
<style>
</style>

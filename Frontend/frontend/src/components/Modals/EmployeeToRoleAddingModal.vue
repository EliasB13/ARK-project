<template>
  <div>
    <b-modal
      v-model="showAddingModal"
      centered
      :showClose="false"
      size="lg"
      no-close-on-backdrop
    >
      <div slot="modal-header">{{ $t("rolesPage.modal.title") }}</div>
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
        <p-button simple @click="closeModal">{{ $t("closeBtn") }}</p-button>
        <p-button type="success" class="ml-auto" @click="addReaderClick">{{
          $t("addBtn")
        }}</p-button>
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
    return {};
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
    tableCols() {
      return [
        {
          column: "personCardId",
          displayName: this.$t("rolePage.cols.number")
        },
        {
          column: "name",
          displayName: this.$t("rolePage.cols.name")
        },
        {
          column: "surname",
          displayName: this.$t("rolePage.cols.surname")
        },
        {
          column: "isEmployee",
          displayName: this.$t("rolePage.cols.isEmpl")
        },
        {
          column: "employeesRoleId",
          displayName: this.$t("rolePage.cols.role")
        },
        {
          column: "workingDayStartTime",
          displayName: this.$t("rolePage.cols.start")
        },
        {
          column: "workingDayEndTime",
          displayName: this.$t("rolePage.cols.end")
        }
      ];
    },
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
<style></style>

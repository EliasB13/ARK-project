<template>
  <div>
    <b-modal v-model="showAddingModal" centered :showClose="false">
      <div slot="modal-header">{{ $t("readers.restrictingModal.title") }}</div>
      <template>
        <div class="scrollable-list">
          <paper-table
            :removingMode="true"
            :data="readers"
            :columns="tableCols"
            @click="click"
            ref="table"
          ></paper-table>
        </div>
      </template>
      <template slot="modal-footer">
        <p-button simple @click="closeModal">{{ $t("closeBtn") }}</p-button>
        <p-button type="success" class="ml-auto" @click="addReaderClick">{{
          $t("readers.restrictingModal.restrictBtn")
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
  name: "reader-adding-modal",
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
      readers: state => state.readers.readers,
      readersStatus: state => state.readers.status
    }),
    tableCols() {
      return [
        {
          column: "id",
          displayName: this.$t("rolePage.tableCols.number")
        },
        {
          column: "name",
          displayName: this.$t("rolePage.tableCols.name")
        },
        {
          column: "description",
          displayName: this.$t("rolePage.tableCols.description")
        },
        {
          column: "isEntrance",
          displayName: this.$t("rolePage.tableCols.isEntrance")
        }
      ];
    },
    showSpinner() {
      return (
        this.readersStatus.readersLoading ||
        this.status.readerRestricting ||
        this.status.readerUnrestricting
      );
    }
  },
  methods: {
    ...mapActions("role", ["restrictReader"]),
    ...mapActions("readers", ["getReaders"]),
    addReaderClick() {
      this.readers.forEach(r => {
        if (r.selected)
          this.restrictReader({ readerId: r.id, roleId: this.roleId });
      });
      this.closeModal();
    },
    closeModal() {
      this.$parent.showModal = false;
      this.$refs.table.resetSelection();
      this.readers.map(r => {
        r.selected = false;
        return r;
      });
    },
    click(item) {
      let reader = this.readers.find(c => c.id === item.id);
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
    this.getReaders();
  }
};
</script>
<style></style>

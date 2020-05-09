<template>
  <div>
    <b-modal v-model="showAddingModal" centered :showClose="false">
      <div slot="modal-header">{{ $t("readers.modal.title") }}</div>
      <template>
        <form role="form">
          <fg-input
            alternative
            class="mb-3"
            :placeholder="$t('readers.cols.name')"
            :label="$t('readers.cols.name')"
            v-model="readerToAdd.name"
          ></fg-input>
          <fg-input
            :placeholder="$t('readers.cols.description')"
            :label="$t('readers.cols.description')"
            v-model="readerToAdd.description"
          ></fg-input>
          <b-form-checkbox v-model="readerToAdd.isEntrance" class="mb-3">{{
            $t("readers.modal.isEntrance")
          }}</b-form-checkbox>
        </form>
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

export default {
  name: "reader-adding-modal",
  components: {
    VueTimepicker
  },
  data() {
    return {
      readerToAdd: {
        name: "",
        description: "",
        isEntrance: false
      }
    };
  },
  props: {
    showAddingModal: Boolean
  },
  computed: {
    ...mapState({
      status: state => state.readers.status
    })
  },
  methods: {
    ...mapActions("readers", ["addReader"]),
    addReaderClick() {
      if (this.readerToAdd.name && this.readerToAdd.description) {
        this.addReader(this.readerToAdd);
        this.closeModal();
      }
    },
    closeModal() {
      this.$parent.showModal = false;
    }
  },
  watch: {
    showAddingModal: function(newV, oldV) {
      if (!newV) {
        this.readerToAdd = {
          name: "",
          description: "",
          isEntrance: false
        };
        this.closeModal();
      }
    }
  }
};
</script>
<style></style>

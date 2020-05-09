<template>
  <div>
    <b-modal v-model="showAddingModal" centered :showClose="false">
      <div slot="modal-header">{{ $t("rolePage.modal.title") }}</div>
      <template>
        <form role="form">
          <fg-input
            alternative
            class="mb-3"
            :placeholder="$t('rolePage.modal.name')"
            :label="$t('rolePage.modal.name')"
            v-model="roleToAdd.name"
          ></fg-input>
          <fg-input
            :placeholder="$t('rolePage.modal.description')"
            :label="$t('rolePage.modal.description')"
            v-model="roleToAdd.description"
          ></fg-input>
        </form>
      </template>
      <template slot="modal-footer">
        <p-button simple @click="closeModal">{{ $t("closeBtn") }}</p-button>
        <p-button type="success" class="ml-auto" @click="addRoleClick">{{
          $t("addBtn")
        }}</p-button>
      </template>
    </b-modal>
  </div>
</template>
<script>
import { mapState, mapActions } from "vuex";

export default {
  name: "role-adding-modal",
  data() {
    return {
      roleToAdd: {
        name: "",
        description: ""
      }
    };
  },
  props: {
    showAddingModal: Boolean
  },
  computed: {
    ...mapState({
      status: state => state.roles.status
    })
  },
  methods: {
    ...mapActions("roles", ["addRole"]),
    addRoleClick() {
      if (this.roleToAdd.name && this.roleToAdd.description) {
        this.addRole(this.roleToAdd);
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
        this.roleToAdd = {
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

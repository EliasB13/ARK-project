<template>
  <div>
    <b-modal v-model="showAddingModal" centered :showClose="false">
      <div slot="modal-header">Add role</div>
      <template>
        <form role="form">
          <fg-input
            alternative
            class="mb-3"
            placeholder="Name"
            label="Name"
            v-model="roleToAdd.name"
          ></fg-input>
          <fg-input placeholder="Description" label="Description" v-model="roleToAdd.description"></fg-input>
        </form>
      </template>
      <template slot="modal-footer">
        <p-button simple @click="closeModal">Close</p-button>
        <p-button type="success" class="ml-auto" @click="addRoleClick">Add role</p-button>
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
<style>
</style>

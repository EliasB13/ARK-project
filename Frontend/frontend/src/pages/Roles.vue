<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <h4 class="card-title">Employees roles</h4>
              <p class="card-category my-2">Here you can explore and manage your employees roles</p>
            </b-col>
            <b-col v-else-if="removingMode">
              <h4 class="card-title">Delete role</h4>
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
            :hoverable="true"
            :removingMode="removingMode"
            :data="roles"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>
      </card>
      <role-adding-modal :showAddingModal="showModal"></role-adding-modal>
    </div>

    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled mb-2" label="loading"></b-spinner>
      <br />Loading
    </div>
  </div>
</template>
</template>
<script>
import RoleAddingModal from "../components/Modals/RoleAddingModal.vue";
import { PaperTable } from "@/components";
import { mapState, mapActions } from "vuex";

export default {
  components: {
    PaperTable,
    RoleAddingModal
  },
  data() {
    return {
      tableCols: [
        {
          column: "id",
          displayName: "Number"
        },
        {
          column: "name",
          displayName: "Name"
        },
        {
          column: "description",
          displayName: "Description"
        },
        {
          column: "isAnonymous",
          displayName: "Is anonymous"
        }
      ],
      removingMode: false,
      showModal: false
    };
  },
  computed: {
    ...mapState({
      roles: state => state.roles.roles,
      status: state => state.roles.status
    }),
    showSpinner() {
      return (
        this.status.rolesLoading ||
        this.status.roleAdding ||
        this.status.roleRemoving
      );
    }
  },
  methods: {
    ...mapActions("roles", ["getRoles", "addRole", "deleteRole"]),
    addClick() {
      this.showModal = true;
    },
    deleteClick() {
      if (!this.removingMode) this.removingMode = true;
      else {
        this.roles.forEach(r => {
          if (r.selected) this.deleteRole(r.id);
        });
        this.removingMode = false;
      }
    },
    click(item) {
      let role = this.roles.find(r => r.id === item.id);
      role.selected = !role.selected;
    },
    resetClick() {
      this.removingMode = false;
      this.roles.map(r => {
        r.selected = false;
        return r;
      });
    }
  },
  mounted() {
    this.getRoles();
  },
  watch: {
    roles: function(oldValue, newValue) {
      debugger;
      this.roles.map(r => {
        if (r.isAnonymous) {
          r.name = "-";
          r.description = "-";
          //r.isAnonymous = "+";
        }
        //else r.isAnonymous = "-";
        return r;
      });
    }
  }
};
</script>
<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <h4 class="card-title">{{ $t('rolesPage.rolesTitle') }}</h4>
              <p class="card-category my-2">{{ $t('rolesPage.rolesSubTitle') }}</p>
            </b-col>
            <b-col v-else-if="removingMode">
              <h4 class="card-title">{{ $t('rolesPage.deleteRole') }}</h4>
            </b-col>
            <b-col cols="auto pr-0" align-self="center">
              <p-button v-if="!removingMode" type="success" @click="addClick">
                <i class="ti-plus"></i>
                {{ $t('addBtn') }}
              </p-button>
              <p-button v-else-if="removingMode" type="success" @click="resetClick" outline>{{ $t('resetBtn') }}</p-button>
            </b-col>
            <b-col cols="auto" align-self="center">
              <p-button type="danger" @click="deleteClick">
                <i class="ti-close"></i>
                {{ $t('removeBtn') }}
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
      removingMode: false,
      showModal: false
    };
  },
  computed: {
    ...mapState({
      roles: state => state.roles.roles,
      status: state => state.roles.status
    }),
    tableCols() {
      return [
        {
          column: "id",
          displayName: this.$t('rolesPage.cols.number')
        },
        {
          column: "name",
          displayName: this.$t('rolesPage.cols.name')
        },
        {
          column: "description",
          displayName: this.$t('rolesPage.cols.description')
        },
        {
          column: "isAnonymous",
          displayName: this.$t('rolesPage.cols.isAnon')
        }
      ];
    },
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

      if (!this.removingMode)
        this.$router.push({ name: "role", params: { roleId: role.id } });
      else {
        role.selected = !role.selected;
      }
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
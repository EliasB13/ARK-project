<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header" class="mb-3">
          <b-row>
            <b-col>
              <h4 class="card-title">{{ role.name }}</h4>
              <p class="card-category my-2">{{ role.description }}</p>
            </b-col>
          </b-row>
        </div>
        <!-- <div slot="raw-content" class="table-responsive">
          <hr />
          <paper-table
            :hoverable="true"
            :removingMode="removingMode"
            :data="roles"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>-->
      </card>

      <div class="card p-2 px-4 table-row-hover pointer mt-2 mb-1" @click="employeesCollapseClick">
        <b-row>
          <b-col cols="auto" align-self="center">
            <i class="ti-id-badge" style="color: green"></i>
          </b-col>
          <b-col cols="auto" class="pl-0" align-self="center">
            <p class="my-3">Employees</p>
          </b-col>
          <b-col align-self="center" class="text-right">
            <i class="ti-angle-down"></i>
          </b-col>
        </b-row>
      </div>

      <b-collapse :id="`employees-collapse-${roleId}`" v-model="showEmployeesCollapse" class="mt-2">
        <readers-in-role :roleId="parseInt(roleId)"></readers-in-role>
      </b-collapse>

      <div class="card p-2 px-4 table-row-hover pointer mt-4 mb-1" @click="readersCollapseClick">
        <b-row>
          <b-col cols="auto" align-self="center">
            <i class="ti-rss-alt" style="color: red"></i>
          </b-col>
          <b-col cols="auto" class="pl-0" align-self="center">
            <p class="my-3">Readers</p>
          </b-col>
          <b-col align-self="center" class="text-right">
            <i class="ti-angle-down"></i>
          </b-col>
        </b-row>
      </div>

      <b-collapse :id="`employees-collapse-${roleId}`" v-model="showReadersCollapse" class="mt-2">
        <readers-in-role :roleId="parseInt(roleId)"></readers-in-role>
      </b-collapse>
    </div>

    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled mb-2" label="loading"></b-spinner>
      <br />Loading
    </div>
  </div>
</template>
<script>
import ReadersInRole from "../components/Roles/ReadersInRole.vue";
import { PaperTable } from "@/components";
import { mapState, mapActions } from "vuex";

export default {
  components: {
    PaperTable,
    ReadersInRole
  },
  props: {
    roleId: [Number, String]
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
      showModal: false,
      showReadersCollapse: false,
      showEmployeesCollapse: false
    };
  },
  computed: {
    ...mapState({
      role: state => state.role.role,
      status: state => state.role.status
    }),
    showSpinner() {
      return this.status.roleLoading;
      //||
      //this.status.roleAdding ||
      //this.status.roleRemoving
    }
  },
  methods: {
    ...mapActions("role", ["getRoleById", "addRole", "deleteRole"]),
    employeesCollapseClick() {
      this.showEmployeesCollapse = !this.showEmployeesCollapse;
    },
    readersCollapseClick() {
      this.showReadersCollapse = !this.showReadersCollapse;
    }
  },
  mounted() {
    this.getRoleById(this.roleId);
  },
  watch: {
    role: function(oldValue, newValue) {
      if (this.role.isAnonymous) {
        this.role.name = "Anonymous role";
        this.role.description = "-";
      }
    }
  }
};
</script>
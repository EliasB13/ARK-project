<template>
  <div class="row">
    <div class="col-12">
      <card class="mb-0">
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <p class="card-title">{{ $t("dashboard.empl") }}</p>
              <p class="card-category my-2">
                {{ $t("rolePage.employeesInRoleSubTitle") }}
              </p>
            </b-col>
            <b-col v-else-if="removingMode">
              <h4 class="card-title">{{ $t("rolePage.deleteEmpl") }}</h4>
            </b-col>
            <b-col cols="auto pr-0" align-self="center">
              <p-button
                v-if="!removingMode"
                type="success"
                @click="addClick"
                size="sm"
              >
                <i class="ti-plus"></i>
              </p-button>
              <p-button
                v-else-if="removingMode"
                type="success"
                @click="resetClick"
                outline
                size="sm"
                >{{ $t("resetBtn") }}</p-button
              >
            </b-col>
            <b-col cols="auto" align-self="center">
              <p-button type="danger" @click="deleteClick" size="sm">
                <i class="ti-close"></i>
              </p-button>
            </b-col>
          </b-row>
        </div>
        <div slot="raw-content" class="table-responsive" v-if="!showSpinner">
          <hr />
          <paper-table
            :removingMode="removingMode"
            :data="employees"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>
        <div v-if="showSpinner" class="text-center">
          <b-spinner class="spinner-scaled" label="loading"></b-spinner>
          <br />{{ $t("spinner") }}
        </div>
      </card>
      <employee-adding-modal
        :showAddingModal="showModal"
        :roleId="roleId"
      ></employee-adding-modal>
    </div>
  </div>
</template>
<script>
import EmployeeAddingModal from "../Modals/EmployeeToRoleAddingModal.vue";
import { PaperTable } from "@/components";
import { mapState, mapActions } from "vuex";

export default {
  name: "employees-in-role",
  components: {
    PaperTable,
    EmployeeAddingModal
  },
  props: {
    roleId: Number
  },
  data() {
    return {
      removingMode: false,
      showModal: false
    };
  },
  computed: {
    ...mapState({
      employees: state => state.role.employees,
      status: state => state.role.status
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
      return (
        this.status.employeesLoading ||
        this.status.cardAdding ||
        this.status.cardAdded
      );
    }
  },
  methods: {
    ...mapActions("role", ["getRoleEmployees", "removeCardFromRole"]),
    addClick() {
      this.showModal = true;
    },
    deleteClick() {
      if (!this.removingMode) this.removingMode = true;
      else {
        this.employees.forEach(r => {
          if (r.selected)
            this.removeCardFromRole({
              cardId: r.personCardId,
              roleId: this.roleId
            });
        });
        this.removingMode = false;
      }
    },
    click(item) {
      let reader = this.employees.find(
        e => e.personCardId === item.personCardId
      );
      reader.selected = !reader.selected;
    },
    resetClick() {
      this.removingMode = false;
      this.employees.map(r => {
        r.selected = false;
        return r;
      });
    }
  },
  mounted() {
    this.getRoleEmployees(this.roleId);
  },
  watch: {}
};
</script>

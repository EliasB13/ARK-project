<template>
  <div class="row">
    <div class="col-12">
      <card class="mb-0">
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <p class="card-title">Readers</p>
              <p class="card-category my-2">Manage readers in role</p>
            </b-col>
            <b-col v-else-if="removingMode">
              <h4 class="card-title">Delete reader</h4>
            </b-col>
            <b-col cols="auto pr-0" align-self="center">
              <p-button v-if="!removingMode" type="success" @click="addClick" size="sm">
                <i class="ti-plus"></i>
              </p-button>
              <p-button
                v-else-if="removingMode"
                type="success"
                @click="resetClick"
                outline
                size="sm"
              >Reset</p-button>
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
            :data="readers"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>
        <div v-if="showSpinner" class="text-center">
          <b-spinner class="spinner-scaled" label="loading"></b-spinner>
          <br />Loading
        </div>
      </card>
      <reader-restricting-modal :showAddingModal="showModal" :roleId="roleId"></reader-restricting-modal>
    </div>
  </div>
</template>
<script>
import ReaderRestrictingModal from "../Modals/ReaderRestrictingModal.vue";
import { PaperTable } from "@/components";
import { mapState, mapActions } from "vuex";

export default {
  components: {
    PaperTable,
    ReaderRestrictingModal
  },
  props: {
    roleId: Number
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
          column: "isEntrance",
          displayName: "Is entrance"
        }
      ],
      removingMode: false,
      showModal: false
    };
  },
  computed: {
    ...mapState({
      readers: state => state.role.restrictedReaders,
      status: state => state.role.status
    }),
    showSpinner() {
      return (
        this.status.restrictedReadersLoading ||
        this.status.readerRestricting ||
        this.status.readerUnrestricting
      );
    }
  },
  methods: {
    ...mapActions("role", [
      "getRoleRestrictedReaders",
      "restrictReader",
      "unrestrictReader"
    ]),
    addClick() {
      this.showModal = true;
    },
    deleteClick() {
      if (!this.removingMode) this.removingMode = true;
      else {
        this.readers.forEach(r => {
          if (r.selected)
            this.unrestrictReader({ readerId: r.id, roleId: this.roleId });
        });
        this.removingMode = false;
      }
    },
    click(item) {
      let reader = this.readers.find(c => c.id === item.id);
      reader.selected = !reader.selected;
    },
    resetClick() {
      this.removingMode = false;
      this.readers.map(r => {
        r.selected = false;
        return r;
      });
    }
  },
  mounted() {
    this.getRoleRestrictedReaders(this.roleId);
  },
  watch: {}
};
</script>
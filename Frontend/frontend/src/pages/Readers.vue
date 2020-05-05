<template>
  <div class="row">
    <div class="col-12">
      <card>
        <div slot="header">
          <b-row>
            <b-col v-if="!removingMode">
              <h4 class="card-title">Readers</h4>
              <p class="card-category my-2">Here you can explore and manage your readers</p>
            </b-col>
            <b-col v-else-if="removingMode">
              <h4 class="card-title">Delete reader</h4>
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
            :removingMode="removingMode"
            :data="readers"
            :columns="tableCols"
            @click="click"
          ></paper-table>
        </div>
      </card>
      <reader-adding-modal :showAddingModal="showModal"></reader-adding-modal>
    </div>

    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled" label="loading"></b-spinner>
      <br />Loading
    </div>
  </div>
</template>
<script>
import ReaderAddingModal from "../components/Modals/ReaderAddingModal.vue";
import { PaperTable } from "@/components";
import { mapState, mapActions } from "vuex";

export default {
  components: {
    PaperTable,
    ReaderAddingModal
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
      selectedCards: [],
      showModal: false
    };
  },
  computed: {
    ...mapState({
      readers: state => state.readers.readers,
      status: state => state.readers.status
    }),
    showSpinner() {
      return (
        this.status.readersLoading ||
        this.status.readerAdding ||
        this.status.readerRemoving
      );
    }
  },
  methods: {
    ...mapActions("readers", ["getReaders", "addReader", "deleteReader"]),
    addClick() {
      this.showModal = true;
    },
    deleteClick() {
      if (!this.removingMode) this.removingMode = true;
      else {
        this.readers.forEach(r => {
          if (r.selected) this.deleteReader(r.id);
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
    this.getReaders();
  },
  watch: {
    readers: function(oldValue, newValue) {
      this.readers.map(r => {
        r.isEntrance = r.isEntrance ? "+" : "-";
        return r;
      });
    }
  }
};
</script>
<style>
</style>

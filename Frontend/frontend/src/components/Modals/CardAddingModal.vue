<template>
  <div>
    <b-modal v-model="showAddingModal" centered :showClose="false">
      <div slot="modal-header">{{ $t("personCardsPage.modal.title") }}</div>
      <template>
        <form role="form">
          <fg-input
            alternative
            class="mb-3"
            :placeholder="$t('personCardsPage.modal.name')"
            :label="$t('personCardsPage.modal.name')"
            v-model="cardToAdd.name"
          ></fg-input>
          <fg-input
            alternative
            :placeholder="$t('personCardsPage.modal.surname')"
            :label="$t('personCardsPage.modal.surname')"
            v-model="cardToAdd.surname"
          ></fg-input>
          <b-form-checkbox v-model="cardToAdd.isEmployee" class="mb-3">{{
            $t("personCardsPage.modal.isEmpl")
          }}</b-form-checkbox>
          <div v-if="cardToAdd.isEmployee">
            <div>
              <label>{{ $t("personCardsPage.modal.start") }}</label>
              <br />
              <vue-timepicker
                advanced-keyboard
                format="HH:mm"
                v-model="cardToAdd.workingDayStartTime"
                style="border-radius: 10px"
              ></vue-timepicker>
            </div>
            <br />
            <div class="mb-1">
              <label>{{ $t("personCardsPage.modal.end") }}</label>
              <br />
              <vue-timepicker
                advanced-keyboard
                format="HH:mm"
                v-model="cardToAdd.workingDayEndTime"
              ></vue-timepicker>
            </div>
            <div class="mt-3">
              <label>{{ $t("personCardsPage.modal.role") }}</label>
              <br />
              <b-form-select
                v-model="cardToAdd.employeesRoleId"
                :options="options"
              ></b-form-select>
            </div>
          </div>
        </form>
      </template>
      <template slot="modal-footer">
        <p-button simple @click="closeModalClick">{{
          $t("closeBtn")
        }}</p-button>
        <p-button type="success" class="ml-auto" @click="addCardClick">{{
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
  name: "card-adding-modal",
  components: {
    VueTimepicker
  },
  data() {
    return {
      cardToAdd: {
        name: "",
        surname: "",
        employeesRoleId: 0,
        isEmployee: false,
        workingDayStartTime: {
          HH: "00",
          mm: "00"
        },
        workingDayEndTime: {
          HH: "00",
          mm: "00"
        }
      },
      selectionMode: false,
      options: []
    };
  },
  props: {
    showAddingModal: Boolean
  },
  computed: {
    ...mapState({
      status: state => state.cards.status,
      roles: state => state.roles.roles,
      rolesStatus: state => state.roles.status
    })
  },
  methods: {
    ...mapActions("cards", ["addCard"]),
    addCardClick() {
      if (this.cardToAdd.name && this.cardToAdd.surname) {
        let card = this.cardToAdd;

        card.workingDayStartTime =
          card.workingDayStartTime.HH + ":" + card.workingDayStartTime.mm;
        card.workingDayEndTime =
          card.workingDayEndTime.HH + ":" + card.workingDayEndTime.mm;
        if (!card.isEmployee) {
          card.employeesRoleId = this.roles.find(r => r.isAnonymous).id;
          card.workingDayStartTime = "00:00";
          card.workingDayEndTime = "00:00";
          this.addCard(card);
          this.closeModalClick();
        } else {
          this.addCard(card);
          this.closeModalClick();
        }
      }
    },
    closeModalClick() {
      this.$parent.showModal = false;
    }
  },
  watch: {
    showAddingModal: function(newV, oldV) {
      if (!newV) {
        this.cardToAdd = {
          name: "",
          surname: "",
          employeesRoleId: 0,
          isEmployee: false,
          workingDayStartTime: {
            HH: "00",
            mm: "00"
          },
          workingDayEndTime: {
            HH: "00",
            mm: "00"
          }
        };
        this.closeModalClick();
      } else {
        this.options = this.roles.map(r => {
          return {
            value: r.id,
            text: r.name == undefined ? "Anonymous" : r.name
          };
        });
      }
    }
  }
};
</script>
<style>
.display-time {
  border-radius: 4px;
  background-color: #fffcf5;
}
</style>

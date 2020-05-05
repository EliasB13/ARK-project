<template>
  <div class="row">
    <div class="col-xl-4 col-lg-5 col-md-6">
      <user-card
        :companyName="user.companyName"
        :login="user.login"
        :description="user.description"
        :address="user.address"
      ></user-card>
    </div>
    <div class="col-xl-8 col-lg-7 col-md-6">
      <edit-profile-form :user="user"></edit-profile-form>
    </div>
    <div id="overlay" v-if="showSpinner">
      <b-spinner class="spinner-scaled" label="loading"></b-spinner>
      <br />Loading
    </div>
  </div>
</template>
<script>
import { mapActions, mapState } from "vuex";
import EditProfileForm from "./UserProfile/EditProfileForm.vue";
import UserCard from "./UserProfile/UserCard.vue";

export default {
  components: {
    EditProfileForm,
    UserCard
  },
  computed: {
    ...mapState({
      user: state => state.account.user,
      status: state => state.account.status
    }),
    showSpinner() {
      return this.status.accountDataLoading || this.status.userUpdating;
    }
  },
  data() {
    return {
      model: {
        login: "",
        email: "",
        address: "",
        description: "",
        phone: "",
        companyName: ""
      },
      editingMode: false
    };
  },
  methods: {
    ...mapActions("account", ["getAccountData", "updateUser"])
  },
  mounted() {
    this.getAccountData();
  },
  watch: {
    user: function(newV, oldV) {
      if (newV) {
        this.user.address = this.user.address ? newV.address : "-";
        this.user.description = this.user.description ? newV.description : "-";
        this.user.phone = this.user.phone ? newV.phone : "-";
      }
    }
  }
};
</script>
<style>
</style>

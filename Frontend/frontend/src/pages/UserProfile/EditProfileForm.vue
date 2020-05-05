<template>
  <card class="card" title="Edit Profile">
    <div>
      <form @submit.prevent>
        <div class="row">
          <div class="col-md-6">
            <fg-input type="text" label="Username" placeholder="Username" v-model="localUser.login"></fg-input>
          </div>
          <div class="col-md-6">
            <fg-input type="text" label="Email" placeholder="Email" v-model="localUser.email"></fg-input>
          </div>
        </div>

        <hr class="my-2" />

        <div class="row">
          <div class="col-md-12">
            <fg-input
              type="text"
              label="Company"
              placeholder="Company"
              v-model="localUser.companyName"
            ></fg-input>
          </div>
        </div>

        <div class="row">
          <div class="col-md-6">
            <fg-input type="text" label="Address" placeholder="Address" v-model="localUser.address"></fg-input>
          </div>
          <div class="col-md-6">
            <fg-input type="text" label="Phone" placeholder="Phone" v-model="localUser.phone"></fg-input>
          </div>
        </div>

        <hr class="my-2" />

        <div class="row">
          <div class="col-md-12">
            <div class="form-group">
              <label>Description</label>
              <textarea
                rows="5"
                class="form-control border-input"
                placeholder="Here can be your description"
                v-model="localUser.description"
              ></textarea>
            </div>
          </div>
        </div>
        <div class="text-center">
          <p-button type="info" round @click="updateProfile">Update Profile</p-button>
        </div>
        <div class="clearfix"></div>
      </form>
    </div>
  </card>
</template>
<script>
import { mapActions, mapState } from "vuex";

export default {
  props: {
    user: Object
  },
  data() {
    return {
      localUser: {}
    };
  },
  computed: {
    ...mapState({
      status: state => state.account.status
    })
  },
  methods: {
    ...mapActions("account", ["getAccountData", "updateUser"]),
    updateProfile() {
      this.updateUser(this.localUser);
    }
  },
  watch: {
    user: function(newV, oldV) {
      if (newV) {
        this.localUser = this.user;
      }
    }
  }
};
</script>
<style></style>

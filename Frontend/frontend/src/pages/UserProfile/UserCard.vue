<template>
  <card class="card-user">
    <div slot="image">
      <img src="@/assets/img/user.png" class="card-img-top" alt="..." />
    </div>
    <div>
      <div class="author">
        <h4 class="title">
          {{ companyName }}
          <br />
          <a href="#">
            <small>@{{ login }}</small>
          </a>
        </h4>
      </div>
      <p class="description text-center">{{ address }}</p>
    </div>
    <hr />
    <div class="description text-center">{{ description }}</div>
  </card>
</template>
<script>
export default {
  props: {
    companyName: String,
    address: String,
    description: String,
    login: String
  },
  data() {
    return {};
  },
  methods: {}
};
</script>
<style>
</style>

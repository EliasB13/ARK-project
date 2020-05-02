<template>
  <span>{{ message }}</span>
</template>

<script>
export default {
  name: "notification-template",
  props: {
    message: String
  }
};
</script>

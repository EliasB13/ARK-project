<template>
  <table class="table" :class="tableClass">
    <thead>
      <slot name="columns">
        <th v-for="column in columns" :key="column.column">{{ column.displayName }}</th>
      </slot>
    </thead>
    <tbody>
      <tr
        v-for="(item, index) in data"
        :key="index"
        @click="rowClick(item, index)"
        :class="[
          isSelected(index) ? 'item-selected' : 'item-deselected',
          hoverable ? 'table-row-hover' : '',
          hoverable && !removingMode ? 'table-row-hover-bg-color' : '',
          item.isRestricted ? 'table-row-restricted' : '']"
      >
        <slot :row="item">
          <td v-for="(column, index) in columns" :key="index">{{ itemValue(item, column) }}</td>
        </slot>
      </tr>
    </tbody>
  </table>
</template>
<script>
export default {
  name: "paper-table",
  props: {
    columns: Array,
    data: Array,
    type: {
      type: String, // striped | hover
      default: "striped"
    },
    title: {
      type: String,
      default: ""
    },
    subTitle: {
      type: String,
      default: ""
    },
    removingMode: Boolean,
    hoverable: Boolean
  },
  data() {
    return {
      selected: []
    };
  },
  computed: {
    tableClass() {
      return `table-${this.type}`;
    }
  },
  methods: {
    hasValue(item, column) {
      return item[column.toLowerCase()] !== "undefined";
    },
    itemValue(item, column) {
      if (typeof item[column.column] === "boolean") {
        return item[column.column] ? "+" : "-";
      }
      if (column.isInner) {
        if (typeof item[column.outer][column.column] === "boolean") {
          return item[column.outer][column.column] ? "+" : "-";
        }
        return item[column.outer][column.column];
      }
      return item[column.column];
    },
    rowClick(item, index) {
      if (this.removingMode)
        this.$set(this.selected, index, !this.selected[index]);
      this.$emit("click", item);
    },
    isSelected(index) {
      return this.selected[index];
    },
    resetSelection() {
      this.selected = [];
    }
  },
  watch: {
    removingMode: {
      handler: function(newValue, oldValue) {
        if (!newValue) this.selected = [];
      },
      deep: true
    }
  }
};
</script>
<style>
.selected-row {
  background-color: gray !important;
}
</style>

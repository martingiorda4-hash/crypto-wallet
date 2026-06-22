<script setup>
// import { RouterLink, RouterView } from 'vue-router'
import { useRoute, RouterView } from 'vue-router';
import menuLateral from './components/menuLateral.vue';  

const route = useRoute()
</script>

<template>
  <div v-if="route.path === '/'">
    <RouterView/> 
  </div>

  <div v-else>
    <menuLateral/>
  </div>

</template>

<style scoped>
</style>

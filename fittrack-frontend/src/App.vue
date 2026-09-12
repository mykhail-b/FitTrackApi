<script setup lang="ts">
import { ref } from 'vue';
import { RouterLink, RouterView } from 'vue-router'

interface NavElement{
  value: string,
  path: string
}

const navElements = ref<NavElement[]>([
  { value: 'Home', path: '/' },
  { value: 'My workouts', path: '/workout' },
  { value: 'My statistics', path: '/me/statistics' },
  { value: 'Profile', path: '/me' },
])
</script>

<template>
  <header class="d-flex flex-wrap align-items-center justify-content-between py-3 mb-4 border-bottom">
  <nav class="nav">
    <RouterLink
      v-for="navElement in navElements"
      :key="navElement.path"
      :to="navElement.path"
      class="nav-link px-3 fs-5 text-dark"
      active-class="fw-semibold text-primary"
    >
      {{ navElement.value }}
    </RouterLink>
  </nav>

  <div class="d-flex gap-2">
    <RouterLink to="/login" class="btn btn-primary">Login</RouterLink>
    <RouterLink to="/register" class="btn btn-light border">Register</RouterLink>
  </div>
</header>
  <RouterView />
</template>

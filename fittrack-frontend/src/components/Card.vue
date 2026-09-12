<template>
    <div class="card">
        <img class="card-img-top" :src="image" alt="Card image cap">
        <div class="card-body">
            <h5 class="card-title">{{ title }}</h5>
            <p class="card-text">{{ cardBody }}</p>
            <a href="#" class="btn btn-primary">Go somewhere</a>
        </div>
    </div>
</template>

<script setup lang="ts">
interface CardBase {
    image: string
    title: string
    cardBody: string
}

defineProps<CardBase>()
</script>
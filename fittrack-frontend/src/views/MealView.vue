<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'

interface MealDto {
    id: string
    date: string
    totalCalories: number
    totalProtein: number
    totalFat: number
    totalCarbs: number
}

const meals = ref<MealDto[]>([])
const loading = ref(false)
const error = ref<string | null>(null)

const selectedDate = ref(new Date().toISOString().slice(0, 10))

const totals = computed(() => {
    return meals.value.reduce(
        (acc, meal) => {
            acc.calories += meal.totalCalories
            acc.protein += meal.totalProtein
            acc.fat += meal.totalFat
            acc.carbs += meal.totalCarbs
            return acc
        },
        { calories: 0, protein: 0, fat: 0, carbs: 0 }
    )
})

async function fetchMeals() {
    loading.value = true
    error.value = null

    try {
        const response = await fetch(
            `/api/v1/meal?date=${selectedDate.value}`,
            { credentials: 'include' }
        )

        if (!response.ok) {
            throw new Error(`Loading error: ${response.status}`)
        }

        meals.value = await response.json()
    } catch (e) {
        error.value = e instanceof Error ? e.message : 'Failed to load meals'
    } finally {
        loading.value = false
    }
}

async function deleteMeal(id: string) {
    if (!confirm('Delete meal?')) return

    try {
        const response = await fetch(`/api/v1/meal/${id}`, {
            method: 'DELETE',
            credentials: 'include'
        })

        if (!response.ok) {
            throw new Error(`Could not delete: ${response.status}`)
        }

        await fetchMeals()
    } catch (e) {
        error.value = e instanceof Error ? e.message : 'Deletion error'
    }
}

onMounted(fetchMeals)
</script>

<template>
    <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>Nutrition</h2>
            <router-link to="/meals/new" class="btn btn-primary">Add a meal</router-link>
        </div>

        <div class="mb-4 d-flex align-items-center gap-2">
            <label for="date" class="form-label mb-0">Date:</label>
            <input
                id="date"
                type="date"
                class="form-control w-auto"
                v-model="selectedDate"
                @change="fetchMeals"
            />
        </div>

        <div v-if="loading" class="text-center py-5">
            <div class="spinner-border" role="status"></div>
        </div>

        <div v-else-if="error" class="alert alert-danger">
            {{ error }}
        </div>

        <div v-else>
            <div class="card mb-4">
                <div class="card-body">
                    <h5 class="card-title">Daily total</h5>
                    <div class="row text-center">
                        <div class="col-3">
                            <div class="fw-bold">{{ totals.calories.toFixed(0) }}</div>
                            <small class="text-muted">kcal</small>
                        </div>
                        <div class="col-3">
                            <div class="fw-bold">{{ totals.protein.toFixed(1) }}</div>
                            <small class="text-muted">proteins</small>
                        </div>
                        <div class="col-3">
                            <div class="fw-bold">{{ totals.fat.toFixed(1) }}</div>
                            <small class="text-muted">fats</small>
                        </div>
                        <div class="col-3">
                            <div class="fw-bold">{{ totals.carbs.toFixed(1) }}</div>
                            <small class="text-muted">carbs</small>
                        </div>
                    </div>
                </div>
            </div>

            <div v-if="meals.length === 0" class="text-center text-muted py-4">
                There are no recorded meals for this day.
            </div>

            <div class="list-group">
                <div class="list-group-item" v-for="meal in meals" :key="meal.id">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div>{{ meal.totalCalories.toFixed(0) }} kcal</div>
                            <small class="text-muted">
                                Protein: {{ meal.totalProtein.toFixed(1) }} g ·
                                Fat: {{ meal.totalFat.toFixed(1) }} g ·
                                Carbs: {{ meal.totalCarbs.toFixed(1) }} g
                            </small>
                        </div>
                        <div class="d-flex gap-2">
                            <router-link :to="`/meals/${meal.id}`" class="btn btn-sm btn-outline-primary">
                                Open
                            </router-link>
                            <button class="btn btn-sm btn-outline-danger" @click="deleteMeal(meal.id)">
                                Delete
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
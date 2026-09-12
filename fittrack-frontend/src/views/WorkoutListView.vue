<script setup lang="ts">
import { ref, onMounted } from 'vue'

interface WorkoutSetDto {
    exerciseId: string
    setNumber: number
    reps: number
    weight: number
}

interface WorkoutDto {
    id: string
    date: string
    notes: string
    sets: WorkoutSetDto[]
}

interface PagedListResponse<T> {
    items: T[]
    totalCount: number
    pageNumber: number
    pageSize: number
}

const workouts = ref<WorkoutDto[]>([])
const loading = ref(false)
const error = ref<string | null>(null)

const pageNumber = ref(1)
const pageSize = ref(10)
const totalCount = ref(0)

const totalPages = () => Math.max(1, Math.ceil(totalCount.value / pageSize.value))

async function fetchWorkouts() {
    loading.value = true
    error.value = null

    try {
        const response = await fetch(
            `/api/v1/workout?pageNumber=${pageNumber.value}&pageSize=${pageSize.value}`,
            { credentials: 'include' }
        )

        if (!response.ok) {
            throw new Error(`Loading error: ${response.status}`)
        }

        const data: PagedListResponse<WorkoutDto> = await response.json()

        workouts.value = data.items
        totalCount.value = data.totalCount
    } catch (e) {
        error.value = e instanceof Error ? e.message : 'Failed to load workouts'
    } finally {
        loading.value = false
    }
}

async function deleteWorkout(id: string) {
    if (!confirm('Delete workout?')) return

    try {
        const response = await fetch(`/api/v1/workout/${id}`, {
            method: 'DELETE',
            credentials: 'include'
        })

        if (!response.ok) {
            throw new Error(`Could not delete: ${response.status}`)
        }

        await fetchWorkouts()
    } catch (e) {
        error.value = e instanceof Error ? e.message : 'Deletion error'
    }
}

function nextPage() {
    if (pageNumber.value < totalPages()) {
        pageNumber.value++
        fetchWorkouts()
    }
}

function prevPage() {
    if (pageNumber.value > 1) {
        pageNumber.value--
        fetchWorkouts()
    }
}

function formatDate(date: string) {
    return new Date(date).toLocaleDateString()
}

onMounted(fetchWorkouts)
</script>

<template>
    <div class="container py-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>My workouts</h2>
            <router-link to="/workouts/new" class="btn btn-primary">Add workout</router-link>
        </div>

        <div v-if="loading" class="text-center py-5">
            <div class="spinner-border" role="status"></div>
        </div>

        <div v-else-if="error" class="alert alert-danger">
            {{ error }}
        </div>

        <div v-else>
            <div v-if="workouts.length === 0" class="text-center text-muted py-4">
                There are no training sessions for now.
            </div>

            <div class="list-group">
                <div class="list-group-item" v-for="workout in workouts" :key="workout.id">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <h5 class="mb-1">{{ formatDate(workout.date) }}</h5>
                            <p class="mb-1 text-muted">{{ workout.notes }}</p>
                            <small>{{ workout.sets.length }} sets</small>
                        </div>
                        <div class="d-flex gap-2">
                            <router-link :to="`/workouts/${workout.id}`" class="btn btn-sm btn-outline-primary">
                                Open
                            </router-link>
                            <button class="btn btn-sm btn-outline-danger" @click="deleteWorkout(workout.id)">
                                Delete
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <nav class="d-flex justify-content-between align-items-center mt-4">
                <button class="btn btn-outline-secondary" :disabled="pageNumber <= 1" @click="prevPage">
                    Back
                </button>
                <span>Page {{ pageNumber }} of {{ totalPages() }}</span>
                <button class="btn btn-outline-secondary" :disabled="pageNumber >= totalPages()" @click="nextPage">
                    Forward
                </button>
            </nav>
        </div>
    </div>
</template>
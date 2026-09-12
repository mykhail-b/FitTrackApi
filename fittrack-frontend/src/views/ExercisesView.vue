<script setup lang="ts">
import { ref, onMounted } from 'vue'

interface ExerciseShortResponse {
    id: string
    name: string
    category: string
    equipment: string
}

interface PagedListResponse<T> {
    items: T[]
    totalCount: number
    pageNumber: number
    pageSize: number
}

const exercises = ref<ExerciseShortResponse[]>([])
const loading = ref(false)
const error = ref<string | null>(null)

const pageNumber = ref(1)
const pageSize = ref(10)
const totalCount = ref(0)

const totalPages = () => Math.max(1, Math.ceil(totalCount.value / pageSize.value))

async function fetchExercises() {
    loading.value = true
    error.value = null

    try {
        const response = await fetch(
            `/api/v1/exercise?pageNumber=${pageNumber.value}&pageSize=${pageSize.value}`,
            { credentials: 'include' }
        )

        if (!response.ok) {
            throw new Error(`Loading error: ${response.status}`)
        }

        const data: PagedListResponse<ExerciseShortResponse> = await response.json()

        exercises.value = data.items
        totalCount.value = data.totalCount
    } catch (e) {
        error.value = e instanceof Error ? e.message : 'Failed to load exercises'
    } finally {
        loading.value = false
    }
}

function nextPage() {
    if (pageNumber.value < totalPages()) {
        pageNumber.value++
        fetchExercises()
    }
}

function prevPage() {
    if (pageNumber.value > 1) {
        pageNumber.value--
        fetchExercises()
    }
}

onMounted(fetchExercises)
</script>

<template>
    <div class="container py-4">
        <h2 class="mb-4">Exercises</h2>

        <div v-if="loading" class="text-center py-5">
            <div class="spinner-border" role="status"></div>
        </div>

        <div v-else-if="error" class="alert alert-danger">
            {{ error }}
        </div>

        <div v-else>
            <div class="row g-3">
                <div class="col-12 col-md-6 col-lg-4" v-for="exercise in exercises" :key="exercise.id">
                    <div class="card h-100">
                        <div class="card-body">
                            <h5 class="card-title">{{ exercise.name }}</h5>
                            <p class="card-text text-muted mb-1">{{ exercise.category }}</p>
                            <p class="card-text"><small class="text-muted">{{ exercise.equipment }}</small></p>
                        </div>
                    </div>
                </div>
            </div>

            <div v-if="exercises.length === 0" class="text-center text-muted py-4">
                No exercises found.
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
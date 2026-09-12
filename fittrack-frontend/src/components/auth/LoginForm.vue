<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits<{
    success: []
}>()

const email = ref('')
const password = ref('')
const loading = ref(false)
const error = ref<string | null>(null)

async function handleSubmit() {
    loading.value = true
    error.value = null

    try {
        const response = await fetch('/api/v1/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({
                email: email.value,
                password: password.value,
            }),
        })

        const data = await response.json()

        if (!response.ok) {
            throw new Error(data?.error ?? 'Could not sign in')
        }

        emit('success')
    } catch (e) {
        error.value = e instanceof Error ? e.message : 'Could not sign in'
    } finally {
        loading.value = false
    }
}
</script>

<template>
    <form @submit.prevent="handleSubmit">
        <div class="mb-3">
            <label for="login-email" class="form-label">Email</label>
            <input
                id="login-email"
                v-model="email"
                type="email"
                class="form-control"
                required
                autocomplete="email"
            />
        </div>

        <div class="mb-3">
            <label for="login-password" class="form-label">Password</label>
            <input
                id="login-password"
                v-model="password"
                type="password"
                class="form-control"
                required
                autocomplete="current-password"
            />
        </div>

        <div v-if="error" class="alert alert-danger py-2">
            {{ error }}
        </div>

        <button type="submit" class="btn btn-primary w-100" :disabled="loading">
            <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
            Sign In
        </button>
    </form>
</template>
<script setup lang="ts">
import { ref, computed } from 'vue'

const emit = defineEmits<{
    success: []
}>()

const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const loading = ref(false)
const error = ref<string | null>(null)

const passwordsMatch = computed(() => password.value === confirmPassword.value)

async function handleSubmit() {
    error.value = null

    if (!passwordsMatch.value) {
        error.value = 'Passwords do not match'
        return
    }

    loading.value = true

    try {
        const response = await fetch('/api/v1/auth/register', {
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
            throw new Error(data?.error ?? 'Registration failed')
        }

        emit('success')
    } catch (e) {
        error.value = e instanceof Error ? e.message : 'Registration failed'
    } finally {
        loading.value = false
    }
}
</script>

<template>
    <form @submit.prevent="handleSubmit">
        <div class="mb-3">
            <label for="register-email" class="form-label">Email</label>
            <input
                id="register-email"
                v-model="email"
                type="email"
                class="form-control"
                required
                autocomplete="email"
            />
        </div>

        <div class="mb-3">
            <label for="register-password" class="form-label">Password</label>
            <input
                id="register-password"
                v-model="password"
                type="password"
                class="form-control"
                required
                autocomplete="new-password"
            />
        </div>

        <div class="mb-3">
            <label for="register-confirm-password" class="form-label">Confirm password</label>
            <input
                id="register-confirm-password"
                v-model="confirmPassword"
                type="password"
                class="form-control"
                required
                autocomplete="new-password"
            />
            <div v-if="confirmPassword && !passwordsMatch" class="form-text text-danger">
                Passwords do not match
            </div>
        </div>

        <div v-if="error" class="alert alert-danger py-2">
            {{ error }}
        </div>

        <button type="submit" class="btn btn-primary w-100" :disabled="loading">
            <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
            Sign Up
        </button>
    </form>
</template>
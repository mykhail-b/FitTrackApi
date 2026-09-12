<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import LoginForm from '@/components/auth/LoginForm.vue'
import RegisterForm from '@/components/auth/RegisterForm.vue'
const router = useRouter()
const mode = ref<'login' | 'register'>('login')

function handleAuthSuccess() {
    router.push('/')
}
</script>

<template>
    <div class="container d-flex justify-content-center py-5">
        <div class="card" style="max-width: 420px; width: 100%;">
            <div class="card-body p-4">
                <ul class="nav nav-tabs mb-4">
                    <li class="nav-item">
                        <button
                            class="nav-link"
                            :class="{ active: mode === 'login' }"
                            @click="mode = 'login'"
                        >
                            Sign In
                        </button>
                    </li>
                    <li class="nav-item">
                        <button
                            class="nav-link"
                            :class="{ active: mode === 'register' }"
                            @click="mode = 'register'"
                        >
                            Sign Up
                        </button>
                    </li>
                </ul>

                <LoginForm v-if="mode === 'login'" @success="handleAuthSuccess" />
                <RegisterForm v-else @success="handleAuthSuccess" />
            </div>
        </div>
    </div>
</template>
import Auth from '@/views/Auth.vue'
import ExercisesView from '@/views/ExercisesView.vue'
import HomeView from '@/views/HomeView.vue'
import MealView from '@/views/MealView.vue'
import WorkoutListView from '@/views/WorkoutListView.vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path:"/workouts",
      name: 'workouts',
      component: WorkoutListView
    },
    {
      path: "/exercises",
      name: 'exercises',
      component: ExercisesView
    },
    {
      path: '/meals',
      name: 'meals',
      component: MealView
    },
    {
      path:'/auth',
      name:'auth',
      component: Auth
    }
  ],
})

export default router

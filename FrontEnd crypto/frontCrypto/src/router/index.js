import { createRouter, createWebHistory } from 'vue-router'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Login',
      component: () => import('../components/Login.vue'),
    },
    {
      path: '/Dashboard',
      component: () => import('../components/PaginaPrincipal.vue'),
    },
    {
      path: '/Operaciones',
      name: 'Operaciones',
      component: () => import('../components/NuevaTransaction.vue'),
    },
    {
      path: '/Operaciones/:id',
      name: 'OperacionesEditar',
      component: () => import('../components/NuevaTransaction.vue'),
    },
    {
      path: '/Historial',
      name: 'Historial',
      component: () => import('../components/TransactionsHistorial.vue'),
    },
    {
      path: '/EstadoActual',
      name: 'EstadoActual',
      component: () => import('../components/EstadoActual.vue'),
    },
    {
      path: '/Detalle/:id',
      name: 'Detalle',
      component: () => import('../components/DetalleTransactions.vue'),
    },
  ],
})

export default router

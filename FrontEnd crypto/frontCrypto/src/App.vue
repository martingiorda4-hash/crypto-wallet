<script setup>
import { RouterLink, RouterView } from 'vue-router'

import {
        HomeIcon,
        ShoppingCartIcon,
        ArrowsRightLeftIcon,
        ClockIcon,
        CurrencyDollarIcon,
        ArrowLeftStartOnRectangleIcon,
        DocumentCurrencyDollarIcon
    } from '@heroicons/vue/24/solid'
</script>

<template>
  
    
    <div class="contenedor-general">
      <div class="dashboard">
        <div class="logoApp">
          <div class="contenedor-icono">
            <DocumentCurrencyDollarIcon></DocumentCurrencyDollarIcon>
          </div>
          <span>Crypto Wallet</span>
        </div>

        <div class="btnDashboard">
          <RouterLink to="/" class="btn">
            <div class="contenedor-icono">
              <HomeIcon class="icon"/>
            </div>
            <sapan>Inicio</sapan>
          </RouterLink>
        </div>

        <div class="btnDashboard">
          <RouterLink to="/Operaciones" class="btn">
            <div class="contenedor-icono">
              <ShoppingCartIcon class="icon"/>
            </div>
            <span>Operaciones</span>
          </RouterLink>

          <div class="btnDashboard">
            <RouterLink to="/Historial" class="btn">
              <div class="contenedor-icono">
                <ClockIcon class="icon"/>
              </div>
              <span>Historial</span>
            </RouterLink>

            <div class="btnDashboard">
              <button class="btn" id="btnSalir">
                <div class="contenedor-icono">
                  <ArrowLeftStartOnRectangleIcon></ArrowLeftStartOnRectangleIcon>
                </div>
                <span>Salir</span>
              </button>
            </div>
          </div>
        </div>
      </div>
      
      
    </div>

  <RouterView />
</template>

<style scoped>
header{
  background-color: #1a1a2e;
  margin: 20px auto;
  width: 60%;
  border-radius: 50px;
  padding: 12px 30px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  box-shadow: 0 4px 15px rgba(255, 215, 0, 0.3);

}
nav{
  display: flex;
  gap: 30px;
}
nav ul{
  list-style: none;
  display:flex;
  gap: 20px;
  margin: 0;
  padding: 0;
}
nav a{
  color: #FFD700;
  text-decoration: none;
  font-size: 1rem;
  font-weight: bold;
}
nav a:hover{
  color:white;
}
/* header {
  line-height: 1.5;
  max-height: 100vh;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
  } */
</style>

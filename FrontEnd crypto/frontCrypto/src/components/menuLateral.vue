<script setup>
import {
        HomeIcon,
        ShoppingCartIcon,
        ArrowsRightLeftIcon,
        ClockIcon,
        CurrencyDollarIcon,
        ArrowLeftStartOnRectangleIcon,
        DocumentCurrencyDollarIcon
    } from '@heroicons/vue/24/solid'
   import { DocumentTextIcon } from '@heroicons/vue/24/solid'
</script>

<template>
  <div class="contenedor-general">
    <div class="dashboard">
      <div class="logoApp">
        <div class="contenedor-icono">
          <CurrencyDollarIcon style="color: red"/>
        </div>
        <span>Crypto Wallet</span>
      </div>

      <div class="btnDashboard">
        <RouterLink to="/" class="btn">
          <div class="contenedor-icono"><HomeIcon class="icon"/></div>
          <span>Inicio</span>
        </RouterLink>
      </div>

      <div class="btnDashboard">
        <RouterLink to="/Operaciones" class="btn">
          <div class="contenedor-icono"><ShoppingCartIcon class="icon"/></div>
          <span>Operaciones</span>
        </RouterLink>
      </div>

      <div class="btnDashboard">
        <RouterLink to="/Historial" class="btn">
          <div class="contenedor-icono"><ClockIcon class="icon"/></div>
          <span>Historial</span>
        </RouterLink>
      </div>
      
      <div class="btnDashboard">
        <RouterLink to="/EstadoActual" class="btn">
          <div class="contenedor-icono"><DocumentTextIcon class="icon"/></div>
          <span>Estado Actual</span>
        </RouterLink>
      </div>

      <div class="btnDashboard">
        <button class="btn" id="btnSalir">
          <div class="contenedor-icono"><ArrowLeftStartOnRectangleIcon/></div>
          <span>Salir</span>
        </button>
      </div>
    </div>
<div class="contendor-form">
    <RouterView />
</div>

  </div>
</template>

<style scoped>
    *{
        box-sizing: border-box;
    }

    .contenedor-general{
        height: 100vh;
        display: grid;
        grid-template-areas: 'dashboard form';
        grid-template-columns: 20% auto;
    }
    .contenedor-form{
        grid-area: form;
        background-color: #0F1820;
        padding: 40px;
    }
    .dashboard{
        grid-area: dashboard;
        background-color: #0f1f2f;
        display: grid;
        grid-template-rows: 20% 10% 10% 10% 10% 10% 10%;
    }
    .logoApp{
        width: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        color: #DBDDE7;
    }
    .logo{
        color: red;
        background-color: black;
        height: 45px;
        border-radius: 100%
    }
    .btnDashboard{
        margin-top: 20px;
        display: flex;
        justify-content: center;
    }
    .btn{
        width: 70%;
        color: #DBDDE7;
        background-color: #0f1f2f;
        border: none;
        border-radius: 15px;
        outline: none;
        cursor: pointer;
        display:flex;
        align-items: center;
    }
    .btn:hover {
        color: #0C131A;
        background-color: #DBDDE7;
        transform: translateY(-3px);
        transition: 0.2s;
    }
    .contenedor-icono{
        width: 30%;
        display: flex;
        justify-content: center;
    }
    .icon{
        width: 28px;
        height: 25px;
    }
    #btnSalir{
        transform: translateY(40px);
    }

</style>
<template>
    <h1 class="titulo">Bienvenido a tu billetera virtual</h1>
</template>

<script setup>

</script>



<style scoped>
.titulo{
font-size: 3rem;
text-align: center;
margin-top: 50px;
animation: aparecer 1s ease-out;
}
@keyframes aparecer{
from{
    opacity: 0;
    transform: translateY(-20px);
}
to{
    opacity: 1;
    transform: translateY(0);
}
}
</style>
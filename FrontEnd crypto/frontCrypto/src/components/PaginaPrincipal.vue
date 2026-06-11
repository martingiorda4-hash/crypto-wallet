<template>
    <div class="contendedor-usuario">
        <div class="logoUsuario">
            {{ role }}
            <UserIcon class="iconUser"></UserIcon>
            <button class="btn-cerrarSesion" type="button" @click="cerrarSesion">
                Cerrar sesión
            </button>
        </div>

    </div>
    <div class="dashboard-contenedor">
        <h1 class="titulo">Dashboard</h1>
        <h3 class="subtitulo">¡Bienvenido, {{ nombreUsuario }}!</h3>
        <p class="descripcion">Resumen general</p>

        <div class="tarjetas">
            <RouterLink to="/Operaciones" class="tarjeta">
                <ShoppingCartIcon style="color: #FFD700;" class="icon"/>
                <span>Nueva Operación</span>
            </RouterLink>

            <RouterLink style="color:blueviolet" to="/Historial" class="tarjeta">
                <ClockIcon class="icon"/>
                <span>Historial</span>
            </RouterLink>

            <RouterLink to="/EstadoActual" class="tarjeta">
                <CurrencyDollarIcon style="color: green;" class="icon"/>
                <span>Estado actual</span>
            </RouterLink>
        </div>
            <div class="estadisticas">
                <div class="card-estadistica">
                    <div class="encabezado">

                        <span class="tituloEstadisticas">Transacciones totales</span>
                        <ArrowsRightLeftIcon class="iconEstadistica"/>
                    </div>
                    <span class="valor">{{ cantidadTransacciones }}</span>
                </div>

                <div class="card-estadistica">
                    <div class="encabezado">
                        <span class="tituloEstadisticas">Valor total de la cartera</span>
                        <CurrencyDollarIcon class="iconEstadistica"></CurrencyDollarIcon>
                    </div>

                    <span class="valor">{{ totalCartera.toLocaleString('es-AR', { style: 'currency', currency: 'ARS' }) }}</span>
                </div>
            </div>
        </div>
        
    </template>

    <script setup>
    import { ClockIcon, CurrencyDollarIcon, ShoppingCartIcon, UserIcon, ArrowsRightLeftIcon } from '@heroicons/vue/24/solid';
    
import axios from 'axios';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter()

const role = ref('')
const nombreUsuario = ref('')

const cerrarSesion = () => {
    localStorage.removeItem("role")
    localStorage.removeItem("username")
    router.push("/")
}


    const cantidadTransacciones = ref(0);
    const totalCartera = ref(0);

    onMounted(async () =>{
        role.value = localStorage.getItem("role")
        nombreUsuario.value = localStorage.getItem("username")
        
        if(!role.value){
            router.push("/")
            return
        }
        
        try{
            const response = await axios.get('https://localhost:7233/api/Transactions/dashboard')
            totalCartera.value = response.data.totalCartera;
            cantidadTransacciones.value = response.data.cantidadTransacciones;
        }catch(error){
            alert("Error al cargar el total de la cartera"+ error.message);
        }
        

    })  
    </script>



    <style scoped>
    .titulo{
    font-size: 3rem;
    font-weight: 700;
    margin-bottom: 10px;
    animation: aparecer 0.8s ease-out;
    }
    @keyframes aparecer{
        from{
            opacity: 0;
            transform: translateY(-15px);
        }
        to{
            opacity: 1;
            transform: translateY(0);
        }
    }
    .subtitulo{
        font-size: 1.5rem;
    color: #DBDDE7;
    margin-bottom: 5px;
    }
    .descripcion{
        color: #8a9bb0;
        font-size: 1rem;
    }
    .tarjetas{
        display: grid;
        grid-template-columns: repeat(3,180px);
        gap: 20px;
        margin-top: 10px;
    }
    .icon{
        width: 40px;
        height: 40px;
        margin-bottom: 10px;
        
    }
    .dashboard-contenedor{
        padding: 40px;
        color: #dbdde7;
        
        
    }

    .tarjeta{
        width: 180px;
        height: 120px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-decoration: none;
        color: white;
        border-radius: 15px;
        background-color: #162536;
        border: 1px solid #243447;
        transition: .2s;

    }
    .tarjeta:hover{
        transform: translateY(-5px);
        background-color: #1e3044;
    }
    .contendor-usuario{
        grid-area: form;
        background-color: #0F1820;
        display: grid;
        grid-template-columns: 85%;
        grid-template-rows: 20% 80%;
        justify-content: center;
    }

    .logoUsuario{
        color: #DBDDE7;
        display: flex;
        justify-content: flex-end;
        align-items: center;
        margin-right: 80px;
        margin-top: 15px;
    }
    .iconUser{
        width: 45px;
        height: 45px;
        padding: 5px;
        border: 2px solid #293643;
        border-radius: 100%;
    }
    .estadisticas{
        display: flex;
        gap: 20px;
        margin-top: 30px;
    }
    .card-estadistica{
        width: 280px;
        height: 140px;
        background-color: #162536;
        border: 1px solid #243447;
        border-radius: 15px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        padding: 20px;


    }
    .tituloEstadisticas{
        color: #9ca3af;
        font-size: 14px;
    }
    .valor{
        color:#BBDDE7;
        font-size: 28px;
        font-weight: bold;
        margin-top: 10px;
    }
    .encabezado{
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .iconEstadistica{
        width: 28px;
        height: 28px;
        color: #D4AF37;
    }
    .btn-cerrarSesion{
    display: inline-flex;
    align-items: center;
    gap: 8px; 
    padding: 10px 20px;
    background-color: #ff4d4f;
    color: white;
    font-family: sans-serif;
    font-weight: 600;
    text-decoration: none;
    border-radius: 25px; 
    border: none;
    cursor: pointer;
    transition: background-color 0.3s ease, transform 0.2s ease;
    margin-left: 10px;
    }
    .btn-cerrarSesion:hover{
        background-color: #d9363e;
        transform: translateY(-2px);
    }
    .btn-cerrarSesion:active{
        transform: translateY(0);
    }
    </style>
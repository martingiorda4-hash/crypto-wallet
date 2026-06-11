<template>
    <div v-if="transaction" class="container">
        <h2>Detalle de la transacción</h2>
        <div class="card">
            <div class="fila">
                <span class="label">ID de la transacción:</span>
                <span class="valor">{{ transaction.id }}</span>
            </div>
            <div class="fila">
                <span class="label">Tipo:</span>
                <span class="valor"
                    :class="transaction.action === 'purchase' ? 'compra' : 'venta'" >
                    {{ transaction.action === 'purchase' ? 'Compra' : 'Venta' }}
                </span>
            </div>

            <div class="fila">
                <span class="label">Criptomoneda:</span>
                <span class="valor"> {{ nombreCrypto(transaction.cryptoCode) }} ({{ transaction.cryptoCode.toUpperCase() }})</span>
            </div>
            <div class="fila">
                <span class="label">Cantidad:</span>
                <span class="valor">{{ transaction.cryptoAmount}}</span>
            </div>
            <div class="fila">
                <span class="label">Monto:</span>
                <span class="valor">{{transaction.money.toLocaleString('es-AR', {style: 'currency' , currency: 'ARS'})}}</span>
            </div>
            <div class="fila">
                <span class="label">Fecha y hora:</span>
                <span class="valor">{{ new Date(transaction.dateTime).toLocaleString() }}</span>
            </div>
        </div>
        <button class="btn-Volver" @click="router.push('/Historial')" >
            ← Volver
        </button>
    </div>
</template>


<script setup>
import axios from 'axios';
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { useRouter } from 'vue-router';

const router = useRouter()
const route = useRoute()
const transaction = ref(null)

const nombreCrypto = (code) => {
    if(code === 'btc') return 'Bitcoin'
    if(code === 'eth') return 'Ethereum'
    if(code === 'usdc')return 'USDC'
    if(code === 'sol')return 'Solana'
    if(code === 'ada')return 'Cardano'

    return code.toUpperCase()
}

onMounted (async () =>{
    const role = localStorage.getItem("role")
    if(!role){
        router.push('/')
        return
    }
    try{

        const response = await axios.get(`https://localhost:7233/api/Transactions/${route.params.id}`)
        transaction.value = response.data
    }catch(error){
        alert("Error al cargar el detalle de la transacción"+ error.message)
    }
})
</script>


<style scoped>
.container{
    padding: 40px 40px 40px 20px;
    color: #DBDDE7;
    margin-left: 0;
}
.card{
    background-color: #1a252d;
    border: 1px solid #293643;
    border-radius: 10px;
    padding: 20px;
    width: 600px;
    max-width: 700px;
    padding: 20px;
    margin-top: 15px;
}
.fila{
    display: flex;
    padding: 12px 0;
    gap: 10px;
}
.fila:last-child{
    border-bottom: none;
}
.label{
    color: #DBDDE7;
    font-size: 14px;
    width: 150px;
}
.valor{
    color: #DBDDE7;
    font-size: 14px;
    margin-left: 100px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
}
.btn-Volver{
    background-color: #293643;
    color: white;
    width: 100px;
    padding: 10px;
    border-radius: 5px;
    border: none;
    cursor: pointer;
    font-size: 0.9rem;
    margin-top: 10px;
}
.compra{
    background-color: rgba(40, 167, 69, 0.15); 
    color: #4ade80; 
    padding: 4px 10px;
    border-radius: 8px;
    font-size: 12px;
    font-weight: 500;
    display: inline-block;
}
.venta{
    background-color: rgba(220, 53, 69, 0.15);
    color: #f87171; 
    padding: 4px 10px;
    border-radius: 8px;
    font-size: 12px;
    font-weight: 500;
    display: inline-block;
}
</style>
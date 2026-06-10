<template>
    <h1 class="titulo-principal">Estado actual de la cartera</h1>
    <div class="card">
        <div class="table-container">

            <table>
                <thead>
                    <tr>
                        <th>Criptomoneda</th>
                        <th>Cantidad</th>
                        <th>Valor actual (ARS)</th>
                </tr>
            </thead>
            
            <tbody>
                <tr v-for="item in estado" :key= "item.cryptoCode">
                    <td>
                        {{nombreCrypto(item.cryptoCode) }} ({{ item.cryptoCode.toUpperCase() }})
                    </td>
                    
                    <td>
                        {{ item.cantidad }}
                    </td>
                    
                    <td>
                        {{ item.valorPesos.toLocaleString('es-AR', {style: 'currency', currency: 'ARS'}) }}
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="total">
            <span>TOTAL</span>
            <span>
                {{ totalPesos.toLocaleString('es-AR', {style: 'currency', currency: 'ARS'}) }}
            </span>
        </div>

        </div>
    </div>
</template>

<script setup>
import axios from 'axios';
import {ref} from 'vue';
import { onMounted } from 'vue';
const estado = ref([])
const totalPesos = ref(0)
const nombreCrypto = (code) => {
    if(code === 'btc') return 'Bitcoin'
    if(code === 'eth') return 'Ethereum'
    if(code === 'usdc')return 'USDC'
    if(code === 'sol')return 'Solana'
    if(code === 'ada')return 'Cardano'

    return code.toUpperCase()
}

onMounted(async () => {
    try{
        
        const response = await axios.get('https://localhost:7233/api/Transactions/estado');
        
        estado.value = response.data.cryptos;
        totalPesos.value = response.data.totalPesos;
    }catch(error){
        alert("Error al cargar el estado de la cartera"+ error.message);
    }
})
</script>


<style scoped>
.table-container{
    border: 1px solid rgba(255, 255, 255, 0.15);
    border-radius: 8px;
    overflow: hidden;
}
.card {
    padding: 20px;
    max-width: 800px;
    margin: 40px 0;
}
.titulo-principal{
    margin-top: 20px;
    margin-bottom: 15px;
    margin-left: 15px;
    font-size: 30px;
    color: #DBDDE7;
}
table {
    width: 100%;
    border-spacing: 0;
    margin-top: 0;
}

th {
    text-align: left;
    color: #6b7280;
    font-size: 16px;
    padding: 8px 12px;
    font-weight: normal;
}

td {
    padding: 14px 12px;
    font-size: 14px;
    color: white;
    border-bottom: 1px solid rgba(255,255,255,0.05);
}

.total {
    display: flex;
    justify-content: space-between;
    padding: 14px 12px;
    background-color: rgba(34, 197,94,0.1);
    border-radius: 0 0 8px 8px;
}
thead tr{
    background-color: rgba(255,255,255,0.05);
    border-radius: 0;
}
.total span{
    color: #22c55e;
    font-size: 16px;
} 


/* .card{
    background: linear-gradient(180deg, #0b1a3a, #081428);
    padding: 20px;
    border-radius: 14px;
    color: white;
    max-width: 600px;
    margin: 0 auto;
}
.card h2{
    margin-bottom: 10px;
    font-size: 20px;
}
table{
    width: 100%;
    margin-top: 10px;
    border-collapse: collapse;
}
th{
    text-align: left;
    color: #9ca3af;
    font-size: 13px;
    padding-bottom: 8px;
    padding-right: 1px solid rgba(255,255,255,0.1);
    font-weight: normal;
    padding: 10px 8px;
}
td{
    padding: 12px 8px;
    font-size: 14px;
    border-right: 1px solid rgba(255,255,255,0.1);
}
th:last-child, td:last-child{
    border-right: none;
}
tbody tr{
    border-top: 1px solid rgba(255, 255, 255, 0.1);
}
.total{
    margin-top: 10px;
    margin-top: 10px;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
    display: flex;
    justify-content: space-between;
    font-weight: bold;
    font-weight: bold;
}
 */
</style>
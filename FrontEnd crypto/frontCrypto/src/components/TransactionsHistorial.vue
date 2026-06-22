<template>
    <div>
    <h1 class="titulo-principal">Historial de movimientos</h1>
    <table class="table-transacciones">
    <thead>
        <tr>
        <th>Fecha</th>
        <th>Tipo</th>
        <th>Criptomoneda</th>
        <th>Cantidad</th>
        <th>Monto(ARS)</th>
        <th>Acciones</th>
        </tr>
    </thead>
    <tbody v-if="!loading">        

        <tr v-for="transaction in movimientos" :key="transaction.id">
            <td>{{ new Date(transaction.dateTime).toLocaleString() }}</td>
            <td>
                <span :class="transaction.action === 'purchase' ? 'compra' : 'venta'" >
                    {{ transaction.action === 'purchase' ? 'Compra' : 'Venta' }}
                </span>
            </td>


            <td>{{ nombreCrypto(transaction.cryptoCode) }} ({{ transaction.cryptoCode.toUpperCase()}})</td>
            <td>{{ transaction.cryptoAmount }}</td>
            <td>{{ transaction.money.toLocaleString('es-AR', {style: 'currency', currency: 'ARS'})}}</td>
            <td>
            <button  id="ver" @click="router.push(`/Detalle/${transaction.id}`)">
                <i class="fa-solid fa-eye"></i>
            </button>
            <button v-if="role === 'admin'" id="editar" @click="router.push(`/Operaciones/${transaction.id}`)">
                <i class="fa-solid fa-pen-to-square"></i>
            </button>
            <button v-if="role === 'admin'" id="eliminar" @click="abrirModal(transaction.id)">
                <i class="fa-solid fa-trash-can"></i>
            </button>


            </td>
        </tr>
    </tbody>
    </table>
    <p v-if="loading">Cargando movimientos...</p>
    <p v-if="exito" style="color:green">{{ exito }}</p>
    </div>

    <!-- Modal de confirmación -->
    <div v-if="mostrarModal" class="modal">
        <div class="_modal">
            <p>¿Estás seguro de que quieres eliminar esta transacción?</p>
            <button id="modalcancelar" @click="cancelarEliminar">Cancelar</button>
            <button id="modaleliminar" @click="confirmarEliminar">Eliminar</button>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'


const router = useRouter()
const role = ref('')
const loading = ref(false)
const movimientos = ref([])
const mostrarModal = ref(false)
const transaccionEliminar = ref(null)
const exito = ref(null)

const nombreCrypto = (code) => {
    if(code === 'btc') return 'Bitcoin'
    if(code === 'eth') return 'Ethereum'
    if(code === 'usdc')return 'USDC'
    if(code === 'sol')return 'Solana'
    if(code === 'ada')return 'Cardano'

    return code.toUpperCase()
}

onMounted(async () => {
    role.value = localStorage.getItem("role")

    if(!role.value){
        router.push("/")
        return
    }

    loading.value= true
    try{
        const response = await axios.get('https://localhost:7233/api/Transactions')
        movimientos.value = response.data
    } catch (error) {
        alert('Error al cargar los movimientos:', error)
    } finally {
        loading.value = false
    }
})
const abrirModal = (id) => {
    transaccionEliminar.value = id
    mostrarModal.value = true
}

const confirmarEliminar = async () => {
    try{
        await axios.delete(`https://localhost:7233/api/Transactions/${transaccionEliminar.value}`)
        const response = await axios.get('https://localhost:7233/api/Transactions')
        movimientos.value =response.data
        mostrarModal.value = false
        exito.value = 'Transacción eliminada exitosamente.'
        setTimeout(() => {
            exito.value = ''
        }, 5000)
    } catch (error) {
        alert('Error al eliminar la transaccion:', error)
    }
}
const cancelarEliminar = () => {
    mostrarModal.value = false
    transaccionEliminar.value = null
}

</script>

<style scoped>
.titulo-principal{
    color: #DBDDE7;
    margin-top: 10px; 
    margin-left: 8px;
}
.table-transacciones {
    width: 80%;
    border-collapse: collapse;
    margin-top: 15px;
    background: #0f2233;
    border-radius: 10px;
    overflow: hidden;   
    padding-left: 20px;
}
div{
    padding-left: 20px;
}
.table-transacciones th{
    background-color: #162a40;
    color: white;
    padding: 12px;
    text-align: center;
}
.table-transacciones td {
    border-bottom: 1px solid #1e3a5f;
    padding: 10px;
    text-align: center;
    color: #ddd;
}
.table-transacciones tr:hover td {
    background-color: #111128;
    color: #00ff88;
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
button#editar{
    background-color: #1a1a2e;
    color: #FFD700;
    border: 1px solid #FFD700;
    padding: 5px 10px;
    margin-right: 5px;
    cursor: pointer;
    
    border-radius: 5px;
}
button#eliminar{
    background-color: #1a1a2e;
    color: #ff4444;
    border: 1px solid #ff4444;
    padding: 5px 10px;
    margin-right: 5px;
    cursor: pointer;
    border-radius: 5px;
}
button#ver{
    background-color: #1a1a2e;
    color: #4a90e2;
    border: 1px solid #4a90e2;
    padding: 5px 10px;
    margin-right: 5px;
    cursor: pointer;
    border-radius: 5px;
}
button#editar:hover{
    background-color: #FFD700;
    color: #1a1a2e;
}
button#eliminar:hover{
    background-color: #ff4444;
    color: #1a1a2e;
}
button#ver:hover{
    background-color: #4a90e2;
    color: #1a1a2e;
}

.modal{
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 999;
}
._modal{
    background-color: #1a1a2e;
    border: 1px solid #ff4444;
    padding: 30px;
    border-radius: 10px;
    text-align: center;
    color: white;
    min-width: 300px;
}
._modal p{
    margin-bottom: 20px;
    font-size: 1.1rem;
}

._modal button#modaleliminar{
    background-color:  #1a1a2e;
    color: #ff4444;
    border: 1px solid #ff4444;;
    padding: 10px 20px;
    margin-right: 10px;
    cursor: pointer;
    border-radius: 5px;
}
._modal button#modalcancelar{
    background-color: transparent;
    color: white;
    border: 1px solid #888;
    padding: 10px 20px;
    margin-right: 10px;
    cursor: pointer;
    border-radius: 5px;
}
._modal button#modaleliminar:hover{
    background-color: #ff4444;
    color: #1a1a2e;
}
._modal button#modalcancelar:hover{
    background-color: #333;
    color: white;
}
</style>
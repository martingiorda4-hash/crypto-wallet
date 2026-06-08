<template>
    <div class="container">

        <div class="card">
            <h1 style="margin-top: 10px; margin-left: 8px;">Nueva transacción</h1>
            <form @submit.prevent="CrearTransaccion">
                <select v-model="accion">
                    <option value="">Seleccione una accion</option>
                    <option value="purchase">compra</option>
                    <option value="sale">venta</option>
                </select>
                <select v-model="cryptoSeleccionada">
                    <option value="">Seleccione una criptomoneda</option>
                    <option v-for="crypto in cryptos" :key="crypto.id" :value="crypto.code" >
                        {{ crypto.name }}   
                    </option>
                </select>
                <input v-model="cantidadCrypto" type="number" step="0.00000001" placeholder="Cantidad de crypto"/>
                <input  v-model="money" v-if="accion === 'sale'" type="number" placeholder="Cantidad cobrada"/>
                <p v-if="loading">Consultando precio...</p>
                <button v-if="accion === 'purchase' && !loading" type="submit">Comprar</button>
                <button v-if="accion === 'sale' && !loading" type="submit">Vender</button>
            </form>
            <p v-if="exito" style="color: green;">{{ exito }}</p>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const accion = ref('')
const cryptos = ref([])
const cryptoSeleccionada = ref('')
const cantidadCrypto = ref('')
const money = ref('')
const exito = ref(null)
const loading = ref(false)

const CrearTransaccion = async () => {
    if(!accion.value || !cryptoSeleccionada.value || !cantidadCrypto.value || !fecha.value){
        alert('Por favor complete todos los campos')
        return
    }
    if(accion.value === 'sale' && !money.value){
        alert('Por favor ingrese la cantidad cobrada')
        return
    }
    if(parseFloat(cantidadCrypto.value) <= 0){
        alert('La cantidad de crypto debe ser mayor a 0')
        return
    }
    loading.value = true
    try{
        const response = await axios.post('https://localhost:7233/api/Transactions', {
            action: accion.value,
            cryptoCode: cryptoSeleccionada.value,
            cryptoAmount: parseFloat(cantidadCrypto.value) || 0,
            money: accion.value === 'sale' ? parseFloat(money.value) : 0
        })
        exito.value = 'Transacción creada exitosamente' + response.data
        accion.value = ''
        cryptoSeleccionada.value = ''
        cantidadCrypto.value = ''
        money.value = ''
    }catch(err){
        if(err.response && err.response.data){
            alert(err.response.data)        }
        else{

            alert('Error al crear la transacción' + err.message)
        }
}finally{
        loading.value = false
    }
}

onMounted(async () => {
    try{
        const response = await axios.get('https://localhost:7233/api/Transactions/cryptos')
        cryptos.value = response.data
    }catch(err){
        alert('Error al cargar las criptomonedas' + err.message)
    }
})
</script>



<style scoped>
.container{
    display: flex;
    justify-content: center;
    align-items: flex-start;
    min-height: 100vh;
    width: 100%;
    box-shadow: border-box ;
    margin-top: 100px;
}
.card{
    background-color: #242323;
    border-radius: 20px;
    padding: 20px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    width: 400px;
    max-width: 90%;           /* Evita que la tarjeta se corte en pantallas de celulares */
    box-sizing: border-box; 
    
}
form {
display: flex;
flex-direction: column;
gap: 15px;
}
input, select, button {
  margin-top: 10px;
  padding: 10px;
  border-radius: 10px;
}
button{
  background-color: green;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 7px;
  transition: background-color 0.3s ease;
}
button:hover {
  background-color: darkgreen;
  transform: scale(1.05);
}
</style>
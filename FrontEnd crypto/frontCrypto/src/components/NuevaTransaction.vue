<template>
    <div class="container">
            <h1 class="titulo-principal">{{ editando ? 'Actualización de transaccion' : 'Nueva transacción' }}</h1>
            <form class="form" @submit.prevent="CrearTransaccion()">
                <h3 class="titulo">Datos de la transaccion</h3>
            <div class="input">
                <h3>Acción</h3>
                <select class="inputForm" v-model="accion" required>
                    <option value="">Seleccione una accion</option>
                    <option value="purchase">compra</option>
                    <option value="sale">venta</option>
                </select>
            </div>
            <div class="input">
                <h3>Criptomoneda</h3>
                <select class="inputForm" v-model="cryptoSeleccionada" @change="obtenerPrecio" required>
                    <option value="">Seleccione una criptomoneda</option>
                    <option v-for="crypto in cryptos" :key="crypto.id" :value="crypto.code" >
                        {{ crypto.name }}   
                    </option>
                </select>

                <div v-if="accion === 'sale' && cryptoSeleccionada" class="disponible">
                    Disponible: {{ disponible }} {{ cryptoSeleccionada }}

                </div>
            </div>
            <div class="input">
                <h3>Cantidad</h3>
                <input class="inputForm" v-model="cantidadCrypto" type="number" step="0.00000001" placeholder="0.00"/>
            </div>
            <div class="resultado" v-if="accion === 'sale'">
                <label>Monto a cobrar (ARS)</label>
                <p>
                    {{ total.toLocaleString('es-AR', {
                        style: 'currency',
                        currency: 'ARS'
                    })}}
                </p>
            </div>

                <p v-if="loading">Consultando precio...</p>
                <div class="botones">
                    <button  v-if="!editando && accion === 'purchase' && !loading" type="submit" class="btn-compra">
                        Comprar
                    </button>
                    <button  v-if="!editando && accion === 'sale' && !loading" type="submit" class="btn-venta">
                        Vender
                    </button>
                </div>
                
            </form>
            <p v-if="exito" style="color: green;">{{ exito }}</p>
        </div>
        <div class="accionEditar">
            <button class="btn-guardarCambios" v-if="editando && !loading" @click="editarTransaccion()">Guardar cambios</button>
        </div>
        <div class="accionCancelar">
            <button class="btn-Cancelar" v-if="editando && !loading" @click="router.push('/Historial')">Cancelar</button>
        </div>
            
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'
import { useRoute } from 'vue-router'
import { useRouter } from 'vue-router'

const router = useRouter()
const editando = ref(false)
const route = useRoute()
const accion = ref('')
const fecha = ref('')
const cryptos = ref([])
const cryptoSeleccionada = ref('')
const cantidadCrypto = ref(0)
const precio = ref(0)
const total = computed(() => parseFloat(cantidadCrypto.value || 0) * parseFloat(precio.value || 0));
const exito = ref(null)
const loading = ref(false)
const estado = ref([])
const disponible = computed(() => { 
    if(!estado.value || !estado.value.length)return 0;

    const encontrado = estado.value.find(c => c.cryptoCode.toLocaleLowerCase() === cryptoSeleccionada.value.toLocaleLowerCase());
    
    if(encontrado){
        return encontrado.cantidad
    }else{
        
        return 0;
    }
});
const CrearTransaccion = async () => {
    if(!accion.value || !cryptoSeleccionada.value || !cantidadCrypto.value){
        alert('Por favor complete todos los campos')
        return
    }
    if(accion.value === 'sale' && total.value <= 0){
        alert('Monto inválido')
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
            money: accion.value === 'sale' ? parseFloat(total.value) : 0
        })
        exito.value = 'Transacción creada exitosamente' + response.data
        accion.value = ''
        cryptoSeleccionada.value = ''
        cantidadCrypto.value = ''
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
const editarTransaccion = async () =>{
    try{
        
        exito.value = null
        const response = await axios.patch(`https://localhost:7233/api/Transactions/${route.params.id}`,{
            action: accion.value,
            cryptoCode: cryptoSeleccionada.value,
            cryptoAmount: parseFloat(cantidadCrypto.value),
            money: precio.value ? parseFloat(total.value) : 0,
            dateTime: fecha.value
        })
        alert("Transacción editada exitosamente")+ response.data
        router.push('/Historial')
    }catch(error){
        alert("Error al editar una transacción"+ error.message)
    }
}
onMounted(async () =>{
    try{
        //1. Verifica si está con un rol, sino lo manda al componente login
        const role = localStorage.getItem("role")

        if(!role){
            router.push("/")
            return
        }

        //3. cargar las cryptos en el select
        const responseCryptos = await axios.get('https://localhost:7233/api/Transactions/cryptos')
        cryptos.value = responseCryptos.data

        //4. cargar estado para mostrar saldo disponible a la hora de vender
        const responseEstado = await axios.get('https://localhost:7233/api/Transactions/estado');
        estado.value = responseEstado.data.cryptos;
        
        //5. si hay id, es edicion
        if(route.params.id){
            editando.value = true
            const cryptoResponse = await axios.get(`https://localhost:7233/api/Transactions/${route.params.id}`)
                accion.value = cryptoResponse.data.action
                cryptoSeleccionada.value = cryptoResponse.data.cryptoCode
                cantidadCrypto.value = cryptoResponse.data.cryptoAmount
                fecha.value = cryptoResponse.data.dateTime
        }



    }catch(error){
        alert("Error al cargar los datos!"+ error.message)
    }
})

const obtenerPrecio = async () =>{
    if(!cryptoSeleccionada.value)return
    const response = await axios.get(`https://localhost:7233/api/Transactions/precio/${cryptoSeleccionada.value}`);
    precio.value = parseFloat(response.data.precio)
}

</script>



<style scoped>
.titulo-principal{
    color: #DBDDE7;
}
.container{
    padding: 40px;
}
form {
    width: 60%;
    border: 3px solid #293643;
    border-radius: 10px;
    background-color: #1a252d;
    display: flex;
    flex-direction: column;
    padding: 20px;
}
.input{
    display: grid;
    grid-template-areas: 'texto' 'input';
    margin: 0px 20px 0px 20px;
}
.titulo{
    font-size: 25px;
    margin-left: 20px;
    
}
h3{
    grid-area: texto;
    color: #DBDDE7;
    width: 100%;
    margin: 0px auto;
    padding: 20px 0px 5px 0px;
    font-size: 15px;
}
h1{
    margin-top: 10px;
    margin-left: 8px;
}
.inputForm{
    grid-area: input;
    width: 100%;
    max-width: 280px;
    font-size: 15px;
    padding: 10px;
    background-color: #0C131A;
    border: 2px solid #293643;
    outline: none;
    color: #DBDDE7;
    border-radius: 5px;
}
.botones{
    
    margin-top: 15px;
    
}
.botones button{
    width: 100%;
    max-width: 280px;
    padding: 10px;
    border-radius: 10px;
    color: white;
    /* background-color: #1050B8; */
    border: none;
    cursor: pointer;
    font-size: 1rem;
    margin-left: 19px;
    
}
.btn-compra{
    background-color: #16A34A;
}
.btn-compra:hover{
    background-color: #15803D;
}
.btn-venta{
    background-color: #DC2626;
}
.btn-venta:hover{
    background-color: #B91C1C;
}
.inputForm:invalid{
    color: #8a9bb0;
}
.resultado{
    margin-top: 15px;
}
.resultado p{
    height: 38px;
    display: flex;
    align-items: center;
    background-color: #0f2233;
    padding: 8px;
    border-radius: 6px;
    color: white;
    margin-top: 5px;
    font-size: 14px;
    width: 57%;
}
.disponible{
    background-color: rgba(37, 99, 235, 0.15); 
    color:  #60a5fa; 
    padding: 8px 10px;
    border-radius: 8px;
    font-size: 15px;
    font-weight: 500;
    display: inline-block;
    width: 100%;
    max-width: 280px;

}
.accionEditar{
    
    width: 60%;
    display: flex;
    justify-content: flex-end;
    margin-top: -30px;

}
.btn-guardarCambios{
    max-width: 250px;
    width: 100%;
    padding: 10px;
    border-radius: 10px;
    color: white;
    background-color: #1050B8;
    border: none;
    cursor: pointer;
    font-size: 1rem;
    height: 40px;
}
.accionCancelar{
    width: 60%;
    display: flex;
    justify-content: flex-start;
    margin-top: -40px;
    margin-left: 39px;
}   
.btn-Cancelar{
    background-color: #293643;
    color: white;
    width: 100px;
    padding: 10px;
    border-radius: 5px;
    border: none;
    cursor: pointer;
    font-size: 0.9rem;
    height: 40px;
}


</style>
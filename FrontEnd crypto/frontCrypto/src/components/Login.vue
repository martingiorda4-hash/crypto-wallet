<template>
  <div id="logeo">
    <div class="card-login">
      <form>
        <h5 class="card-title">Iniciar Sesión</h5>

        <div class="input-group">
        <span class="input-group-text">
            <UserIcon class="icon"/>
        </span>
        <input type="text" id="username" class="form-control login" placeholder="Usuario">
        </div>

        <div class="input-group">
        <span class="input-group-text">
            <LockClosedIcon class="icon"/>
        </span>
        <input type="password" id="clave" class="form-control login" placeholder="Contraseña">
        </div>

        <button class="btn-primary" type="button" @click="login">
            Ingresar
        </button>

    </form>
    </div>
</div>
</template>

<script setup>
import { UserIcon, LockClosedIcon } from '@heroicons/vue/24/solid'
import { useRouter } from 'vue-router';
import { onMounted } from 'vue';
const router = useRouter()

const login = () => {
    const username = document.getElementById("username").value
    const password = document.getElementById("clave").value
    
    //Validacion
    if(!username || !password){
        alert("Todos los campos son obligatorios")
        return
    }

    if(username === "martin.giorda" && password === "martinAdmin"){
        localStorage.setItem("role", "admin")
        localStorage.setItem("username", username)
    }else{
        localStorage.setItem("role", "user")
        localStorage.setItem("username", username)
    }

    router.push('/Dashboard')
}

onMounted(async () => {
    const role = localStorage.getItem("role")

    if(role){
        router.push("/Dashboard")
    }
    })
</script>

<style scoped>
#logeo {
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
}
.card-login {
    width: 100%;
    max-width: 450px;
    padding: 35px;
    border-radius: 20px;
    backdrop-filter: blur(12px);
    background: rgba(0, 0, 0, 0.25);
    color: #FFFFFF;
}

.card-title {
    text-align: center;
    margin-bottom: 18px;
    font-size: 22px;
    font-weight: 600;
    letter-spacing: 0.5px;
}

.input-group {
    display: flex;
    align-items: center;
    background: rgba(255, 255, 255, 0.25);
    border-radius: 8px;
    overflow: hidden;
    margin-bottom: 12px;
}

.input-group-text {
    padding: 10px 14px;
    color: #FFFFFF;
    font-size: 18px;
    background: transparent;
}

.form-control.login {
    flex: 1;
    border: none;
    background: transparent;
    padding: 10px;
    color: #FFFFFF;
    font-weight: 600;
}

.form-control.login::placeholder {
    color: #EEEEEE;
}

.form-control.login:focus {
    outline: none;
}


.btn-primary {
    width: 100%;
    margin-top: 15px;
    padding: 12px;
    border-radius: 10px;
    border: none;
    font-size: 17px;
    font-weight: 700;
    color: #FFFFFF;
    background: linear-gradient(135deg, #2563eb, #1e3a8a);
    cursor: pointer;
    transition: 0.3s ease;
}

.btn-primary:hover {
    transform: translateY(-2px);
    box-shadow: 0 9px 20px rgba(0, 0, 0, 0.25);
}

.icon{
    width: 20px;
    height: 20px;
    color: white;
}
</style>
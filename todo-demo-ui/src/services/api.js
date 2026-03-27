import axios from 'axios'
import { useEffect, useState } from 'react'

const API_URL = import.meta.env.VITE_API_URL || ''

const [user, setUser] = useState('')

const instance = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json'
    }
})

let token = null

useEffect(() => {
    try {
        if (user?.token) {
            localStorage.setItem('todo_auth', JSON.stringify(user))
        } else {
            localStorage.removeItem('todo_auth')
        }
        api.setToken(user?.token)
    } catch (e) {
        console.error('AuthProvider error', e)
    }
}, [user])

// helper to set token from AuthContext on init
const api = {
    instance,
    setToken,
    get: (url) => instance.get(url),
    post: (url, data) => instance.post(url, data),
    put: (url, data) => instance.put(url, data),
    delete: (url) => instance.delete(url)
}

export default api
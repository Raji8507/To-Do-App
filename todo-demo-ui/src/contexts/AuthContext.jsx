import React, { createContext, useContext, useEffect, useState } from 'react'
import authService from '../services/authService'
import api from '../services/api'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
    const [user, setUser] = useState(() => {
        const raw = localStorage.getItem('todo_auth')
        return raw ? JSON.parse(raw) : null
    })

    useEffect(() => {
        if (user?.token) {
            localStorage.setItem('todo_auth', JSON.stringify(user))
        } else {
            localStorage.removeItem('todo_auth')
        }
        // update axios default header
        api.setToken(user?.token)
    }, [user])

    const login = async (username, password) => {
        const resp = await authService.login({ username, password })
        // resp expected: { token, userId, username, role } (from API's AuthResponseDto)
        if (resp?.token) {
            const u = {
                token: resp.token,
                id: resp.userId,
                username: resp.username,
                role: resp.role
            }
            setUser(u)
        }
        return resp
    }

    const register = async (username, password, role) => {
        return await authService.register({ username, password, role })
    }

    const logout = () => {
        setUser(null)
    }

    return (
        <AuthContext.Provider value={{ user, login, logout, register }}>
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth() {
    return useContext(AuthContext)
}
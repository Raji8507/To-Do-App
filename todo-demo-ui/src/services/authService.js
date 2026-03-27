import api from './api'

const login = async ({ username, password }) => {
    const resp = await api.post('/api/auth/login', { username, password })
    return resp.data
}

const register = async ({ username, password, role }) => {
    const resp = await api.post('/api/auth/register', { username, password, role })
    return resp.data
}

export default { login, register }
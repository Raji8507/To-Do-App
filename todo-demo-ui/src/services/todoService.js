import api from './api'

// API expects /api/ToDos endpoints
const getAll = async () => {
    const resp = await api.get('/api/ToDos')
    return resp.data
}

const getById = async (id) => {
    const resp = await api.get(`/api/ToDos/${id}`)
    return resp.data
}

const create = async (payload) => {
    const resp = await api.post('/api/ToDos', payload)
    return resp.data
}

const update = async (id, payload) => {
    const resp = await api.put(`/api/ToDos/${id}`, payload)
    return resp.data
}

const remove = async (id) => {
    const resp = await api.delete(`/api/ToDos/${id}`)
    return resp.data
}

export default { getAll, getById, create, update, remove }
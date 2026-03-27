import React, { useEffect, useState } from 'react'
import { useAuth } from '../contexts/AuthContext'
import todoService from '../services/todoService'
import api from '../services/api'
import TodoCard from '../components/TodoCard'
import TodoForm from '../components/TodoForm'
import axios from 'axios'

export default function TodosPage() {
    const { user } = useAuth()
    const [todos, setTodos] = useState([])
    const [users, setUsers] = useState([])
    const isManager = user?.role === 'Manager'
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState('')

    useEffect(() => {
        load()
        if (isManager) {
            // load users list so manager can assign (simple approach: call /api/users if exists)
            // If API doesn't expose a /api/users endpoint, we can show only user ids or seed small list
            // For demo we attempt a generic endpoint; if it fails we ignore
            (async () => {
                try {
                    const resp = await api.get('/api/users') // optional endpoint
                    setUsers(resp.data)
                } catch (e) {
                    // ignore; assignment select will still allow blank
                }
            })()
        }
        // eslint-disable-next-line
    }, [])

    const load = async () => {
        setLoading(true)
        setError('')
        try {
            const data = await todoService.getAll()
            setTodos(data)
        } catch (err) {
            setError(err?.response?.data || 'Failed to load todos')
        } finally {
            setLoading(false)
        }
    }

    const handleCreate = async (payload) => {
        try {
            await todoService.create(payload)
            load()
        } catch (err) {
            alert(err?.response?.data || 'Create failed')
        }
    }

    const handleToggleComplete = async (todo) => {
        try {
            await todoService.update(todo.id, { isCompleted: !todo.isCompleted })
            load()
        } catch (err) {
            alert(err?.response?.data || 'Update failed')
        }
    }

    const handleDelete = async (id) => {
        if (!window.confirm('Delete todo?')) return
        try {
            await todoService.remove(id)
            load()
        } catch (err) {
            alert(err?.response?.data || 'Delete failed')
        }
    }

    return (
        <div>
            <h3>ToDos</h3>
            {error && <div className="alert alert-danger">{error}</div>}
            {isManager && <TodoForm onSubmit={handleCreate} users={users} />}
            {loading && <div>Loading...</div>}
            {!loading && todos.length === 0 && <div className="text-muted">No todos found.</div>}
            {todos.map(t => (
                <TodoCard
                    key={t.id}
                    todo={t}
                    onToggleComplete={handleToggleComplete}
                    onDelete={handleDelete}
                    isManager={isManager}
                />
            ))}
        </div>
    )
}
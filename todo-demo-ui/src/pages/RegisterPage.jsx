import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'

export default function RegisterPage() {
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [role, setRole] = useState('Employee')
    const [error, setError] = useState('')
    const { register } = useAuth()
    const navigate = useNavigate()

    const submit = async (e) => {
        e.preventDefault()
        setError('')
        try {
            await register(username, password, role)
            navigate('/login')
        } catch (err) {
            setError(err?.response?.data || 'Register failed')
        }
    }

    return (
        <div className="row justify-content-center">
            <div className="col-md-6">
                <h3>Register</h3>
                {error && <div className="alert alert-danger">{error}</div>}
                <form onSubmit={submit}>
                    <div className="mb-2">
                        <input className="form-control" placeholder="Username" value={username} onChange={e => setUsername(e.target.value)} required />
                    </div>
                    <div className="mb-2">
                        <input className="form-control" placeholder="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} required />
                    </div>
                    <div className="mb-2">
                        <select className="form-select" value={role} onChange={e => setRole(e.target.value)}>
                            <option value="Employee">Employee</option>
                            <option value="Manager">Manager</option>
                        </select>
                    </div>
                    <button className="btn btn-primary">Register</button>
                </form>
            </div>
        </div>
    )
}
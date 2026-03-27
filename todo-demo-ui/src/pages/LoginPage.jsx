import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'

export default function LoginPage() {
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const { login } = useAuth()
    const navigate = useNavigate()

    const submit = async (e) => {
        e.preventDefault()
        setError('')
        try {
            await login(username, password)
            navigate('/todos')
        } catch (err) {
            setError(err?.response?.data || 'Login failed')
        }
    }

    return (
        <div className="row justify-content-center">
            <div className="col-md-6">
                <h3>Login</h3>
                {error && <div className="alert alert-danger">{error}</div>}
                <form onSubmit={submit}>
                    <div className="mb-2">
                        <input className="form-control" placeholder="Username" value={username} onChange={e => setUsername(e.target.value)} required />
                    </div>
                    <div className="mb-2">
                        <input className="form-control" placeholder="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} required />
                    </div>
                    <button className="btn btn-primary">Login</button>
                </form>
            </div>
        </div>
    )
}
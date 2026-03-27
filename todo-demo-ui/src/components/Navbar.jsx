import React from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'

export default function Navbar() {
    const { user, logout } = useAuth()
    const nav = useNavigate()

    const doLogout = () => {
        logout()
        nav('/login')
    }

    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
            <div className="container">
                <Link className="navbar-brand" to="/">Todo Demo</Link>
                <div className="collapse navbar-collapse">
                    <ul className="navbar-nav me-auto">
                        {user && (
                            <li className="nav-item">
                                <Link className="nav-link" to="/todos">Todos</Link>
                            </li>
                        )}
                    </ul>

                    <ul className="navbar-nav ms-auto">
                        {!user && (
                            <>
                                <li className="nav-item"><Link className="nav-link" to="/login">Login</Link></li>
                                <li className="nav-item"><Link className="nav-link" to="/register">Register</Link></li>
                            </>
                        )}
                        {user && (
                            <>
                                <li className="nav-item nav-link">Hello, {user.username} ({user.role})</li>
                                <li className="nav-item">
                                    <button className="btn btn-outline-light btn-sm" onClick={doLogout}>Logout</button>
                                </li>
                            </>
                        )}
                    </ul>
                </div>
            </div>
        </nav>
    )
}
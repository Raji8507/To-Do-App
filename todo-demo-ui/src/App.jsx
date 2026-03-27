import React from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import Navbar from './components/Navbar'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import TodosPage from './pages/TodosPage'
import ProtectedRoute from './components/ProtectedRoute'

function App() {
    return (
        <>
            <Navbar />
            <div className="container mt-4">
                <Routes>
                    <Route path="/" element={<Navigate to="/todos" />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/register" element={<RegisterPage />} />
                    <Route
                        path="/todos"
                        element={
                            <ProtectedRoute>
                                <TodosPage />
                            </ProtectedRoute>
                        }
                    />
                </Routes>
            </div>
        </>
    )
}

export default App
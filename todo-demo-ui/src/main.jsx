import React from 'react'
import { createRoot } from 'react-dom/client'
import App from './App'
// temporarily remove BrowserRouter/AuthProvider to isolate
createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <div>hello</div>
    </React.StrictMode>
)
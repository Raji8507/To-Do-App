import React, { useState } from 'react'

export default function TodoForm({ onSubmit, users }) {
    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')
    const [dueDate, setDueDate] = useState('')
    const [assignedTo, setAssignedTo] = useState('')

    const submit = (e) => {
        e.preventDefault()
        const payload = {
            title,
            description,
            dueDate: dueDate ? new Date(dueDate).toISOString() : null,
            assignedToUserId: assignedTo ? parseInt(assignedTo) : null
        }
        onSubmit(payload)
        setTitle(''); setDescription(''); setDueDate(''); setAssignedTo('')
    }

    return (
        <form onSubmit={submit} className="mb-4">
            <div className="row g-2">
                <div className="col-md-4">
                    <input className="form-control" placeholder="Title" value={title} onChange={e => setTitle(e.target.value)} required />
                </div>
                <div className="col-md-4">
                    <input className="form-control" placeholder="Description" value={description} onChange={e => setDescription(e.target.value)} />
                </div>
                <div className="col-md-2">
                    <input className="form-control" type="date" value={dueDate} onChange={e => setDueDate(e.target.value)} />
                </div>
                <div className="col-md-2">
                    <select className="form-select" value={assignedTo} onChange={e => setAssignedTo(e.target.value)}>
                        <option value="">Assign to (optional)</option>
                        {users?.map(u => <option key={u.id} value={u.id}>{u.username} ({u.role})</option>)}
                    </select>
                </div>
            </div>
            <div className="mt-2">
                <button className="btn btn-primary">Create</button>
            </div>
        </form>
    )
}
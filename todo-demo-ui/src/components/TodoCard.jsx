import React from 'react'

export default function TodoCard({ todo, onToggleComplete, onDelete, isManager }) {
    return (
        <div className="card mb-2">
            <div className="card-body d-flex justify-content-between align-items-center">
                <div>
                    <h5 className="card-title mb-1">{todo.title}</h5>
                    <p className="card-text mb-1">{todo.description}</p>
                    <small className="text-muted">
                        Due: {todo.dueDate ? new Date(todo.dueDate).toLocaleString() : '—'} | AssignedTo: {todo.assignedToUserId ?? '—'} | CreatedBy: {todo.createdByUserId}
                    </small>
                </div>
                <div className="d-flex align-items-center gap-2">
                    <div className="form-check">
                        <input
                            className="form-check-input"
                            type="checkbox"
                            checked={todo.isCompleted}
                            onChange={() => onToggleComplete(todo)}
                            id={`chk-${todo.id}`}
                        />
                        <label className="form-check-label" htmlFor={`chk-${todo.id}`}>Done</label>
                    </div>
                    {isManager && (
                        <button className="btn btn-danger btn-sm" onClick={() => onDelete(todo.id)}>Delete</button>
                    )}
                </div>
            </div>
        </div>
    )
}
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import API_URL from '../config';

const Dashboard = () => {
    const navigate = useNavigate();
    const [result, setResult] = useState<number | null>(null);
    const [a, setA] = useState<number>(0);
    const [b, setB] = useState<number>(0);
    const [error, setError] = useState<string>('');

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (!token) {
            navigate('/');
        }
    }, [navigate]);

    const multiply = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`${API_URL}/math/multiply?a=${a}&b=${b}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.ok) {
                const data = await response.json();
                setResult(data.result);
            } else {
                setError('Failed to perform multiplication');
            }
        } catch (err) {
            console.error(err);
            setError('Something went wrong. Please try again.');
        }
    };

    return (
        <div>
            <h2>Dashboard Erol Aslan</h2>
            <div>
                <input
                    type="number"
                    value={a}
                    onChange={(e) => setA(Number(e.target.value))}
                    placeholder="Enter first number"
                />
                <input
                    type="number"
                    value={b}
                    onChange={(e) => setB(Number(e.target.value))}
                    placeholder="Enter second number"
                />
                <button onClick={multiply}>Multiply</button>
            </div>
            {result !== null && <p>Result: {result}</p>}
            {error && <p style={{ color: 'red' }}>{error}</p>}
        </div>
    );
};

export default Dashboard;

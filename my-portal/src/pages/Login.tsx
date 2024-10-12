import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import API_URL from '../config'; // Doğru URL'yi burada tanımlayın

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            const response = await fetch(`${API_URL}/auth/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, password }),
            });

            if (response.ok) {
                const data = await response.json();
                console.log('Token:', data.token); // Token'ı kontrol etmek için log
                localStorage.setItem('token', data.token); // Token'ı kaydet
                navigate('/dashboard'); // Başarılı giriş sonrası yönlendir
            } else {
                setError('Invalid username or password');
            }
        } catch (err) {
            console.error('Error:', err);
            setError('Something went wrong. Please try again.');
        }
    };

    return (
        <div style={{ textAlign: 'center' }}>
            <h2>Login</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                style={{ marginBottom: '10px', padding: '8px', width: '300px' }}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                style={{ marginBottom: '10px', padding: '8px', width: '300px' }}
            />
            <button onClick={handleLogin} style={{ padding: '10px 20px', cursor: 'pointer' }}>
                Login
            </button>
            <div style={{ marginTop: '20px' }}>
                <p>
                    Don't have an account? <Link to="/signup">Signup</Link>
                </p>
            </div>
        </div>
    );
};

export default Login;

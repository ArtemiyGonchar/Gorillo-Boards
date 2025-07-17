import {use, useEffect, useState} from "react";
import {login} from "../../api/identityApi.js";
import "./Login.css";
import {useNavigate} from "react-router-dom";
import gorillaLoginLogo from "../../assets/gorl3.png";
export default function LoginPage(){
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();


    const handleLogin = async (e) => {
        e.preventDefault();
        if (username.length < 3) return;
        if (password.length < 7) return;
        try{
            const response = await login(username, password);
            localStorage.setItem("token", response.data);
            navigate("/boards");
        }catch (error) {
            console.log(error);
        }
    };

    const isValid = username.length >= 3 && password.length >= 7;

    return (
        <>
            <div className='formContainer'>
                <h1 className='h1'>Gorillo-Boards</h1>
                <img src = {gorillaLoginLogo} />
                <input type='text'
                       placeholder='Username'
                       value={username}
                       onChange={(e) => setUsername(e.target.value)}
                       required={true}
                       className="input"
                />

                <input type='password'
                       placeholder='Password'
                       value={password}
                       onChange={(e) => setPassword(e.target.value)}
                       required={true}
                       className="input"
                />

                <button onClick={handleLogin} className={`button ${isValid ? 'button-valid' : ''}`} disabled={!isValid}>Login</button>
            </div>
        </>
    )
}
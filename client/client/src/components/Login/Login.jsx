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
        try{
            const response = await login(username, password);
            localStorage.setItem("token", response.data);
            console.log(response.data);
            navigate("/boards");
        }catch (error) {
            console.log(error);
        }
    };


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

                <button onClick={handleLogin} className='button'>Login</button>
            </div>
        </>
    )
}
import './Header.css';
import {Link, useNavigate} from 'react-router-dom';
import  gorilloLogo from '../../assets/gorl3.png'
import {useLocation} from "react-router-dom";

// useGetUser from "../../hooks/getUser.jsx";
export const Header = ({username}) => {
    const navigate = useNavigate();
    const logout = () => {
        localStorage.removeItem('token');
        navigate('/login');
    }

    const location = useLocation();
    const linkText = location.pathname === '/boards' ? 'Choose a board' : 'To my boards';

    return (
        <div className='header'>
            <div className='links'>
                <img src={gorilloLogo} style={{height: '55px' }} />
                <Link to='/boards' className='link'>{linkText}</Link>
            </div>
            <div className='links'>
                <span className='span'>{username}</span>
                <button className='button-link' onClick={logout}>Logout</button>
            </div>
        </div>
    )
}
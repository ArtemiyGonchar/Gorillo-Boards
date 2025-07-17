import './Header.css';
import {Link, useNavigate} from 'react-router-dom';
import  gorilloLogo from '../../assets/gorl3.png'
import {useLocation} from "react-router-dom";
import { AiFillQuestionCircle } from "react-icons/ai";
import {useEffect, useState} from "react";
import {user_is_admin} from "../../api/identityApi.js";
import { FaUserPlus } from "react-icons/fa";
import UserManagementDialog from "../User/UserManagementDialog.jsx";
import { MdOutlineAnalytics } from "react-icons/md";
// useGetUser from "../../hooks/getUser.jsx";
export const Header = ({username}) => {
    const navigate = useNavigate();
    const [isAdmin, setAdmin] = useState(false);
    const [isManagingUsers, setManagingUsers] = useState(false);
    useEffect(() => {
        const fetchData = async () => {
            const adminRes = await user_is_admin();
            if (adminRes.data) {
                setAdmin(true);
            }
        }
        fetchData()

    },[])

    const logout = () => {
        localStorage.removeItem('token');
        navigate('/login');
    }

    const location = useLocation();
    const linkText = location.pathname === '/boards' ? 'Choose a board' : 'To my boards';
    const handleCloseManaging = () => setManagingUsers(false);

    const handleNavigateToCharts = () => navigate('/charts');
    return (
        <>
            {isManagingUsers ? (
                <UserManagementDialog handleClose={handleCloseManaging}/>
            ): (
                <></>
            )}
            <div className='header'>


                <div className='links'>
                    <img src={gorilloLogo} style={{height: '55px' }} />
                    <Link to='/boards' className='link'>{linkText}</Link>
                </div>
                <div className='links'>
                    {isAdmin && (
                        <>
                            <FaUserPlus className='icon' onClick={(e) => setManagingUsers(true)}/>
                            <MdOutlineAnalytics className='icon' onClick={handleNavigateToCharts}/>
                        </>
                    )}
                    <span className='span'>{username}</span>
                    <button className='button-link' onClick={logout}>Logout</button>
                </div>
            </div>
        </>
    )
}
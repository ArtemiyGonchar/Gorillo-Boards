import './UserManagementDialog.css'
import {useEffect, useState} from "react";
import {create_user, delete_user, get_all_users} from "../../api/identityApi.js";
import useGetUser from "../../hooks/getUser.jsx";
export default function UserManagementDialog({handleClose}) {
    const [isCreating, setCreating] = useState(false);
    const [isDeleting, setDeleting] = useState(false);
    const [isSuccess, setSuccess] = useState(false);

    const [usernameCreate, setUsernameCreate] = useState('');
    const [displayName, setDisplayName] = useState('');
    const [password, setPassword] = useState('');
    const [role, setRole] = useState('');

    const [isDeletingUser, setDeletingUser] = useState(false);
    const [usernameDelete, setUsernameDelete] = useState('');
    const {user, loadingUser} = useGetUser();

    const [userlist, setUserlist] = useState([]);
    const Roles = {
        "Contributor": 0,
        "Admin": 1,
        "Dev Team": 2,
        "QA Team": 3,
        "Design Team": 4,
        "Executive": 5
    }

    const roleOptions = Object.keys(Roles);

    useEffect(() => {
        const fetchData = async () => {
            const usersRes = await get_all_users();
            setUserlist(usersRes.data);
        }
        fetchData();
    }, [])

    const handleCloseDialog =() => {
        handleClose();
    }

    const handleCreateUser = () => {
        setCreating(true);
        setDeleting(false);
        setSuccess(false);
    }

    const handleDeletingUser = () => {
        setDeleting(true);
        setCreating(false);
        setSuccess(false);
    }

    const handleSelectRole = async (e) => {
        setSuccess(false);
        if (e.target.value === '') {
            return;
        }
        setRole(e.target.value);
    }

    const handleSubmitCreate = async () => {
        if (usernameCreate === '' || usernameCreate.length < 4) return;
        if(displayName === '' || displayName.length < 4 ) return;
        if(password === '' || password.length < 7) return;
        if(role === '') return;
        try {
            await create_user(usernameCreate, displayName, password, role);
            setSuccess(true);
        } catch(e) {
            console.log(e);
            setSuccess(false);
        }
    }

    const handleSubmitDelete = async () => {
        if(usernameDelete === '') return;
        if(user['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] === usernameDelete) return;
        try{
            await delete_user(usernameDelete);
            const usersRes = await get_all_users();
            setUserlist(usersRes.data);
        } catch(e) {
            console.log(e);
        }
    }

    return(
        <>
            <div className='user-dialog-overlay'>
                <div className='user-dialog'>
                    <div className='user-dialog-header'>
                        <div className='user-dialog-header-title'>
                            User Management
                        </div>
                        <span onClick={handleCloseDialog}>X</span>
                    </div>

                    <div className='user-dialog-buttons-container'>
                        <button onClick={handleCreateUser}>Create user</button>
                        <button onClick={handleDeletingUser}>Delete user</button>
                    </div>

                    {isCreating && (
                        <>
                            <div className='user-dialog-body'>
                                <p>Creating new user:</p>
                                <div className='input-center'>
                                    <>Username:</>
                                    <input onChange={(e) => setUsernameCreate(e.target.value)}/>
                                </div>

                                <p></p>
                                <div className='input-center'>
                                    <>Display name:</>
                                    <input onChange={(e) => setDisplayName(e.target.value)}/>
                                </div>
                                <p></p>
                                <div className='input-center'>
                                    <>Password:</>
                                    <input onChange={(e) => setPassword(e.target.value)}/>
                                </div>
                                    <>Role:</>
                                    <select onChange={handleSelectRole}>
                                        <option value=''>Select role</option>
                                        {Object.keys(Roles).map(roleKey =>
                                            (<option key={roleKey} value={Roles[roleKey]}>{roleKey}</option>))
                                        }
                                    </select>
                                <p></p>
                                <button onClick={handleSubmitCreate}>Create</button>

                                {isSuccess && (
                                    <div className='success'>Success</div>
                                )}
                            </div>
                        </>
                    )}

                    {isDeleting && (
                        <>
                            <div className='user-dialog-body'>
                                <p>Deleting users:</p>
                                <>User:</>
                                <select onChange={(e) => {
                                    setUsernameDelete(e.target.value);
                                    setDeletingUser(true);
                                }}>
                                    <option value=''>Select role</option>
                                    {userlist.map(user =>
                                        (<option key={user.id} value={user.userName}>{user.userName}</option>))
                                    }
                                </select>
                                <p></p>
                                {isDeletingUser && (
                                    <>
                                        <button onClick={handleSubmitDelete}>Delete</button>
                                    </>
                                )}

                                {isSuccess && (
                                    <div className='success'>Success</div>
                                )}
                            </div>
                        </>
                    )}
                </div>

            </div>
        </>
    )
}
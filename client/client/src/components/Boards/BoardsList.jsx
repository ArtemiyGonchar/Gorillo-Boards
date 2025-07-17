import {useState, useEffect} from "react";
import {get_boards} from "../../api/boardsApi.js";
import useAuth from "../../hooks/useAuth.jsx";
import {Navigate, useNavigate} from "react-router-dom";
import {Header} from "../Header/Header.jsx";
import useGetUser from "../../hooks/getUser.jsx";
import './BoardList.css';
import {user_is_admin} from "../../api/identityApi.js";
import BoardDialog from "./BoardDialog.jsx";


export default function BoardsList() {
    const [boards, setBoards] = useState([]);
    const {isAuth, loading} = useAuth();
    const {user, loadingUser} = useGetUser();
    const [isAdmin, setAdmin] = useState(false);

    const [isManaging, setManaging] = useState(false);

    const navigate = useNavigate();

    useEffect(()  => {
        const fetchBoards = async () => {
            const response = await get_boards();
            setBoards(response.data);
            const adminRes = await user_is_admin();
            setAdmin(adminRes.data);
        }
        fetchBoards();
    }, []);

    if(loadingUser || loading) {
        return <div>loading...</div>;
    }

    if (!isAuth) {
        return <Navigate to="/login"/>
    }

    if (!user) {
        return <div>loading user...</div>
    }

    const clickOnBoard = (board) => {
        navigate(`/boards/${board.id}`);
    }

    const handleManageBoards = ()=> setManaging(true);
    const handleCloseManageBoards = () => setManaging(false);

    return(
        <>
            <Header username={user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]} />
            <div>
                {isManaging && (
                    <>
                        <BoardDialog closeDialog={handleCloseManageBoards}/>
                    </>
                )}
                <div>
                <ul className='board-list'>
                    {isAdmin ? (
                        <>
                            <div className='add-board' onClick={handleManageBoards}>Manage boards</div>
                        </>
                    ) : (
                        <>
                        </>
                    )}


                    {boards.map(board =>
                        <div
                            key={board.id}
                            className='li-boards'
                            onClick={() => clickOnBoard(board)}
                        >
                            {board.title}
                        </div>)}
                </ul>
                </div>
            </div>
        </>
    )
}
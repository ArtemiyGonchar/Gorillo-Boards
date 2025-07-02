import {useState, useEffect} from "react";
import {get_boards} from "../../api/boardsApi.js";
import useAuth from "../../hooks/useAuth.jsx";
import {Navigate, useNavigate} from "react-router-dom";
import {Header} from "../Header/Header.jsx";
import useGetUser from "../../hooks/getUser.jsx";
import './BoardList.css';


export default function BoardsList() {
    const [boards, setBoards] = useState([]);
    const {isAuth, loading} = useAuth();
    const {user, loadingUser} = useGetUser();
    const navigate = useNavigate();

    useEffect(()  => {
        const fetchBoards = async () => {
            const response = await get_boards();
            setBoards(response.data);
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
        console.log("clicking board", board);
        navigate(`/boards/${board.id}`);
    }



    return(
        <>
            <Header username={user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]} />
            <div>

                <ul className='board-list'>
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
        </>
    )
}
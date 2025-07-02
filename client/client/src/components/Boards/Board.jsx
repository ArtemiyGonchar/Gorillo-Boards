import {useNavigate, useParams} from "react-router-dom";
import {Header} from "../Header/Header.jsx";
import useGetUser from "../../hooks/getUser.jsx";
import {useEffect, useState} from "react";
import {get_board_by_id, has_access_to_board} from "../../api/boardsApi.js";
import {HeaderFilter} from "../Header/HeaderFilter.jsx";
import {HubConnectionBuilder} from "@microsoft/signalr";

export default function Board() {
    const params = useParams();
    const {user, loadingUser} = useGetUser();
    const [board, setBoard] = useState();

    console.log(user);
    const navigate = useNavigate();
    //console.log(params.boardId);

    useEffect(() => {
        const hasAccess = async () => {
            const response = await has_access_to_board(params.boardId);
            console.log("Access ", response);
        }

        const getBoard = async (boardId) => {
            const response = await get_board_by_id(params.boardId);
            console.log("board ", response.data);
            setBoard(response.data);
        }

        hasAccess().catch(() => navigate('/boards/')).then(() => getBoard());

        const connection = new HubConnectionBuilder()
            .withUrl("https://localhost:7007/workflowhub")
            .withAutomaticReconnect()
            .build();

        connection.start().then(() => {
            console.log("Connected to Hub");
            connection.invoke("JoinGroup", params.boardId);
        })
            .catch(error => console.error(error));

        connection.on("WorkflowUpdated", (boardId)  => {
            console.log("Workflow updated", boardId);
            // тут сделаю фетч
        });

    }, [params.boardId]);
/*
    const connectToHub = async () => {
        var connection = new HubConnectionBuilder()
            .withUrl("http://localhost:7007/workflowhub")
            .withAutomaticReconnect()
            .build();

        connection.start().then(() => {
            console.log("Connected to Hub");
            connection.invoke("JoinGroup", params.boardId);
        })
            .catch(error => console.error(error));

        connection.on("WorkflowUpdated", (boardId)  => {
            console.log("Workflow updated", boardId);
            // тут сделаю фетч
        });
    };
*/
    if(loadingUser) {
        return (
            <>
                <Header username="loading"/>
                <div>loading...</div>
            </>
        );
    }



    return(
        <>
            <Header username={user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]}/>
            <HeaderFilter board={board} />
            <div>123</div>
        </>
    );
}
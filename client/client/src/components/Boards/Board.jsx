import {useNavigate, useParams} from "react-router-dom";
import {Header} from "../Header/Header.jsx";
import useGetUser from "../../hooks/getUser.jsx";
import {useEffect, useRef, useState} from "react";
import {get_board_by_id, has_access_to_board} from "../../api/boardsApi.js";
import {HeaderFilter} from "../Header/HeaderFilter.jsx";
import {HubConnectionBuilder} from "@microsoft/signalr";
import State from "../State/State.jsx";
import {
    change_ticket_state,
    change_tickets_order,
    create_state,
    get_states_by_board,
    getTickets
} from "../../api/workflowApi.js";
import './Board.css';
import '../../styles/global.css'

import {
    closestCenter,
    closestCorners,
    DndContext,
    KeyboardSensor,
    PointerSensor,
    useSensor,
    useSensors
} from "@dnd-kit/core";

import {horizontalListSortingStrategy, SortableContext, sortableKeyboardCoordinates} from "@dnd-kit/sortable";
import TicketDialog from "../Ticket/TicketDialog.jsx";
import {TfiPlus} from "react-icons/tfi";

export default function Board() {
    const params = useParams();
    const {user, loadingUser} = useGetUser();
    const [board, setBoard] = useState();
    const [states, setStates] = useState([]);

    const navigate = useNavigate();

    const [ticketsByState, setTicketsByState] = useState({});
    const [filteredTickets, setFilteredTickets] = useState([]);
    const [openTicket, setOpenTicket] = useState(null);
    const [isFiltering, setIsFiltering] = useState(false);


    const sensors = useSensors(
        useSensor(PointerSensor),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates
        })
    );

    useEffect(() => {
        const hasAccess = async () => {
            const response = await has_access_to_board(params.boardId);
        }

        const getBoard = async (boardId) => {
            const response = await get_board_by_id(params.boardId);
            setBoard(response.data);
        }

        const fetchStates = async (boardId) => {
            const response = await get_states_by_board(params.boardId);
            setStates(response.data);
            return response.data;
        }

        const fetchTickets = async (statesData) => {
            const ticketsMap = {};
            for (const state of statesData) {
                const response = await getTickets(params.boardId, state.id);
                ticketsMap[state.id] = response.data;
            }
            setTicketsByState(ticketsMap);

        }

        const init = async () => {
            try
            {
                await hasAccess().catch(() => {
                    return navigate('/boards/');
                });
                await getBoard();
                const statesData = await fetchStates();
                //console.log(statesData);
                await fetchTickets(statesData);
            } catch(e) {
                console.error(e);
            }
        }

        init();
        const connection = new HubConnectionBuilder()
            .withUrl("https://localhost:7007/workflowhub")
            .withAutomaticReconnect()
            .build();

        connection.start().then(() => {
            connection.invoke("JoinGroup", params.boardId);
        })
            .catch(error => console.error(error));

        const reloadAllData = async () => {
            const [boardRes, statesRes] = await Promise.all([
                get_board_by_id(params.boardId),
                get_states_by_board(params.boardId)
            ]);

            setBoard(boardRes.data);
            setStates(statesRes.data);

            const ticketsMap = {};
            for (const state of statesRes.data) {
                const tickets = await getTickets(params.boardId, state.id);
                ticketsMap[state.id] = tickets.data;
            }
            setTicketsByState(ticketsMap);
        };
        connection.on("WorkflowUpdated", reloadAllData);
    }, [params.boardId]);

    const handleDragEnd = async (event) => {
        const {active, over} = event;

        const activeTicketData = active.data.current.ticket;
        const overTicketData = over.data.current.ticket;

        const isOverTicket = over.data.current.type ==='ticket';
        const isOverState = over.data.current.type === 'state';

        if (!(active.id === over.id)){
            if(isOverTicket){
                if(activeTicketData.stateId === overTicketData.stateId){
                    console.log("order before sending to data", overTicketData.order);

                    await  change_tickets_order(params.boardId, active.id, overTicketData.order);
                }
            }

        }
    }

    const handleDragOver = async (event) => {
        const {active, over} = event;

        const activeTicketData = active.data.current.ticket;
        const overTicketData = over.data.current.ticket;

        const isOverTicket = over.data.current.type ==='ticket';
        const isOverState = over.data.current.type === 'state';

        const fromStateId = activeTicketData.stateId;

        if (!(active.id === over.id)){

            if(isOverState){
                await change_ticket_state(params.boardId,active.id, over.id);
            }
            if(isOverTicket){
                if(!(activeTicketData.stateId === overTicketData.stateId)){
                    await change_ticket_state(params.boardId,active.id, overTicketData.stateId);
                }
            }
        }

    }

    const handleOpenTicket = async (ticket) => {
        setOpenTicket(ticket);
    }

    const handleCloseTicket = async () => {
        setOpenTicket(null);
    }

    const handleFilter = (filterType) => {
        let newFilteredTickets = {}
        states.map(state => {
            if(filterType === "requested"){
                newFilteredTickets[state.id] = ticketsByState[state.id].filter(
                    t => t.userRequestor === user['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
                );
                setIsFiltering(true);
            } else if(filterType === "assigned"){
                newFilteredTickets[state.id] = ticketsByState[state.id].filter(
                    t => t.userAssigned === user['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
                );
                setIsFiltering(true);
            } else if(filterType === ''){
                setFilteredTickets([]);
                setIsFiltering(false);
            } else {
                newFilteredTickets[state.id] = ticketsByState[state.id].filter(t => t.ticketLabelId === filterType);
                setIsFiltering(true);
            }

        });

        setFilteredTickets(newFilteredTickets);
    }

    const handleCreateState = async () => {
        try {
            await create_state(params.boardId, "New state");
        } catch (error) {
            console.log(error);
        }
    }
    if(loadingUser) {
        return (
            <>
                <Header username="loading"/>
                <div>loading...</div>
            </>
        );
    }

    // if(states.length === 0) {
    //     return (
    //         <>loading</>
    //     );
    // }

    return(
        <>
            <Header username={user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]}/>
            <HeaderFilter board={board} onFilterChange={handleFilter}/>

            {openTicket && (
                <TicketDialog ticket={openTicket} closeDialog={handleCloseTicket}/>
            )}

            <div className='states-wrapper'>
                <DndContext  onDragEnd={handleDragEnd} onDragOver={handleDragOver} sensors={sensors}>
                    <div className='states-container'>
                                {states.map(state => (
                                    <>
                                        {isFiltering ? (
                                            <State board={board} state={state} key={state.id} tickets={filteredTickets[state.id]} onTicketClick={handleOpenTicket} isFiltering={!isFiltering}/>
                                        ) : (
                                            <State board={board} state={state} key={state.id} tickets={ticketsByState[state.id]} onTicketClick={handleOpenTicket} isFiltering={!isFiltering}/>
                                        )}
                                    </>
                                ))}
                        <div className='state-add' onClick={handleCreateState}><TfiPlus/></div>
                    </div>
                </DndContext>
            </div>
        </>
    );
}

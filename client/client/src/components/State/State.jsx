import './State.css';
import Ticket from '../Ticket/Ticket.jsx'
import {change_state_order, delete_state, getTickets, rename_state} from "../../api/workflowApi.js";
import {useEffect, useState} from "react";
import { TfiPlus } from "react-icons/tfi";
import {rectSortingStrategy, SortableContext, useSortable, verticalListSortingStrategy} from "@dnd-kit/sortable";
import {CSS} from "@dnd-kit/utilities"
import {closestCorners, DndContext, useDroppable} from "@dnd-kit/core";
import { AiFillCaretLeft } from "react-icons/ai";
import { AiFillCaretRight } from "react-icons/ai";
import { AiOutlineClose } from "react-icons/ai";
import {add_ticket} from "../../api/workflowApi.js";

export default function State ({board, state, tickets =[], onTicketClick, isFiltering}) {
    const {setNodeRef, isOver} = useDroppable({id: state.id, data:
            {type: 'state',
                stateId: state.id,
                accepts: ['ticket'],
                disable: isFiltering
            }});

    const [isAddingTicket, setAddingTicket] = useState(false);
    const [ticketTitle, setTicketTitle] = useState('');

    const [isRenamingState, setRenamingState] = useState(false);
    const [renamedState, setRenamedState] = useState(state.title);
    const sortedTickets = [...tickets].sort((a, b) => a.order - b.order);

    const handleAddClick = () => {
        setAddingTicket(true);
    }

    const handleCancelClick = () => {
        setAddingTicket(false);
        setTicketTitle('');
    }

    const handleOkClick = async () => {
        if(!ticketTitle.trim()){
            return;
        }
        await add_ticket(state.boardId, ticketTitle, state.id);
        setAddingTicket(false);
        setTicketTitle('');
    }

    const handleChangeStateOrderLeft = async () => {
        if (state.order === 0) return;
        console.log(state.boardId, state.id, state.order - 1)
        await change_state_order(state.boardId, state.id, state.order - 1);
    }

    const handleChangeStateOrderRight = async () => {
        console.log(state.boardId, state.id, state.order + 1)
        await change_state_order(state.boardId, state.id, state.order + 1);
    }

    const handleRenameState = () => setRenamingState(true);
    const handleCancelRenameState = () => setRenamingState(false);
    const handleSubmitRename = async () => {
        try{
            await rename_state(state.boardId, state.id, renamedState);
            setRenamingState(false);
        } catch (error) {
            console.log(error);
        }
    }

    const handleDeleteState = async () => {
        try{
            await delete_state(state.boardId, state.id);
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <SortableContext id={state.id} items={sortedTickets.map(t => t.id)} strategy={rectSortingStrategy}>
        <div className='state' ref={setNodeRef} >
            <div className='state-header'>
                {isRenamingState ? (
                    <>
                        <div className='state-rename'>
                            <input value={renamedState} onChange={(e) => setRenamedState(e.target.value)}/>
                            <button onClick={handleSubmitRename}>Save</button>
                            <button onClick={handleCancelRenameState}>X</button>
                        </div>
                    </>
                ) : (
                    <>
                        <h2 className='state-header-name' onClick={handleRenameState}>{state.title} </h2>
                        <div>
                            <AiOutlineClose className='state-header-button' onClick={handleDeleteState}/>
                            <>   </>
                            <AiFillCaretLeft className='state-header-button' onClick={handleChangeStateOrderLeft}/>
                            <AiFillCaretRight className='state-header-button' onClick={handleChangeStateOrderRight}/>
                            <>   </>
                            <TfiPlus className='state-header-button' onClick={handleAddClick}/>
                        </div>
                    </>
                )}

            </div>

            {isAddingTicket && (
                <>
                    <div className='add-ticket-form'>
                        <input type="text" value={ticketTitle} onChange={(e) => setTicketTitle(e.target.value)} autoFocus/>
                        <div className='add-ticket-buttons'>
                            <button onClick={handleOkClick}>Ok</button>
                            <button onClick={handleCancelClick}>Cancel</button>
                        </div>
                    </div>
                </>
            )}

                <div className='state-list'>
                    {sortedTickets.map((ticket) => (
                        <Ticket key={ticket.id} ticket={ticket} onClickTicket={() => onTicketClick(ticket)} isFiltering={isFiltering}/>
                    ))}
                </div>

        </div>
        </SortableContext>
    );
}

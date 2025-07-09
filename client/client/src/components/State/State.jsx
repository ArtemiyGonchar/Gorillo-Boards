//
// import './State.css';
// import Ticket from '../Ticket/Ticket.jsx'
// import {getTickets} from "../../api/workflowApi.js";
// import {useEffect, useState} from "react";
//
// import {SortableContext, useSortable, verticalListSortingStrategy} from "@dnd-kit/sortable";
// import {CSS} from "@dnd-kit/utilities"
// import {closestCorners, DndContext, useDroppable} from "@dnd-kit/core";
//
// export default function State ({board, state}) {
//     const {attributes, listeners,
//         setNodeRef, transform, transition} = useSortable({id: state.id});
//
//     const {setNodeRef} = useDroppable({
//         id: state.id,
//         data: {stateId: state.stateId}
//     })
//
//     const style = {
//         transition,
//         transform: CSS.Transform.toString(transform)
//     };
//
//     //console.log( "state",state);
//     const [tickets, setTickets] = useState([]);
//     useEffect(() => {
//         const fetchTickets = async () => {
//             const response = await getTickets(board.id, state.id);
//             //console.log( "response: ",response.data);
//             setTickets(response.data);
//         }
//
//         fetchTickets();
//     }, [board]);
//
//     //console.log("tickets", tickets);
//
//     return (
//         <div className='state' ref={(node) => {setNodeRef(node); setSortableRef(node)} {...attributes} {...listeners} style={style} >
//             <div className='state-header'>
//                 <h2 className='state-header-name'>{state.title}</h2>
//             </div>
//             <div className='state-list'>
//                 <div className='state-list-sort'>
//                     <DndContext collisionDetection={closestCorners}>
//                         <SortableContext items={tickets.map(ticket => ticket.id)} strategy={verticalListSortingStrategy}>
//                             {tickets.map((ticket) =>
//                                 <Ticket key={ticket.id} ticket={ticket} />
//                             )}
//                         </SortableContext>
//                     </DndContext>
//                 </div>
//             </div>
//         </div>
//     );
// }





// import './State.css';
// import Ticket from '../Ticket/Ticket.jsx'
// import {getTickets} from "../../api/workflowApi.js";
// import {useEffect, useState} from "react";
//
// import {SortableContext, useSortable, verticalListSortingStrategy} from "@dnd-kit/sortable";
// import {CSS} from "@dnd-kit/utilities"
// import {closestCorners, DndContext} from "@dnd-kit/core";
//
// export default function State ({board, state}) {
//     const {attributes, listeners,
//         setNodeRef, transform, transition} = useSortable({id: state.id});
//
//     const style = {
//         transition,
//         transform: CSS.Transform.toString(transform)
//     };
//
//     //console.log( "state",state);
//     const [tickets, setTickets] = useState([]);
//     useEffect(() => {
//         const fetchTickets = async () => {
//             const response = await getTickets(board.id, state.id);
//             //console.log( "response: ",response.data);
//             setTickets(response.data);
//         }
//
//         fetchTickets();
//     }, [board]);
//
//     //console.log("tickets", tickets);
//
//     return (
//             <div className='state' ref={setNodeRef} {...attributes} {...listeners} style={style} >
//                 <div className='state-header'>
//                     <h2 className='state-header-name'>{state.title}</h2>
//                 </div>
//                 <div className='state-list'>
//                     <div className='state-list-sort'>
//                         <DndContext collisionDetection={closestCorners}>
//                             <SortableContext items={tickets.map(ticket => ticket.id)} strategy={verticalListSortingStrategy}>
//                                 {tickets.map((ticket) =>
//                                     <Ticket key={ticket.id} ticket={ticket} />
//                                 )}
//                             </SortableContext>
//                         </DndContext>
//                     </div>
//                 </div>
//             </div>
//   );
// }

import './State.css';
import Ticket from '../Ticket/Ticket.jsx'
import {getTickets} from "../../api/workflowApi.js";
import {useEffect, useState} from "react";
import { TfiPlus } from "react-icons/tfi";
import {rectSortingStrategy, SortableContext, useSortable, verticalListSortingStrategy} from "@dnd-kit/sortable";
import {CSS} from "@dnd-kit/utilities"
import {closestCorners, DndContext, useDroppable} from "@dnd-kit/core";
import {add_ticket} from "../../api/workflowApi.js";

export default function State ({board, state, tickets =[], onTicketClick}) {
    const {setNodeRef, isOver} = useDroppable({id: state.id, data: {type: 'state',stateId: state.id, accepts: ['ticket']}});

    const [isAddingTicket, setAddingTicket] = useState(false);
    const [ticketTitle, setTicketTitle] = useState('');

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

    return (
        <SortableContext id={state.id} items={sortedTickets.map(t => t.id)} strategy={rectSortingStrategy}>
        <div className='state' ref={setNodeRef} >
            <div className='state-header'>
                <h2 className='state-header-name'>{state.title} </h2>
                <TfiPlus className='state-header-button' onClick={handleAddClick}/>
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
                        <Ticket key={ticket.id} ticket={ticket} onClickTicket={() => onTicketClick(ticket)}/>
                    ))}
                </div>

        </div>
        </SortableContext>
    );
}

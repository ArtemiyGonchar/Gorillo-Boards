// import './Ticket.css';
// import { TfiAlignJustify } from "react-icons/tfi";
// import {useSortable} from "@dnd-kit/sortable";
// import {CSS} from "@dnd-kit/utilities";
//
// export default function Ticket(ticket){
//     //console.log(ticket.ticket.id);
//     const {attributes, listeners,
//         setNodeRef, transform, transition} = useSortable({id: ticket.ticket.id});
//
//     const style = {
//         transition,
//         transform: CSS.Transform.toString(transform)
//     };
//
//     return (
//         <div className='ticket-wrapper' ref={setNodeRef} {...attributes} {...listeners} style={style}>
//             <div className='ticket'>
//                 <div className='ticket-content'>
//                     <div className='ticket-title'><p>{ticket.ticket.title}</p></div>
//                     <TfiAlignJustify className='ticket-icon' />
//                 </div>
//             </div>
//         </div>
//     );
// }

import './Ticket.css';
import { TfiAlignJustify } from "react-icons/tfi";
import {useSortable} from "@dnd-kit/sortable";
import {CSS} from "@dnd-kit/utilities";
import {useState} from "react";

export default function Ticket({ticket, onClickTicket}) {
    //console.log(ticket.ticket.id);
    const {attributes, listeners,
        setNodeRef, transform} = useSortable({id: ticket.id,
    data: {
            type: 'ticket',
            ticket: {
                ...ticket,
                stateId: ticket.stateId,
            }
    }});

    const style = {
        transform: CSS.Transform.toString(transform)
    };

    return (
        <>
        <div className='ticket-wrapper' ref={setNodeRef}  style={style}>
            <div className='ticket' id={ticket.id}>
                <div className='ticket-content'>
                    <div className='ticket-title' {...attributes} {...listeners}><p>{ticket.title}</p></div>
                    <TfiAlignJustify className='ticket-icon' onClick={(e) =>{ e.stopPropagation(); onClickTicket(ticket)}} />
                </div>
            </div>
        </div>
        </>
    );
}

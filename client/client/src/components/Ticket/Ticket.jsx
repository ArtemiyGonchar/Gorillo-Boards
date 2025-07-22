import './Ticket.css';
import { TfiAlignJustify } from "react-icons/tfi";
import {useSortable} from "@dnd-kit/sortable";
import {CSS} from "@dnd-kit/utilities";
import {useEffect, useState} from "react";
import {get_label_by_id} from "../../api/workflowApi.js";
import {useParams} from "react-router-dom";
import {get_user} from "../../api/identityApi.js";

export default function Ticket({ticket, onClickTicket, isFiltering}) {
    const {attributes, listeners,
        setNodeRef, transform} = useSortable({id: ticket.id,
    data: {
            type: 'ticket',
            disable: isFiltering,
            ticket: {
                ...ticket,
                stateId: ticket.stateId,
            }
    }});
    const [label, setLabel] = useState('');
    const [hasLabel, setHasLabel] = useState(false);
    const [user, setUser] = useState('');
    const params = useParams();
    useEffect(() => {
        if(ticket.ticketLabelId !== null){
            const fetchData = async () => {
                const labelRes = await get_label_by_id(params.boardId, ticket.ticketLabelId);
                setLabel(labelRes.data.title);
                setHasLabel(true);
            }

            fetchData();
        }
        const fetchUser = async () => {
            const userRes = await get_user(ticket.userRequestor);
            setUser(userRes.data)
        }
        fetchUser();
    },[ticket])

    const style = {
        transform: CSS.Transform.toString(transform)
    };
    return (
        <>
        <div className='ticket-wrapper' ref={setNodeRef}  style={style}>
            <div className='ticket' id={ticket.id}>
                <div className='ticket-content'>
                    <div className='ticket-title' {...(isFiltering && attributes)} {...(isFiltering && listeners)}><p>{ticket.title}</p></div>
                    <TfiAlignJustify className='ticket-icon' onClick={(e) =>{ e.stopPropagation(); onClickTicket(ticket)}} />
                </div>
                    {hasLabel ? (
                        <div className='ticket-bottom'>
                            <div className='ticket-sub-status'>{label}</div>
                            <div className='ticket-sub-status'>{user.displayName}</div>
                        </div>
                    ) : (
                        <div className='ticket-bottom'>
                            <div></div>
                            <div className='ticket-sub-status'>{user.displayName}</div>
                        </div>
                    )}
            </div>
        </div>
        </>
    );
}

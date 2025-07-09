import './TicketDialog.css';
import {useEffect, useState} from "react";
import {
    assign_to_ticket,
    rename_ticket,
    start_work_on_ticket,
    end_work_on_ticket,
    is_ticket_in_progress, change_ticket_description
} from "../../api/workflowApi.js";
import {useParams} from "react-router-dom";
import {get_user} from "../../api/identityApi.js";
import { TfiFaceSmile } from "react-icons/tfi";
import { TfiFaceSad } from "react-icons/tfi";
export default function TicketDialog({ticket, closeDialog}) {
    const [editedTicket, setEditedTicket] = useState(ticket);
    const params = useParams();

    const [editTitle, setEditTitle] = useState(editedTicket.title);
    const [isEditingTitle, setEditingTitle] = useState(false);

    const [requestor, setRequestor] = useState('');
    const [assigner, setAssigner] = useState('');
    const [ticketInProgress, setTicketInProgress] = useState(false);

    const [editDescription, setEditDescription] = useState(editedTicket.description || '');
    const [isEditingDescription, setEditingDescription] = useState(false);

    async function fetchInfo() {
        try {
            console.log(editedTicket)
            const responseReq = await get_user(editedTicket.userRequestor);
            setRequestor(responseReq.data);
            if(editedTicket.userAssigned)
            {
                const responseAs = await get_user(editedTicket.userAssigned);
                setAssigner(responseAs.data);
            }
            const inProg = await is_ticket_in_progress(params.boardId, ticket.id);
            setTicketInProgress(inProg.data);

        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        fetchInfo();
    }, [ticket]);


    const handleEditTitle = () => {
        setEditingTitle(true);
        console.log(editedTicket);
    }

    const handleCancelEditTitle = () => {
        setEditingTitle(false);
    }

    const handleSaveTicket = async () => {
        editedTicket.title = editTitle;
        await rename_ticket(params.boardId, editedTicket.id, editTitle);
        setEditingTitle(false);
    }

    const handleAssignee = async () => {
        try{
            const response = await assign_to_ticket(params.boardId, editedTicket.id);
            const responseAs = await get_user(response.data.userId);
            setAssigner(responseAs.data);
        } catch (error) {
            console.log(error);
        }
    }

    const handleStartWorking = async () => {
        try{
            await start_work_on_ticket(params.boardId, editedTicket.id);
            setTicketInProgress(true);
        } catch (error) {
            console.log(error);
        }
    }

    const handleEndWorking = async () => {
        try{
            await end_work_on_ticket(params.boardId, editedTicket.id);
            setTicketInProgress(false);
        } catch (error) {
            console.log(error);
        }
    }

    const handleSaveDescription = async () => {
        editedTicket.description = editDescription;
        await change_ticket_description(params.boardId, editedTicket.id, editDescription);
        setEditingDescription(false);
    }

    const handleEditDescription = () => setEditingDescription(true);
    const handleCancelEditDescription = () => setEditingDescription(false);

    return (
        <>
            <div className='dialog'>
                <div className='dialog-header'>
                    {!isEditingTitle && (
                            <div >
                                <div className='dialog-header-title' onClick={handleEditTitle}>{editedTicket.title}</div>
                            </div>
                    )}

                    {isEditingTitle && (
                        <div>
                            <input value={editTitle} onChange={(e) => setEditTitle(e.target.value)}/>
                            <button className='dialog-header-title-save' onClick={handleSaveTicket}>Save</button>
                            <button className='dialog-header-title-cancel' onClick={handleCancelEditTitle}>X</button>
                        </div>
                    )}

                    <button onClick={closeDialog} className='dialog-close'>X</button>
                </div>

                <div className='dialog-body'>
                    <div className='dialog-user-container'>
                        <div className='dialog-user'><TfiFaceSmile /> Requestor: {requestor.displayName} </div>
                        {assigner
                            ? <div className='dialog-user' onClick={handleAssignee}><TfiFaceSad /> Assigner: {assigner.displayName}</div>
                            : <div className='dialog-user' onClick={handleAssignee}><TfiFaceSad /> Assigner: +</div>
                        }
                    </div>

                    <div className="dialog-timeflow">
                        {ticketInProgress
                            ? <div className='dialog-timeflow-button' onClick={handleEndWorking}>End working</div>
                            : <div className='dialog-timeflow-button' onClick={handleStartWorking}>Start working</div>
                        }
                    </div>

                    <div className='dialog-description'>
                        {!isEditingDescription && (
                            <div onClick={handleEditDescription}>
                                <p>{editedTicket.description || 'Click to add description'}</p>
                            </div>
                        )}
                        {isEditingDescription && (
                            <div>
                                <textarea
                                    value={editDescription}
                                    onChange={(e) => {
                                        setEditDescription(e.target.value);
                                        e.target.style.height = "auto";
                                        e.target.style.height = e.target.scrollHeight + "px";
                                    }}
                                    rows={4}
                                    cols={40}
                                />
                                <button onClick={handleSaveDescription} className='dialog-header-title-save'>Save</button>
                                <button onClick={handleCancelEditDescription} className='dialog-header-title-cancel'>X</button>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </>
    )
}
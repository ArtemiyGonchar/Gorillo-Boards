import './TicketDialog.css';
import {useEffect, useState} from "react";
import {assign_to_ticket, rename_ticket, start_work_on_ticket, end_work_on_ticket} from "../../api/workflowApi.js";
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

    async function fetchUser() {
        try {
            console.log(editedTicket.userRequestor)
            const responseReq = await get_user(editedTicket.userRequestor);
            setRequestor(responseReq.data);
            console.log(editedTicket.userAssigned)
            if(editedTicket.userAssigned)
            {
                const responseAs = await get_user(editedTicket.userAssigned);
                console.log(responseAs);
                setAssigner(responseAs.data);
            }

            //console.log(responseReq.data.displayName);

        } catch (error) {
            console.error("Failed to fetch user", error);
        }
    }

    useEffect(() => {
        fetchUser();
    }, [ticket]);


    const handleEditTitle = () => {
        setEditingTitle(true);
        console.log("Editing ticket...");
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
            console.log('Assigned ticket', response.data);
            // fetchUser();
            //setAssigner(assigner);
            const responseAs = await get_user(response.data.userId);
            console.log(responseAs.data.displayName);
            setAssigner(responseAs.data);
        } catch (error) {
            console.log(error);
        }
    }

    const handleStartWorking = async () => {
        try{
            await start_work_on_ticket(params.boardId, editedTicket.id);
        } catch (error) {
            console.log(error);
        }
    }

    const handleEndWorking = async () => {
        try{
            await end_work_on_ticket(params.boardId, editedTicket.id);
            console.log("end working ticket...");
        } catch (error) {
            console.log(error);
        }
    }
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
                        <div className='dialog-timeflow-button' onClick={handleStartWorking}>Start working</div>
                        <div className='dialog-timeflow-button' onClick={handleEndWorking}>End working</div>
                    </div>
                </div>
            </div>
        </>
    )
}
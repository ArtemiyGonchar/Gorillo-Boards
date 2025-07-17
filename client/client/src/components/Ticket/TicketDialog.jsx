import './TicketDialog.css';
import {useEffect, useState} from "react";
import {
    assign_to_ticket,
    rename_ticket,
    start_work_on_ticket,
    end_work_on_ticket,
    is_ticket_in_progress,
    change_ticket_description,
    get_labels_by_board,
    get_label_by_id,
    add_label_to_ticket,
    create_label, delete_label, get_timelogs_by_ticket, close_ticket, delete_ticket, add_timelog_to_ticket
} from "../../api/workflowApi.js";
import {useParams} from "react-router-dom";
import {get_user, user_is_admin} from "../../api/identityApi.js";
import { TfiFaceSmile } from "react-icons/tfi";
import { TfiFaceSad } from "react-icons/tfi";
import dayjs from "dayjs";

import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

export default function TicketDialog({ticket, closeDialog}) {
    const [editedTicket, setEditedTicket] = useState(ticket);
    const params = useParams();

    const [editTitle, setEditTitle] = useState(editedTicket.title);
    const [isEditingTitle, setEditingTitle] = useState(false);

    const [requestor, setRequestor] = useState('');
    const [assigner, setAssigner] = useState('');
    const [ticketInProgress, setTicketInProgress] = useState(false);
    const [ticketIsClosed, setTicketIsClosed] = useState(ticket.isClosed);
    const [isClosing, setClosing] = useState(false);

    const [editDescription, setEditDescription] = useState(editedTicket.description || '');
    const [isEditingDescription, setEditingDescription] = useState(false);

    const [labels, setLabels] = useState([]);
    const [selectedLabelId, setSelectedLabelId] = useState('');
    const [selectedLabel, setSelectedLabel] = useState('');
    const [editLabel, setEditLabel] = useState(false);
    const [isAddingLabel, setAddLabel] = useState(false);
    const [addingLabelTitle, setAddLabelTitle] = useState('');
    const [labelToDelete, setLabelToDelete] = useState('');

    const [timelogs, setTimelogs] = useState([]);
    const [timelogWorker, setTimelogWorker] = useState({});

    const [isDeleting, setDeleting] = useState(false);

    const [isAdmin, setAdmin] = useState(false);

    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    async function fetchInfo() {
        try {
            const responseReq = await get_user(editedTicket.userRequestor);
            setRequestor(responseReq.data);
            if(editedTicket.userAssigned)
            {
                const responseAs = await get_user(editedTicket.userAssigned);
                setAssigner(responseAs.data);
            }
            const inProg = await is_ticket_in_progress(params.boardId, ticket.id);
            setTicketInProgress(inProg.data);

            const labelsRes = await get_labels_by_board(params.boardId);
            if (editedTicket.ticketLabelId !== null){
                setSelectedLabelId(editedTicket.ticketLabelId);
                const labelRes = await get_label_by_id(params.boardId,editedTicket.ticketLabelId);
                setSelectedLabel(labelRes.data);
            }

            setLabels(labelsRes.data);

            const isAdminRes = await user_is_admin();
            setAdmin(isAdminRes.data);

            const timelogsRes = await get_timelogs_by_ticket(params.boardId, ticket.id);

            const formattedTimelogs = timelogsRes.data.map(log => ({
                ...log,
                startedAt: dayjs(log.startedAt).format('DD-MM-YY HH:mm'),
                endedAt: log.endedAt ? dayjs(log.endedAt).format('DD-MM-YY HH:mm') : null,
            }));

            setTimelogs(formattedTimelogs);

            const users = {};
            for (const log of timelogsRes.data) {
                    try {
                        const response = await get_user(log.userId);
                        users[log.userId] = response.data;
                    } catch (error) {
                        console.error(error);
                    }
            }
            setTimelogWorker(users);


        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        fetchInfo();
    }, [ticket]);


    const handleEditTitle = () => {
        setEditingTitle(true);
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

            const timelogsRes = await get_timelogs_by_ticket(params.boardId, ticket.id);

            const formattedTimelogs = timelogsRes.data.map(log => ({
                ...log,
                startedAt: dayjs(log.startedAt).format('DD-MM-YY HH:mm'),
                endedAt: log.endedAt ? dayjs(log.endedAt).format('DD-MM-YY HH:mm') : null,
            }));
            setTimelogs(formattedTimelogs);

            const users = {};
            for (const log of timelogsRes.data) {
                try {
                    const response = await get_user(log.userId);
                    users[log.userId] = response.data;
                } catch (error) {
                    console.error(error);
                }
            }
            setTimelogWorker(users);
        } catch (error) {
            console.log(error);
        }
    }

    const handleEndWorking = async () => {
        try{
            await end_work_on_ticket(params.boardId, editedTicket.id);
            setTicketInProgress(false);

            const timelogsRes = await get_timelogs_by_ticket(params.boardId, ticket.id);

            const formattedTimelogs = timelogsRes.data.map(log => ({
                ...log,
                startedAt: dayjs(log.startedAt).format('DD-MM-YY HH:mm'),
                endedAt: log.endedAt ? dayjs(log.endedAt).format('DD-MM-YY HH:mm') : null,
            }));
            setTimelogs(formattedTimelogs);

            const users = {};
            for (const log of timelogsRes.data) {
                try {
                    const response = await get_user(log.userId);
                    users[log.userId] = response.data;
                } catch (error) {
                    console.error(error);
                }
            }
            setTimelogWorker(users);
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

    const handleEditLabel = () => setEditLabel(true);

    const handleLabelChange = async (e) => {
        const newLabelId = e.target.value;

        if (newLabelId === selectedLabelId) {
            setEditLabel(false);
        };

        if (newLabelId === null || newLabelId === undefined || newLabelId === '') return;

        try {
            await add_label_to_ticket(params.boardId, newLabelId, editedTicket.id,);
            const newLabel = labels.find(label => label.id === newLabelId);
            setSelectedLabel(newLabel);
            setEditedTicket(prev => ({
                ...prev,
                ticketLabelId: newLabelId,
            }));
            setEditLabel(false);
        } catch (error) {
            console.log(error);
        };
    };
    const handleEditLabels = async () => setAddLabel(true);
    const handleCancelEditLabels = () => setAddLabel(false);
    const handleAddLabel = async () => {
        await create_label(params.boardId, addingLabelTitle);
        const labelsRes = await get_labels_by_board(params.boardId);
        setLabels(labelsRes.data);
        setAddLabelTitle('');
    }

    const handleDeleteLabel = async () => {
        if (labelToDelete === null ||  labelToDelete === undefined || labelToDelete === '') {
            return;
        }
        await delete_label(params.boardId, labelToDelete);
        const labelsRes = await get_labels_by_board(params.boardId);
        setLabels(labelsRes.data);
        setSelectedLabel('');
    }

    const handleClosing = async () => setClosing(true);
    const handleCancelClosing = async () => setClosing(false);
    const handleSubmitClosing = async () => {
        try{
            await close_ticket(params.boardId, ticket.id);
            setTicketIsClosed(true);
        } catch(error) {
            console.log(error);
        }
    };

    const handleDeleteTicket = async () => setDeleting(true);

    const handleCancelDeleteTicket = async () => setDeleting(false);

    const handleSubmitDeleteTicket  = async () => {
        try{
            await delete_ticket(params.boardId, ticket.id);
            closeDialog();
        } catch(error) {
            console.log(error);
        }
    };

    const handleAddTimeTimeLog = async () => {
        console.log(startDate,"????", endDate);
        try{
            if (startDate.getTime() === endDate.getTime()) return;
            if (startDate.getTime() > endDate.getTime()) return;
            if (startDate.getDay() > endDate.getDay()) return;
            await add_timelog_to_ticket(params.boardId, ticket.id, startDate, endDate);
            await fetchInfo();
        } catch (e) {
            console.log(e)
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
                    {ticketIsClosed ? (
                        <></>
                    ) : (
                        <div className="dialog-timeflow">
                            {ticketInProgress
                                ? <div className='dialog-timeflow-button' onClick={handleEndWorking}>End working</div>
                                : <div className='dialog-timeflow-button' onClick={handleStartWorking}>Start working</div>
                            }
                        </div>
                    )}


                    <div className="dialog-description">
                        <div>
                            Start working:
                            <DatePicker className='dialog-date' selected={startDate} onChange={(date) => setStartDate(date)}  showTimeSelect dateFormat="Pp"/>
                        </div>
                        <div>
                            End working:
                            <DatePicker className='dialog-date' selected={endDate} onChange={(date) => setEndDate(date)} showTimeSelect dateFormat="Pp" />
                        </div>
                        {(startDate.getTime() === endDate.getTime()) || (startDate.getTime() > endDate.getTime()) || (startDate.getDay() > endDate.getDay()) ? (
                            <></>
                        ) : (
                            <button onClick={handleAddTimeTimeLog} className='timelog-add'>Add time log</button>
                        )}
                    </div>


                    <div className='dialog-description'>
                        <div className='dialog-header'>Description:</div>
                        {!isEditingDescription && (
                            <div onClick={handleEditDescription}>
                                <p>{editedTicket.description || 'Click to add description +'}</p>
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

                    <div className='dialog-labels'>
                        {editLabel
                            ?
                            <select onChange={handleLabelChange} >
                                <option value="">Select sub-status</option>
                                {labels.map((label) => (
                                    <option key={label.id} value={label.id}>
                                        {label.title}
                                    </option>
                                ))}
                            </select>
                            :
                            <>
                                <div className="dialog-label-h">Sub-status:</div>
                                <div className="dialog-label" onClick={handleEditLabel}>{selectedLabel ? <div className='dialog-label-title'>{selectedLabel.title}</div> : "+"}</div>
                            </>
                        }
                        {isAdmin ?
                            <>
                                {isAddingLabel ? (
                                    <>
                                        <div className='dialog-label-add'>
                                            <input value={addingLabelTitle} onChange={(e) => setAddLabelTitle(e.target.value)}/>
                                            <button onClick={handleAddLabel}>Add</button>
                                        </div>

                                        <div className='dialog-label-delete'>
                                            <select onChange={(e) => setLabelToDelete(e.target.value)}>
                                                <option value="">Select sub-status</option>
                                                {labels.map((label) => (
                                                    <option key={label.id} value={label.id}>
                                                        {label.title}
                                                    </option>
                                                ))}
                                            </select>
                                            <button onClick={handleDeleteLabel}>Delete</button>
                                        </div>
                                        <button className='dialog-label-cancel' onClick={handleCancelEditLabels}>Cancel</button>
                                    </>
                                ): <div className='dialog-label-edit' onClick={handleEditLabels}>Edit sub-statuses</div>}

                            </>
                                : ''
                        }
                    </div>


                        {ticketIsClosed ? (
                            <></>
                        ): (
                            <>
                                {isClosing ? (
                                    <>
                                        <div className='dialog-timeflow'>
                                            <div className="dialog-confirm">
                                                <p>Are you sure you want to close this ticket?</p>
                                                <button onClick={handleSubmitClosing}>Yes</button>
                                                <button onClick={handleCancelClosing}>No</button>
                                            </div>
                                        </div>
                                    </>
                                ) : (
                                    <>
                                        <>
                                            <div className='dialog-timeflow'>
                                                <button className='dialog-close-ticket' onClick={handleClosing}>
                                                    CloseTicket
                                                </button>
                                            </div>
                                        </>
                                    </>
                                )}
                            </>
                        )}


                    {timelogs && (
                        <div className='dialog-timelogs-container'>
                            {timelogs.map((timelog) =>
                                (
                                    <>
                                        <div className='dialog-timelog'>
                                            <p>Time log:</p>
                                            {timelog.endedAt ? (
                                                <>
                                                    <div className='dialog-timelog-time'>Started at: {timelog.startedAt}</div>
                                                    <div className='dialog-timelog-time'>Ended at: {timelog.endedAt}</div>
                                                    {/*{getTimeLogWorker(timelog.userId)}*/}
                                                    <div className='dialog-timelog-user'>
                                                        Worker: {timelogWorker[timelog.userId]?.displayName}
                                                    </div>
                                                </>
                                            ) : (
                                                <>
                                                    <div className='dialog-timelog-in-work'>
                                                        <p>Currently working:</p>
                                                        <div className='dialog-timelog-time'>Started at: {timelog.startedAt}</div>
                                                        <div className='dialog-timelog-user'>
                                                            Worker: {timelogWorker[timelog.userId]?.displayName}
                                                        </div>
                                                    </div>
                                                </>
                                            )}
                                        </div>
                                    </>
                                ))}
                        </div>
                    )}

                    <div className='dialog-delete-container'>
                        {isDeleting ? (
                            <>
                                <div className='dialog-timeflow'>
                                    <div className="dialog-confirm">
                                        <p>Are you sure you want to delete this ticket?</p>
                                        <button onClick={handleSubmitDeleteTicket}>Yes</button>
                                        <button onClick={handleCancelDeleteTicket}>No</button>
                                    </div>
                                </div>
                            </>
                        ) : (
                            <>
                                <div className='dialog-delete-ticket' onClick={handleDeleteTicket}>Delete ticket</div>
                            </>
                        )}
                    </div>
                </div>
            </div>
        </>
    )
}
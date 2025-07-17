import axios from 'axios';
import {toast} from "react-toastify";
import apiConfig from './api-config.json';
const api = axios.create({
    baseURL: apiConfig.workflowApi,
    headers:{
        'Content-Type': 'application/json',
    },
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && typeof error.response.data === "string") {
            const raw = error.response.data;
            let cleanMsg = raw;
            if (raw.includes("System.Exception: ")) {
                cleanMsg = raw.split("System.Exception: ")[1]?.split('\n')[0];
                toast.error(cleanMsg);
            }
        } else {
            toast.error("Something went wrong");
        }
        return Promise.reject(error);
    }
);



export const getTickets = async (boardId,StateId) => {
    return api.post(`boards/${boardId}/states/get-tickets-by-state`, {StateId});
}

export const get_states_by_board = async (boardId) => {
    return api.get(`boards/${boardId}/states/get-states-by-board`);
}

export const change_state_order = async (boardId,stateId, orderTarget) => {
    return api.post(`boards/${boardId}/states/change-state-order`, {boardId, stateId, orderTarget});
}

export const change_ticket_state = async (BoardId,Id, StateId) => {
    return api.post(`boards/${BoardId}/states/change-ticket-state`, {BoardId,Id, StateId});
}

export const change_tickets_order = async (BoardId,Id, OrderTarget) => {
    return api.post(`boards/${BoardId}/states/change-ticket-order`, {BoardId,Id, OrderTarget});
}

export const add_ticket = async (BoardId,Title, StateId) => {
    return api.post(`boards/${BoardId}/states/create-ticket`, {BoardId,Title, StateId});
}


export const rename_ticket = async (BoardId,Id, Title) => {
    return api.post(`boards/${BoardId}/states/rename-ticket`, {BoardId,Id, Title});
}

export const assign_to_ticket = async (BoardId,TicketId) => {
    return api.post(`boards/${BoardId}/states/assign-user-to-ticket`, {BoardId,TicketId});
}

export const start_work_on_ticket = async (BoardId,TicketId) => {
    return api.post(`boards/${BoardId}/states/start-work-on-ticket`, {BoardId,TicketId});
}

export const end_work_on_ticket = async (BoardId,TicketId) => {
    return api.post(`boards/${BoardId}/states/end-work-on-ticket`, {BoardId,TicketId});
}

export const is_ticket_in_progress = async (BoardId,TicketId) => {
    return api.post(`boards/${BoardId}/states/is-ticket-in-progress`, {BoardId,TicketId});
}

export const change_ticket_description = async (BoardId ,Id, Description) => {
    return api.post(`boards/${BoardId}/states/change-ticket-description`, {BoardId,Id, Description});
}

export const get_labels_by_board = async (BoardId) => {
    return api.post(`boards/${BoardId}/states/get-labels-by-board`, {BoardId});
}

export const add_label_to_ticket = async (BoardId, LabelId, TicketId) => {
    return api.post(`boards/${BoardId}/filtering/add-label-to-ticket`, {BoardId, LabelId, TicketId});
}

export const delete_label_from_ticket = async (BoardId, TicketId) => {
    return api.post(`boards/${BoardId}/filtering/delete-label-from-ticket`, {BoardId, TicketId});
}

export const create_label = async (BoardId, Title) => {
    return api.post(`boards/${BoardId}/filtering/create-label`, {BoardId, Title});
}

export const get_label_by_id = async (BoardId, Id) => {
    return api.post(`boards/${BoardId}/filtering/get-label`, {BoardId, Id});
}

export const delete_label = async (BoardId, Id) => {
    return api.post(`boards/${BoardId}/filtering/delete-label`, {BoardId, Id});
}

export const get_timelogs_by_ticket = async (BoardId, TicketId) => {
    return api.post(`boards/${BoardId}/states/get-timelogs-by-ticket`, {BoardId, TicketId});
}

export const close_ticket = async (BoardId, TicketId) => {
    return api.post(`boards/${BoardId}/states/close-ticket`, {BoardId, TicketId});
}

export const delete_ticket = async (BoardId, Id) => {
    return api.post(`boards/${BoardId}/states/delete-ticket`, {BoardId, Id});
}

export const rename_state = async (BoardId, Id, Title) => {
    return api.post(`boards/${BoardId}/states/rename-state`, {BoardId, Id, Title});
}

export const delete_state = async (BoardId, Id) => {
    return api.post(`boards/${BoardId}/states/delete-state`, {BoardId, Id});
}

export const create_state = async (BoardId, Title) => {
    return api.post(`boards/${BoardId}/states/create-state`, {BoardId, Title});
}

export const add_timelog_to_ticket = async (BoardId, TicketId, StartedAt, EndedAt) => {
    return api.post(`boards/${BoardId}/states/add-timelog-to-ticket`, {BoardId, TicketId, StartedAt, EndedAt});
}
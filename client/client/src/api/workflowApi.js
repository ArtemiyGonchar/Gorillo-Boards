import axios from 'axios';
import {toast} from "react-toastify";
const api = axios.create({
    baseURL: 'https://localhost:7007/api',
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
            }
            toast.error(cleanMsg);
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
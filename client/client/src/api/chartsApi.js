import axios from 'axios';
import {toast} from "react-toastify";
import apiConfig from './api-config.json';
const api = axios.create({
    baseURL: apiConfig.chartsApi,
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
})

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

export const get_all_tickets_by_sprint = async (StartedAt, EndedAt) => {
    return api.post('/charts/get-tickets-by-sprint', {StartedAt, EndedAt});
}

export const get_all_tickets_by_sprint_board = async (StartedAt, EndedAt, BoardId) => {
    return api.post('/charts/get-tickets-by-sprint-and-board', {StartedAt, EndedAt, BoardId});
}
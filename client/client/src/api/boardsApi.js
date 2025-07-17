import axios from 'axios';
import apiConfig from './api-config.json';
import {toast} from "react-toastify";
const api = axios.create({
    baseURL: apiConfig.boardsApi,
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

export const get_boards = async () => {
    return api.get('/boards/GetBoards');
}

export const get_all_boards = async () => {
    return api.get('/boards/get-all-boards');
}

export const has_access_to_board = async (boardId) => {
    return api.get(`boards/${boardId}/has-access`);
}

export const get_board_by_id = async (boardId) => {
    return api.get(`boards/${boardId}/get-board-by-id`);
}
export const create_board = async (Title) => {
    return api.post(`admin/boardmanagment/create-board`, {Title});
}

export const allow_board_role = async (Title, Role) => {
    return api.post(`admin/boardmanagment/allow-role-for-board`, {Title,AllowedRole: Number(Role)});
}

export const forbid_board_role = async (Title, Role) => {
    return api.post(`admin/boardmanagment/forbid-role-from-board`, {Title,AllowedRole:Number(Role)});
}

export const delete_board = async (title) => {
    return api.post(`admin/boardmanagment/delete-board`, {title});
}
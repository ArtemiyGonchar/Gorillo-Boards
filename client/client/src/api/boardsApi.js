import axios from 'axios';
const api = axios.create({
    baseURL: 'https://localhost:7239/api',
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

export const get_boards = async () => {
    return api.get('/boards/GetBoards');
}

export const has_access_to_board = async (boardId) => {
    return api.get(`boards/${boardId}/has-access`);
}

export const get_board_by_id = async (boardId) => {
    return api.get(`boards/${boardId}/get-board-by-id`);
}
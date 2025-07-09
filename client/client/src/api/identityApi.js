import axios from 'axios';
import {toast} from "react-toastify";
const api = axios.create({
    baseURL: 'https://localhost:7189/api',
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

export const login = async (UserName, PasswordHash) => {
    return api.post('/auth/login',{UserName,PasswordHash});
}

export const isAuthorized = async () => {
    return api.get('/auth/isAuthorized');
}

export const get_user = async (Id) => {
    return api.post(`/auth/get-user-by-id`,{Id});
}

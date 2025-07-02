import axios from 'axios';
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
})

export const login = async (UserName, PasswordHash) => {
    return api.post('/auth/login',{UserName,PasswordHash});
}

export const isAuthorized = async () => {
    return api.get('/auth/isAuthorized');
}
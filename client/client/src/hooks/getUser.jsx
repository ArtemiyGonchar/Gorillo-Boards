import {useEffect, useState} from "react";
import {jwtDecode} from "jwt-decode";

export default function useGetUser(){
    const [user, setUser] = useState(null);
    const [loadingUser, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if(token){
            const decoded = jwtDecode(token);
            setUser(decoded);
            setLoading(false);
        } else {
            setLoading(true);
        }

    }, [])
    return {user, loadingUser}
}
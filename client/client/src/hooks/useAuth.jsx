//import {jwtDecode} from "jwt-decode";
import {useEffect, useState} from "react";
import {isAuthorized} from "../api/identityApi.js";

//const token = localStorage.getItem('token');
//const decoded = jwtDecode(token);

export default function useAuth(){
    const [isAuth, setIsAuth] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if(!token){
            setIsAuth(false);
            setLoading(false);
            return;
        }
        console.log(token)
        isAuthorized().then(() => {
            setIsAuth(true);
            setLoading(false);
        }).catch((e) => {
            localStorage.removeItem('token');
            setIsAuth(false);
            setLoading(false);
            console.log(e);
        });


    }, []);
    return {isAuth, loading};
}
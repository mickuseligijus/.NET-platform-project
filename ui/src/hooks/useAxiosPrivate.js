import {axiosPrivate} from "../api/axios";
import {useEffect } from "react";
import useAuth from "./useAuth";

const useAxiosPrivate = () => {
    const {auth} = useAuth();

    useEffect(() => {

        const requestIntercept = axiosPrivate.interceptors.request.use(
            // config => {
            //     if(!config.headers['Authorization']){
            //         config.headers['Authorization'] = 'Bearer ${auth?.accessToken}'; 
            //     }
            //     return config;
            // }
            config => {
                // Do something before request is sent
            
                config.headers["Authorization"] = "bearer " + auth?.accessToken;
                return config;
              },
              error => {
                Promise.reject(error);
              }
        )
        // const responseIntercept = axiosPrivate.interceptors.response.use(
        //     response => response
        // )
    },[auth]);

    return axiosPrivate;
}

export default useAxiosPrivate;
import axios from 'axios';
const BASE_URL='https://localhost:7156/api';

export default axios.create({
    baseURL: BASE_URL
})
export const axiosPrivate = axios.create({
    baseURL: 'https://localhost:7156/api',
    headers: {'Content-Type': 'application/json'}
})
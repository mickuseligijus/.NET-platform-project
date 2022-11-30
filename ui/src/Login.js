import {userRef, useState, useEffect, useRef, useContext} from 'react';
import AuthContext from "./context/AuthProvider";
import axios from './api/axios';

const LOGIN_URL = "/login/Login";

const Login = () =>{
    const {setAuth} = useContext(AuthContext);
    const userRef = useRef();
    const errRef = useRef();

    const [user, setUser] = useState('');
    const[pwd, setPwd] = useState('');
    const[errMsg, setErrMsg] = useState('');
    const[success, setSuccess] = useState(false);

    useEffect(() => {
        userRef.current.focus();
    }, [])

    useEffect(() => {
        setErrMsg('');
    },[user,pwd]);

    const handleSubmit = async(e) =>{
        e.preventDefault();

        const response = await axios.post(LOGIN_URL,
            JSON.stringify({UserName: user, Password:pwd}),
            {
                headers: {'Content-Type':'application/json'}
            }
            )
            .then(function(response){
                if(response.status == 200){
                    // const accessToken = response.data;
                    const accessToken = response.data.token;
                    setAuth({user,pwd,accessToken});
                    setUser('');
                    setPwd('');
                    setSuccess(true);
                    console.log(response.data);
                }
            })
            .catch(function(error){
                // console.log(error.response.data);
                setErrMsg(error.response.data);
            });




    }
    return (
        <section>
            <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>
            <h1>Sign in</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="username">Username:</label>
                <input
                    type="text"
                    id="username"
                    ref={userRef}
                    autoComplete="off"
                    onChange={(e) => setUser(e.target.value)}
                    value={user}
                    required
                />
                <label htmlFor="password">Password:</label>
                <input
                    type="password"
                    id="password"
                    ref={userRef}
                    onChange={(e) => setPwd(e.target.value)}
                    value={pwd}
                    required
                />
                <button>Sign in</button>
            </form>
            <p>
                Need an Account?<br />
                <span className = "line">
                    <a href="Register">Sign Up</a>
                </span>
            </p>
        </section>
    );
}

export default Login;
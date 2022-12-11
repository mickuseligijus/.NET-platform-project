import {userRef, useState, useEffect, useRef} from 'react';
import useAuth from './hooks/useAuth';
import axios from './api/axios';
import {Link, useNavigate, useLocation} from "react-router-dom";
import betraveling from './images/betraveling.png';


const LOGIN_URL = "/login/Login";

const Login = () =>{
    const {setAuth} = useAuth();

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";


    const userRef = useRef();
    const errRef = useRef();

    const [user, setUser] = useState('');
    const[pwd, setPwd] = useState('');
    const[errMsg, setErrMsg] = useState('');

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
                    navigate(from,{replace:true});
                    console.log(response.data); 
                }
            })
            .catch(function(error){
                // console.log(error.response.data);
                setErrMsg(error.response.data);
            });




    }
    return (
        <div className='login'>
            <img id="icon" src={betraveling} height={50}/>
            <span id="brand">Be traveling</span>

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
        </div>
    );
}

export default Login;
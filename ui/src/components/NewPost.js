import React, { useEffect, useState } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import profile from '../images/profile.png'
import {Link, useNavigate, useLocation} from 'react-router-dom';
import betraveling from '../images/betraveling.png';

// import {useState, useEffect, useRef} from "react";

const POST_URL = 'post/post';

const NewPost = () =>{
    const [inputValue, setInputValue] = useState("");
    const [textAreaValue, setTextAreaValue] = useState("")
    const axiosPrivate = useAxiosPrivate();

    const navigate = useNavigate();
    // const location = useLocation();
    const from = "/";

    const handleSubmit = async(e) =>{
        e.preventDefault();

        const response = await axiosPrivate.post(POST_URL,
            JSON.stringify({title: inputValue, text:textAreaValue}),
            {
                headers: {'Content-Type':'application/json'}
            }
            )
            .then(function(response){
                if(response.status == 200){
                    // const accessToken = response.data;

                    // setInputValue('');
                    // setPwd('');
                    setInputValue('');
                    setTextAreaValue('');
                    console.log(response.data); 
                    navigate(from,{replace:true});

                }
            })
            .catch(function(error){
                console.log(error.response.data);
                // setErrMsg(error.response.data);
            });

        // console.log(inputValue);


        
    }
return(
    <div className = "NewPost">
        <nav>
            <img id="icon" src={betraveling} height={50}/>
            <span id="brand">Be traveling</span>
            <Link className="navigation" to="/">Main page</Link>
            <a className="navigation" href="/login">Log out</a>

        </nav>

        <div>
            <h1>Create new post</h1>
            <form onSubmit={handleSubmit}>
                <input
                    id="title"
                    type="text"
                    autoComplete="off"
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value)}
                />
                <textarea
                    id="title"
                    type="text"
                    value={textAreaValue}
                    onChange={(e) => setTextAreaValue(e.target.value)}
                />
                <button>
                    Share
                </button>
            </form>
        </div>
    </div>
)
}
export default NewPost;
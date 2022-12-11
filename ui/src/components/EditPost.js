import React, { useEffect, useState } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import profile from '../images/profile.png'
import {Link, useNavigate, useLocation, useParams} from 'react-router-dom';
import betraveling from '../images/betraveling.png';

// import {useState, useEffect, useRef} from "react";

const POST_URL = 'post/update/';

const NewPost = () =>{
    const [inputValue, setInputValue] = useState("");
    const [textAreaValue, setTextAreaValue] = useState("");
    const axiosPrivate = useAxiosPrivate();
    const params = useParams();
    const navigate = useNavigate();
    const from = "/myposts";

    const handleSubmit = async(e) =>{
        e.preventDefault();

        if (window.confirm("Are you sure?") == true) {
            let url = POST_URL + params.params;

            const response = await axiosPrivate.post(url,
                JSON.stringify({title: inputValue, text:textAreaValue}),
                {
                    headers: {'Content-Type':'application/json'}
                }
                )
                .then(function(response){
                    if(response.status == 200){
                        setInputValue('');
                        setTextAreaValue('');
                        console.log(response.data); 
                        navigate(from,{replace:true});
    
                    }
                })
                .catch(function(error){
                    console.log(error.response.data);
                });        
        } 
       
    }
    useEffect(() => {
        fetchData();
      }, []);

      const fetchData = async () => {
        

        console.log(params.params);
        let url = "/post/";
        url += params.params;


        try{
          const response = await axiosPrivate.get(url)
    
          const data = await response.data
  
          console.log(data.title);
  
        setInputValue(data.title);
        setTextAreaValue(data.text);
  
        }
        catch(err){
          console.log(err);
        }
    
        
    
      }

return(
    // <div>hello</div>
    <div className = "NewPost">
        <nav>
            <img id="icon" src={betraveling} height={50}/>
            <span id="brand">Be traveling</span>
            <Link className="navigation" to="/">Main page</Link>
            <Link className="navigation" to="/myposts">Back</Link>
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
                    Update
                </button>
            </form>
        </div>
    </div>
)
}
export default NewPost;
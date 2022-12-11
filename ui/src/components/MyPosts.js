import useAxiosPrivate from "../hooks/useAxiosPrivate";
import React, { useEffect, useState } from "react";
import profile from '../images/profile.png'
import { useNavigate, Link } from "react-router-dom";
import love from '../images/love.png';
import sad from '../images/sad.png';
import funny from '../images/funny.png';
import angry from '../images/angry.png';
import like from '../images/like.png';
import betraveling from '../images/betraveling.png';


// import profile from './images/profile.png'

const USER_INFO_URL = "/post/myposts";

const Home = () =>{
    const[posts, setPosts] = useState([])
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();

    const fetchData = async () => {

     
        try{
          const response = await axiosPrivate.get(USER_INFO_URL)
    
          const data = await response.data
  
          console.log(data);
  
          setPosts(data)
  
        }
        catch(err){
          console.log(err);
        }
    
        // 
    
      }
  
      useEffect(() => {
        fetchData();
      }, []);

      const sendReaction = async (id, reaction) =>{
        // e.preventDefault();
     
        let url= "/post/react/";
        url = url+ id +"/" + reaction;
        console.log(url);
        try{
            const response = await axiosPrivate.post(url);
            const data = await response.data;
            console.log(data);

            fetchData();
        }
        catch(err){
            console.log(err);
        }

    }

    const getReactions = async (id) =>{
        let url = "post/reactions/";
        url = url + id;
        try {
            const response = await axiosPrivate.get(url);
            const data = await response.data;
            console.log(data);
        }
        catch(err){
            console.log(err);
        }
    }

    const editPost = async(id) =>{
        let url = "/post/update/";
        url += id;
        navigate(url,{replace:true});

    }

    const deletePost = async(id) =>{
        if (window.confirm("Are you sure?") == true) {
        
        let url = "post/remove/";
        url = url + id;
        try {
            const response = await axiosPrivate.post(url);
            const data = await response.data;
            console.log(data);
            fetchData();
        }
        catch(err){
            console.log(err);
        }
    }
    }

    return (
    <div>
        <nav>
            <img id="icon" src={betraveling} height={50}/>
            <span id="brand">Be traveling</span>
            <Link className="navigation" to="/">Main page</Link>
            <a className="navigation" href="/login">Log out</a>
        </nav>
        <div>
            <h1>My Posts</h1>
            {
                posts.map((post) =>(
                    <div className="Post" key={post.post.id}>
                        <div id="Person">
                            <img src={profile} alt="" height={75} />
                            <span id="name">{post.userInfo}</span>
                            <span id="time">{ post.post.created.slice(0,10) + " " +post.post.created.slice(11,19)}</span>
                            <Link id="edit" to={`../post/update/${post.post.id}`}>Edit</Link>
                            <a id="delete" onClick={e => deletePost(post.post.id)}>Delete</a>
                        </div>
                        <h3>{post.post.title}</h3>
                        <p>{post.post.text}</p>
                        <div className="Reactions">
                            <a href="">

                            </a>
                            <img className = "Reaction" src={like} alt="like" height={50} onClick={e => sendReaction(post.post.id,"like")}/>
                            <img className = "Reaction" src={love} alt="love" height={50} onClick={e => sendReaction(post.post.id,"love")} />
                            <img className = "Reaction" src={funny} alt="funny" height={50} onClick={e => sendReaction(post.post.id,"funny")} />
                            <img className = "Reaction" src={sad} alt="sad" height={50} onClick={e => sendReaction(post.post.id,"sad")} />
                            <img className = "Reaction" src={angry} alt="angry" height={50} onClick={e => sendReaction(post.post.id,"angry")} />
                        </div>
                        <div className="Person">
                            <label id="lab" onClick={e => getReactions(post.post.id)}>Reactions</label>
                            <span>{post.reactionNumber}</span>
                        </div>
                        <div className="Person">
                            <label id="lab">Comments</label>
                            
                            <span>{post.commentsNumber}</span>
                        </div>
                    </div>
                ))
            }
        </div>
    </div>
    );
}
export default Home;
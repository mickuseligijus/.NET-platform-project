import React, { useEffect, useState } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import {Link, useNavigate, useLocation, useParams} from 'react-router-dom';
import betraveling from '../images/betraveling.png';

// import {useState, useEffect, useRef} from "react";

const POST_URL = 'user/info/';

const NewPost = () =>{
    const [name, setName] = useState("");
    const [lastName, setLastName] = useState("");
    const [location, setLocation] = useState("");
    const [hideLocation, setHideLocation] = useState(false);
    const [display, setDisplay] = useState(false);



    const axiosPrivate = useAxiosPrivate();
    // const params = useParams();
    const navigate = useNavigate();
    const from = "/profile";

    const handleSubmit = async(e) =>{
        e.preventDefault();

        if (window.confirm("Are you sure?") == true) {
            let url = POST_URL;
            console.log(hideLocation);
            console.log(name);

            console.log(lastName);

            console.log(location);


            const response = await axiosPrivate.post(url,
                JSON.stringify({name: name, lastName:lastName, hideLocation:hideLocation, location:location}),
                {
                    headers: {'Content-Type':'application/json'}
                }
                )
                .then(function(response){
                    if(response.status == 200){
                        // setInputValue('');
                        // setTextAreaValue('');
                        setLastName('');
                        setName('');

                        setLocation('');

                        setHideLocation(false);

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
        

        // console.log(params.params);
        let url = "/user/info";
        // url += params.params;

        

        try{
          const response = await axiosPrivate.get(url)
    
          const data = await response.data
        
          if(data.length==0){
            // setDisplay(false);

          }
          else{
            // setDisplay(true);
            setName(data.name);
            setLastName(data.lastName);
            setHideLocation(data.hideLocation);
            setLocation(data.location);

          }
        //   console.log(data.title);   
  

  
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
            <Link className="navigation" to="/profile">Back</Link>
            <a className="navigation" href="/login">Log out</a>

        </nav>
        <div>
            <form onSubmit={handleSubmit}>
                <label htmlFor="name">Name:</label>
                <input
                    id="name"
                    type="text"
                    autoComplete="off"
                    value={name}
                    onChange = {(e) => setName(e.target.value)}
                />
                <br></br>
                <label htmlFor="lastName">Last Name:</label>
                <input
                    id="lastName"
                    type="text"
                    autoComplete="off"
                    value={lastName}
                    onChange = {(e) => setLastName(e.target.value)}
                />
                <br></br>
                <label htmlFor="location">Location:</label>
                <input
                    id="location"
                    type="text"
                    autoComplete="off"
                    value={location}
                    onChange = {(e) => setLocation(e.target.value)}
                />
                <br></br>
                <label htmlFor="hideLocation">Hide location:</label>
                <input 
                    id="hideLocation"
                    type="checkbox" 
                    // id="coding" 
                    // ame="interest" 
                    onChange={(e) => setHideLocation( e.target.checked)}
                />
                <button>Submit</button>
            </form>

        </div>
    </div>
)
}
export default NewPost;
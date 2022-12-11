import React, { useEffect, useState } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import profile from '../images/profile.png'
import {Link} from 'react-router-dom';
import betraveling from '../images/betraveling.png';


const USER_INFO_URL = "/User/Info";

const Users = () =>{
    const[info, setInfo] = useState([])
    const[infoCreated, setInfoCreated] = useState(false);
    const axiosPrivate = useAxiosPrivate();

    const fetchData = async () => {

     
      try{
        const response = await axiosPrivate.get(USER_INFO_URL)
  
        const data = await response.data

        console.log(data);

        setInfo(data)
        if(data==[]){
            setInfoCreated(false);
        }
        else{
            setInfoCreated(true);
        }

      }
      catch(err){
        console.log(err);
      }
  
      // 
  
    }
    

    useEffect(() => {
      fetchData();
    }, []);

    return (

    <div>
        <nav>
            <img id="icon" src={betraveling} height={50}/>
            <span id="brand">Be traveling</span>
            <Link className="navigation" to="/">Main page</Link>
            {/* <Link className="navigation" to="/friends">Back</Link> */}

            <a className="navigation" href="/login">Log out</a>

        </nav>
      <h1>Profile information</h1>
      <>
      {
        infoCreated?(         
            <div>
                <Link id="edit" to="/addinfo">Edit</Link>
                <br></br>

                <label className="space">Name:</label>
                <label>{info.name}</label>
                <br/>
                <label className="space">Last Name:</label>
                <label>{info.lastName}</label>
                <br/>
                <label className="space">Location:</label>
                <label>{info.location}</label>
                <br/>
                <label className="space">HideLocation:</label>
                <label>{String(info.hideLocation)}</label>
                {/* <label>{info.hideLocation}</label> */}
            </div>   
                
        ):
        (
        <div>
            No info provided
            <Link id="edit" to="/addinfo">Add info</Link>
        </div>
        )

      }

    </>

    </div>

  )
}
export default Users;
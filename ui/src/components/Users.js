import React, { useEffect, useState } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import profile from '../images/profile.png'
import {Link} from 'react-router-dom';
import betraveling from '../images/betraveling.png';


const USER_INFO_URL = "/User/people";

const Users = () =>{
    const[people, setPeople] = useState([])
    const axiosPrivate = useAxiosPrivate();

    const fetchData = async () => {

     
      try{
        const response = await axiosPrivate.get(USER_INFO_URL)
  
        const data = await response.data

        console.log(data);

        setPeople(data)

      }
      catch(err){
        console.log(err);
      }
  
      // 
  
    }
    
    const addFriend = async (id) => {
        console.log(id);
        if (window.confirm("Are you sure?") == true) {
        let url = "friend/add/";
        url += id;
        const response = await axiosPrivate.post(url);
        console.log(response.data);
        fetchData();
        }
    }
    useEffect(() => {
      fetchData();
    }, []);

    return (

    <div className ="Friends">
        <nav>
            <img id="icon" src={betraveling} height={50}/>
            <span id="brand">Be traveling</span>
            <Link className="navigation" to="/">Main page</Link>
            <a className="navigation" href="/login">Log out</a>

        </nav>
        <h1>Be travelers</h1>

      
      {people.length > 0 && (

        <table>
         <tbody>
            {people.map((person) => (
              <tr key={person.userId}>
                <td>
                  <img src={profile} alt="" height={75} />
                </td>
                <td>{person.userName}</td>
                <td><a className ="Friend" onClick={e => addFriend(person.userId) }>Add friend</a></td>
              </tr>
            ))}
          </tbody>
        </table>
        
      )}


    </div>

  )
}
export default Users;
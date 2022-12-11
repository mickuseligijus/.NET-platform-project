import React, { useEffect, useState } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import profile from '../images/profile.png'
import {Link} from 'react-router-dom';
import betraveling from '../images/betraveling.png';


const USER_INFO_URL = "/Admin/banned/users";

const Banned = () =>{
    const[users, setUsers] = useState([])
    const axiosPrivate = useAxiosPrivate();

    const fetchData = async () => {

     
      try{
        const response = await axiosPrivate.get(USER_INFO_URL)
  
        const data = await response.data

        console.log(data);

        setUsers(data)

      }
      catch(err){
        console.log(err);
      }
  
      // 
  
    }
    
    const ban = async (id) => {
        console.log(id);
        if (window.confirm("Are you sure?") == true) {
        let url = "admin/unban/";
        url += id;
        const response = await axiosPrivate.post(url);
        console.log(response.data);
        fetchData();
        }
    }
    useEffect(() => {
    console.log("X");
      fetchData();
    }, []);

    return (

    <div>
        {/* LABAS */}
        <nav>
            <img id="icon" src={betraveling} height={50}/>
            <span id="brand">Be traveling</span>
            <Link className="navigation" to="/admin">Users</Link>
            <a className="navigation" href="/login">Log out</a>

        </nav>
        <h1>User list</h1>

      
      {users.length > 0 && (

        <table>
         <tbody>
            {users.map((user) => (
              <tr key={user.id}>
                <td>
                  <img src={profile} alt="" height={75} />
                </td>
                <td>{user.userName}</td>
                <td><a id="edit" onClick={e => ban(user.id) }>Unban</a></td>
              </tr>
            ))}
          </tbody>
        </table>
        
      )}


    </div>

  );
}
export default Banned;
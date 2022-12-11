import React, { useEffect, useState } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import profile from '../images/profile.png'
import {Link} from 'react-router-dom';
import betraveling from '../images/betraveling.png';


const USER_INFO_URL = "/Friend/invitations";

const Users = () =>{
    const[people, setPeople] = useState([]);
    const[display, setDisplay] = useState(false);
    const axiosPrivate = useAxiosPrivate();

    const fetchData = async () => {

     
      try{
        const response = await axiosPrivate.get(USER_INFO_URL)
  
        const data = await response.data

        if(data.length==0){
            setDisplay(false);
        }
        else{
            setDisplay(true);
        }
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

    const confirm = async (id) => {
        console.log(id);
        if (window.confirm("Are you sure?") == true) {
        let url = "friend/invitation/accept/";
        url += id;
        const response = await axiosPrivate.post(url);
        console.log(response.data);
        fetchData();
        }
    }

    const deny = async (id) => {
        console.log(id);
        if (window.confirm("Are you sure?") == true) {
        let url = "friend/invitation/reject/";
        url += id;
        const response = await axiosPrivate.delete(url);
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
            <Link className="navigation" to="/friends">Back</Link>

            <a className="navigation" href="/login">Log out</a>

        </nav>
        <>
        {display?(
            <div>
                <h1>
                    Received invitations
                </h1>
                    <table>
                     <tbody>
                        {people.map((person) => (
                          <tr key={person.id}>
                            <td>
                              <img src={profile} alt="" height={75} />
                            </td>
                            <td>{person.userName}</td>
                            <td><a className ="Friend" onClick={e => confirm(person.id) }>Confirm</a></td>
                            <td><a className ="Friend" onClick={e => deny(person.id) }>Deny</a></td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
            </div>     
        ):
        (
            <div>
                {/* No invitations received */}
                <h1>
                    No invitations received
                </h1>
            </div>
        )}
        </>



    </div>

  )
}
export default Users;
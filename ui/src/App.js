import logo from './logo.svg';
import Register from './Register';
import Layout from './Layout';
import Home from './Home';
import RequireAuth from './RequireAuth';
import Friends from './Friends';
import './App.css';
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link
  } from 'react-router-dom';
  import Login from './Login';
import NewPost from './components/NewPost';
import MyPosts from './components/MyPosts';
import EditPost from './components/EditPost';
import Users from './components/Users';
import Invitations from './components/Invitations';
import Profile from './components/Profile';
import AddInfo from './components/AddInfo';
import Admin from './components/Admin';
import Banned from './components/Banned';






function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        {/* <Route path="/" element={<Home/>}/> */}
        <Route path="login" element={<Login/>}/>
        <Route path="register" element={<Register/>}/>
        {/* <Route path="friends" element={<Friends/>}/> */}

        <Route element={<RequireAuth/>}>
          <Route path="/" element={<Home/>}/>
          <Route path="friends" element={<Friends/>}/>
          <Route path="newPost" element={<NewPost/>}/>
          <Route path="myPosts" element={<MyPosts/>}/>
          <Route path="post/update/:params" element={<EditPost/>}/>
          <Route path="people" element={<Users/>}/>
          <Route path="invitations" element={<Invitations/>}/>
          <Route path="profile" element={<Profile/>}/>
          <Route path="addinfo" element={<AddInfo/>}/>
          <Route path="Admin" element={<Admin/>}/>
          <Route path="banned" element={<Banned/>}/>


        </Route>
      </Route>
    </Routes>
  );
}

export default App;

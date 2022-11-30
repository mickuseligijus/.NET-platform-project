import logo from './logo.svg';
import Register from './Register';
import './App.css';
import {
      BrowserRouter as Router,
      Routes,
      Route,
      Link
  } from 'react-router-dom';
  import Login from './Login';

function App() {
  return (
    // <div text-align="center">
    <main className="App">
    <Router>

    <Routes>
        <Route exact path='/Login' element={< Login />}></Route>
        <Route exact path='/Register' element={< Register />}></Route>
    </Routes>
    </Router>
    </main>
    // </div>

  );
}

export default App;

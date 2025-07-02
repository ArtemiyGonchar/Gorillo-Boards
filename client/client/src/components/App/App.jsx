import '../../styles/global.css'
import {BrowserRouter as Router, Routes, Route, Navigate} from 'react-router-dom';
import LoginPage from "../Login/Login.jsx";
import Boards from "../Boards/BoardsList.jsx";
import Board from "../Boards/Board.jsx";
import State  from "../State/State.jsx";
function App() {
  return (
        <Router>
            <Routes>
                <Route path="/" element={<Boards />} />
                <Route path="/login" element={<LoginPage />}></Route>
                <Route path="/boards" element={<Boards/>}></Route>
                <Route path={"/boards/:boardId"} element={<Board />}></Route>
                <Route path={"/state"} element={<State />}></Route>
            </Routes>
        </Router>
  );
}

export default App

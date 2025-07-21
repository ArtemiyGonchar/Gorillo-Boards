import '../../styles/global.css'
import {BrowserRouter as Router, Routes, Route, Navigate} from 'react-router-dom';
import LoginPage from "../Login/Login.jsx";
import Boards from "../Boards/BoardsList.jsx";
import Board from "../Boards/Board.jsx";
import State  from "../State/State.jsx";
import {ToastContainer, toast, Slide} from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Charts from "../Charts/Charts.jsx";

function App() {
  return (
        <Router>
            <ToastContainer
                transition={Slide}
            />
            <Routes>
                <Route path="/" element={<LoginPage />} />
                <Route path="/login" element={<LoginPage />}></Route>
                        <Route path="/boards" element={<Boards/>}></Route>
                        <Route path={"/boards/:boardId"} element={<Board />}></Route>
                <Route path={"/charts"} element={<Charts />}></Route>
            </Routes>
        </Router>
  );
}

export default App

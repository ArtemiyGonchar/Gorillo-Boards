import './BoardDialog.css'
import {useEffect, useState} from "react";
import {allow_board_role, create_board, delete_board, forbid_board_role, get_all_boards} from "../../api/boardsApi.js";
export default function BoardDialog({closeDialog}){
    const [isCreating, setCreating] = useState(false);
    const [isManaging, setManaging] = useState(false);
    const [isDeleting, setDeleting] = useState(false);
    const [isSuccess, setSuccess] = useState(false);

    const [boards, setBoards] = useState([]);
    const Roles = {
        "Contributor": 0,
        "Admin": 1,
        "Dev Team": 2,
        "QA Team": 3,
        "Design Team": 4,
        "Executive": 5
    }

    const roleOptions = Object.keys(Roles);

    const [boardTitle, setBoardTitle] = useState('');
    const [allowedRole, setAllowRole] = useState('');
    const [selectedBoard, setSelectedBoard] = useState('');
    const [boardForDelete, setBoardForDelete] = useState('');
    const [isDeletingBoard, setDeletingBoard] = useState(false);
    const [boardTitleForDelete, setBoardTitleForDelete] = useState('');

    const fetchInfo = async () => {
        const boardsRes = await get_all_boards();
        setBoards(boardsRes.data);
    }

    useEffect(() => {
        fetchInfo();
    }, [isCreating, isDeleting, isManaging]);

    const handleCreateBoard = () => {
        setCreating(true);
        setManaging(false);
        setDeleting(false);
        setSuccess(false)
    }

    const handleManagingBoard = () => {
        setCreating(false);
        setManaging(true);
        setDeleting(false);
        setSuccess(false)
    }

    const handleDeletingBoard = () => {
        setCreating(false);
        setManaging(false);
        setDeleting(true);
        setSuccess(false)
    }

    const handleSubmitCreateBoard = async () => {
        if (boardTitle === '') {
            return
        }
        try {
            await create_board(boardTitle);
            console.log(boardTitle);
            setSuccess(true);
        } catch (error) {
            console.error(error);
            setSuccess(false)
        }
    }

    const handleAllowRole = async (e) => {
        setSuccess(false);
        if(e.target.value === '') {
            return;
        }
        if (selectedBoard === '') {
            return;
        }
        try {
            await allow_board_role(selectedBoard, e.target.value);
            setSuccess(true);
        } catch (error) {
            console.log(error);
        }
    }
    const handleForbidRole = async (e) => {
        setSuccess(false);
        if(e.target.value === '') {
            return;
        }
        if (selectedBoard === '') {
            return;
        }
        try {
            await forbid_board_role(selectedBoard, e.target.value);
            setSuccess(true);
        } catch (error) {
            console.log(error);
        }
    }
    const handleSelectBoardForRole = async (e) => {
        setSuccess(false);
        if (e.target.value === '') {
            return;
        }
        setSelectedBoard(e.target.value);
    }

    const handleSelectBoardForDeleting = async (e) => {
        setSuccess(false);
        if (e.target.value === '') {
            return;
        }
        setBoardForDelete(e.target.value);
        setDeletingBoard(true);
    }

    const handleDeleteBoard = async () => {
        if (boardForDelete === '') return;
        if (boardForDelete !== boardTitleForDelete) return;
        try {
            await delete_board(boardForDelete);
            setSuccess(true);
            setDeletingBoard(false);
            await fetchInfo();
        } catch (error) {
            console.log(error);
            setSuccess(false);
        }
    }
    return (
        <>
            <div className="board-dialog-overlay">
                <div className="board-dialog">
                    <div className="board-dialog-header">
                        <div className="board-dialog-header-title">Boards manager</div>
                        <span onClick={closeDialog}>X</span>
                    </div>

                    <div className="board-dialog-buttons-container">
                        <button onClick={handleCreateBoard}>Create board</button>
                        <button onClick={handleManagingBoard}>Manage roles</button>
                        <button onClick={handleDeletingBoard}>Delete board</button>
                    </div>

                    {isCreating && (
                        <div className="board-dialog-body">
                            <p>Create new board:</p>
                            <div className="input-center">
                                Title:{" "}
                                <input
                                    placeholder="Enter board title"
                                    onChange={(e) => {
                                        setBoardTitle(e.target.value);
                                        setSuccess(false);
                                    }}
                                />
                                <button onClick={handleSubmitCreateBoard}>Create</button>
                            </div>
                            {isSuccess && <p className="success">Success!</p>}
                        </div>
                    )}

                    {isManaging && (
                        <div className="board-dialog-body">
                            <p>Manage roles for board</p>
                            <div className="input-center">
                                Board:
                                <select onChange={handleSelectBoardForRole}>
                                    <option value="">Select board</option>
                                    {boards.map((board) => (
                                        <option key={board.id} value={board.title}>
                                            {board.title}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className="input-center">
                                Allow role:
                                <select onChange={handleAllowRole}>
                                    <option value="">Select role</option>
                                    {Object.keys(Roles).map((roleKey) => (
                                        <option key={roleKey} value={Roles[roleKey]}>
                                            {roleKey}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className="input-center">
                                Remove role:
                                <select onChange={handleForbidRole}>
                                    <option value="">Select role</option>
                                    {Object.keys(Roles).map((roleKey) => (
                                        <option key={roleKey} value={Roles[roleKey]}>
                                            {roleKey}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            {isSuccess && <p className="success">Success!</p>}
                        </div>
                    )}

                    {isDeleting && (
                        <div className="board-dialog-body">
                            <p>Deleting boards</p>
                            {isDeletingBoard ? (
                                <>
                                    <p>Board title: {boardForDelete}</p>
                                    <p>Type board title for submit</p>
                                    <div className="input-center">
                                        <input
                                            placeholder="Enter board title"
                                            onChange={(e) => setBoardTitleForDelete(e.target.value)}
                                        />
                                        <button onClick={handleDeleteBoard}>Delete</button>
                                    </div>
                                </>
                            ) : (
                                <div className="input-center">
                                    Board:
                                    <select onChange={handleSelectBoardForDeleting}>
                                        <option value="">Select board</option>
                                        {boards.map((board) => (
                                            <option key={board.id} value={board.title}>
                                                {board.title}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                            )}
                            {isSuccess && <p className="success">Success!</p>}
                        </div>
                    )}
                </div>
            </div>
        </>
    );

}
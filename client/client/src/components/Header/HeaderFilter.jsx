import './HeaderFilter.css'
import {useEffect, useState} from "react";
import {get_labels_by_board} from "../../api/workflowApi.js";
import {useParams} from "react-router-dom";

export const HeaderFilter = ({board, onFilterChange}) => {
    const [isSelected, setSelected] = useState(false)
    const [labels, setLabels] = useState([]);
    const [selectedLabelId, setSelectedLabelId] = useState('');
    const params = useParams();

    const fetchData = async () => {
        const labelsRes = await get_labels_by_board(params.boardId);
        setLabels(labelsRes.data);
    }

    useEffect(() => {
        fetchData()
    }, [board]);

    if(board === undefined || board === null || board.length === 0) {
        return (
        <div>
            loading...
        </div>);
    }
    const handleSelected = () => setSelected(true);
    const handleCancel = () => setSelected(false);

    return (
        <div className='header-filter'>
            <div className='board-title'>Board: {board.title}</div>
            <div className='filters'>
                {isSelected ? (
                    <>
                        <button onClick={event => {
                            handleCancel();
                            onFilterChange('');
                        }}>Cancel</button>
                    </>
                ) : (
                    <>
                        <button onClick={() =>
                        {
                            setSelected(true);
                            onFilterChange("assigned");
                        }}>
                            Assigned to me
                        </button>
                        <button onClick={() => {
                            setSelected(true);
                            onFilterChange("requested");
                        }}>Requested by me</button>
                        <select className='filter-select' onChange={(e) =>
                        {
                            setSelectedLabelId(e.target.value);
                            setSelected(true);
                            onFilterChange(e.target.value);
                        }}
                        >

                            <option value=''>Select sub-status</option>
                            {labels.map((label) => (
                                <option key={label.id} value={label.id}>
                                    {label.title}
                                </option>
                            ))}
                        </select>
                    </>
                )}
            </div>
        </div>
    );
}
import './Charts.css'
import {Header} from "../Header/Header.jsx";
import {BarChart, LineChart} from "@mui/x-charts";
import {useEffect, useState} from "react";
import DatePicker from "react-datepicker";
import {get_all_tickets_by_sprint, get_all_tickets_by_sprint_board} from "../../api/chartsApi.js";
import {get_all_boards} from "../../api/boardsApi.js";
export default function Charts(){
    const [openTickets, setOpenTickets] = useState([]);
    const [closedTickets, setClosedTickets] = useState([]);

    const [boards, setBoards] = useState([]);
    const [selectedBoard, setSelectedBoard] = useState('');
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    const [ticketsByDay, setTicketsByDay] = useState({});

    const [averageDuration, setAverageDuration] = useState({});

    const xTicketDays = Object.keys(ticketsByDay);
    const yTicketData = Object.values(ticketsByDay);

    const yAverageWork = Object.values(averageDuration);

    const [showAverageWork, setShowAverageWork] = useState(false);

    useEffect(() => {
        fetchBoards()
    }, []);

    const fetchTickets = async () => {
        let response;
        if (selectedBoard === '') {
            response = await get_all_tickets_by_sprint(startDate, endDate);
        } else{
            response = await get_all_tickets_by_sprint_board(startDate, endDate, selectedBoard);
        }

        const allTickets = response.data;
        const totalDays = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24)) + 1;
        const averageTimePerDay = {};
        const closedPerDay = {};

        for (let i = 1; i <= totalDays; i++) {
            closedPerDay[i] = 0;
            averageTimePerDay[i] = 0;
        }

        const totalTickets = allTickets.length;

        allTickets.forEach(ticket => {
            if (ticket.ticketClose) {
                const averageTime = ((new Date(ticket.ticketClose) - new Date(ticket.createdAt)) / (1000 * 60 * 60)).toFixed(2);
                const closedDate = new Date(ticket.ticketClose);
                const dayOffset = Math.floor((closedDate - startDate) / (1000 * 60 * 60 * 24)) + 1;
                if (closedPerDay[dayOffset] !== undefined) {
                    closedPerDay[dayOffset]++;
                    averageTimePerDay[dayOffset] = averageTime;
                }
            }
        });

        const tempTicketsByDay = {};
        let remaining = totalTickets;

        for (let i = 1; i <= totalDays; i++) {
            remaining -= closedPerDay[i];
            tempTicketsByDay[i] = remaining;
        }

        setTicketsByDay(tempTicketsByDay);
        setAverageDuration(averageTimePerDay)
    };

    const fetchBoards = async () => {
        const boardsRes = await get_all_boards();
        setBoards(boardsRes.data);
    }

    const handleSelectBoard = (event) => {
        setSelectedBoard(event.target.value);
    }
    return (
        <>
            <Header />
            <div className='charts-wrapper'>
                <div className='charts-body'>
                    <div className='chart-settings'>
                        <span>Sprint start:</span>
                        <DatePicker className='charts-date' selected={startDate} onChange={(date) => setStartDate(date)}  showTimeSelect dateFormat="Pp"/>
                        <span>Sprint end:</span>
                        <DatePicker className='charts-date' selected={endDate} onChange={(date) => setEndDate(date)} showTimeSelect dateFormat="Pp" />


                        {showAverageWork ? (
                            <>
                                <button className='chart-settings-button' onClick={ (e) => {setShowAverageWork(false)}}>View ticket burndown chart</button>
                            </>
                        ) : (
                            <>
                                <button className='chart-settings-button' onClick={ (e) => {setShowAverageWork(true)}}>View average work time on ticket</button>
                            </>
                        )}

                        <select onChange={handleSelectBoard}>
                            <option value=''>Tickets from all boards</option>
                            {boards.map((board) => (
                                <option key={board.id} value={board.id}>
                                    {board.title}
                                </option>
                            ))}
                        </select>
                        <button onClick={fetchTickets} className='chart-settings-button'>Get Data</button>
                    </div>

                    <div className='chart'>
                        <BarChart
                            xAxis={[{ scaleType: 'band', data: xTicketDays, label: "Day of sprint"}]}
                            series={[{ data: showAverageWork ? yAverageWork : yTicketData , label: showAverageWork ? "Average work time on ticket in hours" : "Remaining Tickets", color: 'rgba(0, 123, 255, 0.9)',}]}
                            width={600}
                            height={400}
                        />
                    </div>
                </div>
            </div>
        </>
    )
}
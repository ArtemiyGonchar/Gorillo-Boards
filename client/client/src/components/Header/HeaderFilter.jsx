import './HeaderFilter.css'

export const HeaderFilter = ({board}) => {
    if(board === undefined || board === null || board.length === 0) {
        return (
        <div>
            loading...
        </div>);
    }

    return (
        <div className='header-filter'>
            <div className='board-title'>Board: {board.title}</div>
            <span>hello</span>
        </div>
    );
}
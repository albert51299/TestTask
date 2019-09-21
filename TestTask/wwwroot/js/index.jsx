class Content extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return(
            <div>
                content
            </div>
        );
    }
}

ReactDOM.render(
    <Content />,
    document.getElementById("index")
);
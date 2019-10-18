class Content extends React.Component {
    constructor(props) {
        super(props);

        this.patientsHandler = this.patientsHandler.bind(this);
        this.vaccinationsHandler = this.vaccinationsHandler.bind(this);
    }

    patientsHandler() {
        window.location.href = "./patients.html";
    }

    vaccinationsHandler() {
        window.location.href = "./vaccinations.html";
    }

    render() {
        return(
            <div className="container min-vh-100">
                <div className="row align-items-center min-vh-100">
                    <div className="col text-center">
                        <input type="button" value="Пациенты" className="btn btn-primary btn-lg mr-1" onClick={this.patientsHandler}></input>
                        <input type="button" value="Прививки" className="btn btn-primary btn-lg" onClick={this.vaccinationsHandler}></input>
                    </div>
                </div>
            </div>
        );
    }
}

ReactDOM.render(
    <Content />,
    document.getElementById("index")
);
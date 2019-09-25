class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { id: "", patientName: "", vaccineName: "", consent: "false", date: "" };

        this.deleteHandler = this.deleteHandler.bind(this);
        this.cancelHandler = this.cancelHandler.bind(this);
    }

    componentDidMount() {
        let url = "api/vaccination/getvaccination/" + sessionStorage.getItem("vaccinationId");
        sessionStorage.removeItem("vaccinationId");
        fetch(url, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                let formattedName;
                if (data.lastName !== null) {
                    formattedName = data.lastName + " " + data.firstName.substr(0, 1) + ".";
                    formattedName += (data.secondName === null) ? "" : data.secondName.substr(0, 1) + ".";
                }
                else {
                    formattedName = "Удален из базы данных";
                }

                let formattedDate = data.date.substr(0, 10);
                this.setState({ id:data.id ,vaccineName: data.vaccineName, consent: data.consent, 
                    date: formattedDate, patientName: formattedName });
            });
    }

    deleteHandler() {
        let url = "api/vaccination/" + sessionStorage.getItem("vaccinationId");
        sessionStorage.removeItem("vaccinationId");
        fetch("api/vaccination/" + this.state.id, {
            method: "DELETE",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }})
            .then(response => {
                if (response.status === 200) {
                    window.location.href = "./vaccinations.html";
                }
                if (response.status === 404) {
                    alert("Not found");
                }
            });
    }

    cancelHandler() {
        window.location.href = "./vaccinations.html";
    }

    render() {
        return(
            <div className="container min-vh-100">
                <div className="row align-items-center min-vh-100">
                    <div className="col text-center">
                        <div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="vaccine" className="col-sm-2 col-form-label">Препарат</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="vaccine" value={this.state.vaccineName}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="consent" className="col-sm-2 col-form-label">Согласие</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="consent" value={this.state.consent ? "Да" : "Нет"}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="date" className="col-sm-2 col-form-label">Дата проведения</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="date" value={this.state.date}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="patient" className="col-sm-2 col-form-label">Пациент</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="patient" value={this.state.patientName}></input>
                                </div>
                            </div>
                            <input type="button" value="Удалить" className="btn btn-danger mr-1" onClick={this.deleteHandler}></input>
                            <input type="button" value="Отмена" className="btn btn-secondary" onClick={this.cancelHandler}></input>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

ReactDOM.render(
    <Content />,
    document.getElementById("deleteVaccinationForm")
);
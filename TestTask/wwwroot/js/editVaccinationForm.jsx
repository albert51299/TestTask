class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { id: "", patientName: "", vaccines: [], vaccineName: "", consent: "false", date: "", 
            emptyVaccineName: false, emptyDate: false };

        this.onVaccineNameChanged = this.onVaccineNameChanged.bind(this);
        this.onConsentChanged = this.onConsentChanged.bind(this);
        this.onDateChanged = this.onDateChanged.bind(this);

        this.editHandler = this.editHandler.bind(this);
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

        fetch("api/vaccine", {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => this.setState({ vaccines: data }) );
    }

    onVaccineNameChanged(e) {
        this.setState({ vaccineName: e.target.value });
    }

    onConsentChanged(e) {
        this.setState({ consent: e.target.value });
    }

    onDateChanged(e) {
        this.setState({ date: e.target.value });
    }

    editHandler() {
        let vaccineName = this.state.vaccineName;
        let consent = this.state.consent;
        let date = this.state.date;
        
        let doRequest = true;

        this.setState({ emptyVaccineName: false, emptyDate: false, });

        if (vaccineName === "") {
            this.setState({ emptyVaccineName: true });
            doRequest = false;
        }
        if (date === "") {
            this.setState({ emptyDate: true });
            doRequest = false;
        }

        if (doRequest) {
            let data = JSON.stringify({ "id":this.state.id, "vaccineName":vaccineName, "consent":consent, "date":date });

            fetch("api/vaccination", {
                method: "PUT",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: data
            })
                .then(response => {
                    if (response.status === 200) {
                        window.location.href = "./vaccinations.html";
                    }
                    if (response.status === 400) {
                        alert("Bad request");
                    }
                    if (response.status === 404) {
                        alert("Not found");
                    }
                });
        }
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
                                <label htmlFor="inputVaccineName" className="col-sm-2 col-form-label">Название</label>
                                <div className="col-3">
                                    <select className={this.state.emptyVaccineName ? "form-control is-invalid" : "form-control"} id="inputVaccineName" value={this.state.vaccineName} onChange={this.onVaccineNameChanged}>
                                        {
                                            this.state.vaccines.map( function(vaccine) { return <VaccineOption key={vaccine.id} vaccine={vaccine}/> } )
                                        }
                                    </select>
                                    <div className={this.state.emptyVaccineName ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputConsent" className="col-sm-2 col-form-label">Наличие согласия</label>
                                <div className="col-3">
                                    <select className="form-control" id="inputConsent" value={this.state.consent} onChange={this.onConsentChanged}>
                                        <option value="false">Нет</option>
                                        <option value="true">Да</option>
                                    </select>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputDate" className="col-sm-2 col-form-label">Дата проведения</label>
                                <div className="col-3">
                                    <input type="date" className={this.state.emptyDate ? "form-control is-invalid" : "form-control"} id="inputDate" placeholder="дд.мм.гггг"
                                        value={this.state.date} onChange={this.onDateChanged}></input>
                                    <div className={this.state.emptyDate ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="patient" className="col-sm-2 col-form-label">Пациент</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="patient" value={this.state.patientName}></input>
                                </div>
                            </div>
                            <input type="button" value="Изменить" className="btn btn-success mr-1" onClick={this.editHandler}></input>
                            <input type="button" value="Отмена" className="btn btn-danger" onClick={this.cancelHandler}></input>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

class VaccineOption extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: props.vaccine }
    }

    render() {
        return (
            <option>{this.state.data.name}</option>
        );
    }
}

ReactDOM.render(
    <Content />,
    document.getElementById("editVaccinationForm")
);
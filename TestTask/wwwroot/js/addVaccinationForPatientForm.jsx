class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { vaccines: [], vaccineName: "", consent: "false", date: "", patientId: "", patientName: "",
            emptyVaccineName: false, emptyDate: false };

        this.onVaccineNameChanged = this.onVaccineNameChanged.bind(this);
        this.onConsentChanged = this.onConsentChanged.bind(this);
        this.onDateChanged = this.onDateChanged.bind(this);

        this.addHandler = this.addHandler.bind(this);
        this.cancelHandler = this.cancelHandler.bind(this);
    }

    componentDidMount() {
        let getPatientURL = "api/patient/" + sessionStorage.getItem("patientId");
        sessionStorage.removeItem("patientId");
        fetch(getPatientURL, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                let formattedName = data.lastName + " " + data.firstName.substr(0, 1) + ".";
                formattedName += (data.secondName === null) ? "" : data.secondName.substr(0, 1) + ".";

                this.setState({ patientId: data.id, fullName: formattedName });
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

    addHandler() {
        let vaccineName = this.state.vaccineName;
        let consent = this.state.consent;
        let date = this.state.date;
        
        let doRequest = true;

        this.setState({ emptyVaccineName: false, emptyDate: false });

        if (vaccineName === "") {
            this.setState({ emptyVaccineName: true });
            doRequest = false;
        }
        if (date === "") {
            this.setState({ emptyDate: true });
            doRequest = false;
        }

        if (doRequest) {
            let data = JSON.stringify({ "vaccineName":vaccineName, "consent":consent, "date":date, "patientId":this.state.patientId });

            fetch("api/vaccination", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: data
            })
                .then(response => {
                    if (response.status === 200) {
                        sessionStorage.setItem("patientId", this.state.patientId);
                        window.location.href = "/vaccinationsForPatient.html";
                    }
                    if (response.status === 400) {
                        alert("Bad request");
                    }
                });
        }
    }

    cancelHandler() {
        sessionStorage.setItem("patientId", this.state.patientId);
        window.location.href = "/vaccinationsForPatient.html";
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
                                        <option value="" disabled>Выберите препарат</option>
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
                                    <input type="text" readOnly className="form-control-plaintext" id="patient" value={this.state.fullName}></input>
                                </div>
                            </div>
                            <input type="button" value="Добавить" className="btn btn-success mr-1" onClick={this.addHandler}></input>
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
    document.getElementById("addVaccinationForPatientForm")
);
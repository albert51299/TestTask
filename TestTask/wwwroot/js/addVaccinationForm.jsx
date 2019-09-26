import VaccineOption from "./vaccineOption.jsx";

class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { patients: [], vaccines: [], vaccineName: "", consent: "false", date: "", patientId: "", 
            emptyVaccineName: false, emptyDate: false, emptyPatientId: false, incorrectDate: false };

        this.onVaccineNameChanged = this.onVaccineNameChanged.bind(this);
        this.onConsentChanged = this.onConsentChanged.bind(this);
        this.onDateChanged = this.onDateChanged.bind(this);
        this.onPatientIdChanged = this.onPatientIdChanged.bind(this);

        this.addHandler = this.addHandler.bind(this);
        this.cancelHandler = this.cancelHandler.bind(this);
    }

    componentDidMount() {
        fetch("api/patient", {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => this.setState({ patients: data }) );

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

    onPatientIdChanged(e) {
        this.setState({ patientId: e.target.value });
    }

    addHandler() {
        let vaccineName = this.state.vaccineName;
        let consent = this.state.consent;
        let date = this.state.date;
        let patientId = this.state.patientId;
        
        let doRequest = true;

        this.setState({ emptyVaccineName: false, emptyDate: false, emptyPatientId: false, incorrectDate: false });

        // валидация
        if (vaccineName === "") {
            this.setState({ emptyVaccineName: true });
            doRequest = false;
        }
        if (date === "") {
            this.setState({ emptyDate: true });
            doRequest = false;
        }
        if (!this.correctDate(date)) {
            this.setState({ incorrectDate: true });
            doRequest = false;
        }
        if (patientId === "") {
            this.setState({ emptyPatientId: true });
            doRequest = false;
        }

        if (doRequest) {
            let data = JSON.stringify({ "vaccineName":vaccineName, "consent":consent, "date":date, "patientId":patientId });

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
                        window.location.href = "./vaccinations.html";
                    }
                    if (response.status === 400) {
                        alert("Bad request");
                    }
                });
        }
    }

    cancelHandler() {
        window.location.href = "./vaccinations.html";
    }

    correctDate(date) {
        let year = parseInt(date.substr(0, 4));
        let currentYear = new Date().getFullYear();
        if ((year < 2010) || (year > (currentYear + 1))) {
            return false;
        }
        return true;
    }

    render() {
        return(
            <div className="container min-vh-100">
                <div className="row align-items-center min-vh-100">
                    <div className="col text-center">
                        <div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputVaccineName" className="col-sm-2 col-form-label font-weight-bold">*Препарат</label>
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
                                <label htmlFor="inputConsent" className="col-sm-2 col-form-label font-weight-bold">Наличие согласия</label>
                                <div className="col-3">
                                    <select className="form-control" id="inputConsent" value={this.state.consent} onChange={this.onConsentChanged}>
                                        <option value="false">Нет</option>
                                        <option value="true">Да</option>
                                    </select>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputDate" className="col-sm-2 col-form-label font-weight-bold">*Дата проведения</label>
                                <div className="col-3">
                                    <input type="date" className={this.state.emptyDate || this.state.incorrectDate ? "form-control is-invalid" : "form-control"} id="inputDate" placeholder="дд.мм.гггг"
                                        value={this.state.date} onChange={this.onDateChanged}></input>
                                    <div className={this.state.emptyDate ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                    <div className={this.state.incorrectDate ? "invalid-feedback text-left" : "d-none"}>Неверно введена дата</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputPatientId" className="col-sm-2 col-form-label font-weight-bold">*Пациент</label>
                                <div className="col-3">
                                    <select className={this.state.emptyPatientId ? "form-control is-invalid" : "form-control"} id="inputPatientId" value={this.state.patientId} onChange={this.onPatientIdChanged}>
                                        <option value="" disabled>Выберите пациента</option>
                                        {
                                            this.state.patients.map( function(patient) { return <PatientOption key={patient.id} patient={patient}/> } )
                                        }
                                    </select>
                                    <div className={this.state.emptyVaccineName ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
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

class PatientOption extends React.Component {
    constructor(props) {
        super(props);

        var formattedName = props.patient.lastName + " " + props.patient.firstName.substr(0, 1) + ".";
        formattedName += props.patient.secondName === null ? "" : props.patient.secondName.substr(0, 1) + ".";
        this.state = { data: props.patient, fullName: formattedName };
    }

    render() {
        return (
            <option value={this.state.data.id}>{this.state.fullName}</option>
        );
    }
}

ReactDOM.render(
    <Content />,
    document.getElementById("addVaccinationForm")
);
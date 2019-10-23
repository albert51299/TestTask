import VaccineOption from "./vaccineOption.jsx";

class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { vaccines: [], vaccineName: "", consent: "false", date: "", patientId: "", patientName: "",
            emptyVaccineName: false, emptyDate: false, incorrectDate: false };

        this.onVaccineNameChanged = this.onVaccineNameChanged.bind(this);
        this.onConsentChanged = this.onConsentChanged.bind(this);
        this.onDateChanged = this.onDateChanged.bind(this);

        this.addHandler = this.addHandler.bind(this);
        this.cancelHandler = this.cancelHandler.bind(this);
    }

    componentDidMount() {
        let getPatientURL = "api/patient-management/patients/" + sessionStorage.getItem("patientId");
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

        fetch("api/vaccine-management/vaccines", {
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

        this.setState({ emptyVaccineName: false, emptyDate: false, incorrectDate: false });

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

        if (doRequest) {
            let data = JSON.stringify({ "vaccineName":vaccineName, "consent":consent, "date":date, "patientId":this.state.patientId });

            fetch("api/vaccination-management/vaccinations", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: data
            })
                .then(response => {
                    if (response.status === 200) {
                        window.location.href = "/vaccinationsForPatient.html";
                    }
                    if (response.status === 400) {
                        alert("Bad request");
                    }
                });
        }
    }

    cancelHandler() {
        window.location.href = "/vaccinationsForPatient.html";
    }

    correctDate(date) {
        let year = parseInt(date.substr(0, 4));
        let currentYear = new Date().getFullYear();
        if ((year < 1850) || (year > (currentYear + 1))) {
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
                                <label htmlFor="patient" className="col-sm-2 col-form-label font-weight-bold">Пациент</label>
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

ReactDOM.render(
    <Content />,
    document.getElementById("addVaccinationForPatientForm")
);
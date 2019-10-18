import VaccinationForPatientTableRow from "./vaccinationForPatientTableRow.jsx";

class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { id: "", fullName: "", SNILS: "", dateOfBirth: "", gender: "", 
            vaccinations: [], loadState: true };

        this.backHandler = this.backHandler.bind(this);
        this.updateHandler = this.updateHandler.bind(this);
        this.addHandler = this.addHandler.bind(this);
    }

    backHandler() {
        window.location.href = "/patients.html";
    }

    updateHandler() {
        window.location.href = "/vaccinationsForPatient.html";
    }

    addHandler() {
        window.location.href = "/addVaccinationForPatientForm.html";
    }

    componentDidMount() {
        let getPatientURL = "api/patient/" + sessionStorage.getItem("patientId");
        fetch(getPatientURL, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                let formattedName = data.lastName + " " + data.firstName;
                formattedName += (data.secondName === null) ? "" : " " + data.secondName;

                var tempDate = data.dateOfBirth.substr(0, 10);
                var formattedDate = tempDate.substr(8,2) + "." + tempDate.substr(5,2) + "." + tempDate.substr(0,4);

                this.setState({ id: data.id, SNILS: data.snils, fullName: formattedName, dateOfBirth: formattedDate, gender: data.gender });
            });

        let getVaccinationsURL = "api/vaccination/getvaccinations/" + sessionStorage.getItem("patientId");
        fetch(getVaccinationsURL, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => { this.setState({ vaccinations: data, loadState: false }); });
    }

    render() {
        return(
            <div className="container min-vh-100">
                <div className="row align-items-center min-vh-100">
                    <div className="col text-center">
                       <div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="fullName" className="col-sm-2 col-form-label font-weight-bold">ФИО</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="fullName" value={this.state.fullName}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="dateOfBirth" className="col-sm-2 col-form-label font-weight-bold">Дата рождения</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="dateOfBirth" value={this.state.dateOfBirth}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="gender" className="col-sm-2 col-form-label font-weight-bold">Пол</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="gender" value={this.state.gender}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="snils" className="col-sm-2 col-form-label font-weight-bold">СНИЛС</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="snils" value={this.state.SNILS}></input>
                                </div>
                            </div>
                            <div>
                                <input type="button" value="Назад" className="btn btn-secondary btn-lg float-left" onClick={this.backHandler}></input>
                                <input type="button" value="Добавить прививку" className="btn btn-primary btn-lg float-right ml-1" onClick={this.addHandler}></input>
                                <input type="button" value="Обновить" className="btn btn-primary btn-lg float-right ml-1" onClick={this.updateHandler}></input>
                            </div>
                            <div className="clearfix"></div>
                            <table className="table mt-1">
                                <thead>
                                    <tr>
                                        <th scope="col">Препарат</th>
                                        <th scope="col">Согласие</th>
                                        <th scope="col">Дата</th>
                                        <th scope="col"></th>
                                        <th scope="col"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        this.state.vaccinations.map( function(vaccination) { 
                                            return <VaccinationForPatientTableRow key={vaccination.id} vaccination={vaccination}/> 
                                        }, this)
                                    }
                                </tbody>
                            </table>
                            <div className={this.state.loadState ? "alert alert-light" : "d-none"}>
                                Загрузка...
                            </div>
                            <div className={((this.state.vaccinations.length === 0) && (!this.state.loadState)) ? "alert alert-light" : "d-none"}>
                                Прививки отсутствуют
                            </div>
                       </div>
                    </div>
                </div>
            </div>
        );
    }
}

ReactDOM.render(
    <Content />,
    document.getElementById("vaccinationsForPatient")
);
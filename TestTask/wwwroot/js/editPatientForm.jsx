class Content extends React.Component {
    constructor(props) {
        super(props);

        this.state = { id: "", firstName: "", secondName: "", lastName: "", SNILS: "", dateOfBirth: "", gender: "", 
            emptyFName: false, emptyLName: false, emptyDate: false, emptyGender: false, incorrectSNILS: false };

        this.onFirstNameChanged = this.onFirstNameChanged.bind(this);
        this.onSecondNameChanged = this.onSecondNameChanged.bind(this);
        this.onLastNameChanged = this.onLastNameChanged.bind(this);
        this.onDateOfBirthChanged = this.onDateOfBirthChanged.bind(this);
        this.onGenderChanged = this.onGenderChanged.bind(this);
        this.onSNILSChanged = this.onSNILSChanged.bind(this);

        this.editHandler = this.editHandler.bind(this);
        this.cancelHandler = this.cancelHandler.bind(this);
    }

    componentDidMount() {
        let url = "api/patient/" + sessionStorage.getItem("id");
        sessionStorage.removeItem("id");
        fetch(url, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }})
            .then(response => response.json())
            .then(data => {
                var date = data.dateOfBirth.substr(0, 10);
                this.setState({ id: data.id, firstName: data.firstName, secondName: data.secondName, lastName: data.lastName,
                    SNILS: data.snils, dateOfBirth: date, gender: data.gender });
            });
    }

    onFirstNameChanged(e) {
        this.setState({ firstName: e.target.value });
    }

    onSecondNameChanged(e) {
        this.setState({ secondName: e.target.value });
    }

    onLastNameChanged(e) {
        this.setState({ lastName: e.target.value });
    }

    onDateOfBirthChanged(e) {
        this.setState({ dateOfBirth: e.target.value });
    }

    onGenderChanged(e) {
        this.setState({ gender: e.target.value });
    }

    onSNILSChanged(e) {
        this.setState({ SNILS: e.target.value });
    }

    editHandler() {
        let fName = this.state.firstName;
        let sName = this.state.secondName;
        let lName = this.state.lastName;
        let date = this.state.dateOfBirth;
        let gender = this.state.gender;
        let snils = this.state.SNILS;

        let doRequest = true;

        this.setState({ emptyFName: false, emptyLName: false, emptyDate: false, emptyGender: false, incorrectSNILS: false });

        if (fName === "") {
            this.setState({ emptyFName: true });
            doRequest = false;
        }
        if (lName === "") {
            this.setState({ emptyLName: true });
            doRequest = false;
        }
        if (date === "") {
            this.setState({ emptyDate: true });
            doRequest = false;
        }
        if (gender === "") {
            this.setState({ emptyGender: true });
            doRequest = false;
        }
        if ((snils === "") || (!this.correctSNILS(snils))) {
            this.setState({ incorrectSNILS: true });
            doRequest = false;
        }

        if (doRequest) {
            let data = JSON.stringify({ "id":this.state.id, "firstName":fName, "secondName":sName, "lastName":lName, 
                "dateOfBirth":date, "gender":gender,  "snils":snils });

            fetch("api/patient", {
                method: "PUT",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: data
            })
                .then(response => {
                    if (response.status === 200) {
                        window.location.href = "./patients.html";
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
        window.location.href = "./patients.html";
    }

    correctSNILS(snils) {
        let onlyNumbersSNILS = snils.split(new RegExp("[- ]", "g")).join("");

        if ((onlyNumbersSNILS.length !== 11) || (isNaN(onlyNumbersSNILS))) {
            return false;
        }

        // 001 001 998
        if (parseInt(onlyNumbersSNILS.substr(0, 9)) <= 1001998) {
            return false;
        }

        let arrSNILS = onlyNumbersSNILS.split("");
        let checksum = 0;
        for (let i = 0; i < 9; i++) {
            checksum += arrSNILS[i] * (9 - i);
        }

        let enteredChecksum = parseInt(onlyNumbersSNILS.substr(9, 2));
        if (checksum !== enteredChecksum) {
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
                                <label htmlFor="inputLastName" className="col-sm-2 col-form-label">Фамилия</label>
                                <div className="col-3">
                                    <input type="text" className={this.state.emptyLName ? "form-control is-invalid" : "form-control"} id="inputLastName" placeholder="Фамилия"
                                        value={this.state.lastName} onChange={this.onLastNameChanged}></input>
                                    <div className={this.state.emptyLName ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputFirstName" className="col-sm-2 col-form-label">Имя</label>
                                <div className="col-3">
                                    <input type="text" className={this.state.emptyFName ? "form-control is-invalid" : "form-control"} id="inputFirstName" placeholder="Имя"
                                        value={this.state.firstName} onChange={this.onFirstNameChanged}></input>
                                    <div className={this.state.emptyFName ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputSecondName" className="col-sm-2 col-form-label">Отчество</label>
                                <div className="col-3">
                                    <input type="text" className="form-control" id="inputSecondName" placeholder="Отчество"
                                        value={this.state.secondName} onChange={this.onSecondNameChanged}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputDateOfBirth" className="col-sm-2 col-form-label">Дата рождения</label>
                                <div className="col-3">
                                    <input type="date" className={this.state.emptyDate ? "form-control is-invalid" : "form-control"} id="inputDateOfBirth" placeholder="дд.мм.гггг"
                                        value={this.state.dateOfBirth} onChange={this.onDateOfBirthChanged}></input>
                                    <div className={this.state.emptyDate ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputGender" className="col-sm-2 col-form-label">Пол</label>
                                <div className="col-3">
                                    <input type="text" className={this.state.emptyGender ? "form-control is-invalid" : "form-control"} id="inputGender" placeholder="Пол"
                                        value={this.state.gender} onChange={this.onGenderChanged}></input>
                                    <div className={this.state.emptyGender ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputSNILS" className="col-sm-2 col-form-label">СНИЛС</label>
                                <div className="col-3">
                                    <input type="text" className={this.state.incorrectSNILS ? "form-control is-invalid" : "form-control"} id="inputSNILS" placeholder="XXX-XXX-XXX YY"
                                        value={this.state.SNILS} onChange={this.onSNILSChanged}></input>
                                    <div className={this.state.incorrectSNILS ? "invalid-feedback text-left" : "d-none"}>Неверно введен номер СНИЛС</div>
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

ReactDOM.render(
    <Content />,
    document.getElementById("editPatientForm")
);
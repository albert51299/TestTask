class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { firstName: "", secondName: "", lastName: "", SNILS: "", dateOfBirth: "", gender: "", 
            emptyFName: false, emptyLName: false, emptyDate: false, emptyGender: false, incorrectSNILS: false,
            snilsExist: false, incorrectDate: false };

        this.onFirstNameChanged = this.onFirstNameChanged.bind(this);
        this.onSecondNameChanged = this.onSecondNameChanged.bind(this);
        this.onLastNameChanged = this.onLastNameChanged.bind(this);
        this.onDateOfBirthChanged = this.onDateOfBirthChanged.bind(this);
        this.onGenderChanged = this.onGenderChanged.bind(this);
        this.onSNILSChanged = this.onSNILSChanged.bind(this);

        this.addHandler = this.addHandler.bind(this);
        this.cancelHandler = this.cancelHandler.bind(this);
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

    addHandler() {
        let fName = this.state.firstName;
        let sName = this.state.secondName;
        let lName = this.state.lastName;
        let date = this.state.dateOfBirth;
        let gender = this.state.gender;
        let snils = this.state.SNILS;

        let doRequest = true;

        this.setState({ emptyFName: false, emptyLName: false, emptyDate: false, emptyGender: false, incorrectSNILS: false,
            snilsExist: false, incorrectDate: false });

        // валидация
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
        if (!this.correctDate(date)) {
            this.setState({ incorrectDate: true });
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
            let data = JSON.stringify({ "firstName":fName, "secondName":sName, "lastName":lName, 
                "dateOfBirth":date, "gender":gender,  "snils":this.getFormattedSNILS(snils) });

            fetch("api/patient", {
                method: "POST",
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
                    if (response.status === 409) {
                        this.setState({ snilsExist: true });
                    }
                });
        }
    }

    cancelHandler() {
        window.location.href = "./patients.html";
    }

    correctSNILS(snils) {
        // удаление '-' и ' ' из строки
        let onlyNumbersSNILS = snils.split(new RegExp("[- ]", "g")).join("");

        if ((onlyNumbersSNILS.length !== 11) || (isNaN(onlyNumbersSNILS))) {
            return false;
        }

        // если снилс меньше 001 001 998
        if (parseInt(onlyNumbersSNILS.substr(0, 9)) <= 1001998) {
            return false;
        }

        // вычисление контрольной суммы
        let arrSNILS = onlyNumbersSNILS.split("");
        let checksum = 0;
        for (let i = 0; i < 9; i++) {
            checksum += arrSNILS[i] * (9 - i);
        }

        // проверка контрольной суммы
        let enteredChecksum = parseInt(onlyNumbersSNILS.substr(9, 2));
        if ((checksum < 100) && (checksum !== enteredChecksum)) {
            return false;
        }
        if (((checksum === 100) || (checksum === 101)) && (enteredChecksum !== 0)) {
            return false;
        }
        if ((checksum > 101) && ((checksum % 101) !== enteredChecksum)) {
            return false;
        }

        // если все проверки пройдены
        return true;
    }

    getFormattedSNILS(snils) {
        let onlyNumbersSNILS = snils.split(new RegExp("[- ]", "g")).join("");
        return onlyNumbersSNILS.substr(0, 3) + "-" + onlyNumbersSNILS.substr(3, 3) + "-" + onlyNumbersSNILS.substr(6, 3) + " " + onlyNumbersSNILS.substr(9, 2);
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
                                <label htmlFor="inputLastName" className="col-sm-2 col-form-label font-weight-bold">*Фамилия</label>
                                <div className="col-3">
                                    <input type="text" className={this.state.emptyLName ? "form-control is-invalid" : "form-control"} id="inputLastName" placeholder="Фамилия"
                                        value={this.state.lastName} onChange={this.onLastNameChanged}></input>
                                    <div className={this.state.emptyLName ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputFirstName" className="col-sm-2 col-form-label font-weight-bold">*Имя</label>
                                <div className="col-3">
                                    <input type="text" className={this.state.emptyFName || this.state.incorrectFName ? "form-control is-invalid" : "form-control"} id="inputFirstName" placeholder="Имя"
                                        value={this.state.firstName} onChange={this.onFirstNameChanged}></input>
                                    <div className={this.state.emptyFName ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputSecondName" className="col-sm-2 col-form-label font-weight-bold">Отчество</label>
                                <div className="col-3">
                                    <input type="text" className="form-control" id="inputSecondName" placeholder="Отчество"
                                        value={this.state.secondName} onChange={this.onSecondNameChanged}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputDateOfBirth" className="col-sm-2 col-form-label font-weight-bold">*Дата рождения</label>
                                <div className="col-3">
                                    <input type="date" className={this.state.emptyDate || this.state.incorrectDate ? "form-control is-invalid" : "form-control"} id="inputDateOfBirth" placeholder="дд.мм.гггг"
                                        value={this.state.dateOfBirth} onChange={this.onDateOfBirthChanged}></input>
                                    <div className={this.state.emptyDate ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                    <div className={this.state.incorrectDate ? "invalid-feedback text-left" : "d-none"}>Неверно введена дата</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputGender" className="col-sm-2 col-form-label font-weight-bold">*Пол</label>
                                <div className="col-3">
                                    <select className={this.state.emptyGender ? "form-control is-invalid" : "form-control"} id="inputGender" value={this.state.gender} onChange={this.onGenderChanged}>
                                        <option value="" disabled>-</option>
                                        <option value="муж">муж</option>
                                        <option value="жен">жен</option>
                                    </select>
                                    <div className={this.state.emptyGender ? "invalid-feedback text-left" : "d-none"}>Обязательное поле</div>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="inputSNILS" className="col-sm-2 col-form-label font-weight-bold">*СНИЛС</label>
                                <div className="col-3">
                                    <input type="text" className={(this.state.incorrectSNILS || this.state.snilsExist) ? "form-control is-invalid" : "form-control"} id="inputSNILS" placeholder="XXX-XXX-XXX YY"
                                        value={this.state.SNILS} onChange={this.onSNILSChanged}></input>
                                    <small className="form-text text-muted float-left">Символы '-' и ' ' можно пропустить</small>
                                    <div className={this.state.incorrectSNILS ? "invalid-feedback text-left" : "d-none"}>Неверно введен номер СНИЛС</div>
                                    <div className={this.state.snilsExist ? "invalid-feedback text-left" : "d-none"}>Введенный СНИЛС уже зарегистрирован</div>
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
    document.getElementById("addPatientForm")
);
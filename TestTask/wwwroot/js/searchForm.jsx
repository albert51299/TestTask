class SearchForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { firstName: "", secondName: "", lastName: "", SNILS: "", 
            emptyFieldsState: false, incorrectSNILSState: false };

        this.onFirstNameChanged = this.onFirstNameChanged.bind(this);
        this.onSecondNameChanged = this.onSecondNameChanged.bind(this);
        this.onLastNameChanged = this.onLastNameChanged.bind(this);
        this.onSNILSChanged = this.onSNILSChanged.bind(this);

        this.searchHandler = this.searchHandler.bind(this);
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

    onSNILSChanged(e) {
        this.setState({ SNILS: e.target.value });
    }

    searchHandler() {
        this.setState({ emptyFieldsState: false, incorrectSNILSState: false });

        let searchResult = [];

        let fName = this.state.firstName;
        let sName = this.state.secondName;
        let lName = this.state.lastName;
        let snils = this.state.SNILS;

        if ((fName === "") && (sName === "") && (lName === "") && (snils === "")) {
            this.setState({ emptyFieldsState: true });
            return;
        }

        if (snils !== "") {
            if (!this.props.correctSNILS(snils)) {
                this.setState({ incorrectSNILSState: true });
                return;
            }
            // remove "" and "-" at the end of the line
            snils = snils.substr(0, 14);
        }

        for (let patient of this.props.allPatients) {
            searchResult.push(patient);

            if ((fName !== patient.firstName) && (fName !== "")) {
                searchResult.pop();
                continue;
            }
            if ((sName !== patient.secondName) && (sName !== "")) {
                searchResult.pop();
                continue;
            }
            if ((lName !== patient.lastName) && (lName !== "")) {
                searchResult.pop();
                continue;
            }
            if ((snils !== patient.snils) && (snils !== "")) {
                searchResult.pop();
                continue;
            }
        }
        this.props.changeDisplayedPatients(searchResult);
        this.props.changeSearchState(false);
        this.props.changeShowAllPatientsState(false);
    }

    cancelHandler() {
        this.props.changeSearchState(false);
    }

    render() {
        return(
            <div>
                <div className="form-group row">
                    <label htmlFor="inputLastName" className="col-sm-2 col-form-label">Фамилия</label>
                    <div className="col-sm-10">
                        <input type="text" className="form-control" id="inputLastName" placeholder="Фамилия"
                            value={this.state.lastName} onChange={this.onLastNameChanged}></input>
                    </div>
                </div>
                <div className="form-group row">
                    <label htmlFor="inputFirstName" className="col-sm-2 col-form-label">Имя</label>
                    <div className="col-sm-10">
                        <input type="text" className="form-control" id="inputFirstName" placeholder="Имя"
                            value={this.state.firstName} onChange={this.onFirstNameChanged}></input>
                    </div>
                </div>
                <div className="form-group row">
                    <label htmlFor="inputSecondName" className="col-sm-2 col-form-label">Отчество</label>
                    <div className="col-sm-10">
                        <input type="text" className="form-control" id="inputSecondName" placeholder="Отчество"
                            value={this.state.secondName} onChange={this.onSecondNameChanged}></input>
                    </div>
                </div>
                <div className="form-group row">
                    <label htmlFor="inputSNILS" className="col-sm-2 col-form-label">СНИЛС</label>
                    <div className="col-sm-10">
                        <input type="text" className="form-control" id="inputSNILS" placeholder="XXX-XXX-XXX YY"
                            value={this.state.SNILS} onChange={this.onSNILSChanged}></input>
                    </div>
                </div>
                <div className={this.state.incorrectSNILSState ? "alert alert-danger" : "d-none"}>
                    Неверно введен номер СНИЛС
                </div>
                <div className={this.state.emptyFieldsState ? "alert alert-danger" : "d-none"}>
                    Для поиска необходимо задать хотя бы одно поле
                </div>
                <input type="button" value="Поиск" className="btn btn-primary mr-1" onClick={this.searchHandler}></input>
                <input type="button" value="Отмена" className="btn btn-danger" onClick={this.cancelHandler}></input>
            </div>
        );
    }
}

export default SearchForm;
import VaccineOption from "./vaccineOption.jsx";

class SearchVaccinationsForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { firstName: "", secondName: "", lastName: "", vaccineName: "", date: "", vaccines: [], 
            emptyFieldsState: false };

        this.onFirstNameChanged = this.onFirstNameChanged.bind(this);
        this.onSecondNameChanged = this.onSecondNameChanged.bind(this);
        this.onLastNameChanged = this.onLastNameChanged.bind(this);
        this.onVaccineNameChanged = this.onVaccineNameChanged.bind(this);
        this.onDateChanged = this.onDateChanged.bind(this);

        this.searchHandler = this.searchHandler.bind(this);
        this.cancelHandler = this.cancelHandler.bind(this);
    }

    componentDidMount() {
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

    onFirstNameChanged(e) {
        this.setState({ firstName: e.target.value });
    }

    onSecondNameChanged(e) {
        this.setState({ secondName: e.target.value });
    }

    onLastNameChanged(e) {
        this.setState({ lastName: e.target.value });
    }

    onVaccineNameChanged(e) {
        this.setState({ vaccineName: e.target.value });
    }

    onDateChanged(e) {
        this.setState({ date: e.target.value });
    }

    searchHandler() {
        this.setState({ emptyFieldsState: false });

        let searchResult = [];

        let fName = this.state.firstName;
        let sName = this.state.secondName;
        let lName = this.state.lastName;
        let vaccineName = this.state.vaccineName;
        let date = this.state.date;

        if ((fName === "") && (sName === "") && (lName === "") && (vaccineName === "") && (date === "")) {
            this.setState({ emptyFieldsState: true });
            return;
        }

        // не включать в выборку, если прививка не соответсвует всем указанным условиям
        for (let vaccination of this.props.allVaccinations) {
            searchResult.push(vaccination);

            if ((fName !== vaccination.firstName) && (fName !== "")) {
                searchResult.pop();
                continue;
            }
            if ((sName !== vaccination.secondName) && (sName !== "")) {
                searchResult.pop();
                continue;
            }
            if ((lName !== vaccination.lastName) && (lName !== "")) {
                searchResult.pop();
                continue;
            }
            if ((vaccineName !== vaccination.vaccineName) && (vaccineName !== "")) {
                searchResult.pop();
                continue;
            }
            if ((date !== vaccination.date.substr(0, 10)) && (date !== "")) {
                searchResult.pop();
                continue;
            }
        }
        this.props.changeDisplayedVaccinations(searchResult);
        this.props.changeSearchState(false);
        this.props.changeShowAllVaccinationsState(false);
    }

    cancelHandler() {
        this.props.changeSearchState(false);
    }

    render() {
        return(
            <div>
                <div className="form-group row justify-content-center">
                    <label htmlFor="inputLastName" className="col-sm-2 col-form-label font-weight-bold">Фамилия</label>
                    <div className="col-3">
                        <input type="text" className="form-control" id="inputLastName" placeholder="Фамилия"
                            value={this.state.lastName} onChange={this.onLastNameChanged}></input>
                    </div>
                </div>
                <div className="form-group row justify-content-center">
                    <label htmlFor="inputFirstName" className="col-sm-2 col-form-label font-weight-bold">Имя</label>
                    <div className="col-3">
                        <input type="text" className="form-control" id="inputFirstName" placeholder="Имя"
                            value={this.state.firstName} onChange={this.onFirstNameChanged}></input>
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
                    <label htmlFor="inputVaccineName" className="col-sm-2 col-form-label font-weight-bold">Препарат</label>
                    <div className="col-3">
                        <select className="form-control" id="inputVaccineName" value={this.state.vaccineName} onChange={this.onVaccineNameChanged}>
                            <option value="" disabled>Выберите препарат</option>
                            {
                                this.state.vaccines.map( function(vaccine) { return <VaccineOption key={vaccine.id} vaccine={vaccine}/> } )
                            }
                        </select>
                    </div>
                </div>
                <div className="form-group row justify-content-center">
                    <label htmlFor="inputDate" className="col-sm-2 col-form-label font-weight-bold">Дата проведения</label>
                    <div className="col-3">
                        <input type="date" className="form-control" id="inputDate" placeholder="дд.мм.гггг"
                            value={this.state.date} onChange={this.onDateChanged}></input>
                    </div>
                </div>
                <div className="form-group row justify-content-center">
                    <div className={this.state.emptyFieldsState ? "col-5 alert alert-danger" : "d-none"}>
                        Для поиска необходимо задать хотя бы одно поле
                    </div>
                </div>
                <input type="button" value="Поиск" className="btn btn-primary mr-1" onClick={this.searchHandler}></input>
                <input type="button" value="Отмена" className="btn btn-danger" onClick={this.cancelHandler}></input>
            </div>
        );
    }
}

export default SearchVaccinationsForm;
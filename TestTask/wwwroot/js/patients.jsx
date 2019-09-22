import PatientTableRow from "./patientTableRow.jsx";
import SearchForm from "./searchForm.jsx";
import AddForm from "./addForm.jsx";

class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { allPatients: [], displayedPatients: [], searchState: false, addState: false };

        this.changeSearchState = this.changeSearchState.bind(this);
        this.changeAddState = this.changeAddState.bind(this);
        this.changeDisplayedPatients = this.changeDisplayedPatients.bind(this);

        this.updateHandler = this.updateHandler.bind(this);
        this.searchHandler = this.searchHandler.bind(this);
        this.addHandler = this.addHandler.bind(this);
        this.backHandler = this.backHandler.bind(this);

        this.correctSNILS = this.correctSNILS.bind(this);
    }

    changeSearchState(state) {
        this.setState({ searchState: state });
    }

    changeAddState(state) {
        this.setState({ addState: state });
    }

    changeDisplayedPatients(patients) {
        this.setState({ displayedPatients: patients });
    }

    updateHandler() {
        window.location.href = "./patients.html";
    }

    searchHandler() {
        this.setState({ searchState: true });
    }

    addHandler() {
        this.setState({ addState: true });
    }

    backHandler() {
        window.location.href = "./index.html";
    }

    loadAllPatients() {
        fetch("api/Patient", {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }})
            .then(response => response.json())
            .then(data => { 
                this.setState({ allPatients: data });
                this.setState({ displayedPatients: data });
            });
    }

    componentDidMount() {
       this.loadAllPatients();
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
                            <input type="button" value="Назад" className="btn btn-secondary btn-lg float-left" onClick={this.backHandler}></input>
                            <input type="button" value="Обновить" className="btn btn-primary btn-lg float-left ml-1" onClick={this.updateHandler}></input>
                            <input type="button" value="Поиск" className="btn btn-primary btn-lg float-left ml-1" onClick={this.searchHandler}></input>
                            <input type="button" value="Добавить" className="btn btn-primary btn-lg float-left ml-1" onClick={this.addHandler}></input>
                            <div className="clearfix"></div>
                            <table className={ (!this.state.searchState && !this.state.addState) ? "table" : "d-none" }>
                                <thead>
                                    <tr>
                                        <th scope="col">ФИО</th>
                                        <th scope="col">Дата рождения</th>
                                        <th scope="col">СНИЛС</th>
                                        <th scope="col"></th>
                                        <th scope="col"></th>
                                        <th scope="col"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        this.state.displayedPatients.map( function(patient) { 
                                            return <PatientTableRow key={patient.id} patient={patient}/> 
                                        }, this)
                                    }
                                </tbody>
                            </table>
                            <div className={((this.state.displayedPatients.length === 0) && (!this.state.searchState) && (!this.state.addState)) ? "alert alert-light" : "d-none"}>
                                Пациенты не найдены
                            </div>
                            {
                                this.state.searchState ? <SearchForm allPatients={this.state.allPatients} changeDisplayedPatients={this.changeDisplayedPatients} 
                                    changeSearchState={this.changeSearchState} correctSNILS={this.correctSNILS}/> : null
                            }
                            {
                                this.state.addState ? <AddForm changeAddState={this.changeAddState}/> : null
                            }
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

ReactDOM.render(
    <Content />,
    document.getElementById("patients")
);
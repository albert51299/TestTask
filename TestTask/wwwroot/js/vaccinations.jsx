import VaccinationTableRow from "./vaccinationTableRow.jsx";

class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { allVaccinations: [], displayedVaccinations: [], loadState: true, searchState: false, showAllVaccinationsState: true };
    
        this.changeSearchState = this.changeSearchState.bind(this);
        this.changeDisplayedVaccinations = this.changeDisplayedVaccinations.bind(this);
        this.changeShowAllVaccinationsState = this.changeShowAllVaccinationsState.bind(this);

        this.updateHandler = this.updateHandler.bind(this);
        this.showAllHandler = this.showAllHandler.bind(this);
        this.searchHandler = this.searchHandler.bind(this);
        this.addHandler = this.addHandler.bind(this);
        this.backHandler = this.backHandler.bind(this);
    }

    changeSearchState(state) {
        this.setState({ searchState: state });
    }

    changeDisplayedVaccinations(vaccinations) {
        this.setState({ displayedVaccinations: vaccinations });
    }

    changeShowAllVaccinationsState(state) {
        this.setState({ showAllVaccinationsState: state });
    }

    updateHandler() {
        window.location.href = "./vaccinations.html";
    }

    showAllHandler() {
        this.setState({ displayedVaccinations: this.state.allVaccinations, showAllVaccinationsState: true });
    }

    searchHandler() {
        this.setState({ searchState: true });
    }

    addHandler() {
        window.location.href = "./addVaccinationForm.html";
    }

    backHandler() {
        window.location.href = "./index.html";
    }

    loadAllVaccinations() {
        fetch("api/vaccination", {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }})
            .then(response => response.json())
            .then(data => { 
                this.setState({ allVaccinations: data, displayedVaccinations: data, loadState: false });
            });
    }

    componentDidMount() {
       this.loadAllVaccinations();
    }

    render() {
        return(
            <div className="container min-vh-100">
                <div className="row align-items-center min-vh-100">
                    <div className="col text-center">
                        <div>
                        <div className={(this.state.searchState) ? "d-none" : ""}>
                                <input type="button" value="Назад" className="btn btn-secondary btn-lg float-left" onClick={this.backHandler}></input>
                                <input type="button" value="Обновить" className="btn btn-primary btn-lg float-left ml-1" onClick={this.updateHandler}></input>
                                <input type="button" value="Показать все" className={this.state.showAllVaccinationsState ? "d-none" : "btn btn-primary btn-lg float-left ml-1"} onClick={this.showAllHandler}></input>
                                <input type="button" value="Поиск" className="btn btn-primary btn-lg float-left ml-1" onClick={this.searchHandler}></input>
                                <input type="button" value="Добавить" className="btn btn-primary btn-lg float-left ml-1" onClick={this.addHandler}></input>
                            </div>
                            <div className="clearfix"></div>
                            <table className={ !this.state.searchState ? "table" : "d-none" }>
                                <thead>
                                    <tr>
                                        <th scope="col">Название</th>
                                        <th scope="col">Согласие</th>
                                        <th scope="col">Дата</th>
                                        <th scope="col">Пациент</th>
                                        <th scope="col"></th>
                                        <th scope="col"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        this.state.displayedVaccinations.map( function(vaccination) { 
                                            return <VaccinationTableRow key={vaccination.id} vaccination={vaccination}/> 
                                        }, this)
                                    }
                                </tbody>
                            </table>
                            <div className={this.state.loadState ? "alert alert-light" : "d-none"}>
                                Загрузка...
                            </div>
                            <div className={((this.state.displayedVaccinations.length === 0) && (!this.state.searchState) && (!this.state.loadState)) ? "alert alert-light" : "d-none"}>
                                Прививки не найдены
                            </div>
                            {
                                this.state.searchState ? <SearchForm allVaccinations={this.state.allVaccinations} changeDisplayedVaccinations={this.changeDisplayedVaccinations} 
                                    changeSearchState={this.changeSearchState} changeShowAllVaccinationsState={this.changeShowAllVaccinationsState}/> : null
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
    document.getElementById("vaccinations")
);
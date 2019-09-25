class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = { id: "", firstName: "", secondName: "", lastName: "", SNILS: "", dateOfBirth: "", gender: "" };

        this.deleteHandler = this.deleteHandler.bind(this);
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
                var tempDate = data.dateOfBirth.substr(0, 10);
                var date = tempDate.substr(8,2) + "." + tempDate.substr(5,2) + "." + tempDate.substr(0,4);
                this.setState({ id: data.id, firstName: data.firstName, secondName: data.secondName, lastName: data.lastName,
                    SNILS: data.snils, dateOfBirth: date, gender: data.gender });
            });
    }

    deleteHandler() {
        fetch("api/patient/" + this.state.id, {
            method: "DELETE",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }})
            .then(response => {
                if (response.status === 200) {
                    window.location.href = "./patients.html";
                }
                if (response.status === 404) {
                    alert("Not found");
                }
            });
    }

    cancelHandler() {
        window.location.href = "./patients.html";
    }

    render() {
        return(
            <div className="container min-vh-100">
                <div className="row align-items-center min-vh-100">
                    <div className="col text-center">
                        <div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="lName" className="col-sm-2 col-form-label">Фамилия</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="lName" value={this.state.lastName}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="fName" className="col-sm-2 col-form-label">Имя</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="fName" value={this.state.firstName}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="sName" className="col-sm-2 col-form-label">Отчество</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="sName" value={this.state.secondName}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="date" className="col-sm-2 col-form-label">Дата рождения</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="date" value={this.state.dateOfBirth}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="gender" className="col-sm-2 col-form-label">Пол</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="gender" value={this.state.gender}></input>
                                </div>
                            </div>
                            <div className="form-group row justify-content-center">
                                <label htmlFor="snils" className="col-sm-2 col-form-label">СНИЛС</label>
                                <div className="col-3">
                                    <input type="text" readOnly className="form-control-plaintext" id="snils" value={this.state.SNILS}></input>
                                </div>
                            </div>
                            <input type="button" value="Удалить" className="btn btn-danger mr-1" onClick={this.deleteHandler}></input>
                            <input type="button" value="Отмена" className="btn btn-secondary" onClick={this.cancelHandler}></input>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

ReactDOM.render(
    <Content />,
    document.getElementById("deletePatientForm")
);
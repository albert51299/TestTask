class VaccinationForPatientTableRow extends React.Component {
    constructor(props) {
        super(props);

        var tempDate = props.vaccination.date.substr(0, 10);
        var date = tempDate.substr(8,2) + "." + tempDate.substr(5,2) + "." + tempDate.substr(0,4);

        this.state = { vaccination: props.vaccination, formattedDate: date };

        this.editHandler = this.editHandler.bind(this);
        this.deleteHandler = this.deleteHandler.bind(this);
    }

    editHandler() {
        sessionStorage.setItem("vaccinationId", this.props.vaccination.id);
        sessionStorage.setItem("fromPatient", "true");
        window.location.href = "./editVaccinationForm.html";
    }

    deleteHandler() {
        sessionStorage.setItem("vaccinationId", this.props.vaccination.id);
        sessionStorage.setItem("fromPatient", "true");
        window.location.href = "./deleteVaccinationForm.html";
    }

    render() {
        return(
            <tr>
                <td className="text-left">{this.state.vaccination.vaccineName}</td>
                <td>{this.state.vaccination.consent ? "Да" : "Нет"}</td>
                <td>{this.state.formattedDate}</td>
                <td>
                    <input type="button" value="Редактировать" className="btn btn-secondary btn-sm" onClick={this.editHandler}></input>
                </td>
                <td>
                    <input type="button" value="Удалить" className="btn btn-danger btn-sm" onClick={this.deleteHandler}></input>
                </td>
            </tr>
        );
    }
}

export default VaccinationForPatientTableRow;
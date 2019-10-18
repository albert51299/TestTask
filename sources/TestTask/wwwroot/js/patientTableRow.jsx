class PatientTableRow extends React.Component {
    constructor(props) {
        super(props);

        var name = props.patient.lastName + " " + props.patient.firstName + " " + 
            (props.patient.secondName !== null ? this.props.patient.secondName : "");
        var tempDate = props.patient.dateOfBirth.substr(0, 10);
        var date = tempDate.substr(8,2) + "." + tempDate.substr(5,2) + "." + tempDate.substr(0,4);
        this.state = { patient: props.patient, fullName: name, dateOfBirth: date };

        this.vaccinationsHandler = this.vaccinationsHandler.bind(this);
        this.editHandler = this.editHandler.bind(this);
        this.deleteHandler = this.deleteHandler.bind(this);
    }

    vaccinationsHandler() {
        sessionStorage.setItem("patientId", this.props.patient.id);
        window.location.href = "/vaccinationsForPatient.html";
    }

    editHandler() {
        sessionStorage.setItem("patientId", this.props.patient.id);
        window.location.href = "/editPatientForm.html";
    }

    deleteHandler() {
        sessionStorage.setItem("patientId", this.props.patient.id);
        window.location.href = "/deletePatientForm.html";
    }

    render() {
        return(
            <tr>
                <td className="text-left">{this.state.fullName}</td>
                <td>{this.state.dateOfBirth}</td>
                <td>{this.state.patient.snils}</td>
                <td>
                    <input type="button" value="Показать прививки" className="btn btn-primary btn-sm" onClick={this.vaccinationsHandler}></input>
                </td>
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

export default PatientTableRow;
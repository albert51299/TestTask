class PatientTableRow extends React.Component {
    constructor(props) {
        super(props);

        var name = props.patient.lastName + " " + props.patient.firstName + " " + 
            (props.patient.secondName !== null ? this.props.patient.secondName : "");
        var dateTime = new Date(props.patient.dateOfBirth);
        var date = new Date(dateTime.getFullYear(), dateTime.getMonth(), dateTime.getDay()).toLocaleDateString("ru");

        this.state = { patient: props.patient, fullName: name, dateOfBirth: date };
    }

    render() {
        return(
            <tr>
                <td className="text-left">{this.state.fullName}</td>
                <td>{this.state.dateOfBirth}</td>
                <td>{this.state.patient.snils}</td>
                <td>
                    <input type="button" value="Показать прививки" className="btn btn-primary btn-sm"></input>
                </td>
                <td>
                    <input type="button" value="Редактировать" className="btn btn-secondary btn-sm"></input>
                </td>
                <td>
                    <input type="button" value="Удалить" className="btn btn-danger btn-sm"></input>
                </td>
            </tr>
        );
    }
}

export default PatientTableRow;
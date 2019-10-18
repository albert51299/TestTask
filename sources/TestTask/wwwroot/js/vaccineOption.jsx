class VaccineOption extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: props.vaccine }
    }

    render() {
        return (
            <option>{this.state.data.name}</option>
        );
    }
}

export default VaccineOption;
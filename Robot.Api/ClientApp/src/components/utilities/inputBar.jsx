import React from "react";
import axios from "axios";
import { Range, getTrackBackground } from "react-range";
const STEP = 0.1;
const MIN = 0.0;
const MAX = 1.0;
class InputBar extends React.Component {
  state = {
    values: [1.0],
  };

  componentDidMount() {
    console.log("mount");
    this.Status();
  }

  componentWillUnmount() {}

  Status = async () => {
    console.log("response");
    let response = await axios.get("util/ConfigStatus");
    console.log(response);
    this.setState({ values: [response.data.speed] });
  };

  setSpeed = (speed) => {
    let requestSpeed = {
      Speed: speed[0].toFixed(1),
    };
    console.log(requestSpeed.Speed);
    this.setState({ values: speed });
    axios.post("motor/speed", requestSpeed);
  };

  GetPercentage() {
    return this.state.values[0].toFixed(1) * 100;
  }
  render() {
    return (
      <React.Fragment>
        <hr />
        <label>Speed</label>
        <div
          style={{
            display: "flex",
            justifyContent: "center",
            flexWrap: "wrap",
            margin: "2em",
          }}
        >
          <Range
            values={this.state.values}
            step={STEP}
            min={MIN}
            max={MAX}
            onChange={(values) => this.setSpeed(values)}
            renderTrack={({ props, children }) => (
              <div
                onMouseDown={props.onMouseDown}
                onTouchStart={props.onTouchStart}
                style={{
                  ...props.style,
                  height: "36px",
                  display: "flex",
                  width: "100%",
                }}
              >
                <div
                  ref={props.ref}
                  style={{
                    height: "5px",
                    width: "100%",
                    borderRadius: "4px",
                    background: getTrackBackground({
                      values: this.state.values,
                      colors: ["#548BF4", "#ccc"],
                      min: MIN,
                      max: MAX,
                    }),
                    alignSelf: "center",
                  }}
                >
                  {children}
                </div>
              </div>
            )}
            renderThumb={({ props, isDragged }) => (
              <div
                {...props}
                style={{
                  ...props.style,
                  height: "42px",
                  width: "42px",
                  borderRadius: "4px",
                  backgroundColor: "#FFF",
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  boxShadow: "0px 2px 6px #AAA",
                }}
              >
                <div
                  style={{
                    height: "16px",
                    width: "5px",
                    backgroundColor: isDragged ? "#548BF4" : "#CCC",
                  }}
                />
              </div>
            )}
          />
          <output style={{ marginTop: "30px" }} id="output">
            {this.GetPercentage() + "%"}
          </output>
        </div>
        <hr />
      </React.Fragment>
    );
  }
}

export default InputBar;

import React, { useContext } from "react";
import { Joystick } from "react-joystick-component";
import axios from "axios";
import {
  Figure,
  Card,
  Accordion,
  AccordionContext,
  useAccordionButton,
  Spinner,
} from "react-bootstrap";
import holder from "../components/img/holder.jpg";

function ContextAwareToggle({ children, eventKey, callback }) {
  const { activeEventKey } = useContext(AccordionContext);

  const decoratedOnClick = useAccordionButton(
    eventKey,
    () => callback && callback(eventKey)
  );

  const isCurrentEventKey = activeEventKey === eventKey;

  return (
    <button
      type="button"
      style={{ backgroundColor: isCurrentEventKey ? "pink" : "lavender" }}
      onClick={decoratedOnClick}
    >
      {children}
    </button>
  );
}

const handleMove = (e) => {
  let yResult = "";

  if (e.y >= 0) {
    yResult = "up";
  }
  if (e.y <= -1) {
    yResult = "down";
  }
  let directionObj = {
    angle: yResult,
    direction: e.direction,
  };
  console.log(directionObj);
  axios.post("motor/direction", directionObj);
};

const handleStop = async (e) => {
  let request = { response: e.type };
  axios.post("motor/stop", request);
  console.log(request);
};

function DrivePanel() {
  return (
    <Accordion defaultActiveKey="0">
      <Card>
        <Card.Header>
          <ContextAwareToggle eventKey="0">Drive</ContextAwareToggle>
        </Card.Header>
        <Accordion.Collapse eventKey="0">
          <Card.Body>
            <div className="joystick">
              <Joystick
                size={150}
                baseColor="gray"
                stickColor="white"
                throttle={500}
                stop={handleStop}
                move={handleMove}
              />
            </div>
          </Card.Body>
        </Accordion.Collapse>
      </Card>
    </Accordion>
  );
}

function Panel() {
  return (
    <React.Fragment>
      {/* <label>Charging... </label>

      <Spinner animation="border" size="sm" /> */}
      {/* <label>Charged </label> */}

      {/* <label className="redlevelWarning">Please charge low battery !</label> */}
      <div className="battery half charging"> </div>

      <Figure>
        <Figure.Image width={500} height={480} alt="500x480" src={holder} />
      </Figure>
      <DrivePanel />
    </React.Fragment>
  );
}

export default Panel;

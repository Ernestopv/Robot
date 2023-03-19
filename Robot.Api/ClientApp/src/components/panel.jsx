import React, { useContext, useState, useEffect } from "react";
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
  axios.post("motor/direction", directionObj);
};

const handleStop = async (e) => {
  let request = { response: e.type };
  axios.post("motor/stop", request);
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
  const [charging, setCharging] = useState(false);
  const [battery, setBattery] = useState(0);
  const [cameraOn, setCameraOn] = useState(false);
  const [urlImage, setUrlImage] = useState(holder);

  useEffect(() => {
    const interval = setInterval(async () => {
      await handleBatteryStatus();
      await CameraStatus();
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  const handleBatteryStatus = async () => {
    let response = await axios.get("util/battery");
    setBattery(response.data.percentage);
    setCharging(response.data.charging);
  };

  const HandleBatteryLevel = () => {
    let value = "";
    if (charging) value = "charging";
    if (battery <= 3) return "empty " + value;
    if (battery <= 10) return "almostempty " + value;
    if (battery <= 25) return "quarter " + value;
    if (battery <= 50) return "half " + value;
    if (battery <= 75) return "morethanhalf " + value;
    if (battery <= 100) return "full " + value;
  };

  const CameraStatus = async () => {
    let response = await axios.get("util/ConfigStatus");
    setCameraOn(response.data.isCameraOn);
    if (response.data.isCameraOn) CameraAction();
    else setUrlImage(holder);
  };

  const CameraAction = async () => {
    let response = await axios.get("util/Ip");
    var url = "http://" + response.data.ip + ":8090/?action=stream";
    setUrlImage(url);
  };

  return (
    <React.Fragment>
      {battery <= 10 ? (
        <label className="centerLabel redlevelWarning">Low battery !!!</label>
      ) : (
        ""
      )}
      {charging && battery < 100 ? (
        <label className="centerLabel">
          Charging...
          <Spinner animation="border" size="sm" />{" "}
        </label>
      ) : (
        ""
      )}
      {charging && battery === 100 ? (
        <label className="centerLabel">Charged</label>
      ) : (
        ""
      )}

      <div className={`battery + ${HandleBatteryLevel()}`}> </div>

      <Figure>
        <Figure.Image width={500} height={480} alt="500x480" src={urlImage} />
      </Figure>
      <DrivePanel />
    </React.Fragment>
  );
}

export default Panel;

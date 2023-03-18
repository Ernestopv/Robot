import React, { useState, useEffect } from "react";
import { Navbar, Offcanvas, Container, Nav, Button } from "react-bootstrap";
import InputBar from "./utilities/inputBar";
import axios from "axios";

function NavbarTop() {
  const [cameraOn, setCameraOn] = useState(false);



  const handleCameraON = async (isOn) => {

    const config = {
  headers: {
    'Content-Type': 'application/json',
  },
  params: {
     isOn: isOn
  }
}

    let response = await axios.post("util/Camera", { isOn }, config);

    setCameraOn(response.data.isCameraOn);
  };



  

  return (
    <div>
      <Navbar bg="Dark" expand={false}>
        <Container fluid>
          <Navbar.Brand href="#" className="text-white">
            Robot
          </Navbar.Brand>

          <Navbar.Toggle aria-controls="offcanvasNavbar" />
          <Navbar.Offcanvas
            id="offcanvasNavbar"
            aria-labelledby="offcanvasNavbarLabel"
            placement="end"
          >
            <Offcanvas.Header closeButton>
              <Offcanvas.Title id="offcanvasNavbarLabel">
                Settings
              </Offcanvas.Title>
            </Offcanvas.Header>
            <Offcanvas.Body>
              <Nav className="justify-content-end flex-grow-1 pe-3">
                <Nav.Link href="#action1">
                  <InputBar />
                </Nav.Link>
                <Nav.Link href="#action2">
                  <label>Camera</label>
                  <br />
                  <br />
                  {cameraOn ? (
                    <Button onClick={() => handleCameraON(false)}>OFF</Button>
                  ) : (
                    <Button onClick={() => handleCameraON(true)}>ON</Button>
                  )}
                </Nav.Link>

                <Nav.Link href="#action3">
                  <hr />
                  <label>Lights</label>
                  <br />
                  <br />
                  <Button disabled>ON</Button>
                </Nav.Link>
              </Nav>
            </Offcanvas.Body>
          </Navbar.Offcanvas>
        </Container>
      </Navbar>
    </div>
  );
}

export default NavbarTop;

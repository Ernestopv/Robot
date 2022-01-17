import React from "react";
import { Navbar, Offcanvas, Container, Nav, Button } from "react-bootstrap";
import InputBar from "./utilities/inputBar";

function NavbarTop() {
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
                  <Button>ON</Button>
                </Nav.Link>

                <Nav.Link href="#action3">
                  <hr />
                  <label>Lights</label>
                  <br />
                  <br />
                  <Button>ON</Button>
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

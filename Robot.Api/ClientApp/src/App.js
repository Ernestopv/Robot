import "./App.css";
import React, { Component } from "react";
import NavbarTop from "./components/navbar";
import Panel from "./components/panel";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <React.Fragment>
        <NavbarTop />
        <Panel />
      </React.Fragment>
    );
  }
}

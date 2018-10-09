import * as React from 'react';
import { VictoryChart, VictoryBar, VictoryTheme, VictoryAxis, VictoryZoomContainer, VictoryLine, VictoryBrushContainer } from 'victory';

import { AutoComplete, Breadcrumb, DatePicker, Menu, Icon, Row, Col, Table, Layout, Tag, Divider } from 'antd';
import { BrowserRouter, Link, Route } from 'react-router-dom';

const SubMenu = Menu.SubMenu;
const MenuItemGroup = Menu.ItemGroup;
const { Header, Content, Footer, Sider } = Layout;

export default class App extends React.Component<any, any> {

  constructor(props: any) {
    super(props);
    this.state = {};
  }

  componentDidMount() {
    fetch("/api/timeseries/" + this.props.match.params.id)
      .then(res => res.json())
      .then(
        (result) => {
          console.log("loaded");
          this.setState({
            data: result,
          });
        },
        // Note: it's important to handle errors here
        // instead of a catch() block so that we don't swallow
        // exceptions from actual bugs in components.
        (error) => {
        }
      )
  }

  handleZoom(domain: any) {
    this.setState({ selectedDomain: domain });
  }

  handleBrush(domain: any) {
    this.setState({ zoomDomain: domain });
  }

  renderLines() {
    let lines = [];
    console.log(this.state.data);
    console.log("renderlines");
    if (this.state.data != null) {
      // iterate through each column, ignoring first
      for (let i = 1; i < this.state.data[0].length; ++i) {

        let lineData = [];
        // iterate through each line, ignoring headers
        for (let j = 1; j < this.state.data.length; ++j) {
          lineData.push({
            x: Date.parse(this.state.data[j][0]),
            y: this.state.data[j][i]
          });
        }

        lines.push(
          <VictoryLine
            key={this.state.data[0][i]}
            style={{
              data: { stroke: "tomato" }
            }}
            data={lineData}
          />
        )
      }
    }

    console.log(lines);
    return lines;
  }

  render() {
    const lines = this.renderLines();

    return (
      <div>
        <Breadcrumb>
          <Breadcrumb.Item>Home</Breadcrumb.Item>
          <Breadcrumb.Item><a href="">Application Center</a></Breadcrumb.Item>
          <Breadcrumb.Item><a href="">Application List</a></Breadcrumb.Item>
          <Breadcrumb.Item>An Application</Breadcrumb.Item>
        </Breadcrumb>,
        <VictoryChart width={600} height={350} scale={{ x: "time" }}
          containerComponent={
            <VictoryZoomContainer responsive={false}
              zoomDimension="x"
              zoomDomain={this.state.zoomDomain}
              onZoomDomainChange={this.handleZoom.bind(this)}
            />
          }
        >
          {lines}

        </VictoryChart>

        <VictoryChart
          padding={{ top: 0, left: 50, right: 50, bottom: 30 }}
          width={600} height={90} scale={{ x: "time" }}
          containerComponent={
            <VictoryBrushContainer responsive={false}
              brushDimension="x"
              brushDomain={this.state.selectedDomain}
              onBrushDomainChange={this.handleBrush.bind(this)}
            />
          }
        >
          <VictoryAxis
            tickValues={[
              new Date(1985, 1, 1),
              new Date(1990, 1, 1),
              new Date(1995, 1, 1),
              new Date(2000, 1, 1),
              new Date(2005, 1, 1),
              new Date(2010, 1, 1)
            ]}
            tickFormat={(x) => new Date(x).getFullYear()}
          />
          <VictoryLine
            style={{
              data: { stroke: "tomato" }
            }}
            data={[
              { x: new Date(1982, 1, 1), y: 125 },
              { x: new Date(1987, 1, 1), y: 257 },
              { x: new Date(1993, 1, 1), y: 345 },
              { x: new Date(1997, 1, 1), y: 515 },
              { x: new Date(2001, 1, 1), y: 132 },
              { x: new Date(2005, 1, 1), y: 305 },
              { x: new Date(2011, 1, 1), y: 270 },
              { x: new Date(2015, 1, 1), y: 470 }
            ]}
          />
        </VictoryChart>
      </div>
    );
  }
}
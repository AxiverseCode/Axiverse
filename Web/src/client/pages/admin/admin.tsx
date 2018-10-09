import * as React from 'react';
import { AutoComplete, DatePicker, Menu, Icon, Row, Col, Table, Layout, Tag, Divider } from 'antd';
import { BrowserRouter, Link, Route } from 'react-router-dom';

import styles from './admin.css';
import 'antd/dist/antd.css';

import Correlation from './correlation';
import Timeseries from './timeseries';
import Operations from './operations';

const SubMenu = Menu.SubMenu;
const MenuItemGroup = Menu.ItemGroup;
const { Header, Content, Footer, Sider } = Layout;
export interface Props {
  title?: string;
}

export default class Admin extends React.Component<Props> {
  render(): React.ReactNode {
    return (
      <BrowserRouter>
        <Layout>
          <Header>
            <a href="/"><div className={styles.header} /></a>
            <AutoComplete dataSource={['1', '2', '3']} />
            <Menu
                theme="dark"
                mode="horizontal"
                defaultSelectedKeys={['0']}
                style={{ lineHeight: '64px', float: 'right' }}>
              <Menu.Item key="1"><Link to="/hierarchy">Hierarchy</Link></Menu.Item>
              <Menu.Item key="2"><Link to="/correlation">Correlation</Link></Menu.Item>
              <Menu.Item key="3"><Link to="/timeseries/ACY">Timeseries</Link></Menu.Item>
              <Menu.Item key="4"><Link to="/operations">Operations</Link></Menu.Item>
            </Menu>
          </Header>
          <Route
            path="/correlation"
            component={Correlation} />
          <Route
            path="/timeseries/:id"
            component={Timeseries} />
          <Route
            path="/operations"
            component={Operations} />
        </Layout>
      </BrowserRouter>
    );
  }
}
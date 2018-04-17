import * as React from 'react';
import { Accordian, Avatar, Button, Header, Input, SvgComponent, Table } from '../../components'
import styles from './admin.css';

export interface Props {
  title?: string;
}

export default class Admin extends React.Component<Props> {
  render(): React.ReactNode {
    return (
      <div className={styles.container}>
        <header>
          <Header title='Admin'>
            <Avatar size='s'>Axiverse</Avatar>
          </Header>
        </header>
        <nav>
          <Accordian>
          </Accordian>
        </nav>
        <section>
          <div className={styles.content}>
            <SvgComponent></SvgComponent>
            <Table></Table>
          </div>
        </section>
      </div>
    );
  }
}
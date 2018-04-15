import * as React from 'react';
import {Avatar, Button, Header, Input} from '../../components'
import styles from './admin.css';

export interface Props {
  title?: string;
}

class Admin extends React.Component<Props> {
  render(): React.ReactNode {
      return (
        <div className={styles.container}>
            <Header title='Admin'>
              <Avatar size='s'>Axiverse</Avatar>
            </Header>
        </div>
      );
  }
}

export default Admin;
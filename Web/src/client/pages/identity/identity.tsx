import * as React from 'react';
import {Avatar, Button, Header, Input, Link} from '../../components'
import styles from './identity.css';

export interface Props {
  title?: string;
}

class Identity extends React.Component<Props> {
  render(): React.ReactNode {
      return (
        <div className={styles.container}>
        <div className={styles.card}>
          <h1>Login</h1>
          <Avatar size='l'></Avatar>
          <form>
            <Input placeholder='Username'></Input>
            <Input placeholder='Password' type='password'></Input>
            <Button className={styles.btn} value='Click me'></Button>
          </form>
          <Link href='#'>Forgot username or password.</Link>
        </div>
      </div>
      );
  }
}

export default Identity;
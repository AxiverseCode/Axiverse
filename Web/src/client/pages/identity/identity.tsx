import * as React from 'react';
import {Button, Header} from '../../components'
import styles from './header.css';

export interface Props {
  title?: string;
}

function Identity({ title = 'dd' }: Props) {
  return (
    <div>
        <Header title='title'></Header>
        <Button name='Bobbys'></Button>
    </div>
  );
}

export default Identity;
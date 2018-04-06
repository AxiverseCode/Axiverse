import * as React from 'react';
import {Button, Header, Panel} from '../../components'
import styles from './header.css';

export interface Props {
  title?: string;
}

function Identity({ title = 'dd' }: Props) {
  return (
    <div>
        <Header title='Identity'></Header>
        <Panel>
            <Button value='Click me'></Button>
        </Panel>
    </div>
  );
}

export default Identity;
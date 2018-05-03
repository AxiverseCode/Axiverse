import React from 'react';
import ReactDOM from 'react-dom';

import {Admin, Identity} from './pages';
import {Button} from './components';

import styles from './index.css';

ReactDOM.render(
  <Admin></Admin>,
  //<Identity></Identity>,
  document.getElementById('root') as HTMLElement
);
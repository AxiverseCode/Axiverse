import * as React from 'react';
import styles from './identity.css';

export interface Props {
  title?: string;
}

class Identity extends React.Component<Props> {
  render(): React.ReactNode {
      return (
        <div className={styles.container}>
          <div className={styles.card}>
          </div>
        </div>
      );
  }
}

export default Identity;
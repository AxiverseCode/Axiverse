import * as React from 'react';
import styles from './header.css';

export interface Props {
  title: string;
}

class Header extends React.Component<Props> {
  render(): React.ReactNode {
      return (
        <div className={styles.header}>
            Axiverse {this.props.title}
        </div>
      );
  }
}

export default Header;

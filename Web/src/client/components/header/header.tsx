import * as React from 'react';
import styles from './header.css';

export interface Props {
  title: string;
}

function Header({ title }: Props) {
  return (
    <div className={styles.header}>
        Axiverse {title}
    </div>
  );
}

export default Header;
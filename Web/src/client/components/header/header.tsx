import * as React from 'react';
import styles from './header.css';

export interface Props {
  title: string;
}

export default class Header extends React.Component<Props> {
    public render(): React.ReactNode {
        return (
          <div className={styles.header}>
            <span className={styles.title}>
        		Axiverse {this.props.title}
            </span>
            <span className={styles.body}>
                {this.props.children}
            </span>
          </div>
        );
    }
}
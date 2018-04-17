import * as React from 'react';
import classNames from 'classnames';

import styles from './accordian.css';

export interface Props {
    className?: any;
}

export default class Accordian extends React.Component<Props> {
    public static defaultProps: Partial<Props> = {
        
    };

    public render(): React.ReactNode {
        let className = classNames(styles.this, this.props.className);
        return (
            <div className={className}>
                <div className={styles.header}>Inventory</div>
                <div className={styles.header}>Assets</div>
                <div className={styles.content}>
                    <ul>
                        <li>Spaceships</li>
                        <li>Weapons</li>
                        <li>Charges</li>
                        <li>Modules</li>
                        <li>Ore</li>
                        <li>Materials</li>
                        <li>Components</li>
                        <li>Skills</li>
                    </ul>
                </div>
                <div className={styles.header}>Accounts</div>
                {this.props.children}
            </div>
        );
    }
}
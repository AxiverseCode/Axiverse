import * as React from 'react';
import classNames from 'classnames';

import styles from './icon.css';

export interface Props {
    className?: any;
}

export default class Icon extends React.Component<Props> {
    public static defaultProps: Partial<Props> = {
        
    };

    public render(): React.ReactNode {
        let className = classNames(styles.this, this.props.className);
        return (
            <div className={className}>
                {this.props.children}
            </div>
        );
    }
}
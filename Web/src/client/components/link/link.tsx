import * as React from 'react';
import classNames from 'classnames';

import styles from './link.css';

export interface Props {
    className?: any;
    href?: string;
}

class Link extends React.Component<Props> {
    public static defaultProps: Partial<Props> = {
        
    };

    public render(): React.ReactNode {
        let className = classNames(styles.this, this.props.className);
        return (
            <a className={className} href={this.props.href}>
                {this.props.children}
            </a>
        );
    }
}

export default Link;
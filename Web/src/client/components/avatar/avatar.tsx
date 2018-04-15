import * as React from 'react';
import classNames from 'classnames';

import styles from './avatar.css';

type Size = "s" | "m" | "l";
export interface Props {
    className?: any;
    size?: Size;
    src?: string;
    alt?: string;
}

class Avatar extends React.Component<Props> {
    public static defaultProps: Partial<Props> = {
        size: 's',
        alt: 'avatar',
    };

    public render(): React.ReactNode {
        let className = classNames(styles.this, this.props.className, {
            [styles.small] : this.props.size == 's',
            [styles.medium] : this.props.size == 'm',
            [styles.large] : this.props.size == 'l',
        });
        return (
            <div className={styles.container}>
                <div className={className}>
                    <img src={this.props.src || '/nyan.png'} alt={this.props.alt}/>
                </div>
                {this.props.children}
            </div>
        );
    }
}

export default Avatar;